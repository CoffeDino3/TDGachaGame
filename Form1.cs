using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace TDGachaGame
{
    public partial class Form1 : Form
    {
        private FloorMap currentFloor;

        private Image pathTexture;
        private Image openTexture;
        private Image obstacleTexture;
        private Image elevatedTexture;
        private Image entranceTexture;
        private Image exitTexture;
        private EnemyManager enemyManager;
        private PlayerState playerState;
        private readonly TowerManager towerManager = new TowerManager();
        private const float TickDeltaSeconds = 0.033f;
        private Rectangle earlyCallYesButton;
        private Rectangle earlyCallNoButton;

        private bool wasFloorCleared;
        private bool isPaused;
        private bool isGameOver;
        private bool isFloorCleared;
        private readonly PlayerProfile profile;
        private Rectangle settingsLeaveButtonRect;
        private bool showStageIntro = true;
        private DifficultyTier currentTier;
        private Rectangle stageIntroStartRect;
        private bool autoSkipWave;
        private bool doubleSpeed;
        private bool showSettingsPanel;
        private bool showInventoryPanel;
        private int selectedHotbarSlot = -1;
        private int selectedRotation = 0;
        private Point? hoveredMouseLocation;
        private PlacedShape selectedPlacedShapeForRange;
        private PlacedShape selectedShapeForUpgrade;
        private Rectangle upgradePanelRect;
        private Rectangle upgradePanelTitleBarRect;
        private Rectangle upgradeButtonRect;
        private Rectangle sellButtonRect;
        private Rectangle targetFirstRect;
        private Rectangle targetLastRect;
        private Rectangle targetClosestRect;
        private Rectangle targetFarthestRect;
        private Rectangle targetLowestHpRect;
        private Rectangle targetHighestHpRect;
        private Rectangle closeUpgradePanelRect;
        private const int UpgradePanelWidth = 280;
        private const int UpgradePanelTitleBarHeight = 26;
        private Point? upgradePanelCustomPosition;
        private bool isDraggingUpgradePanel;
        private bool justFinishedDraggingUpgradePanel;
        private Point upgradePanelDragMouseStart;
        private Point upgradePanelDragOriginStart;

        private const int HotbarSlotCount = 6;
        private Rectangle[] hotbarSlotRects = new Rectangle[HotbarSlotCount];
        private UnitId?[] hotbarSlotUnits = new UnitId?[]
        {
            new UnitId(ShapeType.Line, ShapeColor.Yellow), null, null, null, null, null
        };
        private int[] hotbarSlotLevels = new int[] { 1, 1, 1, 1, 1, 1 };

        private Rectangle inventoryButtonRect;
        private Rectangle inventoryCloseButtonRect;
        private UnitId? selectedInventoryUnit;
        private (Rectangle Rect, UnitId Unit)[] inventoryOwnedSlots = Array.Empty<(Rectangle, UnitId)>();
        private Rectangle[] inventoryLoadoutSlotRects = new Rectangle[HotbarSlotCount];

        private int inventoryScrollOffset = 0;
        private int inventoryScrollMaxOffset = 0;
        private bool inventoryScrollbarVisible = false;
        private Rectangle inventoryScrollTrackRect;
        private Rectangle inventoryScrollThumbRect;
        private bool isDraggingInventoryScrollbar = false;
        private bool justFinishedDraggingInventoryScrollbar = false;
        private int inventoryScrollDragMouseStartY;
        private int inventoryScrollDragStartOffset;
        private Rectangle inventoryOwnedClipRect;

        private static readonly Dictionary<UnitId, int> ShapeCosts = new Dictionary<UnitId, int>
        {
            { new UnitId(ShapeType.Line, ShapeColor.Black), 220 },
            { new UnitId(ShapeType.Line, ShapeColor.White), 200 },
            { new UnitId(ShapeType.Line, ShapeColor.Red), 160 },
            { new UnitId(ShapeType.Line, ShapeColor.Purple), 140 },
            { new UnitId(ShapeType.Line, ShapeColor.Cyan), 120 },
            { new UnitId(ShapeType.Line, ShapeColor.Green), 100 },
            { new UnitId(ShapeType.Line, ShapeColor.Blue), 80 },
            { new UnitId(ShapeType.Line, ShapeColor.Yellow), 40 },
            { new UnitId(ShapeType.Triangle, ShapeColor.Black), 350 },
            { new UnitId(ShapeType.Triangle, ShapeColor.White), 320 },
            { new UnitId(ShapeType.Triangle, ShapeColor.Red), 260 },
            { new UnitId(ShapeType.Triangle, ShapeColor.Purple), 230 },
            { new UnitId(ShapeType.Triangle, ShapeColor.Cyan), 200 },
            { new UnitId(ShapeType.Triangle, ShapeColor.Green), 170 },
            { new UnitId(ShapeType.Triangle, ShapeColor.Blue), 140 },
            { new UnitId(ShapeType.Triangle, ShapeColor.Yellow), 70 },
            { new UnitId(ShapeType.Square, ShapeColor.Black), 480 },
            { new UnitId(ShapeType.Square, ShapeColor.White), 440 },
            { new UnitId(ShapeType.Square, ShapeColor.Red), 350 },
            { new UnitId(ShapeType.Square, ShapeColor.Purple), 310 },
            { new UnitId(ShapeType.Square, ShapeColor.Cyan), 270 },
            { new UnitId(ShapeType.Square, ShapeColor.Green), 230 },
            { new UnitId(ShapeType.Square, ShapeColor.Blue), 190 },
            { new UnitId(ShapeType.Square, ShapeColor.Yellow), 100 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.Black), 560 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.White), 520 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.Red), 420 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.Purple), 370 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.Cyan), 330 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.Green), 280 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.Blue), 230 },
            { new UnitId(ShapeType.Pentagon, ShapeColor.Yellow), 130 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.Black), 720 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.White), 680 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.Red), 560 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.Purple), 500 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.Cyan), 440 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.Green), 380 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.Blue), 320 },
            { new UnitId(ShapeType.Hexagon, ShapeColor.Yellow), 190 }
        };

        private readonly List<PlacedShape> placedShapes = new List<PlacedShape>();
        private Rectangle settingsButtonRect;
        private Rectangle settingsCloseButtonRect;
        private Rectangle autoSkipToggleRect;
        private Rectangle musicToggleRect;
        private Rectangle doubleSpeedToggleRect;
        private const float MinViewZoom = 1f;
        private const float MaxViewZoom = 6f;
        private const float ZoomPerWheelNotch = 1.15f;
        private const int PanDragThresholdPixels = 4;
        private float viewZoom = 1f;
        private float viewPanX;
        private float viewPanY;
        private bool panPressed;
        private bool isPanningView;
        private bool justFinishedPanningView;
        private Point panMouseStart;
        private float panStartX;
        private float panStartY;
        private const float TileGapKeepFraction = 0f;
        private readonly Dictionary<Image, Rectangle> tileSourceRects = new Dictionary<Image, Rectangle>();

        private const int TopBarHeight = 40;
        private const int BottomBarHeight = 74;
        private int currentStage = 0;
        public Form1() : this(new PlayerProfile())
        {
        }
        public Form1(PlayerProfile profile)
        {
            InitializeComponent();
            this.profile = profile;
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }

            currentStage = profile.TowerFloor;
            Text = $"Transmogify - Tower Floor {currentStage}";

            for (int i = 0; i < HotbarSlotCount; i++)
            {
                hotbarSlotUnits[i] = i < profile.Team.Length ? profile.Team[i] : (UnitId?)null;
                hotbarSlotLevels[i] = Math.Max(1, profile.GetTeamLevel(i));
            }

            LoadTextures();
            SetupGameWindow();
        }
        private readonly struct UpgradeStatRow
        {
            public readonly string Label;
            public readonly string CurrentText;
            public readonly string NextText;
            public readonly bool HasNext;
            public readonly bool NextIsImprovement;

            public UpgradeStatRow(string label, string currentText, string nextText, bool hasNext, bool nextIsImprovement)
            {
                Label = label;
                CurrentText = currentText;
                NextText = nextText;
                HasNext = hasNext;
                NextIsImprovement = nextIsImprovement;
            }
        }

        private List<UpgradeStatRow> BuildUpgradeStatRows(PlacedShape shape, TowerUpgradeTier current, TowerUpgradeTier next)
        {
            var rows = new List<UpgradeStatRow>();
            if (shape.Type == ShapeType.Triangle)
            {
                var (baseDamage, baseRange, baseInterval, _) = TowerUpgradeConfig.GetBaseStats(shape.Type, shape.Color, shape.CollectionLevel);
                bool hasDamage = baseDamage > 0f;

                if (hasDamage)
                {
                    float activeDamageBuff = shape.AttackDamageBuffTimeRemaining > 0f ? shape.AttackDamageBuffMultiplier : 1f;
                    float curDamage = baseDamage * current.DamageMultiplier * activeDamageBuff;
                    float? nextDamage = next != null ? baseDamage * next.DamageMultiplier * activeDamageBuff : (float?)null;
                    rows.Add(MakeRow("Damage", curDamage, nextDamage, "F1", ""));
                }

                float curRange = baseRange * current.RangeMultiplier;
                float? nextRange = next != null ? baseRange * next.RangeMultiplier : (float?)null;
                rows.Add(MakeRow("Range", curRange, nextRange, "F1", " tiles"));

                if (shape.Color != ShapeColor.White)
                {
                    float curInterval = baseInterval * current.AttackIntervalMultiplier;
                    if (shape.AttackSpeedBuffTimeRemaining > 0f) curInterval /= shape.AttackSpeedBuffMultiplier;
                    float curAttacksPerSec = 1f / curInterval;
                    float? nextAttacksPerSec = null;
                    if (next != null)
                    {
                        float nextInterval = baseInterval * next.AttackIntervalMultiplier;
                        if (shape.AttackSpeedBuffTimeRemaining > 0f) nextInterval /= shape.AttackSpeedBuffMultiplier;
                        nextAttacksPerSec = 1f / nextInterval;
                    }
                    rows.Add(MakeRow("Attack Speed", curAttacksPerSec, nextAttacksPerSec, "F2", "/s"));
                }

                switch (shape.Color)
                {
                    case ShapeColor.Purple:
                        rows.Add(MakeRow("Stun Duration", current.SpecialValue, next?.SpecialValue, "F2", "s"));
                        break;
                    case ShapeColor.Green:
                        rows.Add(MakeRow("Poison DPS", current.SpecialValue, next?.SpecialValue, "F1", ""));
                        rows.Add(MakeRow("Poison Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.Black:
                        float curSlowPct = (1f - current.SpecialValue) * 100f;
                        float? nextSlowPct = next != null ? (1f - next.SpecialValue) * 100f : (float?)null;
                        rows.Add(MakeRow("Slow Strength", curSlowPct, nextSlowPct, "F0", "%"));
                        rows.Add(MakeRow("Slow Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.White:
                        rows.Add(MakeRow("Buff Duration", current.SpecialValue, next?.SpecialValue, "F1", "s"));
                        float curCooldown = current.SpecialValue + TowerUpgradeConfig.WhiteCooldownBuffer;
                        float? nextCooldown = next != null ? next.SpecialValue + TowerUpgradeConfig.WhiteCooldownBuffer : (float?)null;
                        rows.Add(MakeRow("Cooldown", curCooldown, nextCooldown, "F1", "s"));
                        rows.Add(MakeRow("Buff Speed x", current.SpecialValue2, next?.SpecialValue2, "F2", "x"));
                        break;
                }

                if (current.CorridorWidthBonus > 0f || (next != null && next.CorridorWidthBonus > 0f))
                {
                    rows.Add(MakeRow("Beam Width +", current.CorridorWidthBonus, next?.CorridorWidthBonus, "F2", " tiles"));
                }

                if (current.DamageBuffMultiplier > 0f || (next != null && next.DamageBuffMultiplier > 0f))
                {
                    float curDmgBuffPct = DamageBuffPercent(current.DamageBuffMultiplier);
                    float? nextDmgBuffPct = next != null ? DamageBuffPercent(next.DamageBuffMultiplier) : (float?)null;
                    rows.Add(MakeRow("Ally Dmg Buff", curDmgBuffPct, nextDmgBuffPct, "F0", "%"));
                }

                return rows;
            }
            if (shape.Type == ShapeType.Line)
            {
                float baseFlat = EnemyManager.LineTrapFlatDamage;
                float trapScale = EnemyManager.GetCombinedTrapScale(shape.Color);

                switch (shape.Color)
                {
                    case ShapeColor.White:
                        rows.Add(MakeRow("Buff Duration", current.SpecialValue, next?.SpecialValue, "F1", "s"));
                        rows.Add(MakeRow("Buff Damage x", current.SpecialValue2, next?.SpecialValue2, "F2", "x"));
                        float curRadius = Math.Max(1f, 1f * current.RangeMultiplier);
                        float? nextRadius = next != null ? Math.Max(1f, 1f * next.RangeMultiplier) : (float?)null;
                        rows.Add(MakeRow("Pulse Radius", curRadius, nextRadius, "F0", " tiles"));
                        break;
                    case ShapeColor.Red:
                        float defaultBurnDps = 10f;
                        float curBurnDps = current.SpecialValue > 0f ? current.SpecialValue : defaultBurnDps * current.DamageMultiplier;
                        float? nextBurnDps = next != null ? (next.SpecialValue > 0f ? next.SpecialValue : defaultBurnDps * next.DamageMultiplier) : (float?)null;
                        float curBurnDur = current.SpecialValue2 > 0f ? current.SpecialValue2 : 2f * current.DamageMultiplier;
                        float? nextBurnDur = next != null ? (next.SpecialValue2 > 0f ? next.SpecialValue2 : 2f * next.DamageMultiplier) : (float?)null;
                        rows.Add(MakeRow("Burn DPS", curBurnDps * trapScale, nextBurnDps.HasValue ? nextBurnDps.Value * trapScale : (float?)null, "F1", ""));
                        rows.Add(MakeRow("Burn Duration", curBurnDur * trapScale, nextBurnDur.HasValue ? nextBurnDur.Value * trapScale : (float?)null, "F1", "s"));
                        break;
                    case ShapeColor.Purple:
                        rows.Add(MakeRow("Stun Duration", current.SpecialValue, next?.SpecialValue, "F2", "s"));
                        break;
                    case ShapeColor.Cyan:
                        float defaultKnock = 2.5f;
                        float curKnock = current.SpecialValue > 0f ? current.SpecialValue : defaultKnock * current.DamageMultiplier;
                        float? nextKnock = next != null ? (next.SpecialValue > 0f ? next.SpecialValue : defaultKnock * next.DamageMultiplier) : (float?)null;
                        rows.Add(MakeRow("Knockback", curKnock * trapScale, nextKnock.HasValue ? nextKnock.Value * trapScale : (float?)null, "F1", " tiles"));
                        break;
                    case ShapeColor.Green:
                        rows.Add(MakeRow("Poison DPS", current.SpecialValue, next?.SpecialValue, "F1", ""));
                        rows.Add(MakeRow("Poison Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.Black:
                        rows.Add(MakeRow("Trap Damage", baseFlat * current.DamageMultiplier * trapScale, next != null ? baseFlat * next.DamageMultiplier * trapScale : (float?)null, "F1", ""));
                        float curSlowPctB = (1f - current.SpecialValue) * 100f;
                        float? nextSlowPctB = next != null ? (1f - next.SpecialValue) * 100f : (float?)null;
                        rows.Add(MakeRow("Slow Strength", curSlowPctB, nextSlowPctB, "F0", "%"));
                        rows.Add(MakeRow("Slow Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.Yellow:
                        rows.Add(MakeRow("Trap Damage", baseFlat * 0.4f * current.DamageMultiplier * trapScale, next != null ? baseFlat * 0.4f * next.DamageMultiplier * trapScale : (float?)null, "F1", ""));
                        rows.Add(MakeRow("Slow Strength", (1f - current.SpecialValue) * 100f, next != null ? (1f - next.SpecialValue) * 100f : (float?)null, "F0", "%"));
                        float curYellowSlowDur = current.SpecialValue2 > 0f ? current.SpecialValue2 : 1.2f * current.DamageMultiplier;
                        float? nextYellowSlowDur = next != null ? (next.SpecialValue2 > 0f ? next.SpecialValue2 : 1.2f * next.DamageMultiplier) : (float?)null;
                        rows.Add(MakeRow("Slow Duration", curYellowSlowDur * trapScale, nextYellowSlowDur.HasValue ? nextYellowSlowDur.Value * trapScale : (float?)null, "F1", "s"));
                        break;
                    case ShapeColor.Blue:
                        rows.Add(MakeRow("Grounded Duration", current.SpecialValue, next?.SpecialValue, "F1", "s"));
                        break;
                }

                return rows;
            }
            if (shape.Type == ShapeType.Square)
            {
                var (baseDamage, baseRange, baseInterval, baseBlast) = TowerUpgradeConfig.GetBaseStats(shape.Type, shape.Color, shape.CollectionLevel);
                bool hasDamage = baseDamage > 0f;

                if (hasDamage)
                {
                    float activeDamageBuff = shape.AttackDamageBuffTimeRemaining > 0f ? shape.AttackDamageBuffMultiplier : 1f;
                    float curDamage = baseDamage * current.DamageMultiplier * activeDamageBuff;
                    float? nextDamage = next != null ? baseDamage * next.DamageMultiplier * activeDamageBuff : (float?)null;
                    rows.Add(MakeRow("Damage", curDamage, nextDamage, "F1", ""));
                }

                float curRange = baseRange * current.RangeMultiplier;
                float? nextRange = next != null ? baseRange * next.RangeMultiplier : (float?)null;
                rows.Add(MakeRow("Range", curRange, nextRange, "F1", " tiles"));

                float curBlast = baseBlast * current.BlastRadiusMultiplier;
                float? nextBlast = next != null ? baseBlast * next.BlastRadiusMultiplier : (float?)null;
                rows.Add(MakeRow("Blast Radius", curBlast, nextBlast, "F2", " tiles"));

                if (shape.Color != ShapeColor.White)
                {
                    float curInterval = baseInterval * current.AttackIntervalMultiplier;
                    float curAttacksPerSec = 1f / curInterval;
                    float? nextAttacksPerSec = null;
                    if (next != null)
                    {
                        float nextInterval = baseInterval * next.AttackIntervalMultiplier;
                        nextAttacksPerSec = 1f / nextInterval;
                    }
                    rows.Add(MakeRow("Attack Speed", curAttacksPerSec, nextAttacksPerSec, "F2", "/s"));
                }

                switch (shape.Color)
                {
                    case ShapeColor.Purple:
                        rows.Add(MakeRow("Stun Duration", current.SpecialValue, next?.SpecialValue, "F2", "s"));
                        break;
                    case ShapeColor.Cyan:
                        rows.Add(MakeRow("Knockback", current.SpecialValue, next?.SpecialValue, "F1", " tiles"));
                        break;
                    case ShapeColor.Green:
                        rows.Add(MakeRow("Poison DPS", current.SpecialValue, next?.SpecialValue, "F1", ""));
                        rows.Add(MakeRow("Poison Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.Black:
                        float curSlowPct = (1f - current.SpecialValue) * 100f;
                        float? nextSlowPct = next != null ? (1f - next.SpecialValue) * 100f : (float?)null;
                        rows.Add(MakeRow("Slow Strength", curSlowPct, nextSlowPct, "F0", "%"));
                        rows.Add(MakeRow("Slow Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        if (current.ExtraBeamCount > 0 || (next != null && next.ExtraBeamCount > 0))
                        {
                            rows.Add(MakeRow("Chain Blasts", current.ExtraBeamCount, next?.ExtraBeamCount, "F0", ""));
                        }
                        break;
                    case ShapeColor.White:
                        rows.Add(MakeRow("Buff Duration", current.SpecialValue, next?.SpecialValue, "F1", "s"));
                        float curCooldownSq = current.SpecialValue + TowerUpgradeConfig.WhiteCooldownBuffer;
                        float? nextCooldownSq = next != null ? next.SpecialValue + TowerUpgradeConfig.WhiteCooldownBuffer : (float?)null;
                        rows.Add(MakeRow("Cooldown", curCooldownSq, nextCooldownSq, "F1", "s"));
                        rows.Add(MakeRow("Buff Damage x", current.SpecialValue2, next?.SpecialValue2, "F2", "x"));
                        break;
                }

                return rows;
            }

            if (shape.Type == ShapeType.Pentagon)
            {
                var (baseDamage, baseRange, _, _) = TowerUpgradeConfig.GetBaseStats(shape.Type, shape.Color, shape.CollectionLevel);
                bool hasDamage = baseDamage > 0f;

                if (hasDamage)
                {
                    float curDamage = baseDamage * current.DamageMultiplier;
                    float? nextDamage = next != null ? baseDamage * next.DamageMultiplier : (float?)null;
                    rows.Add(MakeRow("Damage / Hit", curDamage, nextDamage, "F1", ""));
                }

                float curOrbit = baseRange * current.RangeMultiplier;
                float? nextOrbit = next != null ? baseRange * next.RangeMultiplier : (float?)null;
                rows.Add(MakeRow("Orbit Radius", curOrbit, nextOrbit, "F1", " tiles"));

                var (orbCount, orbRadiusTiles, spinSpeed) = TowerManager.GetPentagonOrbProfile(shape.Color);
                rows.Add(new UpgradeStatRow("Shards", $"{orbCount} ({DescribeOrbSize(orbRadiusTiles)})", null, false, false));
                rows.Add(new UpgradeStatRow("Spin Speed", $"{spinSpeed:F0}°/s", null, false, false));

                switch (shape.Color)
                {
                    case ShapeColor.Purple:
                        rows.Add(MakeRow("Stun Duration", current.SpecialValue, next?.SpecialValue, "F2", "s"));
                        break;
                    case ShapeColor.Cyan:
                        rows.Add(MakeRow("Knockback", current.SpecialValue, next?.SpecialValue, "F1", " tiles"));
                        break;
                    case ShapeColor.Green:
                        rows.Add(MakeRow("Poison DPS", current.SpecialValue, next?.SpecialValue, "F1", ""));
                        rows.Add(MakeRow("Poison Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.Black:
                        float curSlowPctP = (1f - current.SpecialValue) * 100f;
                        float? nextSlowPctP = next != null ? (1f - next.SpecialValue) * 100f : (float?)null;
                        rows.Add(MakeRow("Slow Strength", curSlowPctP, nextSlowPctP, "F0", "%"));
                        rows.Add(MakeRow("Slow Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.White:
                        rows.Add(MakeRow("Charge Multiplier", current.SpecialValue, next?.SpecialValue, "F2", "x"));
                        rows.Add(new UpgradeStatRow("Effect", "Charges next hit of touched ally", null, false, false));
                        break;
                }

                return rows;
            }

            if (shape.Type == ShapeType.Hexagon)
            {
                var (baseAttack, baseAttackSpeed, baseInterval, _) = TowerUpgradeConfig.GetBaseStats(shape.Type, shape.Color, shape.CollectionLevel);
                float activeDamageBuff = shape.AttackDamageBuffTimeRemaining > 0f ? shape.AttackDamageBuffMultiplier : 1f;
                float activeSpeedBuff = shape.AttackSpeedBuffTimeRemaining > 0f ? shape.AttackSpeedBuffMultiplier : 1f;

                float curAttack = baseAttack * current.DamageMultiplier * activeDamageBuff;
                float? nextAttack = next != null ? baseAttack * next.DamageMultiplier * activeDamageBuff : (float?)null;
                float hpMult = TowerManager.GetSummonHealthMultiplier(shape.Color);
                rows.Add(MakeRow("Unit HP", curAttack * hpMult, nextAttack.HasValue ? nextAttack.Value * hpMult : (float?)null, "F0", ""));

                float curAttackSpeed = baseAttackSpeed * current.RangeMultiplier * activeSpeedBuff;
                float? nextAttackSpeed = next != null ? baseAttackSpeed * next.RangeMultiplier * activeSpeedBuff : (float?)null;
                rows.Add(MakeRow("Unit Speed", curAttackSpeed * TowerManager.SummonSpeedPerAttackSpeed, nextAttackSpeed.HasValue ? nextAttackSpeed.Value * TowerManager.SummonSpeedPerAttackSpeed : (float?)null, "F2", " tiles/s"));

                float curInterval = baseInterval * current.AttackIntervalMultiplier;
                if (shape.AttackSpeedBuffTimeRemaining > 0f) curInterval /= shape.AttackSpeedBuffMultiplier;
                float? nextInterval = null;
                if (next != null)
                {
                    float ni = baseInterval * next.AttackIntervalMultiplier;
                    if (shape.AttackSpeedBuffTimeRemaining > 0f) ni /= shape.AttackSpeedBuffMultiplier;
                    nextInterval = ni;
                }
                rows.Add(MakeRow("Spawn Every", curInterval, nextInterval, "F2", "s"));

                switch (shape.Color)
                {
                    case ShapeColor.Purple:
                        rows.Add(MakeRow("Stun Duration", current.SpecialValue, next?.SpecialValue, "F2", "s"));
                        break;
                    case ShapeColor.Cyan:
                        rows.Add(MakeRow("Knockback", current.SpecialValue, next?.SpecialValue, "F1", " tiles"));
                        break;
                    case ShapeColor.Green:
                        rows.Add(MakeRow("Poison DPS", current.SpecialValue, next?.SpecialValue, "F1", ""));
                        rows.Add(MakeRow("Poison Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        break;
                    case ShapeColor.Black:
                        float curSlowPctH = (1f - current.SpecialValue) * 100f;
                        float? nextSlowPctH = next != null ? (1f - next.SpecialValue) * 100f : (float?)null;
                        rows.Add(MakeRow("Slow Strength", curSlowPctH, nextSlowPctH, "F0", "%"));
                        rows.Add(MakeRow("Slow Duration", current.SpecialValue2, next?.SpecialValue2, "F1", "s"));
                        rows.Add(MakeRow("Splits on Death", current.ExtraBeamCount, next?.ExtraBeamCount, "F0", ""));
                        break;
                    case ShapeColor.White:
                        rows.Add(MakeRow("Aura Radius", current.SpecialValue, next?.SpecialValue, "F1", " tiles"));
                        rows.Add(MakeRow("Aura Attack Speed", current.SpecialValue2, next?.SpecialValue2, "F2", "x"));
                        if (current.DamageBuffMultiplier > 0f || (next != null && next.DamageBuffMultiplier > 0f))
                        {
                            float curAuraDmg = DamageBuffPercent(current.DamageBuffMultiplier);
                            float? nextAuraDmg = next != null ? DamageBuffPercent(next.DamageBuffMultiplier) : (float?)null;
                            rows.Add(MakeRow("Aura Damage Buff", curAuraDmg, nextAuraDmg, "F0", "%"));
                        }
                        rows.Add(new UpgradeStatRow("Effect", "Overcharges towers if it dies", null, false, false));
                        break;
                    case ShapeColor.Blue:
                        rows.Add(new UpgradeStatRow("Hits", "Ground + air enemies", null, false, false));
                        break;
                }

                return rows;
            }

            return rows;
        }

        private static float DamageBuffPercent(float multiplier) => multiplier > 0f ? (multiplier - 1f) * 100f : 0f;

        private static string DescribeOrbSize(float orbRadiusTiles)
        {
            if (orbRadiusTiles >= 0.33f) return "large";
            if (orbRadiusTiles >= 0.20f) return "medium";
            return "small";
        }

        private static UpgradeStatRow MakeRow(string label, float current, float? next, string format, string suffix)
        {
            string currentText = current.ToString(format) + suffix;
            if (!next.HasValue)
            {
                return new UpgradeStatRow(label, currentText, null, false, false);
            }
            float diff = next.Value - current;
            string sign = diff >= 0 ? "+" : "";
            string nextText = $"→ {next.Value.ToString(format)}{suffix} ({sign}{diff.ToString(format)})";
            return new UpgradeStatRow(label, currentText, nextText, true, diff > 0f);
        }

        private void DrawUpgradePanel(Graphics g)
        {
            if (selectedShapeForUpgrade == null || !placedShapes.Contains(selectedShapeForUpgrade))
            {
                CloseUpgradePanel();
                return;
            }

            var shape = selectedShapeForUpgrade;
            int currentLevel = shape.UpgradeLevel;
            var currentTier = TowerUpgradeConfig.GetTier(shape.Type, shape.Color, currentLevel);
            var nextTier = TowerUpgradeConfig.GetNextTier(shape.Type, shape.Color, currentLevel);
            bool isMaxLevel = nextTier == null;
            var capstoneMessages = new List<string>();
            if ((shape.Type == ShapeType.Triangle || shape.Type == ShapeType.Square) && nextTier != null)
            {
                if (nextTier.ExtraBeamCount > currentTier.ExtraBeamCount)
                {
                    int totalNext = 1 + nextTier.ExtraBeamCount;
                    if (shape.Type == ShapeType.Triangle)
                    {
                        string ordinal = totalNext switch { 2 => "second", 3 => "third", 4 => "fourth", _ => $"{totalNext}th" };
                        capstoneMessages.Add($"Next level fires a {ordinal} beam!");
                    }
                    else
                    {
                        capstoneMessages.Add($"Next level chains to {nextTier.ExtraBeamCount} more blast(s)!");
                    }
                }
                if (nextTier.DamageBuffMultiplier > currentTier.DamageBuffMultiplier)
                {
                    capstoneMessages.Add("Next level also buffs allies' damage!");
                }
            }

            if (shape.Type == ShapeType.Hexagon && nextTier != null)
            {
                if (shape.Color == ShapeColor.Black && nextTier.ExtraBeamCount > currentTier.ExtraBeamCount)
                {
                    capstoneMessages.Add($"Next level: summons split {nextTier.ExtraBeamCount}x when they die!");
                }
                if (nextTier.DamageBuffMultiplier > currentTier.DamageBuffMultiplier)
                {
                    capstoneMessages.Add("Next level: wisps also buff towers' damage!");
                }
            }

            var statRows = BuildUpgradeStatRows(shape, currentTier, nextTier);
            const int titleHeight = 30;
            const int rowHeight = 36;
            const int capstoneRowHeight = 20;
            const int buttonHeight = 30;
            const int targetLabelHeight = 20;
            const int targetRowHeight = 22;
            const int targetRowSpacing = 6;
            int targetRows = 2;
            int capstoneHeight = capstoneMessages.Count * capstoneRowHeight;

            int contentHeight = titleHeight
                + statRows.Count * rowHeight
                + capstoneHeight
                + 10
                + buttonHeight
                + 14
                + targetLabelHeight
                + targetRows * targetRowHeight + (targetRows - 1) * targetRowSpacing
                + 16;

            int panelHeight = Math.Max(contentHeight, 200);

            if (upgradePanelCustomPosition == null)
            {
                int defaultX = 12;
                int defaultY = TopBarHeight + Math.Max(12, (gamePanel.Height - TopBarHeight - BottomBarHeight - panelHeight) / 2);
                upgradePanelCustomPosition = new Point(defaultX, defaultY);
            }

            var panelRect = new Rectangle(upgradePanelCustomPosition.Value.X, upgradePanelCustomPosition.Value.Y, UpgradePanelWidth, panelHeight);
            upgradePanelRect = panelRect;
            upgradePanelTitleBarRect = new Rectangle(panelRect.X, panelRect.Y, panelRect.Width, UpgradePanelTitleBarHeight);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingMode = CompositingMode.SourceOver;

            using (var bgBrush = new SolidBrush(Color.FromArgb(230, 30, 30, 30)))
            using (var titleBarBrush = new SolidBrush(Color.FromArgb(230, 45, 45, 50)))
            using (var borderPen = new Pen(Color.White, 1.5f))
            {
                g.FillRectangle(bgBrush, panelRect);
                g.FillRectangle(titleBarBrush, upgradePanelTitleBarRect);
                g.DrawRectangle(borderPen, panelRect);
            }

            using (var titleFont = new Font("Segoe UI", 11f, FontStyle.Bold))
            using (var font = new Font("Segoe UI", 9f))
            using (var textBrush = new SolidBrush(Color.White))
            using (var dimBrush = new SolidBrush(Color.Gainsboro))
            using (var goldBrush = new SolidBrush(Color.Gold))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var leftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                int y = panelRect.Y + 4;

                var titleRect = new RectangleF(panelRect.X + 12, y, panelRect.Width - 40, UpgradePanelTitleBarHeight - 4);
                string title = isMaxLevel ? $"{shape.Color} {shape.Type} \u2605 MAX" : $"{shape.Color} {shape.Type} \u2605 Lv.{currentLevel + 1}/{TowerUpgradeConfig.GetTierCount(shape.Type, shape.Color)}";
                g.DrawString(title, titleFont, isMaxLevel ? goldBrush : textBrush, titleRect, leftFormat);
                y = panelRect.Y + titleHeight;

                using (var nextValueFont = new Font("Segoe UI", 8.5f))
                {
                    int rowIndex = 0;
                    foreach (var row in statRows)
                    {
                        if (rowIndex % 2 == 1)
                        {
                            var stripeRect = new RectangleF(panelRect.X + 4, y, panelRect.Width - 8, rowHeight - 2);
                            using (var stripeBrush = new SolidBrush(Color.FromArgb(18, 255, 255, 255)))
                            {
                                g.FillRectangle(stripeBrush, stripeRect);
                            }
                        }

                        var labelRect = new RectangleF(panelRect.X + 12, y + 2, 116, 18);
                        var valueRect = new RectangleF(panelRect.X + 132, y + 2, panelRect.Width - 144, 18);
                        g.DrawString(row.Label + ":", font, dimBrush, labelRect);
                        g.DrawString(row.CurrentText, font, textBrush, valueRect);

                        if (row.HasNext)
                        {
                            var nextRect = new RectangleF(panelRect.X + 12, y + 19, panelRect.Width - 24, 16);
                            using (var nextBrush = new SolidBrush(row.NextIsImprovement ? Color.LightGreen : Color.Orange))
                            {
                                g.DrawString(row.NextText, nextValueFont, nextBrush, nextRect);
                            }
                        }
                        y += rowHeight;
                        rowIndex++;
                    }
                }

                if (capstoneMessages.Count > 0)
                {
                    using (var capstoneBrush = new SolidBrush(Color.LightSkyBlue))
                    using (var smallFont = new Font("Segoe UI", 8f, FontStyle.Italic))
                    {
                        foreach (var message in capstoneMessages)
                        {
                            var capstoneRect = new RectangleF(panelRect.X + 12, y, panelRect.Width - 24, capstoneRowHeight);
                            g.DrawString(message, smallFont, capstoneBrush, capstoneRect);
                            y += capstoneRowHeight;
                        }
                    }
                }

                y += 8;

                int buttonWidth = 100;
                int buttonY = y;

                if (isMaxLevel)
                {
                    var maxRect = new Rectangle(panelRect.X + 12, buttonY, buttonWidth, buttonHeight);
                    upgradeButtonRect = maxRect;
                    using (var maxBrush = new SolidBrush(Color.FromArgb(100, 60, 60, 60)))
                    {
                        g.FillRectangle(maxBrush, maxRect);
                        g.DrawString("MAX", font, goldBrush, maxRect, centerFormat);
                    }
                }
                else
                {
                    upgradeButtonRect = new Rectangle(panelRect.X + 12, buttonY, buttonWidth, buttonHeight);
                    bool canAfford = playerState != null && playerState.Cash >= nextTier.Cost;
                    using (var upgradeBrush = new SolidBrush(canAfford ? Color.SteelBlue : Color.FromArgb(100, 60, 60, 60)))
                    {
                        g.FillRectangle(upgradeBrush, upgradeButtonRect);
                        string label = canAfford ? $"Upgrade (${nextTier.Cost})" : $"Need ${nextTier.Cost}";
                        g.DrawString(label, font, textBrush, upgradeButtonRect, centerFormat);
                    }
                }

                int sellValue = TowerUpgradeConfig.GetSellValue(shape.Type, shape.Color, currentLevel);
                sellButtonRect = new Rectangle(panelRect.Right - buttonWidth - 12, buttonY, buttonWidth, buttonHeight);
                using (var sellBrush = new SolidBrush(Color.Firebrick))
                {
                    g.FillRectangle(sellBrush, sellButtonRect);
                    g.DrawString($"Sell (${sellValue})", font, textBrush, sellButtonRect, centerFormat);
                }

                y = buttonY + buttonHeight + 14;

                var targetLabelRect = new RectangleF(panelRect.X + 12, y, panelRect.Width - 24, 16);
                g.DrawString("Targeting:", font, dimBrush, targetLabelRect, leftFormat);
                y += targetLabelHeight;

                var targetingModes = new (string label, TargetingMode mode)[]
                {
                    ("First", TargetingMode.First),
                    ("Last", TargetingMode.Last),
                    ("Closest", TargetingMode.Closest),
                    ("Farthest", TargetingMode.Farthest),
                    ("Low HP", TargetingMode.LowestHp),
                    ("High HP", TargetingMode.HighestHp)
                };

                int btnWidth = 70;
                int cols = 3;
                int startX = panelRect.X + 12;

                for (int i = 0; i < targetingModes.Length; i++)
                {
                    int col = i % cols;
                    int row = i / cols;
                    var rect = new Rectangle(startX + col * (btnWidth + targetRowSpacing), y + row * (targetRowHeight + targetRowSpacing), btnWidth, targetRowHeight);

                    switch (targetingModes[i].mode)
                    {
                        case TargetingMode.First: targetFirstRect = rect; break;
                        case TargetingMode.Last: targetLastRect = rect; break;
                        case TargetingMode.Closest: targetClosestRect = rect; break;
                        case TargetingMode.Farthest: targetFarthestRect = rect; break;
                        case TargetingMode.LowestHp: targetLowestHpRect = rect; break;
                        case TargetingMode.HighestHp: targetHighestHpRect = rect; break;
                    }

                    bool isSelected = shape.TargetingMode == targetingModes[i].mode;
                    using (var btnBrush = new SolidBrush(isSelected ? Color.SteelBlue : Color.FromArgb(80, 60, 60, 60)))
                    using (var btnPen = new Pen(isSelected ? Color.Gold : Color.Gray, 1f))
                    {
                        g.FillRectangle(btnBrush, rect);
                        g.DrawRectangle(btnPen, rect);
                        g.DrawString(targetingModes[i].label, font, textBrush, rect, centerFormat);
                    }
                }

                closeUpgradePanelRect = new Rectangle(panelRect.Right - 26, panelRect.Y + 3, 20, 20);
                using (var closeBrush = new SolidBrush(Color.FromArgb(150, 80, 80, 80)))
                {
                    g.FillRectangle(closeBrush, closeUpgradePanelRect);
                    g.DrawString("\u2715", font, textBrush, closeUpgradePanelRect, centerFormat);
                }
            }

            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }

        private void LoadTextures()
        {
            string tilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "Tiles");

            pathTexture = LoadTexture(
                Path.Combine(tilePath, "path.png")
            );

            openTexture = LoadTexture(
                Path.Combine(tilePath, "open.png")
            );

            obstacleTexture = LoadTexture(
                Path.Combine(tilePath, "obstacle.png")
            );

            elevatedTexture = LoadTexture(
                Path.Combine(tilePath, "elevated.png")
            );

            entranceTexture = LoadTexture(
                Path.Combine(tilePath, "entrance.png")
            );

            exitTexture = LoadTexture(
                Path.Combine(tilePath, "exit.png")
            );
        }
        private void TryUpgradeShape(PlacedShape shape)
        {
            if (shape == null || playerState == null) return;

            var nextTier = TowerUpgradeConfig.GetNextTier(shape.Type, shape.Color, shape.UpgradeLevel);
            if (nextTier == null) return;

            if (!playerState.TrySpend(nextTier.Cost)) return;

            shape.UpgradeLevel++;
        }

        private void TrySellShape(PlacedShape shape)
        {
            if (shape == null || playerState == null) return;

            int sellValue = TowerUpgradeConfig.GetSellValue(shape.Type, shape.Color, shape.UpgradeLevel);
            playerState.AddCash(sellValue);

            placedShapes.Remove(shape);
            CloseUpgradePanel();
        }
        private void CloseUpgradePanel()
        {
            selectedShapeForUpgrade = null;
            selectedPlacedShapeForRange = null;
        }
        private Image LoadTexture(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show(
                    $"Could not find texture:\n{path}",
                    "Missing Texture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return null;
            }

            return Image.FromFile(path);
        }

        private void SetupGameWindow()
        {
            currentFloor = FloorMap.Generate(currentStage);
            var (tier, waves) = FloorEnemyGenerator.Generate(currentStage);
            currentTier = tier;
            enemyManager = new EnemyManager(currentFloor, waves);
            playerState = new PlayerState(StageProgression.GetStartingCash(currentStage));
        }

        private void ShowResultPanel(bool cleared)
        {
            if (cleared)
            {
                int reward = profile.CompleteTowerFloor(currentStage, out int gemReward);
                lblResultTitle.Text = "FLOOR CLEARED";
                lblResultTitle.ForeColor = Color.LimeGreen;
                lblResultDetail.Text = $"Floor {currentStage} complete!\r\n+{reward} coins   +{gemReward} gems";
            }
            else
            {
                lblResultTitle.Text = "FLOOR LOST";
                lblResultTitle.ForeColor = Color.OrangeRed;
                lblResultDetail.Text = "Your base was destroyed.\r\nRegroup and try again from the menu.";
            }

            CenterResultPanel();
            pnlResult.Visible = true;
            pnlResult.BringToFront();
            btnReturnToMenu.Focus();
        }

        private void CenterResultPanel()
        {
            pnlResult.Location = new Point(
                Math.Max(0, (gamePanel.ClientSize.Width - pnlResult.Width) / 2),
                Math.Max(0, (gamePanel.ClientSize.Height - pnlResult.Height) / 2));
        }

        private void GamePanel_Resize(object sender, EventArgs e)
        {
            CenterResultPanel();
        }

        private void btnReturnToMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfirmLeaveTower()
        {
            var answer = MessageBox.Show(
                this,
                "Leave the tower? Your progress on this floor will be lost.",
                "Leave Tower",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {
                Close();
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!isPaused && !showSettingsPanel && !showStageIntro)
            {
                float delta = TickDeltaSeconds * (doubleSpeed ? 2f : 1f);
                enemyManager?.Update(delta);
                if (enemyManager != null && enemyManager.BaseDestroyed)
                {
                    isGameOver = true;
                    isPaused = true;
                    selectedHotbarSlot = -1;
                    CloseUpgradePanel();
                    ShowResultPanel(false);
                    gamePanel.Invalidate();
                    return;
                }

                enemyManager?.ApplyLineTrapDamage(placedShapes);
                towerManager.Update(placedShapes, enemyManager, delta);

                int cashReward = enemyManager?.ConsumeCashReward() ?? 0;
                if (cashReward > 0)
                {
                    playerState?.AddCash(cashReward);
                }

                if (autoSkipWave && (enemyManager?.AwaitingEarlyWaveCall ?? false))
                {
                    enemyManager.ConfirmEarlyWaveCall();
                }

                bool floorClearedNow = enemyManager?.FloorCleared ?? false;

                if (floorClearedNow && !wasFloorCleared)
                {
                    playerState.AddCash(20 + currentStage * 5);
                    isGameOver = true;
                    isFloorCleared = true;
                    isPaused = true;
                    selectedHotbarSlot = -1;
                    CloseUpgradePanel();
                    ShowResultPanel(true);
                }

                wasFloorCleared = floorClearedNow;
            }

            gamePanel.Invalidate();
        }
        private Rectangle GetWorldViewport()
        {
            int availableWidth = gamePanel.ClientSize.Width;
            int availableHeight = gamePanel.ClientSize.Height - TopBarHeight - BottomBarHeight;
            return new Rectangle(0, TopBarHeight, availableWidth, Math.Max(1, availableHeight));
        }

        private int ComputeZoomedTileSize(Rectangle viewport)
        {
            int fitTile = Math.Min(viewport.Width / currentFloor.Cols, viewport.Height / currentFloor.Rows);
            return Math.Max(1, (int)Math.Round(Math.Max(1, fitTile) * viewZoom));
        }

        private static int ClampViewAxis(int offset, int viewStart, int viewSize, int contentSize)
        {
            if (contentSize <= viewSize)
            {
                return viewStart + (viewSize - contentSize) / 2;
            }

            int min = viewStart + viewSize - contentSize;
            int max = viewStart;
            return Math.Max(min, Math.Min(max, offset));
        }

        private (int tileSize, int offsetX, int offsetY) ComputeGridMetrics()
        {
            var viewport = GetWorldViewport();
            int tileSize = ComputeZoomedTileSize(viewport);

            int gridPixelWidth = tileSize * currentFloor.Cols;
            int gridPixelHeight = tileSize * currentFloor.Rows;

            int centeredX = viewport.X + (viewport.Width - gridPixelWidth) / 2;
            int centeredY = viewport.Y + (viewport.Height - gridPixelHeight) / 2;

            int offsetX = ClampViewAxis(centeredX + (int)Math.Round(viewPanX), viewport.X, viewport.Width, gridPixelWidth);
            int offsetY = ClampViewAxis(centeredY + (int)Math.Round(viewPanY), viewport.Y, viewport.Height, gridPixelHeight);

            return (tileSize, offsetX, offsetY);
        }

        private void ClampViewPan()
        {
            if (currentFloor == null) return;

            var viewport = GetWorldViewport();
            int tileSize = ComputeZoomedTileSize(viewport);
            int gridPixelWidth = tileSize * currentFloor.Cols;
            int gridPixelHeight = tileSize * currentFloor.Rows;

            int centeredX = viewport.X + (viewport.Width - gridPixelWidth) / 2;
            int centeredY = viewport.Y + (viewport.Height - gridPixelHeight) / 2;

            int offsetX = ClampViewAxis(centeredX + (int)Math.Round(viewPanX), viewport.X, viewport.Width, gridPixelWidth);
            int offsetY = ClampViewAxis(centeredY + (int)Math.Round(viewPanY), viewport.Y, viewport.Height, gridPixelHeight);

            viewPanX = offsetX - centeredX;
            viewPanY = offsetY - centeredY;
        }

        private void ZoomViewAt(Point mouse, float newZoom)
        {
            if (currentFloor == null) return;

            newZoom = Math.Max(MinViewZoom, Math.Min(MaxViewZoom, newZoom));
            if (Math.Abs(newZoom - viewZoom) < 0.0001f) return;

            var (oldTile, oldOffsetX, oldOffsetY) = ComputeGridMetrics();
            float gridX = (mouse.X - oldOffsetX) / (float)oldTile;
            float gridY = (mouse.Y - oldOffsetY) / (float)oldTile;

            viewZoom = newZoom;

            var viewport = GetWorldViewport();
            int newTile = ComputeZoomedTileSize(viewport);
            int centeredX = viewport.X + (viewport.Width - newTile * currentFloor.Cols) / 2;
            int centeredY = viewport.Y + (viewport.Height - newTile * currentFloor.Rows) / 2;

            viewPanX = (mouse.X - gridX * newTile) - centeredX;
            viewPanY = (mouse.Y - gridY * newTile) - centeredY;
            ClampViewPan();
        }

        private bool CanPanView()
        {
            return currentFloor != null
                && !showSettingsPanel
                && !showInventoryPanel
                && !showStageIntro
                && selectedHotbarSlot == -1
                && selectedShapeForUpgrade == null;
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (currentFloor == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.CompositingMode = CompositingMode.SourceCopy;

            var (tileSize, offsetX, offsetY) = ComputeGridMetrics();
            var worldViewport = GetWorldViewport();
            g.SetClip(worldViewport);

            for (int r = 0; r < currentFloor.Rows; r++)
            {
                for (int c = 0; c < currentFloor.Cols; c++)
                {
                    if (!worldViewport.IntersectsWith(new Rectangle(offsetX + c * tileSize, offsetY + r * tileSize, tileSize, tileSize)))
                    {
                        continue;
                    }

                    var tile = currentFloor.Tiles[r, c];
                    Image texture = GetTexture(tile.Type);

                    if (texture == null)
                    {
                        g.FillRectangle(Brushes.Magenta, offsetX + c * tileSize, offsetY + r * tileSize, tileSize, tileSize);
                        continue;
                    }

                    Rectangle destination = new Rectangle(offsetX + c * tileSize, offsetY + r * tileSize, tileSize, tileSize);
                    Rectangle source = GetTileSourceRect(texture);
                    g.DrawImage(texture, destination, source.X, source.Y, source.Width, source.Height, GraphicsUnit.Pixel);
                }
            }

            DrawSelectedRange(g, tileSize, offsetX, offsetY);
            DrawPlacedShapes(g, tileSize, offsetX, offsetY);
            DrawEnemies(g, tileSize, offsetX, offsetY);
            DrawSummons(g, tileSize, offsetX, offsetY);
            DrawShotEffects(g, tileSize, offsetX, offsetY);
            DrawBombEffects(g, tileSize, offsetX, offsetY);
            DrawSparkEffects(g, tileSize, offsetX, offsetY);
            DrawPlacementPreview(g, tileSize, offsetX, offsetY);
            g.ResetClip();
            DrawHud(g);
            DrawBottomBar(g);
            DrawSettingsButton(g);

            if (isGameOver)
            {
                using (var dimBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                {
                    g.FillRectangle(dimBrush, 0, 0, gamePanel.ClientSize.Width, gamePanel.ClientSize.Height);
                }
            }

            if (showStageIntro)
            {
                DrawStageIntroOverlay(g);
            }

            if (showSettingsPanel)
            {
                DrawSettingsPanel(g);
            }
            else if (showInventoryPanel)
            {
                DrawInventoryPanel(g);
            }
            if (selectedShapeForUpgrade != null)
            {
                DrawUpgradePanel(g);
            }

        }

        private void DrawHud(Graphics g)
        {
            if (enemyManager == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingMode = CompositingMode.SourceOver;

            int barHeight = TopBarHeight;
            int panelWidth = gamePanel.ClientSize.Width;
            int gearReserve = 40;

            using (var barBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
            {
                g.FillRectangle(barBrush, 0, 0, panelWidth, barHeight);
            }

            using (var font = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.White))
            using (var leftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var rightFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
            {
                var barRect = new RectangleF(0, 0, panelWidth, barHeight);
                var rightTextRect = new RectangleF(0, 0, panelWidth - gearReserve, barHeight);

                string timeText = FormatTime(enemyManager.ElapsedSeconds);
                if (doubleSpeed)
                {
                    timeText += " (2x)";
                }
                g.DrawString(timeText, font, textBrush, Indent(barRect, 12), leftFormat);

                string waveText = enemyManager.IsBetweenWaves
                    ? $"Wave {enemyManager.CurrentWaveNumber} / {enemyManager.TotalWaves} — next in {Math.Ceiling(enemyManager.TimeUntilNextWave)}s"
                    : $"Wave {enemyManager.CurrentWaveNumber} / {enemyManager.TotalWaves}";
                if (autoSkipWave)
                {
                    waveText += " [auto]";
                }
                g.DrawString(waveText, font, textBrush, barRect, centerFormat);

                string healthText = $"Base HP: {Math.Max(0, enemyManager.BaseHealth)}";
                Color healthColor = enemyManager.BaseHealth <= 5 ? Color.OrangeRed : Color.White;
                using (var healthBrush = new SolidBrush(healthColor))
                {
                    g.DrawString(healthText, font, healthBrush, Indent(rightTextRect, -12), rightFormat);
                }
            }

            DrawEarlyWaveCallPrompt(g, barHeight, panelWidth);

            g.SmoothingMode = SmoothingMode.None;
        }

        private void DrawEarlyWaveCallPrompt(Graphics g, int barHeight, int panelWidth)
        {
            if (!enemyManager.AwaitingEarlyWaveCall)
            {
                earlyCallYesButton = Rectangle.Empty;
                earlyCallNoButton = Rectangle.Empty;
                return;
            }

            int promptWidth = 360;
            int promptHeight = 60;
            int promptX = (panelWidth - promptWidth) / 2;
            int promptY = barHeight + 12;
            var promptRect = new Rectangle(promptX, promptY, promptWidth, promptHeight);

            using (var bgBrush = new SolidBrush(Color.FromArgb(210, 20, 20, 20)))
            using (var borderPen = new Pen(Color.White, 1.5f))
            {
                g.FillRectangle(bgBrush, promptRect);
                g.DrawRectangle(borderPen, promptRect);
            }

            using (var font = new Font("Segoe UI", 10f, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.White))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                var messageRect = new RectangleF(promptRect.X + 8, promptRect.Y + 6, promptRect.Width - 16, 22);
                g.DrawString($"Call wave {enemyManager.CurrentWaveNumber + 1} early? (Y/N)", font, textBrush, messageRect, centerFormat);

                int buttonWidth = 70;
                int buttonHeight = 24;
                int buttonY = promptRect.Bottom - buttonHeight - 6;

                earlyCallYesButton = new Rectangle(promptRect.X + promptRect.Width / 2 - buttonWidth - 8, buttonY, buttonWidth, buttonHeight);
                earlyCallNoButton = new Rectangle(promptRect.X + promptRect.Width / 2 + 8, buttonY, buttonWidth, buttonHeight);

                DrawPromptButton(g, font, centerFormat, earlyCallYesButton, "Yes", Color.SeaGreen);
                DrawPromptButton(g, font, centerFormat, earlyCallNoButton, "No", Color.Firebrick);
            }
        }

        private void DrawPromptButton(Graphics g, Font font, StringFormat format, Rectangle rect, string label, Color fillColor)
        {
            using (var brush = new SolidBrush(fillColor))
            using (var textBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(brush, rect);
                g.DrawString(label, font, textBrush, rect, format);
            }
        }

        private void DrawBottomBar(Graphics g)
        {
            if (playerState == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingMode = CompositingMode.SourceOver;

            int panelWidth = gamePanel.ClientSize.Width;
            int panelHeight = gamePanel.ClientSize.Height;
            int barY = panelHeight - BottomBarHeight;

            using (var barBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
            {
                g.FillRectangle(barBrush, 0, barY, panelWidth, BottomBarHeight);
            }

            using (var font = new Font("Segoe UI", 11f, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.Gold))
            using (var leftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                var cashRect = new RectangleF(12, barY, 150, BottomBarHeight);
                g.DrawString($"Cash: {playerState.Cash}", font, textBrush, cashRect, leftFormat);
            }

            DrawHotbarSlots(g, barY);

            g.SmoothingMode = SmoothingMode.None;
        }

        private void DrawHotbarSlots(Graphics g, int barY)
        {
            int panelWidth = gamePanel.ClientSize.Width;

            const int slotSize = 52;
            const int slotGap = 8;
            int totalWidth = HotbarSlotCount * slotSize + (HotbarSlotCount - 1) * slotGap;
            int startX = (panelWidth - totalWidth) / 2;
            int slotY = barY + (BottomBarHeight - slotSize) / 2;

            using (var emptyBrush = new SolidBrush(Color.FromArgb(90, 255, 255, 255)))
            using (var emptyPen = new Pen(Color.Gainsboro, 1.5f))
            using (var selectedPen = new Pen(Color.Gold, 2.5f))
            using (var font = new Font("Segoe UI", 9f))
            using (var textBrush = new SolidBrush(Color.White))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                for (int i = 0; i < HotbarSlotCount; i++)
                {
                    var rect = new Rectangle(startX + i * (slotSize + slotGap), slotY, slotSize, slotSize);
                    hotbarSlotRects[i] = rect;
                    g.FillRectangle(emptyBrush, rect);
                    g.DrawRectangle(i == selectedHotbarSlot ? selectedPen : emptyPen, rect);

                    UnitId? slotUnit = hotbarSlotUnits[i];
                    if (slotUnit.HasValue)
                    {
                        var iconRect = new Rectangle(rect.X, rect.Y + 4, rect.Width, rect.Height - 18);
                        DrawShapeGlyph(g, slotUnit.Value.Type, slotUnit.Value.Color, iconRect, 0, 255);

                        var costRect = new Rectangle(rect.X, rect.Bottom - 16, rect.Width, 14);
                        string costText = ShapeCosts.TryGetValue(slotUnit.Value, out int slotCost) ? $"${slotCost}" : "?";
                        g.DrawString(costText, font, textBrush, costRect, centerFormat);
                    }
                    else
                    {
                        g.DrawString((i + 1).ToString(), font, textBrush, rect, centerFormat);
                    }
                }
            }
        }

        private float IconScaleFactor()
        {
            int panelWidth = gamePanel.ClientSize.Width;
            int panelHeight = gamePanel.ClientSize.Height;
            float scale = Math.Min(panelWidth / 900f, panelHeight / 700f);
            return Math.Max(1f, Math.Min(2f, scale));
        }

        private void DrawSettingsButton(Graphics g)
        {
            int panelWidth = gamePanel.ClientSize.Width;
            float scale = IconScaleFactor();
            int size = (int)Math.Round(28 * scale);
            int margin = (int)Math.Round(6 * scale);
            int y = Math.Max(2, (TopBarHeight - size) / 2);
            settingsButtonRect = new Rectangle(panelWidth - size - margin, y, size, size);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new SolidBrush(Color.FromArgb(180, 60, 60, 60)))
            using (var pen = new Pen(Color.White, 1.2f))
            using (var font = new Font("Segoe UI", 13f * scale))
            using (var textBrush = new SolidBrush(Color.White))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                var glyphRect = new Rectangle(
                    settingsButtonRect.X + 1,
                    settingsButtonRect.Y - 1,
                    settingsButtonRect.Width,
                    settingsButtonRect.Height
                    );
                g.DrawString("\u2699", font, textBrush, glyphRect, centerFormat);
            }

            g.SmoothingMode = SmoothingMode.None;
        }

        private void DrawSettingsPanel(Graphics g)
        {
            int panelWidth = gamePanel.ClientSize.Width;
            int panelHeight = gamePanel.ClientSize.Height;

            const int width = 260;
            const int height = 264;
            var panelRect = new Rectangle((panelWidth - width) / 2, (panelHeight - height) / 2, width, height);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var dimBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
            {
                g.FillRectangle(dimBrush, 0, 0, panelWidth, panelHeight);
            }

            using (var bgBrush = new SolidBrush(Color.FromArgb(230, 30, 30, 30)))
            using (var borderPen = new Pen(Color.White, 1.5f))
            {
                g.FillRectangle(bgBrush, panelRect);
                g.DrawRectangle(borderPen, panelRect);
            }

            using (var titleFont = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var font = new Font("Segoe UI", 10f))
            using (var textBrush = new SolidBrush(Color.White))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var leftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                var titleRect = new RectangleF(panelRect.X, panelRect.Y + 8, panelRect.Width, 24);
                g.DrawString("Settings", titleFont, textBrush, titleRect, centerFormat);

                int rowY = panelRect.Y + 44;
                const int rowSpacing = 34;

                autoSkipToggleRect = DrawSettingsToggleRow(g, font, textBrush, leftFormat, panelRect, rowY, "Auto-skip wave", autoSkipWave);
                rowY += rowSpacing;

                doubleSpeedToggleRect = DrawSettingsToggleRow(g, font, textBrush, leftFormat, panelRect, rowY, "2x speed", doubleSpeed);
                rowY += rowSpacing;

                musicToggleRect = DrawSettingsToggleRow(g, font, textBrush, leftFormat, panelRect, rowY, "Music", !MusicPlayer.Muted);
                rowY += rowSpacing;

                using (var hintFont = new Font("Segoe UI", 8.5f))
                using (var hintBrush = new SolidBrush(Color.Silver))
                {
                    var hintRect = new RectangleF(panelRect.X, rowY - 4, panelRect.Width, 20);
                    g.DrawString("Game paused  \u2022  Esc to close", hintFont, hintBrush, hintRect, centerFormat);
                }

                settingsLeaveButtonRect = new Rectangle(panelRect.X + (panelRect.Width - 140) / 2, panelRect.Bottom - 74, 140, 26);
                using (var leaveBrush = new SolidBrush(Color.FromArgb(70, 70, 95)))
                {
                    g.FillRectangle(leaveBrush, settingsLeaveButtonRect);
                    g.DrawString("Leave Tower", font, textBrush, settingsLeaveButtonRect, centerFormat);
                }

                settingsCloseButtonRect = new Rectangle(panelRect.X + (panelRect.Width - 80) / 2, panelRect.Bottom - 38, 80, 26);
                using (var closeBrush = new SolidBrush(Color.Firebrick))
                {
                    g.FillRectangle(closeBrush, settingsCloseButtonRect);
                    g.DrawString("Close", font, textBrush, settingsCloseButtonRect, centerFormat);
                }
            }

            g.SmoothingMode = SmoothingMode.None;
        }

        private Rectangle DrawSettingsToggleRow(Graphics g, Font font, Brush textBrush, StringFormat leftFormat, Rectangle panelRect, int rowY, string label, bool isChecked)
        {
            var toggleRect = new Rectangle(panelRect.X + 18, rowY, 18, 18);

            using (var boxPen = new Pen(Color.White, 1.5f))
            {
                g.DrawRectangle(boxPen, toggleRect);

                if (isChecked)
                {
                    using (var checkBrush = new SolidBrush(Color.Gold))
                    {
                        g.FillRectangle(checkBrush, toggleRect.X + 3, toggleRect.Y + 3, toggleRect.Width - 6, toggleRect.Height - 6);
                    }
                }
            }

            var labelRect = new RectangleF(toggleRect.Right + 8, toggleRect.Y - 2, panelRect.Width - 60, 22);
            g.DrawString(label, font, textBrush, labelRect, leftFormat);

            return toggleRect;
        }

        private void DrawInventoryButton(Graphics g)
        {
            int panelWidth = gamePanel.ClientSize.Width;
            int panelHeight = gamePanel.ClientSize.Height;
            float scale = IconScaleFactor();
            int size = (int)Math.Round(36 * scale);
            int margin = (int)Math.Round(10 * scale);
            inventoryButtonRect = new Rectangle(
                panelWidth - size - margin,
                (panelHeight - size) / 2,
                size,
                size
            );
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var bgBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
            using (var borderPen = new Pen(Color.White, 1.2f))
            {
                g.FillRectangle(bgBrush, inventoryButtonRect);
                g.DrawRectangle(borderPen, inventoryButtonRect);
            }
            int pad = 8;
            int cell = (size - pad * 2 - 3) / 2;
            int gx = inventoryButtonRect.X + pad;
            int gy = inventoryButtonRect.Y + pad;

            using (var pen = new Pen(Color.White, 1.3f))
            {
                g.DrawRectangle(pen, gx, gy, cell, cell);
                g.DrawRectangle(pen, gx + cell + 3, gy, cell, cell);
                g.DrawRectangle(pen, gx, gy + cell + 3, cell, cell);
                g.DrawRectangle(pen, gx + cell + 3, gy + cell + 3, cell, cell);
            }

            g.SmoothingMode = SmoothingMode.None;
        }

        private void DrawInventoryPanel(Graphics g)
        {
            if (playerState == null)
            {
                return;
            }

            int panelWidth = gamePanel.ClientSize.Width;
            int panelHeight = gamePanel.ClientSize.Height;

            const int width = 380;
            const int slotSize = 52;
            const int slotGap = 10;
            const int columns = 4;
            const int maxVisibleOwnedRows = 3;

            int ownedCount = 0;
            foreach (var kvp in playerState.OwnedShapeCounts)
            {
                if (kvp.Value > 0)
                {
                    ownedCount++;
                }
            }

            int ownedCols = Math.Max(1, Math.Min(columns, ownedCount));
            int ownedRows = ownedCount == 0 ? 1 : (int)Math.Ceiling(ownedCount / (double)ownedCols);
            int ownedGridHeight = ownedCount == 0 ? 24 : ownedRows * slotSize + (ownedRows - 1) * slotGap;

            int visibleOwnedRows = Math.Max(1, Math.Min(ownedRows, maxVisibleOwnedRows));
            int visibleGridHeight = ownedCount == 0 ? 24 : visibleOwnedRows * slotSize + (visibleOwnedRows - 1) * slotGap;

            const int titleAreaHeight = 32;
            const int labelHeight = 18;
            const int sectionGap = 10;
            const int loadoutGridHeight = 44;
            const int closeAreaHeight = 46;
            const int bottomPad = 12;

            int height = titleAreaHeight + labelHeight + sectionGap + visibleGridHeight + sectionGap
                + labelHeight + sectionGap + loadoutGridHeight + closeAreaHeight + bottomPad;

            var panelRect = new Rectangle((panelWidth - width) / 2, (panelHeight - height) / 2, width, height);

            inventoryScrollMaxOffset = Math.Max(0, ownedGridHeight - visibleGridHeight);
            inventoryScrollOffset = Math.Max(0, Math.Min(inventoryScrollOffset, inventoryScrollMaxOffset));
            inventoryScrollbarVisible = inventoryScrollMaxOffset > 0;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var dimBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
            {
                g.FillRectangle(dimBrush, 0, 0, panelWidth, panelHeight);
            }

            using (var bgBrush = new SolidBrush(Color.FromArgb(230, 30, 30, 30)))
            using (var borderPen = new Pen(Color.White, 1.5f))
            {
                g.FillRectangle(bgBrush, panelRect);
                g.DrawRectangle(borderPen, panelRect);
            }

            using (var titleFont = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var labelFont = new Font("Segoe UI", 9f))
            using (var textBrush = new SolidBrush(Color.White))
            using (var dimTextBrush = new SolidBrush(Color.Gainsboro))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var leftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                int y = panelRect.Y + 8;
                var titleRect = new RectangleF(panelRect.X, y, panelRect.Width, 24);
                g.DrawString("Units", titleFont, textBrush, titleRect, centerFormat);
                y += titleAreaHeight;

                var ownedLabelRect = new RectangleF(panelRect.X + 16, y, panelRect.Width - 32, labelHeight);
                g.DrawString("Owned — select one, then tap a slot", labelFont, dimTextBrush, ownedLabelRect, leftFormat);
                y += labelHeight + sectionGap;

                var ownedClipRect = new Rectangle(panelRect.X, y, panelRect.Width, visibleGridHeight);
                inventoryOwnedClipRect = ownedClipRect;
                using (var previousClip = g.Clip)
                {
                    g.SetClip(ownedClipRect);
                    DrawInventoryOwnedRow(g, labelFont, textBrush, centerFormat, panelRect, y - inventoryScrollOffset, ownedCols, slotSize, slotGap);
                    g.SetClip(previousClip, CombineMode.Replace);
                }

                if (inventoryScrollbarVisible)
                {
                    const int trackWidth = 6;
                    int trackX = panelRect.Right - 16;
                    inventoryScrollTrackRect = new Rectangle(trackX, ownedClipRect.Y, trackWidth, visibleGridHeight);

                    float thumbHeightRatio = (float)visibleGridHeight / ownedGridHeight;
                    int thumbHeight = Math.Max(20, (int)(visibleGridHeight * thumbHeightRatio));
                    int thumbTravel = Math.Max(0, visibleGridHeight - thumbHeight);
                    int thumbY = ownedClipRect.Y + (inventoryScrollMaxOffset > 0
                        ? (int)((float)inventoryScrollOffset / inventoryScrollMaxOffset * thumbTravel)
                        : 0);
                    inventoryScrollThumbRect = new Rectangle(trackX, thumbY, trackWidth, thumbHeight);

                    using (var trackBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                    using (var thumbBrush = new SolidBrush(Color.FromArgb(220, 210, 210, 210)))
                    {
                        g.FillRectangle(trackBrush, inventoryScrollTrackRect);
                        g.FillRectangle(thumbBrush, inventoryScrollThumbRect);
                    }
                }
                else
                {
                    inventoryScrollTrackRect = Rectangle.Empty;
                    inventoryScrollThumbRect = Rectangle.Empty;
                }

                y += visibleGridHeight + sectionGap;

                var loadoutLabelRect = new RectangleF(panelRect.X + 16, y, panelRect.Width - 32, labelHeight);
                g.DrawString("Loadout (keys 1-6) — tap empty-handed to clear", labelFont, dimTextBrush, loadoutLabelRect, leftFormat);
                y += labelHeight + sectionGap;

                DrawInventoryLoadoutRow(g, labelFont, textBrush, centerFormat, panelRect, y);

                inventoryCloseButtonRect = new Rectangle(panelRect.X + (panelRect.Width - 80) / 2, panelRect.Bottom - 38, 80, 26);
                using (var closeBrush = new SolidBrush(Color.Firebrick))
                {
                    g.FillRectangle(closeBrush, inventoryCloseButtonRect);
                    g.DrawString("Close", labelFont, textBrush, inventoryCloseButtonRect, centerFormat);
                }
            }

            g.SmoothingMode = SmoothingMode.None;
        }

        private void DrawInventoryOwnedRow(Graphics g, Font font, Brush textBrush, StringFormat centerFormat, Rectangle panelRect, int rowY, int columns, int slotSize, int slotGap)
        {
            var ownedUnits = new List<UnitId>();
            foreach (var kvp in playerState.OwnedShapeCounts)
            {
                if (kvp.Value > 0)
                {
                    ownedUnits.Add(kvp.Key);
                }
            }

            inventoryOwnedSlots = new (Rectangle, UnitId)[ownedUnits.Count];

            if (ownedUnits.Count == 0)
            {
                var emptyRect = new RectangleF(panelRect.X + 16, rowY, panelRect.Width - 32, 20);
                g.DrawString("No units yet — pull the gacha to get some.", font, textBrush, emptyRect, centerFormat);
                return;
            }

            int rowWidth = columns * slotSize + (columns - 1) * slotGap;
            int startX = panelRect.X + (panelRect.Width - rowWidth) / 2;

            using (var emptyBrush = new SolidBrush(Color.FromArgb(90, 255, 255, 255)))
            using (var emptyPen = new Pen(Color.Gainsboro, 1.5f))
            using (var selectedPen = new Pen(Color.Gold, 2.5f))
            {
                for (int i = 0; i < ownedUnits.Count; i++)
                {
                    int col = i % columns;
                    int row = i / columns;
                    var rect = new Rectangle(startX + col * (slotSize + slotGap), rowY + row * (slotSize + slotGap), slotSize, slotSize);
                    UnitId unit = ownedUnits[i];
                    inventoryOwnedSlots[i] = (rect, unit);

                    g.FillRectangle(emptyBrush, rect);
                    g.DrawRectangle(selectedInventoryUnit == unit ? selectedPen : emptyPen, rect);

                    var iconRect = new Rectangle(rect.X, rect.Y + 4, rect.Width, rect.Height - 18);
                    DrawShapeGlyph(g, unit.Type, unit.Color, iconRect, 0, 255);

                    var countRect = new Rectangle(rect.X, rect.Bottom - 16, rect.Width, 14);
                    g.DrawString($"x{playerState.OwnedShapeCounts[unit]}", font, textBrush, countRect, centerFormat);
                }
            }
        }

        private void DrawInventoryLoadoutRow(Graphics g, Font font, Brush textBrush, StringFormat centerFormat, Rectangle panelRect, int rowY)
        {
            const int slotSize = 44;
            const int slotGap = 8;
            int totalWidth = HotbarSlotCount * slotSize + (HotbarSlotCount - 1) * slotGap;
            int startX = panelRect.X + (panelRect.Width - totalWidth) / 2;

            using (var emptyBrush = new SolidBrush(Color.FromArgb(90, 255, 255, 255)))
            using (var emptyPen = new Pen(Color.Gainsboro, 1.5f))
            using (var filledPen = new Pen(Color.Gold, 2f))
            using (var smallFont = new Font(font.FontFamily, 7.5f))
            {
                for (int i = 0; i < HotbarSlotCount; i++)
                {
                    var rect = new Rectangle(startX + i * (slotSize + slotGap), rowY, slotSize, slotSize);
                    inventoryLoadoutSlotRects[i] = rect;

                    UnitId? slotUnit = hotbarSlotUnits[i];

                    g.FillRectangle(emptyBrush, rect);
                    g.DrawRectangle(slotUnit.HasValue ? filledPen : emptyPen, rect);

                    if (slotUnit.HasValue)
                    {
                        var iconRect = new Rectangle(rect.X, rect.Y + 2, rect.Width, rect.Height - 14);
                        DrawShapeGlyph(g, slotUnit.Value.Type, slotUnit.Value.Color, iconRect, 0, 255);
                    }

                    var numberRect = new Rectangle(rect.X, rect.Bottom - 14, rect.Width, 12);
                    g.DrawString((i + 1).ToString(), smallFont, textBrush, numberRect, centerFormat);
                }
            }
        }

        private void DrawStageIntroOverlay(Graphics g)
        {
            int panelWidth = gamePanel.ClientSize.Width;
            int panelHeight = gamePanel.ClientSize.Height;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingMode = CompositingMode.SourceOver;

            using (var dimBrush = new SolidBrush(Color.FromArgb(170, 0, 0, 0)))
            {
                g.FillRectangle(dimBrush, 0, 0, panelWidth, panelHeight);
            }

            const int boxWidth = 400;
            const int boxHeight = 230;
            var boxRect = new Rectangle((panelWidth - boxWidth) / 2, (panelHeight - boxHeight) / 2, boxWidth, boxHeight);

            Color tierColor = currentTier switch
            {
                DifficultyTier.Easy => Color.LightGreen,
                DifficultyTier.Normal => Color.Gold,
                DifficultyTier.Hard => Color.Orange,
                _ => Color.OrangeRed
            };

            using (var bgBrush = new SolidBrush(Color.FromArgb(235, 25, 25, 30)))
            using (var borderPen = new Pen(Color.White, 2f))
            using (var titleFont = new Font("Segoe UI", 24f, FontStyle.Bold))
            using (var infoFont = new Font("Segoe UI", 11f))
            using (var buttonFont = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var hintFont = new Font("Segoe UI", 8.5f))
            using (var whiteBrush = new SolidBrush(Color.White))
            using (var tierBrush = new SolidBrush(tierColor))
            using (var hintBrush = new SolidBrush(Color.Silver))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.FillRectangle(bgBrush, boxRect);
                g.DrawRectangle(borderPen, boxRect);

                g.DrawString($"STAGE {currentStage}", titleFont, whiteBrush, new RectangleF(boxRect.X, boxRect.Y + 14, boxRect.Width, 44), format);
                g.DrawString($"Difficulty: {currentTier}", infoFont, tierBrush, new RectangleF(boxRect.X, boxRect.Y + 62, boxRect.Width, 22), format);

                int totalWaves = enemyManager?.TotalWaves ?? 0;
                int baseHp = enemyManager?.BaseHealth ?? 0;
                g.DrawString($"{totalWaves} waves   \u2022   Base HP: {baseHp}", infoFont, whiteBrush, new RectangleF(boxRect.X, boxRect.Y + 88, boxRect.Width, 22), format);

                stageIntroStartRect = new Rectangle(boxRect.X + (boxRect.Width - 140) / 2, boxRect.Y + 130, 140, 38);
                using (var buttonBrush = new SolidBrush(Color.SeaGreen))
                {
                    g.FillRectangle(buttonBrush, stageIntroStartRect);
                    g.DrawString("START", buttonFont, whiteBrush, stageIntroStartRect, format);
                }

                g.DrawString("Place your towers, then press Start (or Enter / Space)", hintFont, hintBrush, new RectangleF(boxRect.X, boxRect.Bottom - 40, boxRect.Width, 30), format);
            }

            g.SmoothingMode = SmoothingMode.None;
        }

        private void DrawGameOverOverlay(Graphics g)
        {
            int panelWidth = gamePanel.ClientSize.Width;
            int panelHeight = gamePanel.ClientSize.Height;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingMode = CompositingMode.SourceOver;

            using (var dimBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
            {
                g.FillRectangle(dimBrush, 0, 0, panelWidth, panelHeight);
            }

            const int boxWidth = 360;
            const int boxHeight = 120;
            var boxRect = new Rectangle((panelWidth - boxWidth) / 2, (panelHeight - boxHeight) / 2, boxWidth, boxHeight);

            using (var bgBrush = new SolidBrush(Color.FromArgb(235, 30, 20, 20)))
            using (var borderPen = new Pen(Color.Firebrick, 2f))
            using (var titleFont = new Font("Segoe UI", 22f, FontStyle.Bold))
            using (var subFont = new Font("Segoe UI", 10f))
            using (var titleBrush = new SolidBrush(Color.OrangeRed))
            using (var subBrush = new SolidBrush(Color.White))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.FillRectangle(bgBrush, boxRect);
                g.DrawRectangle(borderPen, boxRect);

                var titleRect = new RectangleF(boxRect.X, boxRect.Y + 10, boxRect.Width, 60);
                g.DrawString("STAGE LOST", titleFont, titleBrush, titleRect, format);

                var subRect = new RectangleF(boxRect.X, boxRect.Y + 70, boxRect.Width, 36);
                g.DrawString("Your base was destroyed.", subFont, subBrush, subRect, format);
            }

            g.SmoothingMode = SmoothingMode.None;
        }

        private static RectangleF Indent(RectangleF rect, float amount)
        {
            return new RectangleF(rect.X + amount, rect.Y, rect.Width, rect.Height);
        }

        private static string FormatTime(float totalSeconds)
        {
            int seconds = (int)totalSeconds;
            int minutes = seconds / 60;
            seconds %= 60;
            return $"{minutes}:{seconds:D2}";
        }

        private void DrawPlacedShapes(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (placedShapes.Count == 0)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var shape in placedShapes)
            {
                Rectangle tileRect = new Rectangle(
                    offsetX + shape.Col * tileSize,
                    offsetY + shape.Row * tileSize,
                    tileSize,
                    tileSize
                );
                float displayRotation = (shape.Type == ShapeType.Triangle || shape.Type == ShapeType.Pentagon) ? shape.FacingDegrees
                    : shape.Type == ShapeType.Hexagon ? 0f
                    : shape.Rotation;
                DrawShapeGlyph(g, shape.Type, shape.Color, tileRect, displayRotation, 255);

                DrawCooldownOverlay(g, shape, tileRect, displayRotation);
                DrawSpeedBuffBadge(g, shape, tileRect);
                DrawDamageBuffBadge(g, shape, tileRect);
                DrawChargedBadge(g, shape, tileRect);

                if (shape.Type == ShapeType.Pentagon)
                {
                    DrawPentagonOrbitShards(g, shape, tileSize, offsetX, offsetY);
                }
            }

            g.SmoothingMode = SmoothingMode.None;
        }
        private void DrawSpeedBuffBadge(Graphics g, PlacedShape shape, Rectangle tileRect)
        {
            if (shape.AttackSpeedBuffTimeRemaining <= 0f)
            {
                return;
            }

            var previousCompositing = g.CompositingMode;
            g.CompositingMode = CompositingMode.SourceOver;

            int badgeSize = Math.Max(10, tileRect.Width / 3);
            var badgeRect = new Rectangle(tileRect.Right - badgeSize - 1, tileRect.Y + 1, badgeSize, badgeSize);

            using (var badgeBrush = new SolidBrush(Color.FromArgb(230, 60, 220, 90)))
            using (var badgePen = new Pen(Color.FromArgb(230, 20, 90, 30), 1f))
            {
                g.FillEllipse(badgeBrush, badgeRect);
                g.DrawEllipse(badgePen, badgeRect);
            }

            using (var boltFont = new Font("Segoe UI", badgeSize * 0.65f, FontStyle.Bold))
            using (var boltBrush = new SolidBrush(Color.FromArgb(230, 20, 60, 20)))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString("\u26A1", boltFont, boltBrush, badgeRect, centerFormat);
            }

            g.CompositingMode = previousCompositing;
        }
        private void DrawDamageBuffBadge(Graphics g, PlacedShape shape, Rectangle tileRect)
        {
            if (shape.AttackDamageBuffTimeRemaining <= 0f)
            {
                return;
            }

            var previousCompositing = g.CompositingMode;
            g.CompositingMode = CompositingMode.SourceOver;

            int badgeSize = Math.Max(10, tileRect.Width / 3);
            var badgeRect = new Rectangle(tileRect.X + 1, tileRect.Y + 1, badgeSize, badgeSize);

            using (var badgeBrush = new SolidBrush(Color.FromArgb(230, 230, 110, 40)))
            using (var badgePen = new Pen(Color.FromArgb(230, 100, 40, 10), 1f))
            {
                g.FillEllipse(badgeBrush, badgeRect);
                g.DrawEllipse(badgePen, badgeRect);
            }

            using (var swordFont = new Font("Segoe UI", badgeSize * 0.55f, FontStyle.Bold))
            using (var swordBrush = new SolidBrush(Color.FromArgb(230, 60, 25, 5)))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString("\u2694", swordFont, swordBrush, badgeRect, centerFormat);
            }

            g.CompositingMode = previousCompositing;
        }

        private void DrawChargedBadge(Graphics g, PlacedShape shape, Rectangle tileRect)
        {
            if (!shape.HasBankedCrit)
            {
                return;
            }

            var previousCompositing = g.CompositingMode;
            g.CompositingMode = CompositingMode.SourceOver;

            int badgeSize = Math.Max(10, tileRect.Width / 3);
            var badgeRect = new Rectangle(tileRect.X + 1, tileRect.Bottom - badgeSize - 1, badgeSize, badgeSize);

            using (var badgeBrush = new SolidBrush(Color.FromArgb(230, 235, 235, 245)))
            using (var badgePen = new Pen(Color.FromArgb(230, 120, 120, 60), 1f))
            {
                g.FillEllipse(badgeBrush, badgeRect);
                g.DrawEllipse(badgePen, badgeRect);
            }

            using (var starFont = new Font("Segoe UI", badgeSize * 0.6f, FontStyle.Bold))
            using (var starBrush = new SolidBrush(Color.FromArgb(230, 150, 130, 20)))
            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString("\u2726", starFont, starBrush, badgeRect, centerFormat);
            }

            g.CompositingMode = previousCompositing;
        }

        private void DrawCooldownOverlay(Graphics g, PlacedShape shape, Rectangle tileRect, float rotationDegrees)
        {
            if (shape.AttackCooldownRemaining <= 0f)
            {
                return;
            }
            float interval = shape.GetCurrentAttackInterval();
            if (interval <= 0f)
            {
                return;
            }

            float ratio = shape.AttackCooldownRemaining / interval;
            ratio = Math.Max(0f, Math.Min(1f, ratio));
            if (ratio <= 0f)
            {
                return;
            }

            using (var path = BuildShapeGlyphPath(shape.Type, tileRect))
            {
                if (path.PointCount == 0)
                {
                    return;
                }

                float centerX = tileRect.X + tileRect.Width / 2f;
                float centerY = tileRect.Y + tileRect.Height / 2f;

                using (var rotation = new Matrix())
                {
                    rotation.RotateAt(rotationDegrees, new PointF(centerX, centerY));
                    path.Transform(rotation);
                }

                float overlayHeight = tileRect.Height * ratio;
                var overlayRect = new RectangleF(tileRect.X, tileRect.Bottom - overlayHeight, tileRect.Width, overlayHeight);

                using (var shapeRegion = new Region(path))
                {
                    shapeRegion.Intersect(overlayRect);

                    var previousCompositing = g.CompositingMode;
                    g.CompositingMode = CompositingMode.SourceOver;

                    using (var overlayBrush = new SolidBrush(Color.FromArgb(150, 90, 90, 90)))
                    {
                        g.FillRegion(overlayBrush, shapeRegion);
                    }

                    g.CompositingMode = previousCompositing;
                }
            }
        }
        private void DrawShotEffects(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (towerManager.ActiveShotEffects.Count == 0)
            {
                return;
            }

            var previousCompositing = g.CompositingMode;
            var previousSmoothing = g.SmoothingMode;
            g.CompositingMode = CompositingMode.SourceOver;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var effect in towerManager.ActiveShotEffects)
            {
                float progress = effect.Duration > 0f ? Math.Min(1f, effect.Age / effect.Duration) : 1f;
                int alpha = (int)(220 * (1f - progress));
                if (alpha <= 0)
                {
                    continue;
                }

                PointF from = new PointF(offsetX + effect.FromCol * tileSize, offsetY + effect.FromRow * tileSize);
                PointF to = new PointF(offsetX + effect.ToCol * tileSize, offsetY + effect.ToRow * tileSize);

                Color beamColor = GetShapeDisplayColor(effect.Color);
                float corridorPixelHalfWidth = effect.CorridorHalfWidth * tileSize;
                if (corridorPixelHalfWidth > 1f)
                {
                    float dx = to.X - from.X;
                    float dy = to.Y - from.Y;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy);
                    if (length > 0.0001f)
                    {
                        float perpX = -dy / length * corridorPixelHalfWidth;
                        float perpY = dx / length * corridorPixelHalfWidth;

                        PointF[] corridorQuad =
                        {
                            new PointF(from.X + perpX, from.Y + perpY),
                            new PointF(to.X + perpX, to.Y + perpY),
                            new PointF(to.X - perpX, to.Y - perpY),
                            new PointF(from.X - perpX, from.Y - perpY)
                        };

                        int corridorAlpha = Math.Max(0, (int)(alpha * 0.35f));
                        using (var corridorBrush = new SolidBrush(Color.FromArgb(corridorAlpha, beamColor)))
                        {
                            g.FillPolygon(corridorBrush, corridorQuad);
                        }
                    }
                }

                float beamWidth = Math.Max(2f, tileSize * 0.12f);

                using (var beamPen = new Pen(Color.FromArgb(alpha, beamColor), beamWidth))
                {
                    beamPen.StartCap = LineCap.Round;
                    beamPen.EndCap = LineCap.Round;
                    g.DrawLine(beamPen, from, to);
                }
            }

            g.SmoothingMode = previousSmoothing;
            g.CompositingMode = previousCompositing;
        }
        private void DrawBombEffects(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (towerManager.ActiveBombEffects.Count == 0)
            {
                return;
            }

            var previousCompositing = g.CompositingMode;
            var previousSmoothing = g.SmoothingMode;
            g.CompositingMode = CompositingMode.SourceOver;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var effect in towerManager.ActiveBombEffects)
            {
                float progress = effect.Duration > 0f ? Math.Min(1f, effect.Age / effect.Duration) : 1f;
                int alpha = (int)(220 * (1f - progress));
                if (alpha <= 0)
                {
                    continue;
                }

                float centerX = offsetX + effect.Col * tileSize;
                float centerY = offsetY + effect.Row * tileSize;
                Color blastColor = GetShapeDisplayColor(effect.Color);
                float ringRadiusPixels = effect.Radius * tileSize * Math.Min(1f, progress * 1.6f);
                var ringRect = new RectangleF(centerX - ringRadiusPixels, centerY - ringRadiusPixels, ringRadiusPixels * 2f, ringRadiusPixels * 2f);

                using (var ringFillBrush = new SolidBrush(Color.FromArgb(Math.Max(0, (int)(alpha * 0.3f)), blastColor)))
                using (var ringPen = new Pen(Color.FromArgb(alpha, blastColor), Math.Max(2f, tileSize * 0.08f)))
                {
                    g.FillEllipse(ringFillBrush, ringRect);
                    g.DrawEllipse(ringPen, ringRect);
                }
                if (progress < 0.5f)
                {
                    float shrinkProgress = progress / 0.5f;
                    float squareHalfPixels = tileSize * 0.28f * (1f - shrinkProgress);
                    var squareRect = new RectangleF(centerX - squareHalfPixels, centerY - squareHalfPixels, squareHalfPixels * 2f, squareHalfPixels * 2f);
                    using (var squareBrush = new SolidBrush(Color.FromArgb(alpha, blastColor)))
                    using (var squareOutline = new Pen(Color.FromArgb(alpha, GetShapeOutlineColor(effect.Color)), 1f))
                    {
                        g.FillRectangle(squareBrush, squareRect);
                        g.DrawRectangle(squareOutline, squareRect.X, squareRect.Y, squareRect.Width, squareRect.Height);
                    }
                }
            }

            g.SmoothingMode = previousSmoothing;
            g.CompositingMode = previousCompositing;
        }

        private void DrawSparkEffects(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (towerManager.ActiveSparkEffects.Count == 0)
            {
                return;
            }

            var previousCompositing = g.CompositingMode;
            var previousSmoothing = g.SmoothingMode;
            g.CompositingMode = CompositingMode.SourceOver;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var effect in towerManager.ActiveSparkEffects)
            {
                float progress = effect.Duration > 0f ? Math.Min(1f, effect.Age / effect.Duration) : 1f;
                int alpha = (int)(220 * (1f - progress));
                if (alpha <= 0)
                {
                    continue;
                }

                float centerX = offsetX + effect.Col * tileSize;
                float centerY = offsetY + effect.Row * tileSize;
                Color sparkColor = GetShapeDisplayColor(effect.Color);
                float sparkRadiusPixels = effect.Radius * tileSize * (1f + progress * 0.8f);
                var sparkRect = new RectangleF(centerX - sparkRadiusPixels, centerY - sparkRadiusPixels, sparkRadiusPixels * 2f, sparkRadiusPixels * 2f);

                using (var sparkBrush = new SolidBrush(Color.FromArgb(Math.Max(0, (int)(alpha * 0.6f)), sparkColor)))
                using (var sparkPen = new Pen(Color.FromArgb(alpha, Color.White), 1.2f))
                {
                    g.FillEllipse(sparkBrush, sparkRect);
                    g.DrawEllipse(sparkPen, sparkRect);
                }
            }

            g.SmoothingMode = previousSmoothing;
            g.CompositingMode = previousCompositing;
        }
        private static PointF[] BuildHexagonPoints(float centerX, float centerY, float radius)
        {
            var points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = Math.PI * 2 * i / 6;
                points[i] = new PointF(
                    centerX + radius * (float)Math.Cos(angle),
                    centerY + radius * (float)Math.Sin(angle)
                );
            }
            return points;
        }
        private void DrawSummons(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (towerManager.ActiveSummons.Count == 0)
            {
                return;
            }

            var previousCompositing = g.CompositingMode;
            var previousSmoothing = g.SmoothingMode;
            g.CompositingMode = CompositingMode.SourceOver;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var summon in towerManager.ActiveSummons)
            {
                float centerX = offsetX + summon.Col * tileSize;
                float centerY = offsetY + summon.Row * tileSize;
                float radius = Math.Max(4f, TowerManager.SummonRadiusTiles * tileSize);
                Color fillColor = GetShapeDisplayColor(summon.Color);

                if (summon.Color == ShapeColor.White && summon.AuraRadius > 0f)
                {
                    float auraPixels = summon.AuraRadius * tileSize;
                    var auraRect = new RectangleF(centerX - auraPixels, centerY - auraPixels, auraPixels * 2f, auraPixels * 2f);
                    using (var auraBrush = new SolidBrush(Color.FromArgb(28, Color.White)))
                    using (var auraPen = new Pen(Color.FromArgb(90, Color.White), 1f))
                    {
                        g.FillEllipse(auraBrush, auraRect);
                        g.DrawEllipse(auraPen, auraRect);
                    }
                }

                var points = BuildHexagonPoints(centerX, centerY, radius);
                using (var bodyBrush = new SolidBrush(Color.FromArgb(235, fillColor)))
                using (var bodyPen = new Pen(Color.FromArgb(235, GetShapeOutlineColor(summon.Color)), 1.2f))
                {
                    g.FillPolygon(bodyBrush, points);
                    g.DrawPolygon(bodyPen, points);
                }

                float hpFraction = summon.MaxHealth > 0f ? Math.Max(0f, Math.Min(1f, summon.Health / summon.MaxHealth)) : 0f;
                float barWidth = radius * 2f;
                float barHeight = Math.Max(2f, tileSize * 0.06f);
                var barRect = new RectangleF(centerX - radius, centerY - radius - barHeight - 2f, barWidth, barHeight);
                using (var barBackBrush = new SolidBrush(Color.FromArgb(150, 20, 20, 20)))
                using (var barFillBrush = new SolidBrush(Color.FromArgb(230, Color.LimeGreen)))
                {
                    g.FillRectangle(barBackBrush, barRect);
                    g.FillRectangle(barFillBrush, barRect.X, barRect.Y, barRect.Width * hpFraction, barRect.Height);
                }
            }

            g.SmoothingMode = previousSmoothing;
            g.CompositingMode = previousCompositing;
        }

        private void DrawPentagonOrbitShards(Graphics g, PlacedShape shape, int tileSize, int offsetX, int offsetY)
        {
            var (orbCount, orbRadiusTiles, _) = TowerManager.GetPentagonOrbProfile(shape.Color);
            float orbitRadiusTiles = TowerManager.GetPentagonOrbitRadius(shape);
            var orbPositions = TowerManager.ComputePentagonOrbPositions(shape, orbCount, orbitRadiusTiles);

            Color shardColor = GetShapeDisplayColor(shape.Color);
            float shardRadiusPixels = Math.Max(4f, orbRadiusTiles * tileSize);

            var previousCompositing = g.CompositingMode;
            g.CompositingMode = CompositingMode.SourceOver;

            using (var shardBrush = new SolidBrush(Color.FromArgb(235, shardColor)))
            using (var shardOutline = new Pen(Color.FromArgb(235, GetShapeOutlineColor(shape.Color)), 1f))
            {
                foreach (var (orbRow, orbCol) in orbPositions)
                {
                    float shardX = offsetX + orbCol * tileSize;
                    float shardY = offsetY + orbRow * tileSize;
                    var shardRect = new RectangleF(shardX - shardRadiusPixels, shardY - shardRadiusPixels, shardRadiusPixels * 2f, shardRadiusPixels * 2f);
                    g.FillEllipse(shardBrush, shardRect);
                    g.DrawEllipse(shardOutline, shardRect);
                }
            }

            g.CompositingMode = previousCompositing;
        }

        private void DrawSelectedRange(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (selectedPlacedShapeForRange == null)
            {
                return;
            }

            if (!placedShapes.Contains(selectedPlacedShapeForRange))
            {
                selectedPlacedShapeForRange = null;
                return;
            }

            var shape = selectedPlacedShapeForRange;
            Rectangle tileRect = new Rectangle(
                offsetX + shape.Col * tileSize,
                offsetY + shape.Row * tileSize,
                tileSize,
                tileSize
            );
            g.CompositingMode = CompositingMode.SourceOver;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var highlightPen = new Pen(Color.FromArgb(150, Color.Gold), 2f))
            {
                g.DrawRectangle(highlightPen, tileRect);
            }

            if (shape.Type == ShapeType.Triangle)
            {
                DrawShapeRangeRectangle(g, offsetX, offsetY, tileSize, shape.Row, shape.Col, shape.Type, shape.Color, shape.Rotation, shape.GetRangeMultiplier(), fillAlpha: 60, outlineAlpha: 210);
            }
            else if (shape.Type == ShapeType.Square)
            {
                DrawShapeRangeRectangle(g, offsetX, offsetY, tileSize, shape.Row, shape.Col, shape.Type, shape.Color, shape.Rotation, shape.GetRangeMultiplier(), fillAlpha: 55, outlineAlpha: 200);
                float blastRadiusTiles = shape.GetCurrentBlastRadius();
                float centerX = offsetX + (shape.Col + 0.5f) * tileSize;
                float centerY = offsetY + (shape.Row + 0.5f) * tileSize;
                float blastPixelRadius = blastRadiusTiles * tileSize;
                var blastRect = new RectangleF(centerX - blastPixelRadius, centerY - blastPixelRadius, blastPixelRadius * 2f, blastPixelRadius * 2f);
                using (var blastFillBrush = new SolidBrush(Color.FromArgb(35, GetShapeDisplayColor(shape.Color))))
                using (var blastOutlinePen = new Pen(Color.FromArgb(160, GetShapeDisplayColor(shape.Color)), 1.5f))
                {
                    g.FillEllipse(blastFillBrush, blastRect);
                    g.DrawEllipse(blastOutlinePen, blastRect);
                }
            }
            else if (shape.Type == ShapeType.Pentagon)
            {
                DrawOrbitRingPreview(g, offsetX, offsetY, tileSize, shape.Row, shape.Col, TowerManager.GetPentagonOrbitRadius(shape), shape.Color, fillAlpha: 50, outlineAlpha: 210);
            }
            else if (shape.Type == ShapeType.Line && shape.Color == ShapeColor.White)
            {
                var tier = TowerUpgradeConfig.GetTier(shape.Type, shape.Color, shape.UpgradeLevel);
                int radius = Math.Max(1, (int)Math.Round(1f * tier.RangeMultiplier));

                using (var fillBrush = new SolidBrush(Color.FromArgb(45, Color.Azure)))
                using (var outlinePen = new Pen(Color.FromArgb(150, Color.Azure), 1.5f))
                {
                    for (int dr = -radius; dr <= radius; dr++)
                    {
                        for (int dc = -radius; dc <= radius; dc++)
                        {
                            int r = shape.Row + dr;
                            int c = shape.Col + dc;
                            if (r < 0 || c < 0 || r >= currentFloor.Rows || c >= currentFloor.Cols) continue;
                            var rect = new Rectangle(offsetX + c * tileSize, offsetY + r * tileSize, tileSize, tileSize);
                            g.FillRectangle(fillBrush, rect);
                            g.DrawRectangle(outlinePen, rect);
                        }
                    }
                }
            }

            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceCopy;
        }
        private void DrawShapeRangeRectangle(Graphics g, int offsetX, int offsetY, int tileSize, int row, int col, ShapeType type, ShapeColor color, int rotationDegrees, float rangeMultiplier, int fillAlpha, int outlineAlpha)
        {
            var (halfWidthTiles, halfHeightTiles) = type == ShapeType.Square
                ? TowerManager.GetOrientedSquareRangeExtents(color, rotationDegrees, rangeMultiplier)
                : TowerManager.GetOrientedRangeExtents(color, rotationDegrees, rangeMultiplier);
            float towerCenterX = offsetX + (col + 0.5f) * tileSize;
            float towerCenterY = offsetY + (row + 0.5f) * tileSize;
            float halfWidthPixels = halfWidthTiles * tileSize;
            float halfHeightPixels = halfHeightTiles * tileSize;

            var rangeRect = new RectangleF(
                towerCenterX - halfWidthPixels,
                towerCenterY - halfHeightPixels,
                halfWidthPixels * 2f,
                halfHeightPixels * 2f
            );

            Color rangeColor = GetRangeIndicatorColor(color);
            using (var fillBrush = new SolidBrush(Color.FromArgb(fillAlpha, rangeColor)))
            using (var outlinePen = new Pen(Color.FromArgb(outlineAlpha, rangeColor), 2.2f))
            {
                g.FillRectangle(fillBrush, rangeRect);
                g.DrawRectangle(outlinePen, rangeRect.X, rangeRect.Y, rangeRect.Width, rangeRect.Height);
            }
        }

        private void DrawOrbitRingPreview(Graphics g, int offsetX, int offsetY, int tileSize, int row, int col, float orbitRadiusTiles, ShapeColor color, int fillAlpha, int outlineAlpha)
        {
            float centerX = offsetX + (col + 0.5f) * tileSize;
            float centerY = offsetY + (row + 0.5f) * tileSize;
            float radiusPixels = orbitRadiusTiles * tileSize;
            var ringRect = new RectangleF(centerX - radiusPixels, centerY - radiusPixels, radiusPixels * 2f, radiusPixels * 2f);
            Color ringColor = GetRangeIndicatorColor(color);

            using (var fillBrush = new SolidBrush(Color.FromArgb(fillAlpha, ringColor)))
            using (var outlinePen = new Pen(Color.FromArgb(outlineAlpha, ringColor), 2.2f))
            {
                g.FillEllipse(fillBrush, ringRect);
                g.DrawEllipse(outlinePen, ringRect);
            }
        }

        private void DrawPlacementPreview(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (selectedHotbarSlot == -1 || hoveredMouseLocation == null || currentFloor == null)
            {
                return;
            }

            UnitId? slotUnit = hotbarSlotUnits[selectedHotbarSlot];
            if (slotUnit == null)
            {
                return;
            }

            Point mouse = hoveredMouseLocation.Value;
            if (mouse.X < offsetX || mouse.Y < offsetY)
            {
                return;
            }

            int col = (mouse.X - offsetX) / tileSize;
            int row = (mouse.Y - offsetY) / tileSize;

            if (row < 0 || row >= currentFloor.Rows || col < 0 || col >= currentFloor.Cols)
            {
                return;
            }

            Rectangle tileRect = new Rectangle(
                offsetX + col * tileSize,
                offsetY + row * tileSize,
                tileSize,
                tileSize
            );

            if (slotUnit.Value.Type == ShapeType.Triangle || slotUnit.Value.Type == ShapeType.Square)
            {
                var previousCompositing = g.CompositingMode;
                g.CompositingMode = CompositingMode.SourceOver;
                DrawShapeRangeRectangle(g, offsetX, offsetY, tileSize, row, col, slotUnit.Value.Type, slotUnit.Value.Color, selectedRotation, rangeMultiplier: CollectionUpgradeConfig.GetRangeMultiplier(hotbarSlotLevels[selectedHotbarSlot]), fillAlpha: 65, outlineAlpha: 220);
                g.CompositingMode = previousCompositing;
            }
            else if (slotUnit.Value.Type == ShapeType.Pentagon)
            {
                var (_, baseOrbitRange, _, _) = TowerUpgradeConfig.GetBaseStats(slotUnit.Value.Type, slotUnit.Value.Color, hotbarSlotLevels[selectedHotbarSlot]);
                var previousCompositing = g.CompositingMode;
                g.CompositingMode = CompositingMode.SourceOver;
                DrawOrbitRingPreview(g, offsetX, offsetY, tileSize, row, col, baseOrbitRange, slotUnit.Value.Color, fillAlpha: 60, outlineAlpha: 220);
                g.CompositingMode = previousCompositing;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawShapeGlyph(g, slotUnit.Value.Type, slotUnit.Value.Color, tileRect, selectedRotation, 110);
            g.SmoothingMode = SmoothingMode.None;
        }
        private static bool RequiresElevatedGround(UnitId unit)
        {
            return unit.Color == ShapeColor.Blue && (unit.Type == ShapeType.Triangle || unit.Type == ShapeType.Square || unit.Type == ShapeType.Pentagon || unit.Type == ShapeType.Hexagon);
        }
        private static Color GetShapeDisplayColor(ShapeColor color)
        {
            return color switch
            {
                ShapeColor.Black => Color.FromArgb(40, 40, 40),
                ShapeColor.White => Color.White,
                ShapeColor.Red => Color.Firebrick,
                ShapeColor.Purple => Color.MediumPurple,
                ShapeColor.Cyan => Color.Cyan,
                ShapeColor.Green => Color.LimeGreen,
                ShapeColor.Blue => Color.DodgerBlue,
                ShapeColor.Yellow => Color.Gold,
                _ => Color.Gainsboro
            };
        }
        private static Color GetShapeOutlineColor(ShapeColor color)
        {
            return color == ShapeColor.Black ? Color.Gainsboro : Color.Black;
        }
        private static Color GetRangeIndicatorColor(ShapeColor color)
        {
            return color == ShapeColor.Black ? Color.FromArgb(190, 190, 200) : GetShapeDisplayColor(color);
        }
        private static GraphicsPath BuildShapeGlyphPath(ShapeType type, Rectangle tileRect)
        {
            var path = new GraphicsPath();

            switch (type)
            {
                case ShapeType.Line:
                    {
                        int barHeight = Math.Max(3, tileRect.Height / 6);
                        var barRect = new Rectangle(
                            tileRect.X + 2,
                            tileRect.Y + (tileRect.Height - barHeight) / 2,
                            tileRect.Width - 4,
                            barHeight
                        );
                        path.AddRectangle(barRect);
                        break;
                    }

                case ShapeType.Triangle:
                    {
                        int pad = Math.Max(4, tileRect.Width / 8);
                        var apex = new Point(tileRect.X + tileRect.Width / 2, tileRect.Y + pad);
                        var baseLeft = new Point(tileRect.X + pad, tileRect.Bottom - pad);
                        var baseRight = new Point(tileRect.Right - pad, tileRect.Bottom - pad);
                        path.AddPolygon(new[] { apex, baseLeft, baseRight });
                        break;
                    }

                case ShapeType.Square:
                    {
                        int pad = Math.Max(4, tileRect.Width / 7);
                        var squareRect = new Rectangle(
                            tileRect.X + pad,
                            tileRect.Y + pad,
                            tileRect.Width - pad * 2,
                            tileRect.Height - pad * 2
                        );
                        path.AddRectangle(squareRect);
                        break;
                    }

                case ShapeType.Pentagon:
                    {
                        int pad = Math.Max(4, tileRect.Width / 8);
                        float centerX = tileRect.X + tileRect.Width / 2f;
                        float centerY = tileRect.Y + tileRect.Height / 2f;
                        float radius = Math.Min(tileRect.Width, tileRect.Height) / 2f - pad;
                        var points = new PointF[5];
                        for (int i = 0; i < 5; i++)
                        {
                            double angle = (Math.PI * 2 * i / 5) - (Math.PI / 2);
                            points[i] = new PointF(
                                centerX + radius * (float)Math.Cos(angle),
                                centerY + radius * (float)Math.Sin(angle)
                            );
                        }
                        path.AddPolygon(points);
                        break;
                    }

                case ShapeType.Hexagon:
                    {
                        int pad = Math.Max(4, tileRect.Width / 8);
                        float centerX = tileRect.X + tileRect.Width / 2f;
                        float centerY = tileRect.Y + tileRect.Height / 2f;
                        float radius = Math.Min(tileRect.Width, tileRect.Height) / 2f - pad;
                        path.AddPolygon(BuildHexagonPoints(centerX, centerY, radius));
                        break;
                    }

                default:
                    break;
            }

            return path;
        }

        private void DrawShapeGlyph(Graphics g, ShapeType type, ShapeColor color, Rectangle tileRect, float rotationDegrees, int alpha)
        {
            float centerX = tileRect.X + tileRect.Width / 2f;
            float centerY = tileRect.Y + tileRect.Height / 2f;

            using (var path = BuildShapeGlyphPath(type, tileRect))
            {
                if (path.PointCount > 0)
                {
                    using (var rotation = new Matrix())
                    {
                        rotation.RotateAt(rotationDegrees, new PointF(centerX, centerY));
                        path.Transform(rotation);
                    }

                    Color fill = GetShapeDisplayColor(color);
                    Color outline = GetShapeOutlineColor(color);
                    float outlineWidth = type == ShapeType.Triangle ? 1.2f : 1f;

                    using (var fillBrush = new SolidBrush(Color.FromArgb(alpha, fill)))
                    using (var outlinePen = new Pen(Color.FromArgb(alpha, outline), outlineWidth))
                    {
                        g.FillPath(fillBrush, path);
                        g.DrawPath(outlinePen, path);
                    }
                }
            }
        }


        private void DrawEnemies(Graphics g, int tileSize, int offsetX, int offsetY)
        {
            if (enemyManager == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var enemy in enemyManager.ActiveEnemies)
            {
                var (row, col) = enemyManager.GetInterpolatedPosition(enemy);
                float centerX = offsetX + col * tileSize;
                float centerY = offsetY + row * tileSize;
                float radius = tileSize * GetEnemyRadiusScale(enemy.Color) * (enemy.IsBoss ? 1.6f : 1f);

                DrawEnemyHealthCircle(g, enemy, centerX, centerY, radius);
            }

            g.SmoothingMode = SmoothingMode.None;
        }

        private void DrawEnemyHealthCircle(Graphics g, Enemy enemy, float centerX, float centerY, float radius)
        {
            float hpFraction = enemy.MaxHealth > 0f ? enemy.CurrentHealth / enemy.MaxHealth : 0f;
            hpFraction = Math.Max(0f, Math.Min(1f, hpFraction));

            float diameter = radius * 2f;
            var boundingRect = new RectangleF(centerX - radius, centerY - radius, diameter, diameter);
            using (var lostBrush = new SolidBrush(Color.FromArgb(90, 40, 40, 40)))
            {
                g.FillEllipse(lostBrush, boundingRect);
            }

            if (enemy.IsBoss)
            {
                using (var bossPen = new Pen(Color.Crimson, 3f))
                {
                    g.DrawEllipse(bossPen, boundingRect);
                }
            }

            if (hpFraction > 0f)
            {
                float sweepAngle = 360f * hpFraction;
                using (var brush = new SolidBrush(GetEnemyColor(enemy.Color)))
                {
                    g.FillPie(brush, boundingRect.X, boundingRect.Y, boundingRect.Width, boundingRect.Height, -90f, sweepAngle);
                }
            }
        }

        private float GetEnemyRadiusScale(EnemyColor color)
        {
            return color switch
            {
                EnemyColor.Green => 0.35f,
                _ => 0.28f
            };
        }

        private Color GetEnemyColor(EnemyColor color)
        {
            return color switch
            {
                EnemyColor.Grey => Color.Gray,
                EnemyColor.Yellow => Color.Yellow,
                EnemyColor.Aqua => Color.Aqua,
                EnemyColor.Green => Color.Green,
                EnemyColor.Blue => Color.DodgerBlue,
                _ => Color.White
            };
        }
        private Rectangle GetTileSourceRect(Image texture)
        {
            if (tileSourceRects.TryGetValue(texture, out Rectangle cached))
            {
                return cached;
            }

            var full = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle result = full;

            using (var bmp = new Bitmap(texture))
            {
                var data = bmp.LockBits(full, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                try
                {
                    int stride = Math.Abs(data.Stride);
                    byte[] pixels = new byte[stride * bmp.Height];
                    System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);

                    int minX = bmp.Width, minY = bmp.Height, maxX = -1, maxY = -1;
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            if (pixels[y * stride + x * 4 + 3] > 128)
                            {
                                if (x < minX) minX = x;
                                if (x > maxX) maxX = x;
                                if (y < minY) minY = y;
                                if (y > maxY) maxY = y;
                            }
                        }
                    }

                    if (maxX >= minX && maxY >= minY)
                    {
                        int keepLeft = (int)(minX * TileGapKeepFraction);
                        int keepTop = (int)(minY * TileGapKeepFraction);
                        int keepRight = (int)((bmp.Width - 1 - maxX) * TileGapKeepFraction);
                        int keepBottom = (int)((bmp.Height - 1 - maxY) * TileGapKeepFraction);

                        int left = minX - keepLeft;
                        int top = minY - keepTop;
                        int right = maxX + keepRight;
                        int bottom = maxY + keepBottom;
                        result = new Rectangle(left, top, right - left + 1, bottom - top + 1);
                    }
                }
                finally
                {
                    bmp.UnlockBits(data);
                }
            }

            tileSourceRects[texture] = result;
            return result;
        }

        private Image GetTexture(TileType type)
        {
            return type switch
            {
                TileType.Path => pathTexture,
                TileType.Obstacle => obstacleTexture,
                TileType.Elevated => elevatedTexture,
                TileType.Entrance => entranceTexture,
                TileType.Exit => exitTexture,
                TileType.Open => openTexture,
                _ => openTexture
            };
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (pnlResult.Visible)
            {
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (showInventoryPanel)
                {
                    showInventoryPanel = false;
                    selectedInventoryUnit = null;
                }
                else
                {
                    showSettingsPanel = !showSettingsPanel;
                }

                panPressed = false;
                isPanningView = false;
                gamePanel.Cursor = Cursors.Default;
                gamePanel.Invalidate();
                e.Handled = true;
                return;
            }
            if (showSettingsPanel)
            {
                return;
            }

            if (showStageIntro)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
                {
                    showStageIntro = false;
                    e.Handled = true;
                    gamePanel.Invalidate();
                }
                return;
            }

            int? numberSlot = GetHotbarSlotFromKey(e.KeyCode);
            if (numberSlot.HasValue)
            {
                selectedHotbarSlot = selectedHotbarSlot == numberSlot.Value ? -1 : numberSlot.Value;
                selectedRotation = 0;
                gamePanel.Invalidate();
                return;
            }

            if (e.KeyCode == Keys.R && selectedHotbarSlot != -1)
            {
                selectedRotation = (selectedRotation + 90) % 360;
                gamePanel.Invalidate();
                return;
            }
            if (e.KeyCode == Keys.R && selectedPlacedShapeForRange != null)
            {
                selectedPlacedShapeForRange.Rotation = (selectedPlacedShapeForRange.Rotation + 90) % 360;
                gamePanel.Invalidate();
                return;
            }

            if (enemyManager == null || !enemyManager.AwaitingEarlyWaveCall)
            {
                return;
            }

            if (e.KeyCode == Keys.Y)
            {
                enemyManager.ConfirmEarlyWaveCall();
            }
            else if (e.KeyCode == Keys.N)
            {
                enemyManager.DeclineEarlyWaveCall();
            }
        }

        private int? GetHotbarSlotFromKey(Keys key)
        {
            switch (key)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    return 0;
                case Keys.D2:
                case Keys.NumPad2:
                    return 1;
                case Keys.D3:
                case Keys.NumPad3:
                    return 2;
                case Keys.D4:
                case Keys.NumPad4:
                    return 3;
                case Keys.D5:
                case Keys.NumPad5:
                    return 4;
                case Keys.D6:
                case Keys.NumPad6:
                    return 5;
                default:
                    return null;
            }
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (justFinishedPanningView)
            {
                justFinishedPanningView = false;
                return;
            }

            if (justFinishedDraggingUpgradePanel)
            {
                justFinishedDraggingUpgradePanel = false;
                return;
            }

            if (justFinishedDraggingInventoryScrollbar)
            {
                justFinishedDraggingInventoryScrollbar = false;
                return;
            }

            if (showInventoryPanel)
            {
                HandleInventoryPanelClick(e.Location);
                return;
            }

            if (showSettingsPanel)
            {
                if (autoSkipToggleRect.Contains(e.Location)) autoSkipWave = !autoSkipWave;
                else if (doubleSpeedToggleRect.Contains(e.Location)) doubleSpeed = !doubleSpeed;
                else if (musicToggleRect.Contains(e.Location)) MusicPlayer.ToggleMute();
                else if (settingsLeaveButtonRect.Contains(e.Location)) ConfirmLeaveTower();
                else if (settingsCloseButtonRect.Contains(e.Location)) showSettingsPanel = false;
                return;
            }

            if (showStageIntro)
            {
                if (stageIntroStartRect.Contains(e.Location))
                {
                    showStageIntro = false;
                }
                return;
            }

            if (isGameOver)
            {
                if (settingsButtonRect.Contains(e.Location))
                {
                    showSettingsPanel = true;
                }
                return;
            }

            if (selectedShapeForUpgrade != null)
            {
                if (closeUpgradePanelRect.Contains(e.Location))
                {
                    CloseUpgradePanel();
                    return;
                }

                if (upgradeButtonRect.Contains(e.Location))
                {
                    TryUpgradeShape(selectedShapeForUpgrade);
                    return;
                }

                if (sellButtonRect.Contains(e.Location))
                {
                    TrySellShape(selectedShapeForUpgrade);
                    return;
                }

                if (targetFirstRect.Contains(e.Location))
                {
                    selectedShapeForUpgrade.TargetingMode = TargetingMode.First;
                    return;
                }
                if (targetLastRect.Contains(e.Location))
                {
                    selectedShapeForUpgrade.TargetingMode = TargetingMode.Last;
                    return;
                }
                if (targetClosestRect.Contains(e.Location))
                {
                    selectedShapeForUpgrade.TargetingMode = TargetingMode.Closest;
                    return;
                }
                if (targetFarthestRect.Contains(e.Location))
                {
                    selectedShapeForUpgrade.TargetingMode = TargetingMode.Farthest;
                    return;
                }
                if (targetLowestHpRect.Contains(e.Location))
                {
                    selectedShapeForUpgrade.TargetingMode = TargetingMode.LowestHp;
                    return;
                }
                if (targetHighestHpRect.Contains(e.Location))
                {
                    selectedShapeForUpgrade.TargetingMode = TargetingMode.HighestHp;
                    return;
                }

                if (!upgradePanelRect.Contains(e.Location))
                {
                    CloseUpgradePanel();
                }
                else
                {
                    return;
                }
            }

            if (settingsButtonRect.Contains(e.Location))
            {
                showSettingsPanel = true;
                return;
            }

            if (inventoryButtonRect.Contains(e.Location))
            {
                showInventoryPanel = true;
                selectedInventoryUnit = null;
                inventoryScrollOffset = 0;
                return;
            }

            if (enemyManager != null && enemyManager.AwaitingEarlyWaveCall)
            {
                if (earlyCallYesButton.Contains(e.Location))
                {
                    enemyManager.ConfirmEarlyWaveCall();
                    return;
                }
                if (earlyCallNoButton.Contains(e.Location))
                {
                    enemyManager.DeclineEarlyWaveCall();
                    return;
                }
            }

            for (int i = 0; i < hotbarSlotRects.Length; i++)
            {
                if (hotbarSlotRects[i].Contains(e.Location))
                {
                    selectedHotbarSlot = selectedHotbarSlot == i ? -1 : i;
                    selectedRotation = 0;
                    return;
                }
            }

            if (selectedHotbarSlot != -1 && TryPlaceSelectedShape(e.Location))
            {
                return;
            }

            if (selectedHotbarSlot == -1)
            {
                var clickedShape = GetPlacedShapeAtLocation(e.Location);
                if (clickedShape != null && (clickedShape.Type == ShapeType.Triangle || clickedShape.Type == ShapeType.Line || clickedShape.Type == ShapeType.Square || clickedShape.Type == ShapeType.Pentagon || clickedShape.Type == ShapeType.Hexagon))
                {
                    selectedShapeForUpgrade = clickedShape;
                    selectedPlacedShapeForRange = clickedShape;
                }
                else
                {
                    CloseUpgradePanel();
                }
                return;
            }
        }

        private PlacedShape GetPlacedShapeAtLocation(Point clickLocation)
        {
            if (currentFloor == null)
            {
                return null;
            }

            if (!GetWorldViewport().Contains(clickLocation))
            {
                return null;
            }

            var (tileSize, offsetX, offsetY) = ComputeGridMetrics();

            if (clickLocation.X < offsetX || clickLocation.Y < offsetY)
            {
                return null;
            }

            int col = (clickLocation.X - offsetX) / tileSize;
            int row = (clickLocation.Y - offsetY) / tileSize;

            foreach (var shape in placedShapes)
            {
                if (shape.Row == row && shape.Col == col)
                {
                    return shape;
                }
            }

            return null;
        }

        private void GamePanel_MouseMove(object sender, MouseEventArgs e)
        {
            hoveredMouseLocation = e.Location;

            if (panPressed)
            {
                int panDx = e.Location.X - panMouseStart.X;
                int panDy = e.Location.Y - panMouseStart.Y;

                if (!isPanningView && (Math.Abs(panDx) > PanDragThresholdPixels || Math.Abs(panDy) > PanDragThresholdPixels))
                {
                    isPanningView = true;
                    gamePanel.Cursor = Cursors.SizeAll;
                }

                if (isPanningView)
                {
                    viewPanX = panStartX + panDx;
                    viewPanY = panStartY + panDy;
                    ClampViewPan();
                }
                return;
            }

            if (isDraggingInventoryScrollbar)
            {
                int dy = e.Location.Y - inventoryScrollDragMouseStartY;
                float trackTravel = Math.Max(1, inventoryScrollTrackRect.Height - inventoryScrollThumbRect.Height);
                int deltaOffset = (int)(dy / trackTravel * inventoryScrollMaxOffset);
                inventoryScrollOffset = Math.Max(0, Math.Min(inventoryScrollDragStartOffset + deltaOffset, inventoryScrollMaxOffset));
                return;
            }

            if (isDraggingUpgradePanel)
            {
                int dx = e.Location.X - upgradePanelDragMouseStart.X;
                int dy2 = e.Location.Y - upgradePanelDragMouseStart.Y;
                int newX = upgradePanelDragOriginStart.X + dx;
                int newY = upgradePanelDragOriginStart.Y + dy2;
                newX = Math.Max(-UpgradePanelWidth + 60, Math.Min(newX, gamePanel.Width - 60));
                newY = Math.Max(0, Math.Min(newY, Math.Max(0, gamePanel.Height - UpgradePanelTitleBarHeight)));

                upgradePanelCustomPosition = new Point(newX, newY);
            }
        }

        private void GamePanel_MouseEnter(object sender, EventArgs e)
        {
            gamePanel.Focus();
        }

        private void GamePanel_MouseLeave(object sender, EventArgs e)
        {
            hoveredMouseLocation = null;
        }

        private void GamePanel_MouseDown(object sender, MouseEventArgs e)
        {
            gamePanel.Focus();
            justFinishedPanningView = false;

            if (showInventoryPanel && inventoryScrollbarVisible && inventoryScrollThumbRect.Contains(e.Location))
            {
                isDraggingInventoryScrollbar = true;
                inventoryScrollDragMouseStartY = e.Location.Y;
                inventoryScrollDragStartOffset = inventoryScrollOffset;
                return;
            }

            if (showInventoryPanel && inventoryScrollbarVisible && inventoryScrollTrackRect.Contains(e.Location))
            {
                float trackTravel = Math.Max(1, inventoryScrollTrackRect.Height - inventoryScrollThumbRect.Height);
                float ratio = (e.Location.Y - inventoryScrollTrackRect.Y - inventoryScrollThumbRect.Height / 2f) / trackTravel;
                inventoryScrollOffset = (int)(Math.Max(0f, Math.Min(1f, ratio)) * inventoryScrollMaxOffset);
                justFinishedDraggingInventoryScrollbar = true;
                return;
            }

            if (selectedShapeForUpgrade != null
                && upgradePanelTitleBarRect.Contains(e.Location)
                && !closeUpgradePanelRect.Contains(e.Location))
            {
                isDraggingUpgradePanel = true;
                upgradePanelDragMouseStart = e.Location;
                upgradePanelDragOriginStart = upgradePanelCustomPosition ?? upgradePanelRect.Location;
                return;
            }
            if (e.Button == MouseButtons.Left && CanPanView() && GetWorldViewport().Contains(e.Location))
            {
                panPressed = true;
                isPanningView = false;
                panMouseStart = e.Location;
                panStartX = viewPanX;
                panStartY = viewPanY;
            }
        }

        private void GamePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (panPressed)
            {
                if (isPanningView)
                {
                    justFinishedPanningView = true;
                }

                panPressed = false;
                isPanningView = false;
                gamePanel.Cursor = Cursors.Default;
            }

            if (isDraggingInventoryScrollbar)
            {
                isDraggingInventoryScrollbar = false;
                justFinishedDraggingInventoryScrollbar = true;
            }

            if (isDraggingUpgradePanel)
            {
                isDraggingUpgradePanel = false;
                justFinishedDraggingUpgradePanel = true;
            }
        }

        private void GamePanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (showInventoryPanel)
            {
                if (!inventoryScrollbarVisible) return;

                inventoryScrollOffset -= e.Delta / 3;
                inventoryScrollOffset = Math.Max(0, Math.Min(inventoryScrollOffset, inventoryScrollMaxOffset));
                gamePanel.Invalidate();
                return;
            }

            if (showSettingsPanel || showStageIntro || currentFloor == null) return;
            if (!GetWorldViewport().Contains(e.Location)) return;

            float factor = (float)Math.Pow(ZoomPerWheelNotch, e.Delta / 120.0);
            ZoomViewAt(e.Location, viewZoom * factor);
            gamePanel.Invalidate();
        }

        private bool TryPlaceSelectedShape(Point clickLocation)
        {
            UnitId? slotUnit = hotbarSlotUnits[selectedHotbarSlot];
            if (slotUnit == null || currentFloor == null || playerState == null)
            {
                return false;
            }

            if (!GetWorldViewport().Contains(clickLocation))
            {
                return false;
            }

            var (tileSize, offsetX, offsetY) = ComputeGridMetrics();

            if (clickLocation.X < offsetX || clickLocation.Y < offsetY)
            {
                return false;
            }

            int col = (clickLocation.X - offsetX) / tileSize;
            int row = (clickLocation.Y - offsetY) / tileSize;

            if (row < 0 || row >= currentFloor.Rows || col < 0 || col >= currentFloor.Cols)
            {
                return false;
            }

            UnitId unit = slotUnit.Value;
            if (unit.Type == ShapeType.Line && currentFloor.Tiles[row, col].Type != TileType.Path)
            {
                Console.WriteLine("Line can only be placed on path tiles.");
                return true;
            }

            bool requiresElevatedGround = RequiresElevatedGround(unit);

            if (unit.Type != ShapeType.Line)
            {
                TileType requiredTile = requiresElevatedGround ? TileType.Elevated : TileType.Open;
                if (currentFloor.Tiles[row, col].Type != requiredTile)
                {
                    string requiredName = requiresElevatedGround ? "elevated" : "open ground";
                    Console.WriteLine($"{unit.Color} {unit.Type} can only be placed on {requiredName} tiles.");
                    return true;
                }
            }

            if (IsTileOccupied(row, col))
            {
                Console.WriteLine("That tile already has a shape on it.");
                return true;
            }

            if (!ShapeCosts.TryGetValue(unit, out int cost))
            {
                Console.WriteLine($"{unit} doesn't have a placement cost set up yet.");
                return true;
            }

            if (!playerState.TrySpend(cost))
            {
                Console.WriteLine($"Not enough cash to place {unit} (costs {cost}).");
                return true;
            }

            placedShapes.Add(new PlacedShape(row, col, unit.Type, unit.Color, selectedRotation) { CollectionLevel = hotbarSlotLevels[selectedHotbarSlot] });
            return true;
        }

        private void HandleInventoryPanelClick(Point location)
        {
            if (inventoryCloseButtonRect.Contains(location))
            {
                showInventoryPanel = false;
                selectedInventoryUnit = null;
                return;
            }

            if (inventoryOwnedClipRect.Contains(location))
            {
                foreach (var owned in inventoryOwnedSlots)
                {
                    if (owned.Rect.Contains(location))
                    {
                        selectedInventoryUnit = selectedInventoryUnit == owned.Unit ? (UnitId?)null : owned.Unit;
                        return;
                    }
                }
            }

            for (int i = 0; i < inventoryLoadoutSlotRects.Length; i++)
            {
                if (inventoryLoadoutSlotRects[i].Contains(location))
                {
                    hotbarSlotUnits[i] = selectedInventoryUnit;
                    hotbarSlotLevels[i] = selectedInventoryUnit.HasValue ? profile.GetBestLevel(selectedInventoryUnit.Value) : 1;
                    return;
                }
            }

        }

        private bool IsTileOccupied(int row, int col)
        {
            foreach (var shape in placedShapes)
            {
                if (shape.Row == row && shape.Col == col)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.Selectable,
                true
            );
        }
    }
}