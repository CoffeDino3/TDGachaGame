using System;
using System.Collections.Generic;
using System.Linq;

namespace TDGachaGame
{
    public class TowerManager
    {
        private static readonly Dictionary<ShapeColor, (float halfWidth, float halfHeight)> RangeExtentsByColor =
            new Dictionary<ShapeColor, (float, float)>
            {
                [ShapeColor.Yellow] = (1.5f, 1.0f),
                [ShapeColor.Red] = (2.5f, 2.5f),
                [ShapeColor.Purple] = (1.5f, 2.0f),
                [ShapeColor.Cyan] = (2.5f, 1.5f),
                [ShapeColor.Green] = (1.5f, 1.5f),
                [ShapeColor.Black] = (2.0f, 2.0f),
                [ShapeColor.White] = (2.5f, 2.5f),
                [ShapeColor.Blue] = (3.5f, 3.5f),
            };

        private static readonly Dictionary<ShapeColor, (float halfWidth, float halfHeight)> SquareRangeExtentsByColor =
            new Dictionary<ShapeColor, (float, float)>
            {
                [ShapeColor.Yellow] = (2.0f, 2.0f),
                [ShapeColor.Red] = (2.5f, 2.5f),
                [ShapeColor.Purple] = (2.2f, 2.2f),
                [ShapeColor.Cyan] = (2.4f, 2.4f),
                [ShapeColor.Green] = (2.2f, 2.2f),
                [ShapeColor.Black] = (2.3f, 2.3f),
                [ShapeColor.White] = (2.6f, 2.6f),
                [ShapeColor.Blue] = (3.0f, 3.0f),
            };
        public const int MaxCyanKnockbacksPerEnemy = 1;

        private const float TriangleTurnRateDegreesPerSecond = 720f;
        private const float ShotEffectDurationSeconds = 0.18f;

        private const float BombEffectDurationSeconds = 0.3f;

        private const float ChainBlastDamageFalloff = 0.6f;
        private const float EnemyCollisionRadiusTiles = 0.18f;
        private const float AllyCollisionRadiusTiles = 0.3f;
        private const float SparkEffectDurationSeconds = 0.15f;

        private readonly struct PentagonOrbProfile
        {
            public readonly int OrbCount;
            public readonly float OrbRadiusTiles;
            public readonly float SpinDegreesPerSecond;

            public PentagonOrbProfile(int orbCount, float orbRadiusTiles, float spinDegreesPerSecond)
            {
                OrbCount = orbCount;
                OrbRadiusTiles = orbRadiusTiles;
                SpinDegreesPerSecond = spinDegreesPerSecond;
            }
        }
        private static readonly Dictionary<ShapeColor, PentagonOrbProfile> PentagonOrbProfiles =
            new Dictionary<ShapeColor, PentagonOrbProfile>
            {
                [ShapeColor.Yellow] = new PentagonOrbProfile(orbCount: 3, orbRadiusTiles: 0.20f, spinDegreesPerSecond: 160f),
                [ShapeColor.Red] = new PentagonOrbProfile(orbCount: 1, orbRadiusTiles: 0.42f, spinDegreesPerSecond: 110f),
                [ShapeColor.Purple] = new PentagonOrbProfile(orbCount: 2, orbRadiusTiles: 0.27f, spinDegreesPerSecond: 90f),
                [ShapeColor.Cyan] = new PentagonOrbProfile(orbCount: 4, orbRadiusTiles: 0.18f, spinDegreesPerSecond: 190f),
                [ShapeColor.Green] = new PentagonOrbProfile(orbCount: 2, orbRadiusTiles: 0.19f, spinDegreesPerSecond: 130f),
                [ShapeColor.Black] = new PentagonOrbProfile(orbCount: 5, orbRadiusTiles: 0.20f, spinDegreesPerSecond: 140f),
                [ShapeColor.White] = new PentagonOrbProfile(orbCount: 1, orbRadiusTiles: 0.34f, spinDegreesPerSecond: 35f),
                [ShapeColor.Blue] = new PentagonOrbProfile(orbCount: 1, orbRadiusTiles: 0.36f, spinDegreesPerSecond: 100f),
            };

        public static (int orbCount, float orbRadiusTiles, float spinDegreesPerSecond) GetPentagonOrbProfile(ShapeColor color)
        {
            return PentagonOrbProfiles.TryGetValue(color, out var profile)
                ? (profile.OrbCount, profile.OrbRadiusTiles, profile.SpinDegreesPerSecond)
                : (3, 0.20f, 140f);
        }
        private readonly Dictionary<PlacedShape, HashSet<Enemy>> pentagonEnemyTouchStates = new Dictionary<PlacedShape, HashSet<Enemy>>();
        private readonly Dictionary<PlacedShape, HashSet<PlacedShape>> pentagonAllyTouchStates = new Dictionary<PlacedShape, HashSet<PlacedShape>>();

        public class ShotEffect
        {
            public float FromRow;
            public float FromCol;
            public float ToRow;
            public float ToCol;
            public float Age;
            public float Duration;
            public ShapeColor Color;

            public float CorridorHalfWidth;
        }

        public class BombEffect
        {
            public float Row;
            public float Col;
            public float Radius;
            public float Age;
            public float Duration;
            public ShapeColor Color;
        }

        public class SparkEffect
        {
            public float Row;
            public float Col;
            public float Radius;
            public float Age;
            public float Duration;
            public ShapeColor Color;
        }

        public const float SummonSpeedPerAttackSpeed = 1.5f;
        public const float SummonRadiusTiles = 0.22f;

        private static readonly Dictionary<ShapeColor, float> SummonHealthMultipliers = new Dictionary<ShapeColor, float>
        {
            [ShapeColor.Yellow] = 2.0f,
            [ShapeColor.Red] = 1.5f,
            [ShapeColor.Purple] = 5.0f,
            [ShapeColor.Cyan] = 1.5f,
            [ShapeColor.Green] = 5.0f,
            [ShapeColor.Black] = 2.0f,
            [ShapeColor.White] = 8.0f,
            [ShapeColor.Blue] = 2.0f,
        };

