using System;
using System.Drawing;
using System.Windows.Forms;

namespace TDGachaGame
{
    public enum MenuChoice
    {
        Quit,
        EnterTower,
        BackToTitle
    }
    public partial class Form3 : Form
    {
        private readonly PlayerProfile profile;
        private UnitSlotControl[] slots;
        public MenuChoice Choice
        {
            get;
            private set;
        } = MenuChoice.Quit;

        public Form3() : this(new PlayerProfile())
        {
        }

        public Form3(PlayerProfile profile)
        {
            this.profile = profile;
            InitializeComponent();

            slots = new[] { slot1, slot2, slot3, slot4, slot5, slot6 };

            foreach (UnitSlotControl slot in slots)
            {
                slot.Interactive = true;
                slot.EmptyText = "+ Add unit";
                slot.MouseUp += Slot_MouseUp;
                tipMain.SetToolTip(slot, "Click to choose a unit. Right-click to empty the slot.");
            }

            UpdateMusicButton();
            LoadTeamSlots();
            RefreshProfileDisplay();
        }

        private void LoadTeamSlots()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Unit = i < profile.Team.Length ? profile.Team[i] : (UnitId?)null;
                slots[i].Level = profile.GetTeamLevel(i);
            }
        }

        private void RefreshProfileDisplay()
        {
            lblCoins.Text = $"Coins: {profile.Coins}";
            lblGems.Text = $"Gems: {profile.Gems}";
            btnEnterTower.Text = $"ENTER THE TOWER  -  Floor {profile.TowerFloor}";
            tsslHighest.Text = $"Highest floor cleared: {profile.HighestFloorCleared}";

            RefreshStageInfo();
            bool hasTeam = !profile.IsTeamEmpty;
            btnEnterTower.BackColor = hasTeam ? Color.FromArgb(46, 160, 90) : Color.FromArgb(70, 78, 96);
            btnEnterTower.FlatAppearance.MouseOverBackColor = hasTeam ? Color.FromArgb(60, 190, 110) : Color.FromArgb(86, 95, 116);

            if (!hasTeam)
            {
                tsslMessage.Text = "Your team is empty. Open the Collection and pick some units.";
            }
            else
            {
                tsslMessage.Text = profile.HighestFloorCleared > 0
                    ? "Nice work! Ready for the next floor?"
                    : "Welcome! Take your team into the tower.";
            }
        }
        private void RefreshStageInfo()
        {
            int stage = profile.TowerFloor;
            int nextChangeStage = (stage / 5 + 1) * 5;
            int stagesToGo = nextChangeStage - stage;

            lblStageNumber.Text = stage.ToString();
            lblBest.Text = profile.HighestFloorCleared > 0
                ? $"Best: stage {profile.HighestFloorCleared}"
                : "Nothing cleared yet";

            lblWavesValue.Text = FloorEnemyGenerator.GetWaveCount(stage).ToString();
            lblRewardCoins.Text = $"+{PlayerProfile.GetFloorReward(stage):N0} coins";
            lblRewardGems.Text = $"+{PlayerProfile.GetFloorGemReward(stage):N0} gems";
            lblCashValue.Text = StageProgression.GetStartingCash(stage).ToString("N0");
            lblNextValue.Text = $"Stage {nextChangeStage}";
            lblNextSub.Text = stagesToGo == 1 ? "next stage" : $"in {stagesToGo} stages";
        }

        private void OpenCollection(int targetSlot)
        {
            using (var collection = new Form4(profile, targetSlot))
            {
                collection.ShowDialog(this);
            }

            LoadTeamSlots();
            RefreshProfileDisplay();
        }

        private void Slot_MouseUp(object sender, MouseEventArgs e)
        {
            var slot = (UnitSlotControl)sender;
            if (!slot.ClientRectangle.Contains(e.Location))
            {
                return;
            }

            int index = Array.IndexOf(slots, slot);
            if (index < 0)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                profile.Unequip(index);
                LoadTeamSlots();
                RefreshProfileDisplay();
            }
            else if (e.Button == MouseButtons.Left)
            {
                OpenCollection(index);
            }
        }

        private void btnEnterTower_Click(object sender, EventArgs e)
        {
            if (profile.IsTeamEmpty)
            {
                MessageBox.Show(
                    this,
                    "Your team is empty - pick at least one unit from the Collection first.",
                    "No units equipped",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                OpenCollection(-1);
                return;
            }

            Choice = MenuChoice.EnterTower;
            Close();
        }

        private void btnBackToTitle_Click(object sender, EventArgs e)
        {
            Choice = MenuChoice.BackToTitle;
            Close();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Choice = MenuChoice.Quit;
            Close();
        }
        private void btnSummon_Click(object sender, EventArgs e)
        {
            using (var summon = new Form6(profile))
            {
                summon.ShowDialog(this);
            }

            RefreshProfileDisplay();
        }
        private void btnMusic_Click(object sender, EventArgs e)
        {
            MusicPlayer.ToggleMute();
            UpdateMusicButton();
        }

        private void UpdateMusicButton()
        {
            btnMusic.Text = MusicPlayer.Muted ? "\U0001F507" : "\U0001F50A";
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            using (var dialog = new Form5(profile))
            {
                dialog.Redeemed += (s, args) => RefreshProfileDisplay();
                dialog.ShowDialog(this);
            }

            RefreshProfileDisplay();
        }

        private void btnCollection_Click(object sender, EventArgs e)
        {
            OpenCollection(-1);
        }
    }
}