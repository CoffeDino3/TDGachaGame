namespace TDGachaGame
{
    public static class EnemyStatsTable
    {
        public const float BossHealthMultiplier = 3.5f;
        public const float BossSpeedMultiplier = 0.9f;
        public const int BossExitDamageBonus = 2;

        public static float GetStageHealthMultiplier(int stage)
        {
            int cleared = System.Math.Max(0, stage - 1);
            return 1f + cleared * 0.08f + cleared * cleared * 0.0015f;
        }

        public static float GetStageSpeedMultiplier(int stage)
        {
            int cleared = System.Math.Max(0, stage - 1);
            return 1f + System.Math.Min(0.3f, cleared * 0.004f);
        }

        public static float GetBaseHealth(EnemyColor color)
        {
            return color switch
            {
                EnemyColor.Grey => 50,
                EnemyColor.Green => 180f,
                EnemyColor.Aqua => 60f,
                EnemyColor.Yellow => 50f,
                EnemyColor.Blue => 140f,
                _ => 50f
            };
        }

        public static float GetBaseSpeed(EnemyColor color)
        {
            return color switch
            {
                EnemyColor.Grey => 1.0f,
                EnemyColor.Yellow => 2.2f,
                EnemyColor.Aqua => 1.0f,
                EnemyColor.Green => 0.7f,
                EnemyColor.Blue => 1.1f,
                _ => 1.0f
            };
        }

        public static int GetBaseDamageToExit(EnemyColor color)
        {
            return color switch
            {
                EnemyColor.Green => 3,
                EnemyColor.Blue => 2,
                _ => 1
            };
        }
    }
}