        public static float GetSummonHealthMultiplier(ShapeColor color)
        {
            return SummonHealthMultipliers.TryGetValue(color, out var m) ? m : 1f;
        }
        private const float BlackGhostHealthFraction = 0.4f;
        private const float WispAuraRefreshSeconds = 0.6f;
        private const float WispExpiryOverchargeMultiplier = 1.25f;
        private const float WispExpiryOverchargeSeconds = 3f;

        public class Summon
        {
            public PlacedShape Owner;
            public ShapeColor Color;
            public float Progress;
            public float Speed;

            public float MaxHealth;
            public float Health;
            public float Attack;
            public float EffectValue1;
            public float EffectValue2;
            public int SplitsRemaining;
            public float AuraRadius;
            public float AuraSpeedMultiplier;
            public float AuraDamageMultiplier;
            public float Row;
            public float Col;
            public HashSet<Enemy> Touched = new HashSet<Enemy>();
        }

        public List<Summon> ActiveSummons { get; } = new List<Summon>();

        public List<ShotEffect> ActiveShotEffects { get; } = new List<ShotEffect>();
        public List<BombEffect> ActiveBombEffects { get; } = new List<BombEffect>();
        public List<SparkEffect> ActiveSparkEffects { get; } = new List<SparkEffect>();

        public static float GetPentagonOrbitRadius(PlacedShape shape)
        {
            var (_, baseRange, _, _) = TowerUpgradeConfig.GetBaseStats(shape.Type, shape.Color);
            return baseRange * shape.GetRangeMultiplier();
        }
        public static List<(float row, float col)> ComputePentagonOrbPositions(PlacedShape shape, int orbCount, float orbitRadius)
        {
            var positions = new List<(float row, float col)>(orbCount);
            float centerRow = shape.Row + 0.5f;
            float centerCol = shape.Col + 0.5f;
            for (int i = 0; i < orbCount; i++)
            {
                double angle = (shape.FacingDegrees + i * (360.0 / orbCount)) * Math.PI / 180.0;
                float orbRow = centerRow - (float)Math.Cos(angle) * orbitRadius;
                float orbCol = centerCol + (float)Math.Sin(angle) * orbitRadius;
                positions.Add((orbRow, orbCol));
            }
            return positions;
        }

        public static (float halfWidth, float halfHeight) GetTriangleRangeExtents(ShapeColor color)
        {
            return RangeExtentsByColor.TryGetValue(color, out var extents) ? extents : (1.5f, 1.5f);
        }

        public static (float halfWidth, float halfHeight) GetOrientedRangeExtents(ShapeColor color, int rotationDegrees, float rangeMultiplier)
        {
            var (baseHalfWidth, baseHalfHeight) = GetTriangleRangeExtents(color);
            float halfWidth = baseHalfWidth * rangeMultiplier;
            float halfHeight = baseHalfHeight * rangeMultiplier;

            int normalized = ((rotationDegrees % 360) + 360) % 360;
            bool sideways = normalized == 90 || normalized == 270;
            return sideways ? (halfHeight, halfWidth) : (halfWidth, halfHeight);
        }

        public static (float halfWidth, float halfHeight) GetOrientedRangeExtentsForShape(PlacedShape shape)
        {
            return GetOrientedRangeExtents(shape.Color, shape.Rotation, shape.GetRangeMultiplier());
        }

        public static (float halfWidth, float halfHeight) GetSquareRangeExtents(ShapeColor color)
        {
            return SquareRangeExtentsByColor.TryGetValue(color, out var extents) ? extents : (2.0f, 2.0f);
        }

        public static (float halfWidth, float halfHeight) GetOrientedSquareRangeExtents(ShapeColor color, int rotationDegrees, float rangeMultiplier)
        {
            var (baseHalfWidth, baseHalfHeight) = GetSquareRangeExtents(color);
            return (baseHalfWidth * rangeMultiplier, baseHalfHeight * rangeMultiplier);
        }

        public static (float halfWidth, float halfHeight) GetOrientedSquareRangeExtentsForShape(PlacedShape shape)
        {
            return GetOrientedSquareRangeExtents(shape.Color, shape.Rotation, shape.GetRangeMultiplier());
        }

        private static (bool canGround, bool canElevated, bool prioritizeElevated) GetTargetingProfile(ShapeColor color)
        {
            return color switch
            {
                ShapeColor.Blue => (true, true, true),
                ShapeColor.Black => (true, true, false),
                _ => (true, false, false)
            };
        }


        private static bool PrefersUnaffectedTargets(ShapeColor color)
        {
            return color == ShapeColor.Purple || color == ShapeColor.Green;
        }

        private static bool IsAlreadyAffected(ShapeColor color, Enemy enemy)
        {
            return color switch
            {
                ShapeColor.Purple => enemy.IsStunned,
                ShapeColor.Green => enemy.IsAffectedByDot,
                _ => false
            };
        }


        private static float GetBaseCorridorHalfWidth(ShapeColor color)
        {
            return color == ShapeColor.Cyan ? 1.0f : 0.5f;
        }

