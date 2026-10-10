using System;
using System.Collections.Generic;

namespace TDGachaGame
{
    public class EnemyManager
    {
        private enum WaveState
        {
            Spawning,
            InterWaveDelay,
            AllWavesComplete
        }

        public int BaseHealth
        {
            get;
            private set;
        }

        public List<Enemy> ActiveEnemies
        {
            get;
        } = new List<Enemy>();

        public int CurrentWaveNumber
        {
            get;
            private set;
        }

        public int TotalWaves
        {
            get;
        }

        public float ElapsedSeconds
        {
            get;
            private set;
        }

        public bool IsBetweenWaves => state == WaveState.InterWaveDelay;

        public float TimeUntilNextWave => IsBetweenWaves
            ? Math.Max(0f, WaveGapSeconds - interWaveTimer)
            : 0f;
        public bool AwaitingEarlyWaveCall
        {
            get;
            private set;
        }

        private readonly List<List<Enemy>> waves;
        private int waveIndex;
        private Queue<Enemy> spawnQueue = new Queue<Enemy>();
        private float spawnTimer;
        private float interWaveTimer;
        private float waveElapsedSeconds;
        private bool earlyCallOffered;
        private WaveState state;

        private const float SpawnInterval = 0.8f;
        private const float WaveGapSeconds = 3f;
        private const float EarlyCallThresholdSeconds = 30f;
        public const float LineTrapFlatDamage = 15f;
        public const float TrapEffectScale = 1.0f;

        private static readonly Dictionary<ShapeColor, float> TrapColorScaleOverrides = new Dictionary<ShapeColor, float>
        {
            [ShapeColor.Red] = 1.15f,
            [ShapeColor.Cyan] = 1.35f,
            [ShapeColor.Yellow] = 1.5f,
            [ShapeColor.Black] = 1.0f,
            [ShapeColor.White] = 1.0f,
            [ShapeColor.Green] = 1.0f,
            [ShapeColor.Blue] = 1.0f,
            [ShapeColor.Purple] = 1.0f,
        };

        public static float GetCombinedTrapScale(ShapeColor color)
        {
            float overrideScale = TrapColorScaleOverrides.TryGetValue(color, out var v) ? v : 1f;
            return TrapEffectScale * overrideScale;
        }

        private readonly List<(int r, int c)> waypoints;
        private int pendingCashReward;

        public bool FloorCleared => state == WaveState.AllWavesComplete;
        public bool BaseDestroyed => BaseHealth <= 0;

        public EnemyManager(FloorMap floor, List<List<Enemy>> enemyWaves, int baseHealth = 20)
        {
            waypoints = floor.PathWaypoints;
            BaseHealth = baseHealth;

            waves = enemyWaves;
            TotalWaves = Math.Max(1, waves.Count);

            waveIndex = 0;
            CurrentWaveNumber = 1;
            LoadWave(waveIndex);
        }

        private void LoadWave(int index)
        {
            spawnQueue = new Queue<Enemy>();

            if (index < waves.Count)
            {
                foreach (var enemy in waves[index])
                {
                    spawnQueue.Enqueue(enemy);
                }
            }

            CurrentWaveNumber = index + 1;
            spawnTimer = 0f;
            waveElapsedSeconds = 0f;
            earlyCallOffered = false;
            AwaitingEarlyWaveCall = false;
            state = WaveState.Spawning;
        }

        public void ConfirmEarlyWaveCall()
        {
            if (!AwaitingEarlyWaveCall || waveIndex + 1 >= waves.Count)
            {
                return;
            }

            waveIndex++;

            foreach (var enemy in waves[waveIndex])
            {
                spawnQueue.Enqueue(enemy);
            }

            CurrentWaveNumber = waveIndex + 1;
            waveElapsedSeconds = 0f;
            earlyCallOffered = false;
            AwaitingEarlyWaveCall = false;
        }
        public void DeclineEarlyWaveCall()
        {
            AwaitingEarlyWaveCall = false;
        }

        public void Update(float deltaSeconds)
        {
            if (!BaseDestroyed && !FloorCleared)
            {
                ElapsedSeconds += deltaSeconds;
            }

            UpdateSpawning(deltaSeconds);
            UpdateActiveEnemies(deltaSeconds);
            UpdateWaveTransition(deltaSeconds);
            UpdateEarlyWaveCallPrompt(deltaSeconds);
        }

