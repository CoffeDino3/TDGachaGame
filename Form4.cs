using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TDGachaGame
{
    public partial class Form4 : Form
    {
        private const string DefaultStatus = "Drag a unit onto a slot, or pick a unit then a slot. Right-click a unit to equip it fast.";

        private readonly PlayerProfile profile;
        private readonly UnitSlotControl[] teamSlots = new UnitSlotControl[PlayerProfile.TeamSize];
        private readonly List<UnitSlotControl> cards = new List<UnitSlotControl>();
        private int selectedCopy = -1;
        private int targetSlot = -1;

        private const string DragMarker = "TDGachaGame.Unit";
        private int dragCopy = -1;
        private Point dragStart;
        private bool dragArmed;

        public Form4() : this(new PlayerProfile(), -1)
        {
        }
        public Form4(PlayerProfile profile, int targetSlot)
        {
            this.profile = profile;
            InitializeComponent();

            BuildTeamSlots();
            BuildCards();
            BuildFilter();

            lblCount.Text = $"{profile.Collection.Count} / {profile.MaxCollectionSlots} slots";
            RefreshExpandButton();

            if (targetSlot >= 0 && targetSlot < PlayerProfile.TeamSize)
            {
                this.targetSlot = targetSlot;
                SetStatus($"Pick a unit for slot {targetSlot + 1}.");
            }
            else
            {
                SetStatus(DefaultStatus);
            }

            RefreshAll();
        }

        private void BuildTeamSlots()
        {
            for (int i = 0; i < teamSlots.Length; i++)
            {
                var slot = new UnitSlotControl
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(5),
                    SlotNumber = i + 1,
                    Interactive = true,
                    EmptyText = "Empty"
                };

                slot.MouseUp += TeamSlot_MouseUp;

                slot.AllowDrop = true;
                slot.DragEnter += TeamSlot_DragOver;
                slot.DragOver += TeamSlot_DragOver;
                slot.DragLeave += TeamSlot_DragLeave;
                slot.DragDrop += TeamSlot_DragDrop;
                slot.MouseDown += DragSource_MouseDown;
                slot.MouseMove += DragSource_MouseMove;

                teamSlots[i] = slot;
                tlpTeam.Controls.Add(slot, i, 0);
            }
        }

        private void BuildCards()
        {
            flowUnits.SuspendLayout();

            for (int i = 0; i < profile.MaxCollectionSlots; i++)
            {
                bool owned = i < profile.Collection.Count;

                var card = new UnitSlotControl
                {
                    Size = new Size(112, 132),
                    Margin = new Padding(6),
                    SlotNumber = i + 1,
                    ShowSlotNumber = true,
                    EmptyText = "Empty slot",
                    Interactive = owned,
                    Unit = owned ? profile.Collection[i] : (UnitId?)null,
                    Level = owned ? profile.GetLevel(i) : 0
                };

                card.MouseUp += Card_MouseUp;
                card.MouseDown += DragSource_MouseDown;
                card.MouseMove += DragSource_MouseMove;
                cards.Add(card);
                flowUnits.Controls.Add(card);
            }

            flowUnits.ResumeLayout(true);
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
        private int GetCopyIndex(UnitSlotControl control)
        {
            int cardIndex = cards.IndexOf(control);
            if (cardIndex >= 0)
            {
                return cardIndex < profile.Collection.Count ? cardIndex : -1;
            }

            int teamIndex = Array.IndexOf(teamSlots, control);
            return teamIndex >= 0 ? profile.GetTeamCopy(teamIndex) : -1;
        }

        private void RefreshAll()
        {
            UnitId?[] team = profile.Team;

            for (int i = 0; i < teamSlots.Length; i++)
            {
                teamSlots[i].Unit = team[i];
                teamSlots[i].Level = profile.GetTeamLevel(i);
                teamSlots[i].Selected = i == targetSlot;
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (i >= profile.Collection.Count)
                {
                    continue;
                }

                int slot = profile.FindInTeam(i);

                cards[i].Badge = slot >= 0 ? $"Slot {slot + 1}" : "";
                cards[i].Level = profile.GetLevel(i);
                cards[i].Selected = i == selectedCopy;
            }
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void PlaceUnit(int copyIndex, int slot)
        {
            profile.Equip(copyIndex, slot);
            selectedCopy = -1;
            targetSlot = -1;
            SetStatus($"{profile.Collection[copyIndex]} is now in slot {slot + 1}.");
            RefreshAll();
        }
        private void QuickEquip(int copyIndex)
        {
            UnitId unit = profile.Collection[copyIndex];
            int current = profile.FindInTeam(copyIndex);

            if (current >= 0)
            {
                profile.Unequip(current);
                SetStatus($"{unit} is off the team.");
            }
            else
            {
                int free = profile.FirstEmptySlot();
                if (free < 0)
                {
                    SetStatus("The team is full. Right-click a slot to free one up first.");
                    return;
                }

                profile.Equip(copyIndex, free);
                SetStatus($"{unit} is now in slot {free + 1}.");
            }

            selectedCopy = -1;
            targetSlot = -1;
            RefreshAll();
        }

        private void Card_MouseUp(object sender, MouseEventArgs e)
        {
            var card = (UnitSlotControl)sender;

            if (!card.ClientRectangle.Contains(e.Location))
            {
                return;
            }

            int copy = GetCopyIndex(card);
            if (copy < 0)
            {
                return;
            }

            UnitId unit = profile.Collection[copy];

            if (e.Button == MouseButtons.Right)
            {
                QuickEquip(copy);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (targetSlot >= 0)
                {
                    PlaceUnit(copy, targetSlot);
                    return;
                }

                bool alreadySelected = selectedCopy == copy;
                selectedCopy = alreadySelected ? -1 : copy;
                SetStatus(alreadySelected ? DefaultStatus : $"{unit} selected - now pick a slot.");
                RefreshAll();
            }
        }

        private void TeamSlot_MouseUp(object sender, MouseEventArgs e)
        {
            var slot = (UnitSlotControl)sender;

            if (!slot.ClientRectangle.Contains(e.Location))
            {
                return;
            }

            int index = Array.IndexOf(teamSlots, slot);
            if (index < 0)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                profile.Unequip(index);
                if (targetSlot == index)
                {
                    targetSlot = -1;
                }

                SetStatus($"Slot {index + 1} is empty.");
                RefreshAll();
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (selectedCopy >= 0)
                {
                    PlaceUnit(selectedCopy, index);
                    return;
                }

                targetSlot = targetSlot == index ? -1 : index;
                SetStatus(targetSlot >= 0 ? $"Pick a unit for slot {index + 1}." : DefaultStatus);
                RefreshAll();
            }
        }

        private void DragSource_MouseDown(object sender, MouseEventArgs e)
        {
            var source = (UnitSlotControl)sender;
            dragArmed = e.Button == MouseButtons.Left && GetCopyIndex(source) >= 0;
            dragStart = e.Location;
        }

        private void DragSource_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragArmed || e.Button != MouseButtons.Left)
            {
                return;
            }
            Size threshold = SystemInformation.DragSize;
            if (Math.Abs(e.X - dragStart.X) < threshold.Width / 2 &&
                Math.Abs(e.Y - dragStart.Y) < threshold.Height / 2)
            {
                return;
            }

            var source = (UnitSlotControl)sender;
            int copy = GetCopyIndex(source);
            if (copy < 0)
            {
                return;
            }

            dragArmed = false;
            dragCopy = copy;
            selectedCopy = -1;
            targetSlot = -1;
            SetStatus($"Dragging {profile.Collection[copy]} - drop it on a slot.");
            RefreshAll();

            source.DoDragDrop(DragMarker, DragDropEffects.Move);

            dragCopy = -1;
            dragArmed = false;
            RefreshAll();
        }

        private void TeamSlot_DragOver(object sender, DragEventArgs e)
        {
            var slot = (UnitSlotControl)sender;

            if (dragCopy >= 0)
            {
                e.Effect = DragDropEffects.Move;
                slot.Selected = true;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TeamSlot_DragLeave(object sender, EventArgs e)
        {
            var slot = (UnitSlotControl)sender;
            slot.Selected = Array.IndexOf(teamSlots, slot) == targetSlot;
        }

        private void TeamSlot_DragDrop(object sender, DragEventArgs e)
        {
            var slot = (UnitSlotControl)sender;
            int index = Array.IndexOf(teamSlots, slot);

            if (index < 0 || dragCopy < 0)
            {
                return;
            }

            PlaceUnit(dragCopy, index);
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

            selectedCopy = -1;
            lblCount.Text = $"{profile.Collection.Count} / {profile.MaxCollectionSlots} slots";
            RefreshExpandButton();
            cmbFilter_SelectedIndexChanged(cmbFilter, EventArgs.Empty);
            RefreshAll();
        }
        private void RefreshExpandButton()
        {
            if (profile.IsCollectionAtMaxSize)
            {
                btnExpand.Text = "Max size reached";
                btnExpand.Enabled = false;
            }
            else
            {
                btnExpand.Text = $"+{PlayerProfile.ExpansionStep} slots  -  {profile.NextExpansionCost:N0} gems";
                btnExpand.Enabled = true;
            }
        }
        private void btnExpand_Click(object sender, EventArgs e)
        {
            int cost = profile.NextExpansionCost;

            DialogResult answer = MessageBox.Show(
                this,
                $"Expand your collection by {PlayerProfile.ExpansionStep} slots for {cost:N0} gems?\n\nYou have {profile.Gems:N0} gems.",
                "Expand collection",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
            {
                return;
            }

            bool ok = profile.TryExpandCollection(out string message);
            RebuildCards();
            SetStatus(message);
        }
        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            using (var upgrade = new Form7(profile, selectedCopy))
            {
                upgrade.ShowDialog(this);
            }

            RebuildCards();
            SetStatus(DefaultStatus);
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            using (var sell = new Form8(profile, selectedCopy))
            {
                sell.ShowDialog(this);
            }

            RebuildCards();
            SetStatus(DefaultStatus);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            profile.ClearTeam();
            selectedCopy = -1;
            targetSlot = -1;
            SetStatus("Team cleared.");
            RefreshAll();
        }
    }
}