        public void Update(List<PlacedShape> placedShapes, EnemyManager enemyManager, float deltaSeconds)
        {
            if (enemyManager == null) return;

            PrunePentagonTouchStates(placedShapes);

            foreach (var shape in placedShapes)
            {
                if (shape.Type == ShapeType.Pentagon)
                {
                    TickBuffTimers(shape, deltaSeconds);
                    UpdatePentagonOrbit(shape, placedShapes, enemyManager, deltaSeconds);
                    continue;
                }

                if (shape.Type == ShapeType.Hexagon)
                {
                    UpdateHexagon(shape, enemyManager, deltaSeconds);
                    continue;
                }

                if (shape.Type != ShapeType.Triangle && shape.Type != ShapeType.Square) continue;

                if (shape.Type == ShapeType.Triangle)
                {
                    UpdateTriangleFacing(shape, enemyManager, deltaSeconds);
                }

                if (shape.AttackSpeedBuffTimeRemaining > 0f)
                {
                    shape.AttackSpeedBuffTimeRemaining = Math.Max(0f, shape.AttackSpeedBuffTimeRemaining - deltaSeconds);
                }

                if (shape.AttackDamageBuffTimeRemaining > 0f)
                {
                    shape.AttackDamageBuffTimeRemaining = Math.Max(0f, shape.AttackDamageBuffTimeRemaining - deltaSeconds);
                }

                if (shape.AttackCooldownRemaining > 0f)
                {
                    float speedMultiplier = shape.AttackSpeedBuffTimeRemaining > 0f ? shape.AttackSpeedBuffMultiplier : 1f;
                    shape.AttackCooldownRemaining -= deltaSeconds * speedMultiplier;
                    continue;
                }

                if (shape.Type == ShapeType.Triangle)
                {
                    TryFireTriangle(shape, placedShapes, enemyManager);
                }
                else
                {
                    TryFireSquare(shape, placedShapes, enemyManager);
                }
            }

            UpdateSummons(placedShapes, enemyManager, deltaSeconds);

            for (int i = ActiveShotEffects.Count - 1; i >= 0; i--)
            {
                var effect = ActiveShotEffects[i];
                effect.Age += deltaSeconds;
                if (effect.Age >= effect.Duration)
                {
                    ActiveShotEffects.RemoveAt(i);
                }
            }

            for (int i = ActiveBombEffects.Count - 1; i >= 0; i--)
            {
                var effect = ActiveBombEffects[i];
                effect.Age += deltaSeconds;
                if (effect.Age >= effect.Duration)
                {
                    ActiveBombEffects.RemoveAt(i);
                }
            }

            for (int i = ActiveSparkEffects.Count - 1; i >= 0; i--)
            {
                var effect = ActiveSparkEffects[i];
                effect.Age += deltaSeconds;
                if (effect.Age >= effect.Duration)
                {
                    ActiveSparkEffects.RemoveAt(i);
                }
            }
        }

        private static void TickBuffTimers(PlacedShape shape, float deltaSeconds)
        {
            if (shape.AttackSpeedBuffTimeRemaining > 0f)
            {
                shape.AttackSpeedBuffTimeRemaining = Math.Max(0f, shape.AttackSpeedBuffTimeRemaining - deltaSeconds);
            }

            if (shape.AttackDamageBuffTimeRemaining > 0f)
            {
                shape.AttackDamageBuffTimeRemaining = Math.Max(0f, shape.AttackDamageBuffTimeRemaining - deltaSeconds);
            }
        }
        private void UpdateHexagon(PlacedShape shape, EnemyManager enemyManager, float deltaSeconds)
        {
            TickBuffTimers(shape, deltaSeconds);

            if (shape.AttackCooldownRemaining > 0f)
            {
                shape.AttackCooldownRemaining = Math.Max(0f, shape.AttackCooldownRemaining - deltaSeconds);
                return;
            }

            if (!AnyEnemyWorthSummoningFor(shape, enemyManager)) return;

            SpawnSummon(shape, enemyManager);
            shape.AttackCooldownRemaining = shape.GetCurrentAttackInterval();
        }

        private static bool AnyEnemyWorthSummoningFor(PlacedShape shape, EnemyManager enemyManager)
        {
            var (canGround, canElevated, _) = GetTargetingProfile(shape.Color);

            foreach (var enemy in enemyManager.ActiveEnemies)
            {
                if (enemy.IsDead) continue;
                if (enemy.IsElevated ? canElevated : canGround) return true;
            }

            return false;
        }

        private void SpawnSummon(PlacedShape shape, EnemyManager enemyManager)
        {
            float attack = shape.ConsumeDamageWithBankedCrit();
            float health = attack * GetSummonHealthMultiplier(shape.Color);
            var (startRow, startCol) = enemyManager.GetPathPosition(enemyManager.PathEndProgress);

            var summon = new Summon
            {
                Owner = shape,
                Color = shape.Color,
                Progress = enemyManager.PathEndProgress,
                Speed = shape.GetSummonAttackSpeed() * SummonSpeedPerAttackSpeed,
                MaxHealth = health,
                Health = health,
                Attack = attack,
                Row = startRow,
                Col = startCol
            };

            switch (shape.Color)
            {
                case ShapeColor.Purple:
                case ShapeColor.Cyan:
                    summon.EffectValue1 = shape.GetSpecialValue1();
                    break;
                case ShapeColor.Green:
                    summon.EffectValue1 = shape.GetSpecialValue1();
                    summon.EffectValue2 = shape.GetSpecialValue2();
                    break;
                case ShapeColor.Black:
                    summon.EffectValue1 = shape.GetSpecialValue1();
                    summon.EffectValue2 = shape.GetSpecialValue2();
                    summon.SplitsRemaining = shape.GetExtraBeamCount();
                    break;
                case ShapeColor.White:
                    summon.AuraRadius = shape.GetSpecialValue1();
                    summon.AuraSpeedMultiplier = shape.GetSpecialValue2();
                    summon.AuraDamageMultiplier = shape.GetDamageBuffMultiplier();
                    break;
            }

            ActiveSummons.Add(summon);
            FireSparkEffect(startRow, startCol, 0.3f, shape.Color);
        }

