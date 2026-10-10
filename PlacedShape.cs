using System;

namespace TDGachaGame
{
    public enum TargetingMode
    {
        First,
        Last,
        Closest,
        Farthest,
        LowestHp,
        HighestHp
    }

    public class PlacedShape
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public ShapeType Type { get; set; }
        public ShapeColor Color { get; set; }
        public int Rotation { get; set; }
        public float FacingDegrees { get; set; }
        public float AttackCooldownRemaining { get; set; }
        public float AttackSpeedBuffTimeRemaining { get; set; }
        public float AttackDamageBuffTimeRemaining { get; set; }
        public float AttackSpeedBuffMultiplier { get; set; } = 1f;
        public float AttackDamageBuffMultiplier { get; set; } = 1f;
        public int UpgradeLevel { get; set; } = 0;
        public int CollectionLevel { get; set; } = 1;
        public TargetingMode TargetingMode { get; set; } = TargetingMode.First;
        public bool HasBankedCrit { get; set; }
        public float BankedCritMultiplier { get; set; } = 1f;

        public void ApplyAttackSpeedBuff(float multiplier, float durationSeconds)
        {
            AttackSpeedBuffMultiplier = AttackSpeedBuffTimeRemaining > 0f
                ? Math.Max(AttackSpeedBuffMultiplier, multiplier)
                : multiplier;
            AttackSpeedBuffTimeRemaining = Math.Max(AttackSpeedBuffTimeRemaining, durationSeconds);
        }

        public void ApplyAttackDamageBuff(float multiplier, float durationSeconds)
        {
            AttackDamageBuffMultiplier = AttackDamageBuffTimeRemaining > 0f
                ? Math.Max(AttackDamageBuffMultiplier, multiplier)
                : multiplier;
            AttackDamageBuffTimeRemaining = Math.Max(AttackDamageBuffTimeRemaining, durationSeconds);
        }
        public void ApplyBankedCrit(float multiplier)
        {
            if (!HasBankedCrit || multiplier > BankedCritMultiplier)
            {
                BankedCritMultiplier = multiplier;
            }
            HasBankedCrit = true;
        }

        private TowerUpgradeTier CurrentTier => TowerUpgradeConfig.GetTier(Type, Color, UpgradeLevel);

        public float GetCurrentDamage()
        {
            var (baseDamage, _, _, _) = TowerUpgradeConfig.GetBaseStats(Type, Color, CollectionLevel);
            float damageMultiplier = AttackDamageBuffTimeRemaining > 0f ? AttackDamageBuffMultiplier : 1f;
            return baseDamage * CurrentTier.DamageMultiplier * damageMultiplier;
        }
        public float ConsumeDamageWithBankedCrit()
        {
            float damage = GetCurrentDamage();
            if (HasBankedCrit)
            {
                damage *= BankedCritMultiplier;
                HasBankedCrit = false;
            }
            return damage;
        }
        public float GetRangeMultiplier() => CurrentTier.RangeMultiplier * CollectionUpgradeConfig.GetRangeMultiplier(CollectionLevel);
        public float GetSummonAttackSpeed()
        {
            var (_, baseAttackSpeed, _, _) = TowerUpgradeConfig.GetBaseStats(Type, Color, CollectionLevel);
            float attackSpeed = baseAttackSpeed * CurrentTier.RangeMultiplier;
            if (AttackSpeedBuffTimeRemaining > 0f)
                attackSpeed *= AttackSpeedBuffMultiplier;
            return attackSpeed;
        }

        public float GetCurrentAttackInterval()
        {
            float interval;
            if (Color == ShapeColor.White && Type != ShapeType.Hexagon)
            {
                interval = CurrentTier.SpecialValue + TowerUpgradeConfig.WhiteCooldownBuffer;
            }
            else
            {
                var (_, _, baseInterval, _) = TowerUpgradeConfig.GetBaseStats(Type, Color, CollectionLevel);
                interval = baseInterval * CurrentTier.AttackIntervalMultiplier;
            }

            if (AttackSpeedBuffTimeRemaining > 0f)
                interval /= AttackSpeedBuffMultiplier;
            return interval;
        }

        public float GetSpecialValue1() => CurrentTier.SpecialValue;
        public float GetSpecialValue2() => CurrentTier.SpecialValue2;
        public float GetCorridorWidthBonus() => CurrentTier.CorridorWidthBonus;
        public int GetExtraBeamCount() => CurrentTier.ExtraBeamCount;
        public bool HasExtraBeam() => CurrentTier.ExtraBeamCount > 0;
        public float GetDamageBuffMultiplier() => CurrentTier.DamageBuffMultiplier;
        public float GetCurrentBlastRadius()
        {
            var (_, _, _, baseBlast) = TowerUpgradeConfig.GetBaseStats(Type, Color, CollectionLevel);
            return baseBlast * CurrentTier.BlastRadiusMultiplier;
        }

        public PlacedShape(int row, int col, ShapeType type, ShapeColor color, int rotation = 0)
        {
            Row = row;
            Col = col;
            Type = type;
            Color = color;
            Rotation = rotation;
            FacingDegrees = rotation;
            UpgradeLevel = 0;
            TargetingMode = TargetingMode.First;
        }
    }
}