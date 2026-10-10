using System;
using System.Collections.Generic;

namespace TDGachaGame
{
    public class Enemy
    {
        public EnemyColor Color
        {
            get;
        }

        public float MaxHealth
        {
            get;
        }

        public float CurrentHealth
        {
            get;
            private set;
        }

        public float Speed
        {
            get;
        }

        public int DamageToExit
        {
            get;
        }

        public float PathProgress
        {
            get;
            private set;
        }

        public int LastTrapCheckIndex
        {
            get;
            set;
        } = -1;

        public int SplitGeneration
        {
            get;
        }

        public int StageNumber
        {
            get;
        }

        public bool IsBoss
        {
            get;
        }

        public bool IsDead => CurrentHealth <= 0f;

        public bool IsStunned => stunTimeRemaining > 0f;

        public bool IsAffectedByDot => dotTimeRemaining > 0f;

        public bool IsElevated => Color == EnemyColor.Blue && groundedTimeRemaining <= 0f;

        private float slowFactor = 1f;
        private float slowTimeRemaining;
        private float stunTimeRemaining;
        private float dotDamagePerSecond;
        private float dotTimeRemaining;
        private float groundedTimeRemaining;

        public Enemy(EnemyColor color, int splitGeneration = 0, int stageNumber = 1, bool isBoss = false)
        {
            Color = color;
            SplitGeneration = splitGeneration;
            StageNumber = Math.Max(1, stageNumber);
            IsBoss = isBoss;

            float splitScale = splitGeneration > 0 ? 0.5f : 1f;
            float bossHealth = isBoss ? EnemyStatsTable.BossHealthMultiplier : 1f;
            float bossSpeed = isBoss ? EnemyStatsTable.BossSpeedMultiplier : 1f;
            int bossDamage = isBoss ? EnemyStatsTable.BossExitDamageBonus : 0;

            MaxHealth = EnemyStatsTable.GetBaseHealth(color) * EnemyStatsTable.GetStageHealthMultiplier(StageNumber) * splitScale * bossHealth;
            CurrentHealth = MaxHealth;
            Speed = EnemyStatsTable.GetBaseSpeed(color) * EnemyStatsTable.GetStageSpeedMultiplier(StageNumber) * bossSpeed;
            DamageToExit = Math.Max(1, EnemyStatsTable.GetBaseDamageToExit(color) - splitGeneration) + bossDamage;
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth = Math.Max(0f, CurrentHealth - amount);
        }

        public void ApplySlow(float speedMultiplier, float durationSeconds)
        {
            slowFactor = Math.Min(slowFactor, speedMultiplier);
            slowTimeRemaining = Math.Max(slowTimeRemaining, durationSeconds);
        }

        public void ApplyStun(float durationSeconds)
        {
            stunTimeRemaining = Math.Max(stunTimeRemaining, durationSeconds);
        }

        public void ApplyBurn(float damagePerSecond, float durationSeconds)
        {
            dotDamagePerSecond = Math.Max(dotDamagePerSecond, damagePerSecond);
            dotTimeRemaining = Math.Max(dotTimeRemaining, durationSeconds);
        }

        public void ApplyPoison(float damagePerSecondPerStack, float durationSeconds)
        {
            dotDamagePerSecond += damagePerSecondPerStack;
            dotTimeRemaining = Math.Max(dotTimeRemaining, durationSeconds);
        }

        public void ApplyKnockback(float tiles)
        {
            PathProgress = Math.Max(0f, PathProgress - tiles);
        }
        private Dictionary<object, int> knockbackCountsBySource;
        public bool TryApplyKnockback(object source, float tiles, int maxPerSource)
        {
            knockbackCountsBySource ??= new Dictionary<object, int>();
            knockbackCountsBySource.TryGetValue(source, out int used);
            if (used >= maxPerSource)
            {
                return false;
            }

            knockbackCountsBySource[source] = used + 1;
            ApplyKnockback(tiles);
            return true;
        }

        public void ApplyGrounded(float durationSeconds)
        {
            groundedTimeRemaining = Math.Max(groundedTimeRemaining, durationSeconds);
        }

        public void Advance(float deltaSeconds)
        {
            TickStatusEffects(deltaSeconds);

            float effectiveSpeed = IsStunned ? 0f : Speed * slowFactor;
            PathProgress += effectiveSpeed * deltaSeconds;
        }

        private void TickStatusEffects(float deltaSeconds)
        {
            if (dotTimeRemaining > 0f)
            {
                TakeDamage(dotDamagePerSecond * deltaSeconds);
                dotTimeRemaining -= deltaSeconds;

                if (dotTimeRemaining <= 0f)
                {
                    dotTimeRemaining = 0f;
                    dotDamagePerSecond = 0f;
                }
            }

            if (slowTimeRemaining > 0f)
            {
                slowTimeRemaining -= deltaSeconds;

                if (slowTimeRemaining <= 0f)
                {
                    slowTimeRemaining = 0f;
                    slowFactor = 1f;
                }
            }

            if (stunTimeRemaining > 0f)
            {
                stunTimeRemaining = Math.Max(0f, stunTimeRemaining - deltaSeconds);
            }

            if (groundedTimeRemaining > 0f)
            {
                groundedTimeRemaining = Math.Max(0f, groundedTimeRemaining - deltaSeconds);
            }
        }
        public Enemy[] OnDeath(float maxProgress = float.MaxValue)
        {
            if (Color == EnemyColor.Aqua && SplitGeneration < 2)
            {
                const float spread = 0.25f;
                var ahead = new Enemy(EnemyColor.Aqua, SplitGeneration + 1, StageNumber);
                var behind = new Enemy(EnemyColor.Aqua, SplitGeneration + 1, StageNumber);
                ahead.PathProgress = Math.Min(PathProgress + spread, maxProgress - 0.01f);
                behind.PathProgress = Math.Max(0f, PathProgress - spread);
                return new[] { ahead, behind };
            }

            return Array.Empty<Enemy>();
        }
    }
}