        private void UpdateSummons(List<PlacedShape> placedShapes, EnemyManager enemyManager, float deltaSeconds)
        {
            if (ActiveSummons.Count == 0) return;

            float hitRadius = SummonRadiusTiles + EnemyCollisionRadiusTiles;
            float hitRadiusSq = hitRadius * hitRadius;

            for (int i = ActiveSummons.Count - 1; i >= 0; i--)
            {
                var summon = ActiveSummons[i];
                summon.Progress -= summon.Speed * deltaSeconds;
                if (summon.Progress <= 0f)
                {
                    ActiveSummons.RemoveAt(i);
                    continue;
                }

                var (row, col) = enemyManager.GetPathPosition(summon.Progress);
                summon.Row = row;
                summon.Col = col;

                if (summon.Color == ShapeColor.White)
                {
                    ApplyWispAura(summon, placedShapes);
                }

                var (canGround, canElevated, _) = GetTargetingProfile(summon.Color);

                foreach (var enemy in enemyManager.ActiveEnemies)
                {
                    if (enemy.IsDead || summon.Touched.Contains(enemy)) continue;
                    if (enemy.IsElevated ? !canElevated : !canGround) continue;

                    var (er, ec) = enemyManager.GetInterpolatedPosition(enemy);
                    float dr = er - summon.Row;
                    float dc = ec - summon.Col;
                    if (dr * dr + dc * dc > hitRadiusSq) continue;

                    summon.Touched.Add(enemy);
                    float summonHp = summon.Health;
                    float enemyHp = enemy.CurrentHealth;

                    ApplySummonContactEffect(summon, enemy, summonHp);
                    summon.Health -= enemyHp;
                    FireSparkEffect(er, ec, 0.22f, summon.Color);

                    if (summon.Health <= 0f) break;
                }

                if (summon.Health <= 0f)
                {
                    ActiveSummons.RemoveAt(i);
                    OnSummonDestroyed(summon, placedShapes);
                }
            }
        }

        private static void ApplySummonContactEffect(Summon summon, Enemy enemy, float collisionDamage)
        {
            switch (summon.Color)
            {
                case ShapeColor.Black:
                    enemy.TakeDamage(collisionDamage);
                    enemy.ApplySlow(summon.EffectValue1, summon.EffectValue2);
                    break;
                case ShapeColor.Purple:
                    enemy.ApplyStun(summon.EffectValue1);
                    break;
                case ShapeColor.Cyan:
                    enemy.TakeDamage(collisionDamage);
                    enemy.TryApplyKnockback(summon.Owner, summon.EffectValue1, MaxCyanKnockbacksPerEnemy);
                    break;
                case ShapeColor.Green:
                    enemy.ApplyPoison(summon.EffectValue1, summon.EffectValue2);
                    break;
                case ShapeColor.White:
                    break;
                default:
                    enemy.TakeDamage(collisionDamage);
                    break;
            }
        }

        private static void ApplyWispAura(Summon wisp, List<PlacedShape> placedShapes)
        {
            float radiusSq = wisp.AuraRadius * wisp.AuraRadius;

            foreach (var ally in placedShapes)
            {
                if (ReferenceEquals(ally, wisp.Owner) || ally.Type == ShapeType.Line) continue;

                float dr = ally.Row + 0.5f - wisp.Row;
                float dc = ally.Col + 0.5f - wisp.Col;
                if (dr * dr + dc * dc > radiusSq) continue;

                ally.ApplyAttackSpeedBuff(wisp.AuraSpeedMultiplier, WispAuraRefreshSeconds);
                if (wisp.AuraDamageMultiplier > 0f)
                {
                    ally.ApplyAttackDamageBuff(wisp.AuraDamageMultiplier, WispAuraRefreshSeconds);
                }
            }
        }

        private void OnSummonDestroyed(Summon summon, List<PlacedShape> placedShapes)
        {
            if (summon.Color == ShapeColor.Black && summon.SplitsRemaining > 0)
            {
                ActiveSummons.Add(new Summon
                {
                    Owner = summon.Owner,
                    Color = summon.Color,
                    Progress = summon.Progress,
                    Speed = summon.Speed,
                    MaxHealth = summon.MaxHealth * BlackGhostHealthFraction,
                    Health = summon.MaxHealth * BlackGhostHealthFraction,
                    Attack = summon.Attack * BlackGhostHealthFraction,
                    EffectValue1 = summon.EffectValue1,
                    EffectValue2 = summon.EffectValue2,
                    SplitsRemaining = summon.SplitsRemaining - 1,
                    Row = summon.Row,
                    Col = summon.Col,
                    Touched = new HashSet<Enemy>(summon.Touched)
                });
                FireSparkEffect(summon.Row, summon.Col, 0.3f, summon.Color);
            }
            else if (summon.Color == ShapeColor.White)
            {
                float radiusSq = summon.AuraRadius * summon.AuraRadius;
                foreach (var ally in placedShapes)
                {
                    if (ReferenceEquals(ally, summon.Owner) || ally.Type == ShapeType.Line) continue;

                    float dr = ally.Row + 0.5f - summon.Row;
                    float dc = ally.Col + 0.5f - summon.Col;
                    if (dr * dr + dc * dc > radiusSq) continue;

                    ally.AttackCooldownRemaining = 0f;
                    ally.ApplyAttackDamageBuff(WispExpiryOverchargeMultiplier, WispExpiryOverchargeSeconds);
                    FireSparkEffect(ally.Row + 0.5f, ally.Col + 0.5f, 0.35f, ShapeColor.White);
                }
            }
        }

        private void UpdateTriangleFacing(PlacedShape shape, EnemyManager enemyManager, float deltaSeconds)
        {
            if (shape.Color == ShapeColor.White)
            {
                shape.FacingDegrees = shape.Rotation;
                return;
            }

            var (halfWidth, halfHeight) = GetOrientedRangeExtentsForShape(shape);
            var (canGround, canElevated, prioritizeElevated) = GetTargetingProfile(shape.Color);

            Enemy target = FindPriorityEnemyInRange(shape, enemyManager, halfWidth, halfHeight, canGround, canElevated, prioritizeElevated, null);
            if (target == null) return;

            float? desiredDegrees = GetDesiredFacingDegrees(shape, enemyManager, target);
            if (desiredDegrees.HasValue)
            {
                shape.FacingDegrees = RotateTowards(shape.FacingDegrees, desiredDegrees.Value, TriangleTurnRateDegreesPerSecond * deltaSeconds);
            }
        }