        private void UpdateEarlyWaveCallPrompt(float deltaSeconds)
        {
            if (state != WaveState.Spawning)
            {
                return;
            }

            if (AwaitingEarlyWaveCall || earlyCallOffered)
            {
                return;
            }

            if (waveIndex + 1 >= waves.Count)
            {
                return;
            }

            waveElapsedSeconds += deltaSeconds;

            if (waveElapsedSeconds >= EarlyCallThresholdSeconds)
            {
                AwaitingEarlyWaveCall = true;
                earlyCallOffered = true;
            }
        }

        private void UpdateWaveTransition(float deltaSeconds)
        {
            if (state != WaveState.Spawning)
            {
                if (state == WaveState.InterWaveDelay)
                {
                    interWaveTimer += deltaSeconds;

                    if (interWaveTimer >= WaveGapSeconds)
                    {
                        interWaveTimer = 0f;
                        waveIndex++;
                        LoadWave(waveIndex);
                    }
                }

                return;
            }

            bool waveDrained = spawnQueue.Count == 0 && ActiveEnemies.Count == 0;

            if (!waveDrained)
            {
                return;
            }

            if (waveIndex + 1 < waves.Count)
            {
                state = WaveState.InterWaveDelay;
                interWaveTimer = 0f;
            }
            else
            {
                state = WaveState.AllWavesComplete;
            }

            pendingCashReward += GetWaveClearReward();
        }

        private void UpdateSpawning(float deltaSeconds)
        {
            if (spawnQueue.Count == 0)
            {
                return;
            }

            spawnTimer += deltaSeconds;

            if (spawnTimer >= SpawnInterval)
            {
                spawnTimer = 0f;
                ActiveEnemies.Add(spawnQueue.Dequeue());
            }
        }

        private void UpdateActiveEnemies(float deltaSeconds)
        {
            for (int i = ActiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = ActiveEnemies[i];

                if (enemy.IsDead)
                {
                    pendingCashReward += GetKillReward(enemy);
                    ActiveEnemies.RemoveAt(i);
                    ActiveEnemies.AddRange(enemy.OnDeath(waypoints.Count - 1));
                    continue;
                }

                enemy.Advance(deltaSeconds);

                if (enemy.PathProgress >= waypoints.Count - 1)
                {
                    BaseHealth -= enemy.DamageToExit;
                    ActiveEnemies.RemoveAt(i);
                }
            }
        }
        public float PathEndProgress => waypoints.Count - 1;
        public (float r, float c) GetPathPosition(float progress)
        {
            int index = (int)progress;
            index = System.Math.Max(0, System.Math.Min(index, waypoints.Count - 1));

            if (index >= waypoints.Count - 1)
            {
                var last = waypoints[waypoints.Count - 1];
                return (last.r + 0.5f, last.c + 0.5f);
            }

            var from = waypoints[index];
            var to = waypoints[index + 1];
            float t = System.Math.Max(0f, progress - index);

            return (from.r + 0.5f + (to.r - from.r) * t, from.c + 0.5f + (to.c - from.c) * t);
        }

        public (float r, float c) GetInterpolatedPosition(Enemy enemy)
        {
            int index = (int)enemy.PathProgress;
            index = System.Math.Min(index, waypoints.Count - 1);

            if (index >= waypoints.Count - 1)
            {
                var last = waypoints[waypoints.Count - 1];
                return (last.r + 0.5f, last.c + 0.5f);
            }

            var from = waypoints[index];
            var to = waypoints[index + 1];
            float t = enemy.PathProgress - index;

            return (from.r + 0.5f + (to.r - from.r) * t, from.c + 0.5f + (to.c - from.c) * t);
        }
        public void ApplyLineTrapDamage(IReadOnlyList<PlacedShape> placedShapes)
        {
            Dictionary<(int r, int c), PlacedShape> trapTiles = null;

            foreach (var enemy in ActiveEnemies)
            {
                int currentIndex = Math.Min((int)enemy.PathProgress, waypoints.Count - 1);
                if (currentIndex <= enemy.LastTrapCheckIndex)
                {
                    continue;
                }

                enemy.LastTrapCheckIndex = currentIndex;

                if (trapTiles == null)
                {
                    trapTiles = new Dictionary<(int r, int c), PlacedShape>();

                    foreach (var shape in placedShapes)
                    {
                        if (shape.Type == ShapeType.Line)
                        {
                            trapTiles[(shape.Row, shape.Col)] = shape;
                        }
                    }
                }

                var cell = waypoints[currentIndex];

                if (trapTiles.TryGetValue((cell.r, cell.c), out PlacedShape trapShape))
                {
                    ApplyLineTrapEffect(enemy, trapShape, cell, placedShapes);
                }
            }
        }

