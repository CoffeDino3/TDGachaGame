using System;
using System.Collections.Generic;

namespace TDGachaGame
{
    public static class FloorEnemyGenerator
    {
        private static readonly Random rng = new Random();

        public static (DifficultyTier tier, List<List<Enemy>> waves) Generate(int stageNumber)
        {
            DifficultyTier tier = RollDifficultyTier(stageNumber);
            List<List<Enemy>> waves = BuildWaves(stageNumber, tier);

            return (tier, waves);
        }
        public static int GetWaveCount(int stageNumber)
        {
            return 10 + Math.Max(0, stageNumber) / 5;
        }

        private static List<List<Enemy>> BuildWaves(int stageNumber, DifficultyTier tier)
        {
            int waveCount = GetWaveCount(stageNumber);

            var waves = new List<List<Enemy>>();
            int previousCount = 0;

            for (int w = 0; w < waveCount; w++)
            {
                int count = RollWaveEnemyCount(stageNumber, tier);

                if (count <= previousCount)
                {
                    count = previousCount + rng.Next(1, 4);
                }

                previousCount = count;
                List<Enemy> wave = BuildEnemyList(count, stageNumber);

                if (w == waveCount - 1)
                {
                    int bossIndex = rng.Next(wave.Count);
                    wave[bossIndex] = new Enemy(wave[bossIndex].Color, 0, stageNumber, true);
                }

                waves.Add(wave);
            }

            return waves;
        }

        private static int RollWaveEnemyCount(int stageNumber, DifficultyTier tier)
        {
            int baseCount = 2 + stageNumber / 3;

            int tierBonus = tier switch
            {
                DifficultyTier.Easy => 0,
                DifficultyTier.Normal => 2,
                DifficultyTier.Hard => 4,
                DifficultyTier.Extreme => 6,
                _ => 0
            };

            int count = baseCount + tierBonus + rng.Next(-2, 5);
            return Math.Max(1, count);
        }

        private static List<Enemy> BuildEnemyList(int count, int stageNumber)
        {
            var pool = new List<Enemy>();
            var availableColors = GetAvailableColors(stageNumber);

            for (int i = 0; i < count; i++)
            {
                EnemyColor color = availableColors[rng.Next(availableColors.Count)];
                pool.Add(new Enemy(color, 0, stageNumber));
            }

            return pool;
        }
        private static DifficultyTier RollDifficultyTier(int stageNumber)
        {
            return StageProgression.RollTier(rng, stageNumber);
        }

        private static List<EnemyColor> GetAvailableColors(int stageNumber)
        {
            var colors = new List<EnemyColor> { EnemyColor.Grey };
            if (stageNumber >= 2)
            {
                colors.Add(EnemyColor.Yellow);
            }
            if (stageNumber >= 3)
            {
                colors.Add(EnemyColor.Aqua);
            }
            if (stageNumber >= 4)
            {
                colors.Add(EnemyColor.Green);
            }
            if (stageNumber >= 10)
            {
                colors.Add(EnemyColor.Blue);
            }

            return colors;
        }
    }
}