        private static float? GetDesiredFacingDegrees(PlacedShape shape, EnemyManager enemyManager, Enemy target)
        {
            var (er, ec) = enemyManager.GetInterpolatedPosition(target);
            float dr = er - (shape.Row + 0.5f);
            float dc = ec - (shape.Col + 0.5f);

            if (Math.Abs(dr) < 0.0001f && Math.Abs(dc) < 0.0001f) return null;
            return (float)(Math.Atan2(dc, -dr) * 180.0 / Math.PI);
        }

        private static float RotateTowards(float currentDegrees, float targetDegrees, float maxDeltaDegrees)
        {
            float diff = NormalizeAngle(targetDegrees - currentDegrees);
            if (Math.Abs(diff) <= maxDeltaDegrees) return NormalizeAngle(targetDegrees);
            return NormalizeAngle(currentDegrees + Math.Sign(diff) * maxDeltaDegrees);
        }

        private static float NormalizeAngle(float degrees)
        {
            degrees %= 360f;
            if (degrees > 180f) degrees -= 360f;
            else if (degrees < -180f) degrees += 360f;
            return degrees;
        }


        private Enemy FindPriorityEnemyInRange(PlacedShape shape, EnemyManager enemyManager,
            float halfWidth, float halfHeight, bool canGround, bool canElevated, bool prioritizeElevated,
            HashSet<Enemy> excludeSet)
        {
            float towerRow = shape.Row + 0.5f;
            float towerCol = shape.Col + 0.5f;

            List<Enemy> validEnemies = new List<Enemy>();

            foreach (var enemy in enemyManager.ActiveEnemies)
            {
                if (excludeSet != null && excludeSet.Contains(enemy)) continue;

                bool elevated = enemy.IsElevated;
                if (elevated && !canElevated) continue;
                if (!elevated && !canGround) continue;

                var (er, ec) = enemyManager.GetInterpolatedPosition(enemy);
                float dr = er - towerRow;
                float dc = ec - towerCol;

                if (!IsInRectRange(dr, dc, halfWidth, halfHeight)) continue;
                validEnemies.Add(enemy);
            }

            if (validEnemies.Count == 0) return null;

            if (prioritizeElevated)
            {
                var elevatedEnemies = validEnemies.Where(e => e.IsElevated).ToList();
                if (elevatedEnemies.Count > 0) validEnemies = elevatedEnemies;
            }

            if (PrefersUnaffectedTargets(shape.Color))
            {
                var unaffectedEnemies = validEnemies.Where(e => !IsAlreadyAffected(shape.Color, e)).ToList();
                if (unaffectedEnemies.Count > 0) validEnemies = unaffectedEnemies;
            }

            switch (shape.TargetingMode)
            {
                case TargetingMode.First:
                    return validEnemies.OrderByDescending(e => e.PathProgress).First();
                case TargetingMode.Last:
                    return validEnemies.OrderBy(e => e.PathProgress).First();
                case TargetingMode.Closest:
                    return validEnemies.OrderBy(e =>
                    {
                        var (er, ec) = enemyManager.GetInterpolatedPosition(e);
                        float dr = er - towerRow;
                        float dc = ec - towerCol;
                        return dr * dr + dc * dc;
                    }).First();
                case TargetingMode.Farthest:
                    return validEnemies.OrderByDescending(e =>
                    {
                        var (er, ec) = enemyManager.GetInterpolatedPosition(e);
                        float dr = er - towerRow;
                        float dc = ec - towerCol;
                        return dr * dr + dc * dc;
                    }).First();
                case TargetingMode.LowestHp:
                    return validEnemies.OrderBy(e => e.CurrentHealth / e.MaxHealth).First();
                case TargetingMode.HighestHp:
                    return validEnemies.OrderByDescending(e => e.CurrentHealth / e.MaxHealth).First();
                default:
                    return validEnemies.OrderByDescending(e => e.PathProgress).First();
            }
        }

        private void TryFireTriangle(PlacedShape shape, List<PlacedShape> placedShapes, EnemyManager enemyManager)
        {
            var (halfWidth, halfHeight) = GetOrientedRangeExtentsForShape(shape);

            if (shape.Color == ShapeColor.White)
            {
                if (BuffAlliesInRange(shape, placedShapes, halfWidth, halfHeight))
                {
                    shape.AttackCooldownRemaining = shape.GetCurrentAttackInterval();
                }
                return;
            }

            var (canGround, canElevated, prioritizeElevated) = GetTargetingProfile(shape.Color);

            Enemy primary = FindPriorityEnemyInRange(shape, enemyManager, halfWidth, halfHeight, canGround, canElevated, prioritizeElevated, null);
            if (primary == null) return;

            var targets = FindEnemiesAlongLineToward(shape, enemyManager, primary, halfWidth, halfHeight, canGround, canElevated);
            ApplyTriangleEffect(shape, targets);
            FireShotEffect(shape, enemyManager, primary, halfWidth, halfHeight);


            var alreadyHit = new HashSet<Enemy>(targets);
            int extraBeams = shape.GetExtraBeamCount();
            for (int beam = 0; beam < extraBeams; beam++)
            {
                Enemy nextTarget = FindPriorityEnemyInRange(shape, enemyManager, halfWidth, halfHeight, canGround, canElevated, prioritizeElevated, alreadyHit);
                if (nextTarget == null) break;

                var nextTargets = FindEnemiesAlongLineToward(shape, enemyManager, nextTarget, halfWidth, halfHeight, canGround, canElevated);
                ApplyTriangleEffect(shape, nextTargets);
                FireShotEffect(shape, enemyManager, nextTarget, halfWidth, halfHeight);
                alreadyHit.UnionWith(nextTargets);
            }

            shape.AttackCooldownRemaining = shape.GetCurrentAttackInterval();
        }

