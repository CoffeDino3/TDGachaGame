using System;
using System.Collections.Generic;

namespace TDGachaGame
{

    public class TowerUpgradeTier
    {
        public int Level;
        public int Cost;
        public float DamageMultiplier = 1f;
        public float RangeMultiplier = 1f;
        public float AttackIntervalMultiplier = 1f;
        public float SpecialValue;
        public float SpecialValue2;
        public float CorridorWidthBonus;
        public int ExtraBeamCount;
        public float DamageBuffMultiplier;
        public float BlastRadiusMultiplier = 1f;
    }

    public static class TowerUpgradeConfig
    {

        public const float WhiteCooldownBuffer = 2.0f;

        private static readonly Dictionary<UnitId, (float baseDamage, float baseRange, float baseInterval, float baseBlast)> BaseStats
            = new Dictionary<UnitId, (float, float, float, float)>
            {
                [new UnitId(ShapeType.Triangle, ShapeColor.Yellow)] = (5f, 1.5f, 0.4f, 0f),
                [new UnitId(ShapeType.Triangle, ShapeColor.Red)] = (45f, 2.5f, 2.5f, 0f),
                [new UnitId(ShapeType.Triangle, ShapeColor.Purple)] = (0f, 1.5f, 1.5f, 0f),
                [new UnitId(ShapeType.Triangle, ShapeColor.Cyan)] = (13f, 2.5f, 1.0f, 0f),
                [new UnitId(ShapeType.Triangle, ShapeColor.Green)] = (0f, 1.5f, 1.2f, 0f),
                [new UnitId(ShapeType.Triangle, ShapeColor.Black)] = (16f, 2.0f, 1.4f, 0f),
                [new UnitId(ShapeType.Triangle, ShapeColor.White)] = (0f, 2.5f, 1.3f, 0f),
                [new UnitId(ShapeType.Triangle, ShapeColor.Blue)] = (30f, 3.5f, 1.6f, 0f),


                [new UnitId(ShapeType.Line, ShapeColor.Yellow)] = (5f, 1.5f, 0.4f, 0f),
                [new UnitId(ShapeType.Line, ShapeColor.Red)] = (45f, 2.5f, 2.5f, 0f),
                [new UnitId(ShapeType.Line, ShapeColor.Purple)] = (0f, 1.5f, 1.5f, 0f),
                [new UnitId(ShapeType.Line, ShapeColor.Cyan)] = (13f, 2.5f, 1.0f, 0f),
                [new UnitId(ShapeType.Line, ShapeColor.Green)] = (0f, 1.5f, 1.2f, 0f),
                [new UnitId(ShapeType.Line, ShapeColor.Black)] = (16f, 2.0f, 1.4f, 0f),
                [new UnitId(ShapeType.Line, ShapeColor.White)] = (0f, 2.5f, 1.3f, 0f),
                [new UnitId(ShapeType.Line, ShapeColor.Blue)] = (30f, 3.5f, 1.6f, 0f),


                [new UnitId(ShapeType.Square, ShapeColor.Yellow)] = (7f, 2.0f, 0.9f, 0.55f),
                [new UnitId(ShapeType.Square, ShapeColor.Red)] = (60f, 2.5f, 2.8f, 0.95f),
                [new UnitId(ShapeType.Square, ShapeColor.Purple)] = (10f, 2.2f, 1.8f, 0.8f),
                [new UnitId(ShapeType.Square, ShapeColor.Cyan)] = (18f, 2.4f, 1.4f, 0.85f),
                [new UnitId(ShapeType.Square, ShapeColor.Green)] = (6f, 2.2f, 1.5f, 0.85f),
                [new UnitId(ShapeType.Square, ShapeColor.Black)] = (20f, 2.3f, 1.6f, 0.85f),
                [new UnitId(ShapeType.Square, ShapeColor.White)] = (0f, 2.6f, 1.7f, 0.9f),
                [new UnitId(ShapeType.Square, ShapeColor.Blue)] = (38f, 3.0f, 1.9f, 0.85f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.Yellow)] = (6f, 1.8f, 0.75f, 0f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.Red)] = (50f, 2.0f, 2.6f, 0f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.Purple)] = (0f, 1.8f, 2.0f, 0f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.Cyan)] = (14f, 2.2f, 1.3f, 0f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.Green)] = (0f, 1.8f, 1.6f, 0f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.Black)] = (18f, 2.0f, 1.5f, 0f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.White)] = (0f, 2.2f, 1.8f, 0f),
                [new UnitId(ShapeType.Pentagon, ShapeColor.Blue)] = (34f, 2.8f, 1.9f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Yellow)] = (12f, 0.90f, 1.7f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Red)] = (90f, 0.70f, 5.5f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Purple)] = (10f, 0.85f, 3.0f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Cyan)] = (24f, 0.95f, 2.6f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Green)] = (10f, 0.85f, 2.6f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Black)] = (28f, 0.90f, 3.0f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.White)] = (14f, 0.95f, 6.0f, 0f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Blue)] = (40f, 0.95f, 4.5f, 0f),
            };

        private static readonly Dictionary<UnitId, List<TowerUpgradeTier>> Tiers = BuildAllTiers();

        public static (float damage, float range, float interval, float blast) GetBaseStats(ShapeType type, ShapeColor color)
            => BaseStats[new UnitId(type, color)];
        public static (float damage, float range, float interval, float blast) GetBaseStats(ShapeType type, ShapeColor color, int collectionLevel)
        {
            var b = BaseStats[new UnitId(type, color)];
            return (
                b.baseDamage * CollectionUpgradeConfig.GetDamageMultiplier(collectionLevel),
                b.baseRange * CollectionUpgradeConfig.GetRangeMultiplier(collectionLevel),
                b.baseInterval * CollectionUpgradeConfig.GetIntervalMultiplier(collectionLevel),
                b.baseBlast * CollectionUpgradeConfig.GetBlastMultiplier(collectionLevel));
        }

        public static int GetTierCount(ShapeType type, ShapeColor color) => Tiers[new UnitId(type, color)].Count;
        public static int GetMaxUpgradeLevel(ShapeType type, ShapeColor color) => Tiers[new UnitId(type, color)].Count - 1;
        public static TowerUpgradeTier GetTier(ShapeType type, ShapeColor color, int upgradeLevel)
        {
            var list = Tiers[new UnitId(type, color)];
            int index = Math.Max(0, Math.Min(upgradeLevel, list.Count - 1));
            return list[index];
        }

        public static TowerUpgradeTier GetNextTier(ShapeType type, ShapeColor color, int upgradeLevel)
        {
            var list = Tiers[new UnitId(type, color)];
            int nextIndex = upgradeLevel + 1;
            return nextIndex < list.Count ? list[nextIndex] : null;
        }

        public static int GetSellValue(ShapeType type, ShapeColor color, int upgradeLevel)
        {
            var list = Tiers[new UnitId(type, color)];
            int total = 0;
            int cappedLevel = Math.Min(upgradeLevel, list.Count - 1);
            for (int i = 1; i <= cappedLevel; i++)
            {
                total += list[i].Cost;
            }
            return (int)(total * 0.6f);
        }

        private static Dictionary<UnitId, List<TowerUpgradeTier>> BuildAllTiers()
        {
            return new Dictionary<UnitId, List<TowerUpgradeTier>>
            {
                [new UnitId(ShapeType.Triangle, ShapeColor.Yellow)] = BuildTiers(
                    count: 3, costs: new[] { 20, 50 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.05f, intervalShrink: 0.82f),

                [new UnitId(ShapeType.Triangle, ShapeColor.Red)] = BuildTiers(
                    count: 4, costs: new[] { 100, 230, 420 },
                    dmgGrowth: 1.35f, rangeGrowth: 1.08f, intervalShrink: 0.97f),

                [new UnitId(ShapeType.Triangle, ShapeColor.Purple)] = BuildTiers(
                    count: 5, costs: new[] { 80, 170, 300, 460 },
                    dmgGrowth: 1f, rangeGrowth: 1.08f, intervalShrink: 0.97f,
                    special1: i => 1.0f + i * 0.35f,
                    extraBeamCount: i => i == 4 ? 1 : 0),

                [new UnitId(ShapeType.Triangle, ShapeColor.Cyan)] = BuildTiers(
                    count: 5, costs: new[] { 90, 180, 300, 450 },
                    dmgGrowth: 1.18f, rangeGrowth: 1.10f, intervalShrink: 0.95f,
                    corridorBonus: i => i * 0.18f),

                [new UnitId(ShapeType.Triangle, ShapeColor.Black)] = BuildTiers(
                    count: 5, costs: new[] { 160, 330, 560, 840 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.08f, intervalShrink: 0.96f,
                    special1: i => 0.70f - i * 0.05f,
                    special2: i => 5f + i * 1f,
                    extraBeamCount: i => i >= 4 ? 2 : (i >= 2 ? 1 : 0)),

                [new UnitId(ShapeType.Triangle, ShapeColor.White)] = BuildTiers(
                    count: 4, costs: new[] { 130, 270, 480 },
                    dmgGrowth: 1f, rangeGrowth: 1.17f, intervalShrink: 0.95f,
                    special1: i => 2.0f + i * 0.5f,
                    special2: i => 1.5f + i * 0.15f,
                    damageBuffMultiplier: i => i == 3 ? 1.15f : 0f),

                [new UnitId(ShapeType.Triangle, ShapeColor.Green)] = BuildTiers(
                    count: 6, costs: new[] { 60, 120, 195, 290, 400 },
                    dmgGrowth: 1f, rangeGrowth: 1.06f, intervalShrink: 0.97f,
                    special1: i => 4f + i * 1.2f,
                    special2: i => 4f + i * 0.6f,
                    corridorBonus: i => i * 0.12f),

                [new UnitId(ShapeType.Triangle, ShapeColor.Blue)] = BuildTiers(
                    count: 3, costs: new[] { 100, 220 },
                    dmgGrowth: 1.25f, rangeGrowth: 1.12f, intervalShrink: 0.95f),

                [new UnitId(ShapeType.Line, ShapeColor.Yellow)] = BuildTiers(
                    count: 3, costs: new[] { 20, 50 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.05f, intervalShrink: 0.82f),

                [new UnitId(ShapeType.Line, ShapeColor.Red)] = BuildTiers(
                    count: 4, costs: new[] { 100, 230, 420 },
                    dmgGrowth: 1.35f, rangeGrowth: 1.08f, intervalShrink: 0.97f),

                [new UnitId(ShapeType.Line, ShapeColor.Purple)] = BuildTiers(
                    count: 5, costs: new[] { 80, 170, 300, 460 },
                    dmgGrowth: 1f, rangeGrowth: 1.08f, intervalShrink: 0.97f,
                    special1: i => 1.0f + i * 0.35f,
                    extraBeamCount: i => i == 4 ? 1 : 0),

                [new UnitId(ShapeType.Line, ShapeColor.Cyan)] = BuildTiers(
                    count: 5, costs: new[] { 90, 180, 300, 450 },
                    dmgGrowth: 1.18f, rangeGrowth: 1.10f, intervalShrink: 0.95f,
                    corridorBonus: i => i * 0.18f),

                [new UnitId(ShapeType.Line, ShapeColor.Black)] = BuildTiers(
                    count: 5, costs: new[] { 160, 330, 560, 840 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.08f, intervalShrink: 0.96f,
                    special1: i => 0.70f - i * 0.05f,
                    special2: i => 5f + i * 1f,
                    extraBeamCount: i => i >= 4 ? 2 : (i >= 2 ? 1 : 0)),

                [new UnitId(ShapeType.Line, ShapeColor.White)] = BuildTiers(
                    count: 4, costs: new[] { 130, 270, 480 },
                    dmgGrowth: 1f, rangeGrowth: 1.17f, intervalShrink: 0.95f,
                    special1: i => 2.0f + i * 0.5f,
                    special2: i => 1.5f + i * 0.15f,
                    damageBuffMultiplier: i => i == 3 ? 1.15f : 0f),

                [new UnitId(ShapeType.Line, ShapeColor.Green)] = BuildTiers(
                    count: 6, costs: new[] { 60, 120, 195, 290, 400 },
                    dmgGrowth: 1f, rangeGrowth: 1.06f, intervalShrink: 0.97f,
                    special1: i => 4f + i * 1.2f,
                    special2: i => 4f + i * 0.6f,
                    corridorBonus: i => i * 0.12f),

                [new UnitId(ShapeType.Line, ShapeColor.Blue)] = BuildTiers(
                    count: 3, costs: new[] { 100, 220 },
                    dmgGrowth: 1.25f, rangeGrowth: 1.12f, intervalShrink: 0.95f),

                [new UnitId(ShapeType.Square, ShapeColor.Yellow)] = BuildTiers(
                    count: 3, costs: new[] { 30, 70 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.05f, intervalShrink: 0.85f,
                    blastGrowth: 1.08f),

                [new UnitId(ShapeType.Square, ShapeColor.Red)] = BuildTiers(
                    count: 4, costs: new[] { 130, 280, 480 },
                    dmgGrowth: 1.3f, rangeGrowth: 1.06f, intervalShrink: 0.97f,
                    blastGrowth: 1.15f),

                [new UnitId(ShapeType.Square, ShapeColor.Purple)] = BuildTiers(
                    count: 5, costs: new[] { 90, 190, 330, 500 },
                    dmgGrowth: 1f, rangeGrowth: 1.06f, intervalShrink: 0.97f,
                    special1: i => 0.9f + i * 0.3f,
                    blastGrowth: 1.06f),

                [new UnitId(ShapeType.Square, ShapeColor.Cyan)] = BuildTiers(
                    count: 5, costs: new[] { 100, 200, 330, 480 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.08f, intervalShrink: 0.96f,
                    special1: i => 2.0f + i * 0.5f,
                    blastGrowth: 1.08f),

                [new UnitId(ShapeType.Square, ShapeColor.Green)] = BuildTiers(
                    count: 6, costs: new[] { 65, 130, 210, 310, 420 },
                    dmgGrowth: 1f, rangeGrowth: 1.05f, intervalShrink: 0.97f,
                    special1: i => 3.5f + i * 1.1f,
                    special2: i => 4f + i * 0.6f,
                    blastGrowth: 1.1f),

                [new UnitId(ShapeType.Square, ShapeColor.Black)] = BuildTiers(
                    count: 5, costs: new[] { 170, 350, 590, 880 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.07f, intervalShrink: 0.97f,
                    special1: i => 0.68f - i * 0.05f,
                    special2: i => 4.5f + i * 1f,
                    blastGrowth: 1.05f,
                    extraBeamCount: i => i >= 4 ? 2 : (i >= 2 ? 1 : 0)),

                [new UnitId(ShapeType.Square, ShapeColor.White)] = BuildTiers(
                    count: 4, costs: new[] { 140, 290, 500 },
                    dmgGrowth: 1f, rangeGrowth: 1.15f, intervalShrink: 0.95f,
                    special1: i => 2.5f + i * 0.4f,
                    special2: i => 1.2f + i * 0.12f,
                    blastGrowth: 1.1f),

                [new UnitId(ShapeType.Square, ShapeColor.Blue)] = BuildTiers(
                    count: 3, costs: new[] { 120, 250 },
                    dmgGrowth: 1.25f, rangeGrowth: 1.1f, intervalShrink: 0.95f,
                    blastGrowth: 1.1f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.Yellow)] = BuildTiers(
                    count: 3, costs: new[] { 35, 80 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.05f, intervalShrink: 1f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.Red)] = BuildTiers(
                    count: 4, costs: new[] { 140, 300, 520 },
                    dmgGrowth: 1.3f, rangeGrowth: 1.06f, intervalShrink: 1f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.Purple)] = BuildTiers(
                    count: 5, costs: new[] { 100, 210, 360, 540 },
                    dmgGrowth: 1f, rangeGrowth: 1.06f, intervalShrink: 1f,
                    special1: i => 1.0f + i * 0.3f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.Cyan)] = BuildTiers(
                    count: 5, costs: new[] { 110, 220, 360, 520 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.08f, intervalShrink: 1f,
                    special1: i => 1.5f + i * 0.4f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.Green)] = BuildTiers(
                    count: 6, costs: new[] { 70, 140, 225, 330, 450 },
                    dmgGrowth: 1f, rangeGrowth: 1.05f, intervalShrink: 1f,
                    special1: i => 3.5f + i * 1.1f,
                    special2: i => 4f + i * 0.6f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.Black)] = BuildTiers(
                    count: 5, costs: new[] { 180, 370, 620, 920 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.07f, intervalShrink: 1f,
                    special1: i => 0.68f - i * 0.05f,
                    special2: i => 4.5f + i * 1f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.White)] = BuildTiers(
                    count: 4, costs: new[] { 150, 310, 530 },
                    dmgGrowth: 1f, rangeGrowth: 1.15f, intervalShrink: 1f,
                    special1: i => 1.6f + i * 0.3f),

                [new UnitId(ShapeType.Pentagon, ShapeColor.Blue)] = BuildTiers(
                    count: 3, costs: new[] { 130, 270 },
                    dmgGrowth: 1.25f, rangeGrowth: 1.1f, intervalShrink: 1f),

                [new UnitId(ShapeType.Hexagon, ShapeColor.Yellow)] = BuildTiers(
                    count: 3, costs: new[] { 40, 90 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.07f, intervalShrink: 0.90f),

                [new UnitId(ShapeType.Hexagon, ShapeColor.Red)] = BuildTiers(
                    count: 4, costs: new[] { 160, 330, 560 },
                    dmgGrowth: 1.3f, rangeGrowth: 1.06f, intervalShrink: 0.95f),

                [new UnitId(ShapeType.Hexagon, ShapeColor.Purple)] = BuildTiers(
                    count: 5, costs: new[] { 115, 240, 400, 600 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.06f, intervalShrink: 0.95f,
                    special1: i => 0.9f + i * 0.3f),

                [new UnitId(ShapeType.Hexagon, ShapeColor.Cyan)] = BuildTiers(
                    count: 5, costs: new[] { 125, 250, 400, 570 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.07f, intervalShrink: 0.95f,
                    special1: i => 1.5f + i * 0.4f),
                [new UnitId(ShapeType.Hexagon, ShapeColor.Green)] = BuildTiers(
                    count: 6, costs: new[] { 80, 160, 250, 370, 500 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.05f, intervalShrink: 0.96f,
                    special1: i => 3.5f + i * 1.1f,
                    special2: i => 4f + i * 0.6f),

                [new UnitId(ShapeType.Hexagon, ShapeColor.Black)] = BuildTiers(
                    count: 5, costs: new[] { 200, 410, 680, 1000 },
                    dmgGrowth: 1.15f, rangeGrowth: 1.06f, intervalShrink: 0.96f,
                    special1: i => 0.68f - i * 0.05f,
                    special2: i => 4.5f + i * 1f,
                    extraBeamCount: i => i >= 4 ? 2 : (i >= 2 ? 1 : 0)),

                [new UnitId(ShapeType.Hexagon, ShapeColor.White)] = BuildTiers(
                    count: 4, costs: new[] { 170, 340, 580 },
                    dmgGrowth: 1.12f, rangeGrowth: 1.05f, intervalShrink: 0.95f,
                    special1: i => 1.6f + i * 0.3f,
                    special2: i => 1.25f + i * 0.1f,
                    damageBuffMultiplier: i => i == 3 ? 1.15f : 0f),

                [new UnitId(ShapeType.Hexagon, ShapeColor.Blue)] = BuildTiers(
                    count: 3, costs: new[] { 145, 300 },
                    dmgGrowth: 1.25f, rangeGrowth: 1.08f, intervalShrink: 0.94f),
            };
        }

        private static List<TowerUpgradeTier> BuildTiers(
            int count, int[] costs, float dmgGrowth, float rangeGrowth, float intervalShrink,
            Func<int, float> special1 = null, Func<int, float> special2 = null,
            Func<int, float> corridorBonus = null, Func<int, int> extraBeamCount = null,
            Func<int, float> damageBuffMultiplier = null, float blastGrowth = 1f)
        {
            var list = new List<TowerUpgradeTier>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new TowerUpgradeTier
                {
                    Level = i + 1,
                    Cost = i == 0 ? 0 : costs[i - 1],
                    DamageMultiplier = (float)Math.Pow(dmgGrowth, i),
                    RangeMultiplier = (float)Math.Pow(rangeGrowth, i),
                    AttackIntervalMultiplier = (float)Math.Pow(intervalShrink, i),
                    SpecialValue = special1 != null ? special1(i) : 0f,
                    SpecialValue2 = special2 != null ? special2(i) : 0f,
                    CorridorWidthBonus = corridorBonus != null ? corridorBonus(i) : 0f,
                    ExtraBeamCount = extraBeamCount != null ? extraBeamCount(i) : 0,
                    DamageBuffMultiplier = damageBuffMultiplier != null ? damageBuffMultiplier(i) : 0f,
                    BlastRadiusMultiplier = (float)Math.Pow(blastGrowth, i)
                });
            }
            return list;
        }
    }
}