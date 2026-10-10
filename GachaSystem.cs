using System;
using System.Collections.Generic;

namespace TDGachaGame
{
    public sealed class PullResult
    {
        public UnitId Unit { get; }
        public double Chance { get; }
        public int RarityIndex { get; }

        public string RarityName => GachaSystem.RarityNames[RarityIndex];

        public string ChanceText => GachaSystem.FormatChance(Chance);

        public PullResult(UnitId unit, double chance)
        {
            Unit = unit;
            Chance = chance;
            RarityIndex = GachaSystem.GetRarityIndex(chance);
        }
    }
    public static class GachaSystem
    {
        public const int GemCostPerPull = 50;
        public const int MaxPullsAtOnce = 50;

        public static readonly string[] RarityNames = { "Common", "Uncommon", "Rare", "Epic", "Legendary" };
        private static readonly (int Stage, double[] Odds)[] ShapeAnchors =
        {
            (1,   new[] { 93.0,  6.0,  0.8,  0.18, 0.02 }),
            (10,  new[] { 86.0, 11.0,  2.4,  0.55, 0.05 }),
            (20,  new[] { 74.0, 19.0,  5.5,  1.3,  0.2  }),
            (35,  new[] { 55.0, 28.0, 12.0,  4.2,  0.8  }),
            (50,  new[] { 38.0, 31.0, 20.0,  9.0,  2.0  }),
            (100, new[] { 12.0, 24.0, 30.0, 24.0, 10.0  })
        };
        private static readonly Dictionary<ShapeColor, double> ColorWeights = new Dictionary<ShapeColor, double>
        {
            [ShapeColor.Red] = 15,
            [ShapeColor.Purple] = 15,
            [ShapeColor.Cyan] = 15,
            [ShapeColor.Green] = 15,
            [ShapeColor.Blue] = 15,
            [ShapeColor.Yellow] = 15,
            [ShapeColor.Black] = 5,
            [ShapeColor.White] = 5
        };
        public static double[] GetShapeWeights(int stage)
        {
            stage = Math.Max(1, stage);
            int count = PlayerProfile.PlayableShapes.Length;
            double[] w;

            if (stage >= ShapeAnchors[ShapeAnchors.Length - 1].Stage)
            {
                w = Fit(ShapeAnchors[ShapeAnchors.Length - 1].Odds, count);
            }
            else
            {
                w = Fit(ShapeAnchors[0].Odds, count);

                for (int i = 0; i < ShapeAnchors.Length - 1; i++)
                {
                    var a = ShapeAnchors[i];
                    var b = ShapeAnchors[i + 1];

                    if (stage >= a.Stage && stage <= b.Stage)
                    {
                        double t = (stage - a.Stage) / (double)(b.Stage - a.Stage);
                        double[] wa = Fit(a.Odds, count);
                        double[] wb = Fit(b.Odds, count);

                        for (int k = 0; k < count; k++)
                        {
                            w[k] = wa[k] + (wb[k] - wa[k]) * t;
                        }
                        break;
                    }
                }
            }

            double total = 0;
            foreach (double value in w)
            {
                total += value;
            }

            for (int k = 0; k < count; k++)
            {
                w[k] /= total;
            }

            return w;
        }
        public static double GetColorChance(ShapeColor color)
        {
            double total = 0;
            foreach (double weight in ColorWeights.Values)
            {
                total += weight;
            }

            return ColorWeights.TryGetValue(color, out double mine) ? mine / total : 0;
        }
        public static double GetChance(UnitId unit, int stage)
        {
            int index = Array.IndexOf(PlayerProfile.PlayableShapes, unit.Type);
            if (index < 0)
            {
                return 0;
            }

            return GetShapeWeights(stage)[index] * GetColorChance(unit.Color);
        }
        public static UnitId Roll(Random rng, int stage)
        {
            double[] weights = GetShapeWeights(stage);
            ShapeType[] shapes = PlayerProfile.PlayableShapes;

            double roll = rng.NextDouble();
            double running = 0;
            ShapeType shape = shapes[shapes.Length - 1];

            for (int i = 0; i < weights.Length; i++)
            {
                running += weights[i];
                if (roll < running)
                {
                    shape = shapes[i];
                    break;
                }
            }

            return new UnitId(shape, RollColor(rng));
        }

        private static ShapeColor RollColor(Random rng)
        {
            double total = 0;
            foreach (double weight in ColorWeights.Values)
            {
                total += weight;
            }

            double roll = rng.NextDouble() * total;
            double running = 0;
            ShapeColor last = ShapeColor.Red;

            foreach (var pair in ColorWeights)
            {
                running += pair.Value;
                last = pair.Key;
                if (roll < running)
                {
                    return pair.Key;
                }
            }

            return last;
        }
        public static int GetRarityIndex(double chance)
        {
            if (chance >= 0.10)
            {
                return 0;
            }
            if (chance >= 0.03)
            {
                return 1;
            }
            if (chance >= 0.008)
            {
                return 2;
            }
            if (chance >= 0.002)
            {
                return 3;
            }

            return 4;
        }

        public static string FormatChance(double chance)
        {
            return (chance * 100.0).ToString("0.####") + "%";
        }
        public static int GetMaxAffordablePulls(PlayerProfile profile)
        {
            return Math.Min(MaxPullsAtOnce, profile.Gems / GemCostPerPull);
        }
        public static bool TryPull(PlayerProfile profile, int count, Random rng, out List<PullResult> results, out string message)
        {
            results = new List<PullResult>();

            if (count <= 0)
            {
                message = $"You need at least {GemCostPerPull} gems for a summon.";
                return false;
            }

            int cost = count * GemCostPerPull;

            if (profile.Gems < cost)
            {
                message = $"Not enough gems: {count} summon{(count == 1 ? "" : "s")} cost {cost}, you have {profile.Gems}.";
                return false;
            }

            if (profile.FreeCollectionSlots < count)
            {
                message = $"Not enough collection space: you need {count} free slot{(count == 1 ? "" : "s")} but only have {profile.FreeCollectionSlots}. Nothing was spent.";
                return false;
            }

            profile.TrySpendGems(cost);
            int stage = profile.TowerFloor;

            for (int i = 0; i < count; i++)
            {
                UnitId unit = Roll(rng, stage);
                profile.TryAddUnit(unit);
                results.Add(new PullResult(unit, GetChance(unit, stage)));
            }

            message = "";
            return true;
        }
        private static double[] Fit(double[] row, int count)
        {
            var result = new double[count];
            Array.Copy(row, result, Math.Min(row.Length, count));
            return result;
        }
    }
}