        private void FireShotEffect(PlacedShape shape, EnemyManager enemyManager, Enemy target, float halfWidth, float halfHeight)
        {
            float towerRow = shape.Row + 0.5f;
            float towerCol = shape.Col + 0.5f;
            var (targetRow, targetCol) = enemyManager.GetInterpolatedPosition(target);
            float dirRow = targetRow - towerRow;
            float dirCol = targetCol - towerCol;
            float length = (float)Math.Sqrt(dirRow * dirRow + dirCol * dirCol);

            if (length > 0.0001f)
            {
                dirRow /= length;
                dirCol /= length;
                shape.FacingDegrees = (float)(Math.Atan2(dirCol, -dirRow) * 180.0 / Math.PI);
            }

            float reach = GetReachAlongDirection(dirRow, dirCol, halfWidth, halfHeight);
            float corridorHalfWidth = GetBaseCorridorHalfWidth(shape.Color) + shape.GetCorridorWidthBonus();

            ActiveShotEffects.Add(new ShotEffect
            {
                FromRow = towerRow,
                FromCol = towerCol,
                ToRow = towerRow + dirRow * reach,
                ToCol = towerCol + dirCol * reach,
                Age = 0f,
                Duration = ShotEffectDurationSeconds,
                Color = shape.Color,
                CorridorHalfWidth = corridorHalfWidth
            });
        }

        private static float GetReachAlongDirection(float dirRow, float dirCol, float halfWidth, float halfHeight)
        {
            float reach = float.PositiveInfinity;
            if (Math.Abs(dirCol) > 0.0001f) reach = Math.Min(reach, halfWidth / Math.Abs(dirCol));
            if (Math.Abs(dirRow) > 0.0001f) reach = Math.Min(reach, halfHeight / Math.Abs(dirRow));
            return float.IsPositiveInfinity(reach) ? halfWidth : reach;
        }

        private void ApplyTriangleEffect(PlacedShape shape, List<Enemy> targets)
        {
            if (targets.Count == 0) return;
            float damage = shape.ConsumeDamageWithBankedCrit();

            switch (shape.Color)
            {
                case ShapeColor.Black:
                    {
                        float slowStrength = shape.GetSpecialValue1();
                        float slowDuration = shape.GetSpecialValue2();
                        foreach (var enemy in targets)
                        {
                            enemy.TakeDamage(damage);
                            enemy.ApplySlow(slowStrength, slowDuration);
                        }
                        break;
                    }
                case ShapeColor.Red:
                    targets[0].TakeDamage(damage);
                    break;
                case ShapeColor.Purple:
                    targets[0].ApplyStun(shape.GetSpecialValue1());
                    break;
                case ShapeColor.Cyan:
                    foreach (var enemy in targets)
                    {
                        enemy.TakeDamage(damage);
                    }
                    break;
                case ShapeColor.Green:
                    {
                        float poisonDps = shape.GetSpecialValue1();
                        float poisonDuration = shape.GetSpecialValue2();
                        foreach (var enemy in targets)
                        {
                            enemy.ApplyPoison(poisonDps, poisonDuration);
                        }
                        break;
                    }
                case ShapeColor.Blue:
                    targets[0].TakeDamage(damage);
                    break;
                case ShapeColor.Yellow:
                    targets[0].TakeDamage(damage);
                    break;
            }
        }

        private bool BuffAlliesInRange(PlacedShape source, List<PlacedShape> placedShapes, float halfWidth, float halfHeight)
        {
            float towerRow = source.Row + 0.5f;
            float towerCol = source.Col + 0.5f;
            bool buffedAny = false;

            float buffDuration = source.GetSpecialValue1();
            float buffMultiplier = source.GetSpecialValue2();
            float damageBuffMultiplier = source.GetDamageBuffMultiplier();

            foreach (var ally in placedShapes)
            {
                if (ReferenceEquals(ally, source) || ally.Type == ShapeType.Line) continue;

                float dr = ally.Row + 0.5f - towerRow;
                float dc = ally.Col + 0.5f - towerCol;

                if (!IsInRectRange(dr, dc, halfWidth, halfHeight)) continue;

                ally.ApplyAttackSpeedBuff(buffMultiplier, buffDuration);
                if (damageBuffMultiplier > 0f)
                {
                    ally.ApplyAttackDamageBuff(damageBuffMultiplier, buffDuration);
                }
                buffedAny = true;
            }

            return buffedAny;
        }


        private void TryFireSquare(PlacedShape shape, List<PlacedShape> placedShapes, EnemyManager enemyManager)
        {
            var (halfWidth, halfHeight) = GetOrientedSquareRangeExtentsForShape(shape);

            if (shape.Color == ShapeColor.White)
            {
                if (OverchargeAndBuffAlliesInRange(shape, placedShapes, halfWidth, halfHeight))
                {
                    shape.AttackCooldownRemaining = shape.GetCurrentAttackInterval();
                }
                return;
            }

            var (canGround, canElevated, prioritizeElevated) = GetTargetingProfile(shape.Color);

            Enemy primary = FindPriorityEnemyInRange(shape, enemyManager, halfWidth, halfHeight, canGround, canElevated, prioritizeElevated, null);
            if (primary == null) return;

            float blastRadius = shape.GetCurrentBlastRadius();
            var (targetRow, targetCol) = enemyManager.GetInterpolatedPosition(primary);
            var hitEnemies = FindEnemiesInBlast(enemyManager, targetRow, targetCol, blastRadius, canGround, canElevated);
            ApplySquareEffect(shape, hitEnemies);
            FireBombEffect(shape, targetRow, targetCol, blastRadius);

            if (shape.Color == ShapeColor.Black)
            {
                var alreadyHit = new HashSet<Enemy>(hitEnemies);
                int chainCount = shape.GetExtraBeamCount();
                float falloff = 1f;

                for (int chain = 0; chain < chainCount; chain++)
                {
                    Enemy nextTarget = FindPriorityEnemyInRange(shape, enemyManager, halfWidth, halfHeight, canGround, canElevated, prioritizeElevated, alreadyHit);
                    if (nextTarget == null) break;

                    falloff *= ChainBlastDamageFalloff;
                    var (chainRow, chainCol) = enemyManager.GetInterpolatedPosition(nextTarget);
                    var chainHits = FindEnemiesInBlast(enemyManager, chainRow, chainCol, blastRadius, canGround, canElevated);
                    ApplySquareEffect(shape, chainHits, falloff);
                    FireBombEffect(shape, chainRow, chainCol, blastRadius);
                    alreadyHit.UnionWith(chainHits);
                }
            }

            shape.AttackCooldownRemaining = shape.GetCurrentAttackInterval();
        }

