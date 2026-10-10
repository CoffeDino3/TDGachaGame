using System;

namespace TDGachaGame
{
    public static class CollectionUpgradeConfig
    {
        public const int MaxLevel = 50;
        public const int ExpPerLevelStep = 100;
        public const int ExpCurveBend = 4;
        public const float DamagePerLevel = 0.03f;
        public const float RangePerLevel = 0.005f;
        public const float SpeedPerLevel = 0.005f;
        public const float BlastPerLevel = 0.008f;

        public const float MaxDamageMultiplier = 2.5f;
        public const float MaxRangeMultiplier = 1.25f;
        public const float MinIntervalMultiplier = 0.7f;
        public const float MaxBlastMultiplier = 1.4f;

        public static int GetExpToNext(int level)
        {
            if (level >= MaxLevel)
            {
                return 0;
            }

            int l = Math.Max(1, level);
            return ExpPerLevelStep * l + ExpCurveBend * (l - 1) * l;
        }
        public static int GetTotalExpForLevel(int level)
        {
            int l = Math.Max(1, Math.Min(level, MaxLevel));
            int total = 0;

            for (int k = 1; k < l; k++)
            {
                total += GetExpToNext(k);
            }

            return total;
        }
        public static int PreviewLevel(int level, int exp, int extraExp)
        {
            long total = GetTotalExpForLevel(level) + exp + (long)extraExp;
            int result = level;

            while (result < MaxLevel && total >= GetTotalExpForLevel(result + 1))
            {
                result++;
            }

            return result;
        }


        private static int GetShapeExp(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Line: return 100;
                case ShapeType.Triangle: return 400;
                case ShapeType.Square: return 900;
                case ShapeType.Pentagon: return 1800;
                case ShapeType.Hexagon: return 3500;
                default: return 100;
            }
        }
        public static int GetColorWeight(ShapeColor color)
        {
            switch (color)
            {
                case ShapeColor.Yellow: return 1;
                case ShapeColor.Blue: return 2;
                case ShapeColor.Green: return 2;
                case ShapeColor.Cyan: return 3;
                case ShapeColor.Purple: return 3;
                case ShapeColor.Red: return 4;
                case ShapeColor.White: return 5;
                case ShapeColor.Black: return 6;
                default: return 1;
            }
        }

        public static int GetMergeExp(UnitId unit)
        {
            return GetShapeExp(unit.Type) * GetColorWeight(unit.Color);
        }


        private static int GetShapeCostFactor(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Line: return 1;
                case ShapeType.Triangle: return 2;
                case ShapeType.Square: return 3;
                case ShapeType.Pentagon: return 4;
                case ShapeType.Hexagon: return 5;
                default: return 1;
            }
        }

        public static int GetLevelUpCost(UnitId unit, int level)
        {
            if (level >= MaxLevel)
            {
                return 0;
            }

            return GetExpToNext(level) * GetShapeCostFactor(unit.Type);
        }

        private static int GetShapeSellBase(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Line: return 40;
                case ShapeType.Triangle: return 120;
                case ShapeType.Square: return 250;
                case ShapeType.Pentagon: return 450;
                case ShapeType.Hexagon: return 800;
                default: return 40;
            }
        }

        public static int GetSellValue(UnitId unit, int level)
        {
            float baseValue = GetShapeSellBase(unit.Type) * GetColorWeight(unit.Color);
            float levelBonus = 1f + 0.04f * (Math.Max(1, level) - 1);
            return (int)Math.Round(baseValue * levelBonus);
        }


        public static float GetDamageMultiplier(int level) => Math.Min(MaxDamageMultiplier, 1f + DamagePerLevel * (Math.Max(1, level) - 1));

        public static float GetRangeMultiplier(int level) => Math.Min(MaxRangeMultiplier, 1f + RangePerLevel * (Math.Max(1, level) - 1));

        public static float GetIntervalMultiplier(int level) => Math.Max(MinIntervalMultiplier, 1f - SpeedPerLevel * (Math.Max(1, level) - 1));

        public static float GetBlastMultiplier(int level) => Math.Min(MaxBlastMultiplier, 1f + BlastPerLevel * (Math.Max(1, level) - 1));
    }
}