        private void ApplyLineTrapEffect(Enemy enemy, PlacedShape trapShape, (int r, int c) trapPosition, IReadOnlyList<PlacedShape> placedShapes)
        {
            var tier = TowerUpgradeConfig.GetTier(trapShape.Type, trapShape.Color, trapShape.UpgradeLevel);
            float combinedScale = GetCombinedTrapScale(trapShape.Color);

            switch (trapShape.Color)
            {
                case ShapeColor.Black:
                    float blackDamage = LineTrapFlatDamage * tier.DamageMultiplier * combinedScale;
                    enemy.TakeDamage(blackDamage);
                    float slowMultiplier = tier.SpecialValue > 0f ? tier.SpecialValue : 0.6f;
                    float slowDuration = tier.SpecialValue2 > 0f ? tier.SpecialValue2 : 2f;
                    enemy.ApplySlow(slowMultiplier, slowDuration * combinedScale);
                    break;

                case ShapeColor.White:
                    foreach (var ally in placedShapes)
                    {
                        if (ally.Type == ShapeType.Line)
                        {
                            continue;
                        }

                        int rowDistance = Math.Abs(ally.Row - trapPosition.r);
                        int colDistance = Math.Abs(ally.Col - trapPosition.c);

                        int radius = Math.Max(1, (int)Math.Round(1f * tier.RangeMultiplier));

                        if (Math.Max(rowDistance, colDistance) <= radius)
                        {
                            float buffMultiplier = tier.SpecialValue2 > 0f ? tier.SpecialValue2 : 1.2f;
                            float buffDuration = tier.SpecialValue > 0f ? tier.SpecialValue : 5f;
                            float finalMultiplier = 1f + (buffMultiplier - 1f) * combinedScale;
                            ally.ApplyAttackDamageBuff(finalMultiplier, buffDuration * combinedScale);
                        }
                    }
                    break;

                case ShapeColor.Red:
                    float defaultBurnDps = 10f;
                    float burnDps = tier.SpecialValue > 0f ? tier.SpecialValue : defaultBurnDps * tier.DamageMultiplier;
                    float burnDur = tier.SpecialValue2 > 0f ? tier.SpecialValue2 : 2f * tier.DamageMultiplier;
                    enemy.ApplyBurn(burnDps * combinedScale, burnDur * combinedScale);
                    break;

                case ShapeColor.Purple:
                    float stunDur = tier.SpecialValue > 0f ? tier.SpecialValue : 1.5f;
                    enemy.ApplyStun(stunDur * combinedScale);
                    break;

                case ShapeColor.Cyan:
                    float defaultKnock = 2.5f;
                    float knock = tier.SpecialValue > 0f ? tier.SpecialValue : defaultKnock * tier.DamageMultiplier;
                    enemy.ApplyKnockback(knock * combinedScale);
                    break;

                case ShapeColor.Green:
                    float poisonDPS = tier.SpecialValue > 0f ? tier.SpecialValue : 4f;
                    float poisonDur = tier.SpecialValue2 > 0f ? tier.SpecialValue2 : 4f;
                    enemy.ApplyPoison(poisonDPS * combinedScale, poisonDur * combinedScale);
                    break;

                case ShapeColor.Blue:
                    float groundedDur = tier.SpecialValue > 0f ? tier.SpecialValue : 2.5f;
                    enemy.ApplyGrounded(groundedDur * combinedScale);
                    break;

                case ShapeColor.Yellow:
                    float yellowDamage = LineTrapFlatDamage * 0.4f * tier.DamageMultiplier * combinedScale;
                    enemy.TakeDamage(yellowDamage);
                    float yellowSlow = tier.SpecialValue > 0f ? tier.SpecialValue : 0.5f;
                    float yellowSlowDur = tier.SpecialValue2 > 0f ? tier.SpecialValue2 : 1.2f * tier.DamageMultiplier;
                    enemy.ApplySlow(yellowSlow, yellowSlowDur * combinedScale);
                    break;
            }
        }

        public int ConsumeCashReward()
        {
            int amount = pendingCashReward;
            pendingCashReward = 0;
            return amount;
        }

        private int GetKillReward(Enemy enemy)
        {
            return Math.Max(3, (int)(enemy.MaxHealth / 5f));
        }

        private int GetWaveClearReward()
        {
            return 25 + CurrentWaveNumber * 4;
        }
    }
}