        private List<Enemy> FindEnemiesInBlast(EnemyManager enemyManager, float centerRow, float centerCol, float radius, bool canGround, bool canElevated)
        {
            var hits = new List<Enemy>();

            foreach (var enemy in enemyManager.ActiveEnemies)
            {
                bool elevated = enemy.IsElevated;
                if (elevated && !canElevated) continue;
                if (!elevated && !canGround) continue;

                var (er, ec) = enemyManager.GetInterpolatedPosition(enemy);
                float dr = er - centerRow;
                float dc = ec - centerCol;
                if (dr * dr + dc * dc <= radius * radius)
                {
                    hits.Add(enemy);
                }
            }

            return hits;
        }

        private void FireBombEffect(PlacedShape shape, float row, float col, float radius)
        {
            ActiveBombEffects.Add(new BombEffect
            {
                Row = row,
                Col = col,
                Radius = radius,
                Age = 0f,
                Duration = BombEffectDurationSeconds,
                Color = shape.Color
            });
        }


        private void ApplySquareEffect(PlacedShape shape, List<Enemy> targets, float damageFalloff = 1f)
        {
            if (targets.Count == 0) return;
            float damage = shape.ConsumeDamageWithBankedCrit() * damageFalloff;

            switch (shape.Color)
            {
                case ShapeColor.Black:
                    {
                        float slowStrength = shape.GetSpecialValue1();
                        float slowDuration = shape.GetSpecialValue2();
                        foreach (var enemy in targets)
                        {
                            enemy.TakeDamage(damage);
                            enemy.ApplySlow(slowStrength, slowDuration);
                        }
                        break;
                    }
                case ShapeColor.Red:
                    foreach (var enemy in targets) enemy.TakeDamage(damage);
                    break;
                case ShapeColor.Purple:
                    {
                        float stunDuration = shape.GetSpecialValue1();
                        foreach (var enemy in targets)
                        {
                            enemy.TakeDamage(damage);
                            enemy.ApplyStun(stunDuration);
                        }
                        break;
                    }
                case ShapeColor.Cyan:
                    {
                        float knockbackTiles = shape.GetSpecialValue1();
                        foreach (var enemy in targets)
                        {
                            enemy.TakeDamage(damage);
                            enemy.TryApplyKnockback(shape, knockbackTiles, MaxCyanKnockbacksPerEnemy);
                        }
                        break;
                    }
                case ShapeColor.Green:
                    {
                        float poisonDps = shape.GetSpecialValue1();
                        float poisonDuration = shape.GetSpecialValue2();
                        foreach (var enemy in targets)
                        {
                            enemy.ApplyPoison(poisonDps, poisonDuration);
                        }
                        break;
                    }
                case ShapeColor.Blue:
                    foreach (var enemy in targets) enemy.TakeDamage(damage);
                    break;
                case ShapeColor.Yellow:
                    foreach (var enemy in targets) enemy.TakeDamage(damage);
                    break;
            }
        }


        private bool OverchargeAndBuffAlliesInRange(PlacedShape source, List<PlacedShape> placedShapes, float halfWidth, float halfHeight)
        {
            float towerRow = source.Row + 0.5f;
            float towerCol = source.Col + 0.5f;
            bool buffedAny = false;


            float damageBuffDuration = source.GetSpecialValue1();
            float damageBuffMultiplier = source.GetSpecialValue2();

            foreach (var ally in placedShapes)
            {
                if (ReferenceEquals(ally, source) || ally.Type == ShapeType.Line) continue;

                float dr = ally.Row + 0.5f - towerRow;
                float dc = ally.Col + 0.5f - towerCol;

                if (!IsInRectRange(dr, dc, halfWidth, halfHeight)) continue;

                ally.AttackCooldownRemaining = 0f;
                ally.ApplyAttackDamageBuff(damageBuffMultiplier, damageBuffDuration);
                buffedAny = true;
            }

            return buffedAny;
        }

        private void PrunePentagonTouchStates(List<PlacedShape> placedShapes)
        {
            if (pentagonEnemyTouchStates.Count == 0 && pentagonAllyTouchStates.Count == 0) return;

            var alive = new HashSet<PlacedShape>(placedShapes);
            PruneStaleKeys(pentagonEnemyTouchStates, alive);
            PruneStaleKeys(pentagonAllyTouchStates, alive);
        }

        private static void PruneStaleKeys<TValue>(Dictionary<PlacedShape, TValue> dict, HashSet<PlacedShape> alive)
        {
            List<PlacedShape> stale = null;
            foreach (var key in dict.Keys)
            {
                if (!alive.Contains(key))
                {
                    (stale ??= new List<PlacedShape>()).Add(key);
                }
            }
            if (stale != null)
            {
                foreach (var key in stale) dict.Remove(key);
            }
        }

        private static bool IsTouchingAnyOrb(List<(float row, float col)> orbPositions, float targetRow, float targetCol, float orbRadius, float targetRadius)
        {
            float combined = orbRadius + targetRadius;
            float combinedSq = combined * combined;
            foreach (var (orbRow, orbCol) in orbPositions)
            {
                float dr = targetRow - orbRow;
                float dc = targetCol - orbCol;
                if (dr * dr + dc * dc <= combinedSq) return true;
            }
            return false;
        }

