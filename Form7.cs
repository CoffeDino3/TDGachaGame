using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TDGachaGame
{
    public partial class Form7 : Form
    {
        private const string DefaultStatus = "Pick the unit you want to upgrade.";

        private readonly PlayerProfile profile;
        private readonly List<UnitSlotControl> cards = new List<UnitSlotControl>();
        private int targetCopy = -1;
        private readonly HashSet<int> mergeCopies = new HashSet<int>();
        public Form7() : this(new PlayerProfile(), -1)
        {
        }
        public Form7(PlayerProfile profile, int preselectedCopy)
        {
            this.profile = profile;
            InitializeComponent();

            BuildFilter();
            BuildCards();

            if (preselectedCopy >= 0 && preselectedCopy < profile.Collection.Count)
            {
                targetCopy = preselectedCopy;
                rdoPickFodder.Checked = true;
            }

            RefreshAll();
            SetStatus(targetCopy >= 0 ? "Now pick units to merge into it, or buy a level with coins." : DefaultStatus);
        }

        private void BuildFilter()
        {
            cmbFilter.Items.Add("All shapes");
            foreach (ShapeType type in PlayerProfile.PlayableShapes)
            {
                cmbFilter.Items.Add(type.ToString());
            }

            cmbFilter.SelectedIndex = 0;
        }

        private void BuildCards()
        {
            flowUnits.SuspendLayout();

            for (int i = 0; i < profile.Collection.Count; i++)
            {
                var card = new UnitSlotControl
                {
                    Size = new Size(112, 132),
                    Margin = new Padding(6),
                    SlotNumber = i + 1,
                    ShowSlotNumber = true,
                    Interactive = true,
                    Unit = profile.Collection[i],
                    Level = profile.GetLevel(i)
                };

                card.MouseUp += Card_MouseUp;
                cards.Add(card);
                flowUnits.Controls.Add(card);
            }

            flowUnits.ResumeLayout(true);
        }
        private void RebuildCards()
        {
            flowUnits.SuspendLayout();

            foreach (UnitSlotControl card in cards)
            {
                flowUnits.Controls.Remove(card);
                card.Dispose();
            }

            cards.Clear();
            flowUnits.ResumeLayout(false);

            BuildCards();
            cmbFilter_SelectedIndexChanged(cmbFilter, EventArgs.Empty);
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbFilter.SelectedIndex;
            bool showAll = index <= 0;
            ShapeType wanted = showAll ? ShapeType.Line : PlayerProfile.PlayableShapes[index - 1];

            flowUnits.SuspendLayout();
            foreach (UnitSlotControl card in cards)
            {
                card.Visible = showAll || (card.Unit.HasValue && card.Unit.Value.Type == wanted);
            }
            flowUnits.ResumeLayout(true);
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void Card_MouseUp(object sender, MouseEventArgs e)
        {
            var card = (UnitSlotControl)sender;
            if (e.Button != MouseButtons.Left || !card.ClientRectangle.Contains(e.Location))
            {
                return;
            }

            int copy = cards.IndexOf(card);
            if (copy < 0)
            {
                return;
            }

            if (rdoPickFodder.Checked && targetCopy >= 0)
            {
                if (copy == targetCopy)
                {
                    SetStatus("That is the unit you are upgrading. Pick a different unit to merge into it.");
                    return;
                }

                if (!mergeCopies.Add(copy))
                {
                    mergeCopies.Remove(copy);
                }
            }
            else
            {
                mergeCopies.Clear();

                if (copy == targetCopy)
                {
                    targetCopy = -1;
                    SetStatus(DefaultStatus);
                }
                else
                {
                    targetCopy = copy;
                    rdoPickFodder.Checked = true;
                    SetStatus("Now pick units to merge into it, or buy a level with coins.");
                }
            }

            RefreshAll();
        }
        private void RefreshAll()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                UnitSlotControl card = cards[i];
                int teamSlot = profile.FindInTeam(i);

                card.Level = profile.GetLevel(i);

                if (i == targetCopy)
                {
                    card.Selected = true;
                    card.SelectionColor = Color.Gold;
                    card.Badge = "Upgrading";
                }
                else if (mergeCopies.Contains(i))
                {
                    card.Selected = true;
                    card.SelectionColor = Color.OrangeRed;
                    card.Badge = "+" + CollectionUpgradeConfig.GetMergeExp(profile.Collection[i]).ToString("N0") + " exp";
                }
                else
                {
                    card.Selected = false;
                    card.Badge = teamSlot >= 0 ? $"Slot {teamSlot + 1}" : "";
                }
            }

            RefreshDetails();
        }

        private void RefreshDetails()
        {
            lblCoins.Text = $"Coins: {profile.Coins:N0}";

            if (targetCopy < 0)
            {
                slotTarget.Unit = null;
                slotTarget.Level = 0;
                lblTargetName.Text = "No unit picked";
                lblLevel.Text = "Level -";
                pbExp.Value = 0;
                pbExp.Maximum = 1;
                lblExp.Text = "";

                for (int i = 1; i <= 4; i++)
                {
                    SetStatRow(i, "", "");
                }

                btnLevelUp.Text = "LEVEL UP";
                btnLevelUp.Enabled = false;
                lblMergeInfo.Text = "Pick a unit to upgrade first.";
                btnMerge.Text = "MERGE";
                btnMerge.Enabled = false;
                return;
            }

            UnitId unit = profile.Collection[targetCopy];
            int level = profile.GetLevel(targetCopy);
            int exp = profile.GetExp(targetCopy);
            bool isMax = level >= CollectionUpgradeConfig.MaxLevel;

            slotTarget.Unit = unit;
            slotTarget.Level = level;
            lblTargetName.Text = unit.ToString();
            lblLevel.Text = $"Level {level} / {CollectionUpgradeConfig.MaxLevel}";

            pbExp.Value = 0;

            if (isMax)
            {
                pbExp.Maximum = 1;
                pbExp.Value = 1;
                lblExp.Text = "MAX LEVEL";
            }
            else
            {
                int need = CollectionUpgradeConfig.GetExpToNext(level);
                pbExp.Maximum = need;
                pbExp.Value = Math.Max(0, Math.Min(exp, need));
                lblExp.Text = $"{exp:N0} / {need:N0} exp";
            }

            ShowStats(unit, level);
            if (isMax)
            {
                btnLevelUp.Text = "MAX LEVEL";
                btnLevelUp.Enabled = false;
            }
            else
            {
                int cost = CollectionUpgradeConfig.GetLevelUpCost(unit, level);
                bool canAfford = profile.Coins >= cost;

                btnLevelUp.Text = $"LEVEL UP  -  {cost:N0} coins";
                btnLevelUp.Enabled = true;
                btnLevelUp.BackColor = canAfford ? Color.FromArgb(46, 160, 90) : Color.FromArgb(70, 78, 96);
                btnLevelUp.FlatAppearance.MouseOverBackColor = canAfford ? Color.FromArgb(60, 190, 110) : Color.FromArgb(86, 95, 116);
            }
            if (isMax)
            {
                lblMergeInfo.Text = "This unit is at the max level, nothing left to merge into it.";
                btnMerge.Text = "MERGE";
                btnMerge.Enabled = false;
            }
            else if (mergeCopies.Count == 0)
            {
                lblMergeInfo.Text = "Click units on the left to merge them into this one. Merged units are deleted.";
                btnMerge.Text = "MERGE";
                btnMerge.Enabled = false;
            }
            else
            {
                int gained = profile.GetMergeExp(mergeCopies);
                int newLevel = CollectionUpgradeConfig.PreviewLevel(level, exp, gained);
                int missing = CollectionUpgradeConfig.GetTotalExpForLevel(CollectionUpgradeConfig.MaxLevel)
                              - (CollectionUpgradeConfig.GetTotalExpForLevel(level) + exp);
                string waste = gained > missing ? "  (some exp is wasted)" : "";

                lblMergeInfo.Text = $"{mergeCopies.Count} unit{(mergeCopies.Count == 1 ? "" : "s")} picked: +{gained:N0} exp\nLevel {level}  ->  Level {newLevel}{waste}";
                btnMerge.Text = $"MERGE ({mergeCopies.Count})";
                btnMerge.Enabled = true;
            }
        }

        private void SetStatRow(int row, string name, string value)
        {
            Label nameLabel = null;
            Label valueLabel = null;

            switch (row)
            {
                case 1: nameLabel = lblStatName1; valueLabel = lblStatValue1; break;
                case 2: nameLabel = lblStatName2; valueLabel = lblStatValue2; break;
                case 3: nameLabel = lblStatName3; valueLabel = lblStatValue3; break;
                case 4: nameLabel = lblStatName4; valueLabel = lblStatValue4; break;
            }

            nameLabel.Text = name;
            valueLabel.Text = value;
        }
        private void ShowStats(UnitId unit, int level)
        {
            var now = TowerUpgradeConfig.GetBaseStats(unit.Type, unit.Color, level);
            bool hasNext = level < CollectionUpgradeConfig.MaxLevel;
            var next = hasNext ? TowerUpgradeConfig.GetBaseStats(unit.Type, unit.Color, level + 1) : now;

            bool isHexagon = unit.Type == ShapeType.Hexagon;
            bool isPentagon = unit.Type == ShapeType.Pentagon;

            string Compare(float a, float b, string format, string unitText)
            {
                string left = a.ToString(format) + unitText;
                return hasNext ? left + "  ->  " + b.ToString(format) + unitText : left;
            }
            string damageName = isHexagon ? "Attack" : "Damage";
            SetStatRow(1, damageName, now.damage > 0f ? Compare(now.damage, next.damage, "F1", "") : "- (support)");
            if (isHexagon)
            {
                float speedNow = now.range * TowerManager.SummonSpeedPerAttackSpeed;
                float speedNext = next.range * TowerManager.SummonSpeedPerAttackSpeed;
                SetStatRow(2, "Unit speed", Compare(speedNow, speedNext, "F2", " t/s"));
            }
            else
            {
                SetStatRow(2, isPentagon ? "Orbit range" : "Range", Compare(now.range, next.range, "F2", ""));
            }
            SetStatRow(3, isHexagon ? "Spawn every" : "Attack every", Compare(now.interval, next.interval, "F2", "s"));
            if (now.blast > 0f)
            {
                SetStatRow(4, "Blast radius", Compare(now.blast, next.blast, "F2", ""));
            }
            else
            {
                SetStatRow(4, "", "");
            }
        }

        private void btnLevelUp_Click(object sender, EventArgs e)
        {
            bool ok = profile.TryLevelUpWithCoins(targetCopy, out string message);
            SetStatus(message);

            if (ok)
            {
                RefreshAll();
            }
            else
            {
                RefreshDetails();
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (targetCopy < 0 || mergeCopies.Count == 0)
            {
                return;
            }

            List<int> picks = mergeCopies.ToList();
            UnitId target = profile.Collection[targetCopy];

            string question = $"Merge {picks.Count} unit{(picks.Count == 1 ? "" : "s")} into {target}?\n\nThe merged units will be deleted for good.";

            int onTeam = picks.Count(copy => profile.FindInTeam(copy) >= 0);
            int leveled = picks.Count(copy => profile.GetLevel(copy) > 1);

            if (onTeam > 0)
            {
                question += $"\n\n{onTeam} of them {(onTeam == 1 ? "is" : "are")} on your team and will be removed from it.";
            }

            if (leveled > 0)
            {
                question += $"\n\n{leveled} of them {(leveled == 1 ? "is" : "are")} above level 1 - that level is lost.";
            }

            DialogResult answer = MessageBox.Show(this, question, "Merge units", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            bool ok = profile.TryMerge(targetCopy, picks, out int newTargetIndex, out string message);
            SetStatus(message);

            if (!ok)
            {
                return;
            }
            targetCopy = newTargetIndex;
            mergeCopies.Clear();
            RebuildCards();
            RefreshAll();
        }

        private void btnClearPicks_Click(object sender, EventArgs e)
        {
            mergeCopies.Clear();
            RefreshAll();
        }
    }
}