using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TDGachaGame
{
    public partial class Form8 : Form
    {
        private readonly PlayerProfile profile;
        private readonly List<UnitSlotControl> cards = new List<UnitSlotControl>();
        private readonly HashSet<int> picked = new HashSet<int>();
        public Form8() : this(new PlayerProfile(), -1)
        {
        }
        public Form8(PlayerProfile profile, int preselectedCopy)
        {
            this.profile = profile;
            InitializeComponent();

            BuildFilter();
            BuildCards();

            if (preselectedCopy >= 0 && preselectedCopy < profile.Collection.Count)
            {
                picked.Add(preselectedCopy);
            }

            RefreshAll();
            SetStatus("Click units to pick them, then press SELL SELECTED.");
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
                    Level = profile.GetLevel(i),
                    SelectionColor = Color.OrangeRed
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

            if (!picked.Add(copy))
            {
                picked.Remove(copy);
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
                card.Selected = picked.Contains(i);
                card.Badge = picked.Contains(i)
                    ? "+" + profile.GetSellValue(i).ToString("N0")
                    : (teamSlot >= 0 ? $"Slot {teamSlot + 1}" : "");
            }

            lblCoins.Text = $"Coins: {profile.Coins:N0}";

            int total = 0;
            int onTeam = 0;
            foreach (int copy in picked)
            {
                total += profile.GetSellValue(copy);
                if (profile.FindInTeam(copy) >= 0)
                {
                    onTeam++;
                }
            }

            lblSelected.Text = picked.Count == 0
                ? "Nothing selected"
                : $"{picked.Count} unit{(picked.Count == 1 ? "" : "s")} selected";
            lblTotal.Text = $"You get: {total:N0} coins";
            lblTeamWarning.Text = onTeam > 0
                ? $"{onTeam} selected unit{(onTeam == 1 ? " is" : "s are")} on your team and will be removed from it."
                : "";
            btnSell.Enabled = picked.Count > 0;
        }

        private void btnSelectDuplicates_Click(object sender, EventArgs e)
        {
            picked.Clear();

            var groups = new Dictionary<UnitId, List<int>>();
            for (int i = 0; i < profile.Collection.Count; i++)
            {
                UnitId unit = profile.Collection[i];
                if (!groups.TryGetValue(unit, out List<int> list))
                {
                    list = new List<int>();
                    groups[unit] = list;
                }

                list.Add(i);
            }

            int skippedTeam = 0;

            foreach (List<int> copies in groups.Values)
            {
                if (copies.Count < 2)
                {
                    continue;
                }
                int keep = copies
                    .OrderByDescending(c => profile.GetLevel(c))
                    .ThenByDescending(c => profile.GetExp(c))
                    .ThenByDescending(c => profile.FindInTeam(c) >= 0)
                    .First();

                foreach (int copy in copies)
                {
                    if (copy == keep)
                    {
                        continue;
                    }

                    if (profile.FindInTeam(copy) >= 0)
                    {
                        skippedTeam++;
                        continue;
                    }

                    picked.Add(copy);
                }
            }

            RefreshAll();

            if (picked.Count == 0)
            {
                SetStatus("You have no spare duplicates to sell.");
            }
            else
            {
                string skipped = skippedTeam > 0 ? $" Skipped {skippedTeam} on your team." : "";
                SetStatus($"Picked {picked.Count} duplicate{(picked.Count == 1 ? "" : "s")} and kept your best copy of each unit.{skipped}");
            }
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            picked.Clear();
            RefreshAll();
            SetStatus("Selection cleared.");
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (picked.Count == 0)
            {
                return;
            }

            List<int> toSell = picked.ToList();
            int total = toSell.Sum(copy => profile.GetSellValue(copy));
            int onTeam = toSell.Count(copy => profile.FindInTeam(copy) >= 0);
            int leveled = toSell.Count(copy => profile.GetLevel(copy) > 1);

            string question = $"Sell {toSell.Count} unit{(toSell.Count == 1 ? "" : "s")} for {total:N0} coins?\n\nThey will be deleted for good.";

            if (onTeam > 0)
            {
                question += $"\n\n{onTeam} of them {(onTeam == 1 ? "is" : "are")} on your team and will be removed from it.";
            }

            if (leveled > 0)
            {
                question += $"\n\n{leveled} of them {(leveled == 1 ? "is" : "are")} above level 1.";
            }

            DialogResult answer = MessageBox.Show(this, question, "Sell units", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            int coins = profile.SellCopies(toSell);

            picked.Clear();
            RebuildCards();
            RefreshAll();
            SetStatus($"Sold {toSell.Count} unit{(toSell.Count == 1 ? "" : "s")} for {coins:N0} coins.");
        }
    }
}