        private void UpdatePentagonOrbit(PlacedShape shape, List<PlacedShape> placedShapes, EnemyManager enemyManager, float deltaSeconds)
        {
            var (orbCount, orbRadiusTiles, spinSpeed) = GetPentagonOrbProfile(shape.Color);

            shape.FacingDegrees = (shape.FacingDegrees + spinSpeed * deltaSeconds) % 360f;

            float orbitRadius = GetPentagonOrbitRadius(shape);
            var orbPositions = ComputePentagonOrbPositions(shape, orbCount, orbitRadius);

            if (!pentagonEnemyTouchStates.TryGetValue(shape, out var previousEnemyTouching))
            {
                previousEnemyTouching = new HashSet<Enemy>();
            }
            if (!pentagonAllyTouchStates.TryGetValue(shape, out var previousAllyTouching))
            {
                previousAllyTouching = new HashSet<PlacedShape>();
            }

            if (shape.Color == ShapeColor.White)
            {
                var currentAllyTouching = new HashSet<PlacedShape>();
                float critMultiplier = shape.GetSpecialValue1();

                foreach (var ally in placedShapes)
                {
                    if (ReferenceEquals(ally, shape) || ally.Type == ShapeType.Line) continue;

                    float allyRow = ally.Row + 0.5f;
                    float allyCol = ally.Col + 0.5f;
                    if (!IsTouchingAnyOrb(orbPositions, allyRow, allyCol, orbRadiusTiles, AllyCollisionRadiusTiles)) continue;

                    currentAllyTouching.Add(ally);
                    if (!previousAllyTouching.Contains(ally))
                    {
                        ally.ApplyBankedCrit(critMultiplier);
                        FireSparkEffect(allyRow, allyCol, orbRadiusTiles, shape.Color);
                    }
                }

                pentagonAllyTouchStates[shape] = currentAllyTouching;
                pentagonEnemyTouchStates[shape] = previousEnemyTouching;
                return;
            }

            var (canGround, canElevated, _) = GetTargetingProfile(shape.Color);
            var currentEnemyTouching = new HashSet<Enemy>();

            foreach (var enemy in enemyManager.ActiveEnemies)
            {
                bool elevated = enemy.IsElevated;
                if (elevated && !canElevated) continue;
                if (!elevated && !canGround) continue;

                var (er, ec) = enemyManager.GetInterpolatedPosition(enemy);
                if (!IsTouchingAnyOrb(orbPositions, er, ec, orbRadiusTiles, EnemyCollisionRadiusTiles)) continue;

                currentEnemyTouching.Add(enemy);
                if (!previousEnemyTouching.Contains(enemy))
                {
                    ApplyPentagonContactEffect(shape, enemy);
                    FireSparkEffect(er, ec, orbRadiusTiles, shape.Color);
                }
            }

            pentagonEnemyTouchStates[shape] = currentEnemyTouching;
            pentagonAllyTouchStates[shape] = previousAllyTouching;
        }

        private void ApplyPentagonContactEffect(PlacedShape shape, Enemy enemy)
        {
            switch (shape.Color)
            {
                case ShapeColor.Black:
                    {
                        float slowStrength = shape.GetSpecialValue1();
                        float slowDuration = shape.GetSpecialValue2();
                        enemy.TakeDamage(shape.ConsumeDamageWithBankedCrit());
                        enemy.ApplySlow(slowStrength, slowDuration);
                        break;
                    }
                case ShapeColor.Red:
                    enemy.TakeDamage(shape.ConsumeDamageWithBankedCrit());
                    break;
                case ShapeColor.Purple:
                    enemy.ApplyStun(shape.GetSpecialValue1());
                    break;
                case ShapeColor.Cyan:
                    enemy.TakeDamage(shape.ConsumeDamageWithBankedCrit());
                    enemy.TryApplyKnockback(shape, shape.GetSpecialValue1(), MaxCyanKnockbacksPerEnemy);
                    break;
                case ShapeColor.Green:
                    enemy.ApplyPoison(shape.GetSpecialValue1(), shape.GetSpecialValue2());
                    break;
                case ShapeColor.Blue:
                    enemy.TakeDamage(shape.ConsumeDamageWithBankedCrit());
                    break;
                case ShapeColor.Yellow:
                    enemy.TakeDamage(shape.ConsumeDamageWithBankedCrit());
                    break;
            }
        }

        private void FireSparkEffect(float row, float col, float radius, ShapeColor color)
        {
            ActiveSparkEffects.Add(new SparkEffect
            {
                Row = row,
                Col = col,
                Radius = radius,
                Age = 0f,
                Duration = SparkEffectDurationSeconds,
                Color = color
            });
        }

        private List<Enemy> FindEnemiesAlongLineToward(PlacedShape shape, EnemyManager enemyManager, Enemy target,
            float halfWidth, float halfHeight, bool canGround, bool canElevated)
        {
            float towerRow = shape.Row + 0.5f;
            float towerCol = shape.Col + 0.5f;

            var hits = new List<Enemy> { target };

            var (targetRow, targetCol) = enemyManager.GetInterpolatedPosition(target);
            float dirRow = targetRow - towerRow;
            float dirCol = targetCol - towerCol;
            float length = (float)Math.Sqrt(dirRow * dirRow + dirCol * dirCol);

            if (length < 0.0001f) return hits;

            dirRow /= length;
            dirCol /= length;
            float perpRow = -dirCol;
            float perpCol = dirRow;
            float corridorHalfWidth = GetBaseCorridorHalfWidth(shape.Color) + shape.GetCorridorWidthBonus();

            foreach (var enemy in enemyManager.ActiveEnemies)
            {
                if (ReferenceEquals(enemy, target)) continue;

                bool elevated = enemy.IsElevated;
                if (elevated && !canElevated) continue;
                if (!elevated && !canGround) continue;

                var (er, ec) = enemyManager.GetInterpolatedPosition(enemy);
                float dr = er - towerRow;
                float dc = ec - towerCol;

                if (!IsInRectRange(dr, dc, halfWidth, halfHeight)) continue;

                float forward = dr * dirRow + dc * dirCol;
                if (forward < 0f) continue;

                float lateral = dr * perpRow + dc * perpCol;
                if (Math.Abs(lateral) > corridorHalfWidth) continue;

                hits.Add(enemy);
            }

            return hits;
        }

        public static bool IsInRectRange(float dr, float dc, float halfWidth, float halfHeight)
        {
            return Math.Abs(dc) <= halfWidth && Math.Abs(dr) <= halfHeight;
        }
    }
}