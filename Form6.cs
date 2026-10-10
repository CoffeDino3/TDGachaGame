using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TDGachaGame
{
    public partial class Form6 : Form
    {
        private readonly PlayerProfile profile;
        private readonly Random rng = new Random();
        private readonly Queue<PullResult> pending = new Queue<PullResult>();

        private PullResult current;
        private PullResult best;
        private int totalPulls;
        private int revealedCount;
        private bool busy;
        public Form6() : this(new PlayerProfile())
        {
        }

        public Form6(PlayerProfile profile)
        {
            this.profile = profile;
            InitializeComponent();

            display.Revealed += Display_Revealed;
            display.Finished += Display_Finished;

            UpdateMusicButton();
            RefreshCounters();
            SetBusy(false);
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
        private void tmrAnim_Tick(object sender, EventArgs e)
        {
            display.Step();
        }

        private void btnPull1_Click(object sender, EventArgs e)
        {
            StartPull(1);
        }

        private void btnPull5_Click(object sender, EventArgs e)
        {
            StartPull(5);
        }

        private void btnPull10_Click(object sender, EventArgs e)
        {
            StartPull(10);
        }
        private void btnPullMax_Click(object sender, EventArgs e)
        {
            StartPull(GachaSystem.GetMaxAffordablePulls(profile));
        }

        private void StartPull(int count)
        {
            if (busy)
            {
                return;
            }

            if (!GachaSystem.TryPull(profile, count, rng, out List<PullResult> results, out string message))
            {
                ShowError(message);
                return;
            }

            ClearResultCards();

            totalPulls = results.Count;
            revealedCount = 0;
            best = null;
            pending.Clear();

            foreach (PullResult result in results)
            {
                pending.Enqueue(result);

                if (best == null || result.Chance < best.Chance)
                {
                    best = result;
                }
            }

            pbReveal.Maximum = totalPulls;
            pbReveal.Value = 0;

            SetBusy(true);
            RefreshCounters();
            lblStatus.Text = "";

            lblUnitName.Text = "Summoning...";
            lblRarity.Text = "";
            lblChance.Text = "";

            RevealNext();
        }

        private void RevealNext()
        {
            if (pending.Count == 0)
            {
                FinishPulls();
                return;
            }

            current = pending.Dequeue();
            bool fast = totalPulls > 1 && current.RarityIndex < 3;
            display.Play(current, fast);
        }
        private void Display_Revealed(object sender, EventArgs e)
        {
            ShowUnitInfo(current);
        }

        private void ShowUnitInfo(PullResult result)
        {
            if (result == null)
            {
                return;
            }

            lblUnitName.Text = $"{result.Unit.Color} {result.Unit.Type}";
            lblRarity.Text = $"{new string('\u2605', result.RarityIndex + 1)}  {result.RarityName.ToUpper()}";
            lblRarity.ForeColor = GachaDisplayControl.GetRarityColor(result.RarityIndex);
            lblChance.Text = $"Pull chance: {result.ChanceText}";
        }
        private void Display_Finished(object sender, EventArgs e)
        {
            if (current != null)
            {
                AddResultCard(current);
                revealedCount++;
                pbReveal.Value = Math.Min(pbReveal.Maximum, revealedCount);
                current = null;
            }

            RevealNext();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                return;
            }

            if (current != null)
            {
                AddResultCard(current);
                current = null;
            }

            while (pending.Count > 0)
            {
                AddResultCard(pending.Dequeue());
            }
            display.ShowResult(best);
            ShowUnitInfo(best);
            FinishPulls();
        }

        private void FinishPulls()
        {
            SetBusy(false);
            RefreshCounters();
        }

        private void AddResultCard(PullResult result)
        {
            var card = new UnitSlotControl
            {
                Size = new Size(92, 116),
                Margin = new Padding(3),
                ShowSlotNumber = false,
                Interactive = false,
                Unit = result.Unit,
                Badge = result.RarityIndex >= 2 ? result.RarityName.ToUpper() : "",
                Tag = result.RarityIndex
            };

            tipMain.SetToolTip(card, $"{result.Unit} - {result.RarityName} ({result.ChanceText})");
            flowResults.Controls.Add(card);
            flowResults.ScrollControlIntoView(card);
        }

        private void ClearResultCards()
        {
            flowResults.SuspendLayout();

            while (flowResults.Controls.Count > 0)
            {
                Control card = flowResults.Controls[0];
                flowResults.Controls.RemoveAt(0);
                card.Dispose();
            }

            flowResults.ResumeLayout(true);
        }
        private void SetBusy(bool isBusy)
        {
            busy = isBusy;

            btnPull1.Enabled = !isBusy;
            btnPull5.Enabled = !isBusy;
            btnPull10.Enabled = !isBusy;
            btnPullMax.Enabled = !isBusy;
            btnSkip.Enabled = isBusy;
            pbReveal.Visible = isBusy && totalPulls > 1;
        }

        private void RefreshCounters()
        {
            lblGems.Text = $"Gems: {profile.Gems}";
            lblSlots.Text = $"Collection: {profile.Collection.Count} / {profile.MaxCollectionSlots}";

            int max = GachaSystem.GetMaxAffordablePulls(profile);
            btnPullMax.Text = max > 0
                ? $"SUMMON MAX (x{max}){Environment.NewLine}{max * GachaSystem.GemCostPerPull} gems"
                : $"SUMMON MAX{Environment.NewLine}need {GachaSystem.GemCostPerPull} gems";
        }
        private void ShowError(string text)
        {
            lblStatus.Text = text;
        }
        private void btnCode_Click(object sender, EventArgs e)
        {
            using (var dialog = new Form5(profile))
            {
                dialog.Redeemed += (s, args) => RefreshCounters();
                dialog.ShowDialog(this);
            }

            RefreshCounters();
        }
        private void btnOdds_Click(object sender, EventArgs e)
        {
            int stage = profile.TowerFloor;
            double[] shapeOdds = GachaSystem.GetShapeWeights(stage);

            var text = new StringBuilder();
            text.AppendLine($"Odds on stage {stage} (they change as you climb the tower)");
            text.AppendLine();
            text.AppendLine("Shape:");

            for (int i = 0; i < PlayerProfile.PlayableShapes.Length; i++)
            {
                text.AppendLine($"    {PlayerProfile.PlayableShapes[i],-10} {GachaSystem.FormatChance(shapeOdds[i])}");
            }

            text.AppendLine();
            text.AppendLine("Color (same on every stage):");

            foreach (ShapeColor color in Enum.GetValues(typeof(ShapeColor)))
            {
                text.AppendLine($"    {color,-10} {GachaSystem.FormatChance(GachaSystem.GetColorChance(color))}");
            }

            MessageBox.Show(this, text.ToString(), "Summon odds", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}