using System;

namespace TDGachaGame
{
    public static class StageProgression
    {
        private static readonly (int Stage, double Easy, double Normal, double Hard, double Extreme)[] Anchors =
        {
            (1,   80, 17,  2.5, 0.5),
            (5,   62, 29,  8,   1),
            (10,  40, 38,  18,  4),
            (20,  18, 36,  35,  11),
            (35,  7,  25,  43,  25),
            (50,  3,  17,  42,  38),
            (100, 1,  8,   33,  58)
        };
        public static double[] GetTierWeights(int stage)
        {
            double[] w;

            if (stage <= Anchors[0].Stage)
            {
                w = ToArray(Anchors[0]);
            }
            else if (stage >= Anchors[Anchors.Length - 1].Stage)
            {
                w = ToArray(Anchors[Anchors.Length - 1]);
            }
            else
            {
                w = ToArray(Anchors[0]);

                for (int i = 0; i < Anchors.Length - 1; i++)
                {
                    var a = Anchors[i];
                    var b = Anchors[i + 1];

                    if (stage >= a.Stage && stage <= b.Stage)
                    {
                        double t = (stage - a.Stage) / (double)(b.Stage - a.Stage);
                        double[] wa = ToArray(a);
                        double[] wb = ToArray(b);

                        for (int k = 0; k < 4; k++)
                        {
                            w[k] = wa[k] + (wb[k] - wa[k]) * t;
                        }
                        break;
                    }
                }
            }

            double total = w[0] + w[1] + w[2] + w[3];
            for (int k = 0; k < 4; k++)
            {
                w[k] /= total;
            }

            return w;
        }
        public static int GetStartingCash(int stage)
        {
            int cleared = Math.Max(0, stage - 1);
            return DevConfig.TowerStartingCash + (cleared / 3) * 50;
        }

        public static DifficultyTier RollTier(Random rng, int stage)
        {
            double[] weights = GetTierWeights(stage);
            double roll = rng.NextDouble();
            double running = 0;

            for (int i = 0; i < 3; i++)
            {
                running += weights[i];
                if (roll < running)
                {
                    return (DifficultyTier)i;
                }
            }

            return DifficultyTier.Extreme;
        }

        private static double[] ToArray((int Stage, double Easy, double Normal, double Hard, double Extreme) a)
        {
            return new[] { a.Easy, a.Normal, a.Hard, a.Extreme };
        }
    }
}