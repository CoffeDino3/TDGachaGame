namespace TDGachaGame
{
    partial class Form3
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tlpMain = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblGems = new Label();
            lblCoins = new Label();
            lblGameTitle = new Label();
            btnCode = new Button();
            btnMusic = new Button();
            grpTeam = new GroupBox();
            tlpTeam = new TableLayoutPanel();
            slot1 = new UnitSlotControl();
            slot2 = new UnitSlotControl();
            slot3 = new UnitSlotControl();
            slot4 = new UnitSlotControl();
            slot5 = new UnitSlotControl();
            slot6 = new UnitSlotControl();
            pnlStage = new Panel();
            tlpStage = new TableLayoutPanel();
            pnlStageLeft = new Panel();
            pnlStageDivider = new Panel();
            lblStageCaption = new Label();
            lblStageNumber = new Label();
            lblBest = new Label();
            tlpTiles = new TableLayoutPanel();
            pnlTileWaves = new Panel();
            pnlTileWavesAccent = new Panel();
            lblWavesCaption = new Label();
            lblWavesValue = new Label();
            lblWavesSub = new Label();
            pnlTileReward = new Panel();
            pnlTileRewardAccent = new Panel();
            lblRewardCaption = new Label();
            lblRewardCoins = new Label();
            lblRewardGems = new Label();
            pnlTileCash = new Panel();
            pnlTileCashAccent = new Panel();
            lblCashCaption = new Label();
            lblCashValue = new Label();
            lblCashSub = new Label();
            pnlTileNext = new Panel();
            pnlTileNextAccent = new Panel();
            lblNextCaption = new Label();
            lblNextValue = new Label();
            lblNextSub = new Label();
            btnEnterTower = new Button();
            tlpActions = new TableLayoutPanel();
            btnSummon = new Button();
            btnCollection = new Button();
            btnBackToTitle = new Button();
            btnQuit = new Button();
            ssMain = new StatusStrip();
            tsslMessage = new ToolStripStatusLabel();
            tsslHighest = new ToolStripStatusLabel();
            tipMain = new ToolTip(components);
            tlpMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            grpTeam.SuspendLayout();
            tlpTeam.SuspendLayout();
            tlpActions.SuspendLayout();
            pnlStage.SuspendLayout();
            tlpStage.SuspendLayout();
            pnlStageLeft.SuspendLayout();
            tlpTiles.SuspendLayout();
            pnlTileWaves.SuspendLayout();
            pnlTileReward.SuspendLayout();
            pnlTileCash.SuspendLayout();
            pnlTileNext.SuspendLayout();
            ssMain.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(pnlHeader, 0, 0);
            tlpMain.Controls.Add(grpTeam, 0, 1);
            tlpMain.Controls.Add(pnlStage, 0, 2);
            tlpMain.Controls.Add(btnEnterTower, 0, 3);
            tlpMain.Controls.Add(tlpActions, 0, 4);
            tlpMain.Controls.Add(ssMain, 0, 5);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Margin = new Padding(0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 6;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpMain.Size = new Size(900, 600);
            tlpMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(18, 22, 34);
            pnlHeader.Controls.Add(btnMusic);
            pnlHeader.Controls.Add(btnCode);
            pnlHeader.Controls.Add(lblGems);
            pnlHeader.Controls.Add(lblCoins);
            pnlHeader.Controls.Add(lblGameTitle);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(900, 70);
            pnlHeader.TabIndex = 0;
            // 
            // btnMusic
            // 
            btnMusic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMusic.BackColor = Color.FromArgb(40, 48, 70);
            btnMusic.Cursor = Cursors.Hand;
            btnMusic.FlatAppearance.BorderSize = 0;
            btnMusic.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnMusic.FlatStyle = FlatStyle.Flat;
            btnMusic.Font = new Font("Segoe UI Emoji", 11F, FontStyle.Regular, GraphicsUnit.Point);
            btnMusic.ForeColor = Color.White;
            btnMusic.Location = new Point(368, 20);
            btnMusic.Name = "btnMusic";
            btnMusic.Size = new Size(42, 30);
            btnMusic.TabIndex = 4;
            btnMusic.TabStop = false;
            btnMusic.Text = "\U0001F50A";
            btnMusic.UseVisualStyleBackColor = false;
            btnMusic.Click += new EventHandler(btnMusic_Click);
            // 
            // btnCode
            // 
            btnCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCode.BackColor = Color.FromArgb(40, 48, 70);
            btnCode.Cursor = Cursors.Hand;
            btnCode.FlatAppearance.BorderSize = 0;
            btnCode.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnCode.FlatStyle = FlatStyle.Flat;
            btnCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnCode.ForeColor = Color.White;
            btnCode.Location = new Point(420, 20);
            btnCode.Name = "btnCode";
            btnCode.Size = new Size(80, 30);
            btnCode.TabIndex = 3;
            btnCode.Text = "CODE";
            btnCode.UseVisualStyleBackColor = false;
            btnCode.Click += new EventHandler(btnCode_Click);
            // 
            // lblGems
            // 
            lblGems.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblGems.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblGems.ForeColor = Color.Cyan;
            lblGems.Location = new Point(700, 20);
            lblGems.Name = "lblGems";
            lblGems.Size = new Size(180, 30);
            lblGems.TabIndex = 2;
            lblGems.Text = "Gems: 0";
            lblGems.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCoins
            // 
            lblCoins.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCoins.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblCoins.ForeColor = Color.Gold;
            lblCoins.Location = new Point(510, 20);
            lblCoins.Name = "lblCoins";
            lblCoins.Size = new Size(180, 30);
            lblCoins.TabIndex = 1;
            lblCoins.Text = "Coins: 0";
            lblCoins.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblGameTitle
            // 
            lblGameTitle.AutoSize = true;
            lblGameTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblGameTitle.ForeColor = Color.White;
            lblGameTitle.Location = new Point(20, 14);
            lblGameTitle.Name = "lblGameTitle";
            lblGameTitle.Size = new Size(215, 41);
            lblGameTitle.TabIndex = 0;
            lblGameTitle.Text = "Transmogify";
            // 
            // grpTeam
            // 
            grpTeam.Controls.Add(tlpTeam);
            grpTeam.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpTeam.Dock = DockStyle.Fill;
            grpTeam.ForeColor = Color.White;
            grpTeam.Location = new Point(30, 80);
            grpTeam.Margin = new Padding(30, 10, 30, 6);
            grpTeam.Name = "grpTeam";
            grpTeam.Padding = new Padding(10, 6, 10, 10);
            grpTeam.Size = new Size(840, 190);
            grpTeam.TabIndex = 1;
            grpTeam.TabStop = false;
            grpTeam.Text = "Your Team";
            // 
            // tlpTeam
            // 
            tlpTeam.ColumnCount = 6;
            tlpTeam.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tlpTeam.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tlpTeam.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tlpTeam.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tlpTeam.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tlpTeam.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tlpTeam.Controls.Add(slot1, 0, 0);
            tlpTeam.Controls.Add(slot2, 1, 0);
            tlpTeam.Controls.Add(slot3, 2, 0);
            tlpTeam.Controls.Add(slot4, 3, 0);
            tlpTeam.Controls.Add(slot5, 4, 0);
            tlpTeam.Controls.Add(slot6, 5, 0);
            tlpTeam.Dock = DockStyle.Fill;
            tlpTeam.Location = new Point(10, 26);
            tlpTeam.Name = "tlpTeam";
            tlpTeam.RowCount = 1;
            tlpTeam.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTeam.Size = new Size(820, 154);
            tlpTeam.TabIndex = 0;
            // 
            // slot1
            // 
            slot1.BackColor = Color.FromArgb(24, 28, 40);
            slot1.Dock = DockStyle.Fill;
            slot1.Location = new Point(5, 5);
            slot1.Margin = new Padding(5);
            slot1.Name = "slot1";
            slot1.Size = new Size(126, 144);
            slot1.SlotNumber = 1;
            slot1.TabIndex = 0;
            slot1.TabStop = false;
            // 
            // slot2
            // 
            slot2.BackColor = Color.FromArgb(24, 28, 40);
            slot2.Dock = DockStyle.Fill;
            slot2.Location = new Point(141, 5);
            slot2.Margin = new Padding(5);
            slot2.Name = "slot2";
            slot2.Size = new Size(126, 144);
            slot2.SlotNumber = 2;
            slot2.TabIndex = 1;
            slot2.TabStop = false;
            // 
            // slot3
            // 
            slot3.BackColor = Color.FromArgb(24, 28, 40);
            slot3.Dock = DockStyle.Fill;
            slot3.Location = new Point(277, 5);
            slot3.Margin = new Padding(5);
            slot3.Name = "slot3";
            slot3.Size = new Size(126, 144);
            slot3.SlotNumber = 3;
            slot3.TabIndex = 2;
            slot3.TabStop = false;
            // 
            // slot4
            // 
            slot4.BackColor = Color.FromArgb(24, 28, 40);
            slot4.Dock = DockStyle.Fill;
            slot4.Location = new Point(413, 5);
            slot4.Margin = new Padding(5);
            slot4.Name = "slot4";
            slot4.Size = new Size(126, 144);
            slot4.SlotNumber = 4;
            slot4.TabIndex = 3;
            slot4.TabStop = false;
            // 
            // slot5
            // 
            slot5.BackColor = Color.FromArgb(24, 28, 40);
            slot5.Dock = DockStyle.Fill;
            slot5.Location = new Point(549, 5);
            slot5.Margin = new Padding(5);
            slot5.Name = "slot5";
            slot5.Size = new Size(126, 144);
            slot5.SlotNumber = 5;
            slot5.TabIndex = 4;
            slot5.TabStop = false;
            // 
            // slot6
            // 
            slot6.BackColor = Color.FromArgb(24, 28, 40);
            slot6.Dock = DockStyle.Fill;
            slot6.Location = new Point(685, 5);
            slot6.Margin = new Padding(5);
            slot6.Name = "slot6";
            slot6.Size = new Size(130, 144);
            slot6.SlotNumber = 6;
            slot6.TabIndex = 5;
            slot6.TabStop = false;
            // 
            // pnlStage
            // 
            pnlStage.BackColor = Color.FromArgb(24, 28, 40);
            pnlStage.BorderStyle = BorderStyle.FixedSingle;
            pnlStage.Controls.Add(tlpStage);
            pnlStage.Dock = DockStyle.Fill;
            pnlStage.Location = new Point(30, 290);
            pnlStage.Margin = new Padding(30, 6, 30, 6);
            pnlStage.Name = "pnlStage";
            pnlStage.Size = new Size(840, 88);
            pnlStage.TabIndex = 2;
            // 
            // tlpStage
            // 
            tlpStage.ColumnCount = 2;
            tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpStage.Controls.Add(pnlStageLeft, 0, 0);
            tlpStage.Controls.Add(tlpTiles, 1, 0);
            tlpStage.Dock = DockStyle.Fill;
            tlpStage.Location = new Point(0, 0);
            tlpStage.Margin = new Padding(0);
            tlpStage.Name = "tlpStage";
            tlpStage.RowCount = 1;
            tlpStage.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpStage.Size = new Size(838, 86);
            tlpStage.TabIndex = 0;
            // 
            // pnlStageLeft
            // 
            pnlStageLeft.Controls.Add(lblBest);
            pnlStageLeft.Controls.Add(lblStageNumber);
            pnlStageLeft.Controls.Add(lblStageCaption);
            pnlStageLeft.Controls.Add(pnlStageDivider);
            pnlStageLeft.Dock = DockStyle.Fill;
            pnlStageLeft.Location = new Point(0, 0);
            pnlStageLeft.Margin = new Padding(0);
            pnlStageLeft.Name = "pnlStageLeft";
            pnlStageLeft.Size = new Size(170, 86);
            pnlStageLeft.TabIndex = 0;
            // 
            // pnlStageDivider
            // 
            pnlStageDivider.BackColor = Color.FromArgb(60, 70, 95);
            pnlStageDivider.Dock = DockStyle.Right;
            pnlStageDivider.Location = new Point(169, 0);
            pnlStageDivider.Name = "pnlStageDivider";
            pnlStageDivider.Size = new Size(1, 86);
            pnlStageDivider.TabIndex = 0;
            // 
            // lblStageCaption
            // 
            lblStageCaption.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblStageCaption.ForeColor = Color.Silver;
            lblStageCaption.Location = new Point(16, 8);
            lblStageCaption.Name = "lblStageCaption";
            lblStageCaption.Size = new Size(100, 16);
            lblStageCaption.TabIndex = 1;
            lblStageCaption.Text = "STAGE";
            // 
            // lblStageNumber
            // 
            lblStageNumber.Font = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point);
            lblStageNumber.ForeColor = Color.White;
            lblStageNumber.Location = new Point(12, 20);
            lblStageNumber.Name = "lblStageNumber";
            lblStageNumber.Size = new Size(140, 44);
            lblStageNumber.TabIndex = 2;
            lblStageNumber.Text = "1";
            // 
            // lblBest
            // 
            lblBest.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblBest.ForeColor = Color.Silver;
            lblBest.Location = new Point(16, 62);
            lblBest.Name = "lblBest";
            lblBest.Size = new Size(140, 20);
            lblBest.TabIndex = 3;
            lblBest.Text = "Nothing cleared yet";
            // 
            // tlpTiles
            // 
            tlpTiles.ColumnCount = 4;
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.8333321F));
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.25F));
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.9166679F));
            tlpTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpTiles.Controls.Add(pnlTileWaves, 0, 0);
            tlpTiles.Controls.Add(pnlTileReward, 1, 0);
            tlpTiles.Controls.Add(pnlTileCash, 2, 0);
            tlpTiles.Controls.Add(pnlTileNext, 3, 0);
            tlpTiles.Dock = DockStyle.Fill;
            tlpTiles.Location = new Point(170, 0);
            tlpTiles.Margin = new Padding(0);
            tlpTiles.Name = "tlpTiles";
            tlpTiles.Padding = new Padding(4, 6, 10, 6);
            tlpTiles.RowCount = 1;
            tlpTiles.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTiles.Size = new Size(668, 86);
            tlpTiles.TabIndex = 1;
            // 
            // pnlTileWaves
            // 
            pnlTileWaves.BackColor = Color.FromArgb(32, 38, 54);
            pnlTileWaves.Controls.Add(lblWavesSub);
            pnlTileWaves.Controls.Add(lblWavesValue);
            pnlTileWaves.Controls.Add(lblWavesCaption);
            pnlTileWaves.Controls.Add(pnlTileWavesAccent);
            pnlTileWaves.Dock = DockStyle.Fill;
            pnlTileWaves.Location = new Point(4, 6);
            pnlTileWaves.Margin = new Padding(4, 0, 4, 0);
            pnlTileWaves.Name = "pnlTileWaves";
            pnlTileWaves.Size = new Size(128, 74);
            pnlTileWaves.TabIndex = 0;
            // 
            // pnlTileWavesAccent
            // 
            pnlTileWavesAccent.BackColor = Color.FromArgb(150, 120, 235);
            pnlTileWavesAccent.Dock = DockStyle.Left;
            pnlTileWavesAccent.Location = new Point(0, 0);
            pnlTileWavesAccent.Name = "pnlTileWavesAccent";
            pnlTileWavesAccent.Size = new Size(4, 74);
            pnlTileWavesAccent.TabIndex = 0;
            // 
            // lblWavesCaption
            // 
            lblWavesCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblWavesCaption.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblWavesCaption.ForeColor = Color.Silver;
            lblWavesCaption.Location = new Point(14, 8);
            lblWavesCaption.Name = "lblWavesCaption";
            lblWavesCaption.Size = new Size(110, 14);
            lblWavesCaption.TabIndex = 0;
            lblWavesCaption.Text = "WAVES";
            // 
            // lblWavesValue
            // 
            lblWavesValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblWavesValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            lblWavesValue.ForeColor = Color.White;
            lblWavesValue.Location = new Point(14, 20);
            lblWavesValue.Name = "lblWavesValue";
            lblWavesValue.Size = new Size(110, 28);
            lblWavesValue.TabIndex = 1;
            lblWavesValue.Text = "0";
            // 
            // lblWavesSub
            // 
            lblWavesSub.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblWavesSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblWavesSub.ForeColor = Color.Silver;
            lblWavesSub.Location = new Point(14, 48);
            lblWavesSub.Name = "lblWavesSub";
            lblWavesSub.Size = new Size(110, 16);
            lblWavesSub.TabIndex = 2;
            lblWavesSub.Text = "to clear";
            // 
            // pnlTileReward
            // 
            pnlTileReward.BackColor = Color.FromArgb(32, 38, 54);
            pnlTileReward.Controls.Add(lblRewardGems);
            pnlTileReward.Controls.Add(lblRewardCoins);
            pnlTileReward.Controls.Add(lblRewardCaption);
            pnlTileReward.Controls.Add(pnlTileRewardAccent);
            pnlTileReward.Dock = DockStyle.Fill;
            pnlTileReward.Location = new Point(14, 6);
            pnlTileReward.Margin = new Padding(4, 0, 4, 0);
            pnlTileReward.Name = "pnlTileReward";
            pnlTileReward.Size = new Size(196, 74);
            pnlTileReward.TabIndex = 1;
            // 
            // pnlTileRewardAccent
            // 
            pnlTileRewardAccent.BackColor = Color.Gold;
            pnlTileRewardAccent.Dock = DockStyle.Left;
            pnlTileRewardAccent.Location = new Point(0, 0);
            pnlTileRewardAccent.Name = "pnlTileRewardAccent";
            pnlTileRewardAccent.Size = new Size(4, 74);
            pnlTileRewardAccent.TabIndex = 0;
            // 
            // lblRewardCaption
            // 
            lblRewardCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblRewardCaption.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblRewardCaption.ForeColor = Color.Silver;
            lblRewardCaption.Location = new Point(14, 8);
            lblRewardCaption.Name = "lblRewardCaption";
            lblRewardCaption.Size = new Size(178, 14);
            lblRewardCaption.TabIndex = 0;
            lblRewardCaption.Text = "CLEAR REWARD";
            // 
            // lblRewardCoins
            // 
            lblRewardCoins.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblRewardCoins.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblRewardCoins.ForeColor = Color.Gold;
            lblRewardCoins.Location = new Point(14, 26);
            lblRewardCoins.Name = "lblRewardCoins";
            lblRewardCoins.Size = new Size(178, 20);
            lblRewardCoins.TabIndex = 1;
            lblRewardCoins.Text = "+0 coins";
            // 
            // lblRewardGems
            // 
            lblRewardGems.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblRewardGems.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblRewardGems.ForeColor = Color.Cyan;
            lblRewardGems.Location = new Point(14, 46);
            lblRewardGems.Name = "lblRewardGems";
            lblRewardGems.Size = new Size(178, 20);
            lblRewardGems.TabIndex = 2;
            lblRewardGems.Text = "+0 gems";
            // 
            // pnlTileCash
            // 
            pnlTileCash.BackColor = Color.FromArgb(32, 38, 54);
            pnlTileCash.Controls.Add(lblCashSub);
            pnlTileCash.Controls.Add(lblCashValue);
            pnlTileCash.Controls.Add(lblCashCaption);
            pnlTileCash.Controls.Add(pnlTileCashAccent);
            pnlTileCash.Dock = DockStyle.Fill;
            pnlTileCash.Location = new Point(24, 6);
            pnlTileCash.Margin = new Padding(4, 0, 4, 0);
            pnlTileCash.Name = "pnlTileCash";
            pnlTileCash.Size = new Size(142, 74);
            pnlTileCash.TabIndex = 2;
            // 
            // pnlTileCashAccent
            // 
            pnlTileCashAccent.BackColor = Color.FromArgb(90, 200, 120);
            pnlTileCashAccent.Dock = DockStyle.Left;
            pnlTileCashAccent.Location = new Point(0, 0);
            pnlTileCashAccent.Name = "pnlTileCashAccent";
            pnlTileCashAccent.Size = new Size(4, 74);
            pnlTileCashAccent.TabIndex = 0;
            // 
            // lblCashCaption
            // 
            lblCashCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCashCaption.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblCashCaption.ForeColor = Color.Silver;
            lblCashCaption.Location = new Point(14, 8);
            lblCashCaption.Name = "lblCashCaption";
            lblCashCaption.Size = new Size(124, 14);
            lblCashCaption.TabIndex = 0;
            lblCashCaption.Text = "START CASH";
            // 
            // lblCashValue
            // 
            lblCashValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCashValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            lblCashValue.ForeColor = Color.White;
            lblCashValue.Location = new Point(14, 20);
            lblCashValue.Name = "lblCashValue";
            lblCashValue.Size = new Size(124, 28);
            lblCashValue.TabIndex = 1;
            lblCashValue.Text = "0";
            // 
            // lblCashSub
            // 
            lblCashSub.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCashSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblCashSub.ForeColor = Color.Silver;
            lblCashSub.Location = new Point(14, 48);
            lblCashSub.Name = "lblCashSub";
            lblCashSub.Size = new Size(124, 16);
            lblCashSub.TabIndex = 2;
            lblCashSub.Text = "each run";
            // 
            // pnlTileNext
            // 
            pnlTileNext.BackColor = Color.FromArgb(32, 38, 54);
            pnlTileNext.Controls.Add(lblNextSub);
            pnlTileNext.Controls.Add(lblNextValue);
            pnlTileNext.Controls.Add(lblNextCaption);
            pnlTileNext.Controls.Add(pnlTileNextAccent);
            pnlTileNext.Dock = DockStyle.Fill;
            pnlTileNext.Location = new Point(34, 6);
            pnlTileNext.Margin = new Padding(4, 0, 4, 0);
            pnlTileNext.Name = "pnlTileNext";
            pnlTileNext.Size = new Size(155, 74);
            pnlTileNext.TabIndex = 3;
            // 
            // pnlTileNextAccent
            // 
            pnlTileNextAccent.BackColor = Color.Cyan;
            pnlTileNextAccent.Dock = DockStyle.Left;
            pnlTileNextAccent.Location = new Point(0, 0);
            pnlTileNextAccent.Name = "pnlTileNextAccent";
            pnlTileNextAccent.Size = new Size(4, 74);
            pnlTileNextAccent.TabIndex = 0;
            // 
            // lblNextCaption
            // 
            lblNextCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNextCaption.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblNextCaption.ForeColor = Color.Silver;
            lblNextCaption.Location = new Point(14, 8);
            lblNextCaption.Name = "lblNextCaption";
            lblNextCaption.Size = new Size(137, 14);
            lblNextCaption.TabIndex = 0;
            lblNextCaption.Text = "CHANGES AT";
            // 
            // lblNextValue
            // 
            lblNextValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNextValue.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            lblNextValue.ForeColor = Color.White;
            lblNextValue.Location = new Point(14, 20);
            lblNextValue.Name = "lblNextValue";
            lblNextValue.Size = new Size(137, 28);
            lblNextValue.TabIndex = 1;
            lblNextValue.Text = "Stage 5";
            // 
            // lblNextSub
            // 
            lblNextSub.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNextSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblNextSub.ForeColor = Color.Silver;
            lblNextSub.Location = new Point(14, 48);
            lblNextSub.Name = "lblNextSub";
            lblNextSub.Size = new Size(137, 16);
            lblNextSub.TabIndex = 2;
            lblNextSub.Text = "in 4 stages";
            // 
            // btnEnterTower
            // 
            btnEnterTower.BackColor = Color.FromArgb(46, 160, 90);
            btnEnterTower.Cursor = Cursors.Hand;
            btnEnterTower.FlatAppearance.BorderSize = 0;
            btnEnterTower.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 190, 110);
            btnEnterTower.FlatStyle = FlatStyle.Flat;
            btnEnterTower.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            btnEnterTower.ForeColor = Color.White;
            btnEnterTower.Dock = DockStyle.Fill;
            btnEnterTower.Location = new Point(30, 384);
            btnEnterTower.Margin = new Padding(30, 6, 30, 6);
            btnEnterTower.Name = "btnEnterTower";
            btnEnterTower.Size = new Size(840, 72);
            btnEnterTower.TabIndex = 3;
            btnEnterTower.Text = "ENTER THE TOWER";
            btnEnterTower.UseVisualStyleBackColor = false;
            btnEnterTower.Click += new EventHandler(btnEnterTower_Click);
            // 
            // btnSummon
            // 
            btnSummon.BackColor = Color.FromArgb(40, 48, 70);
            btnSummon.Cursor = Cursors.Hand;
            btnSummon.FlatAppearance.BorderSize = 0;
            btnSummon.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnSummon.FlatStyle = FlatStyle.Flat;
            btnSummon.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            btnSummon.ForeColor = Color.White;
            btnSummon.Dock = DockStyle.Fill;
            btnSummon.Location = new Point(5, 5);
            btnSummon.Margin = new Padding(5);
            btnSummon.Name = "btnSummon";
            btnSummon.Size = new Size(200, 56);
            btnSummon.TabIndex = 3;
            btnSummon.Text = "SUMMON";
            btnSummon.UseVisualStyleBackColor = false;
            btnSummon.Click += new EventHandler(btnSummon_Click);
            // 
            // btnCollection
            // 
            btnCollection.BackColor = Color.FromArgb(40, 48, 70);
            btnCollection.Cursor = Cursors.Hand;
            btnCollection.FlatAppearance.BorderSize = 0;
            btnCollection.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnCollection.FlatStyle = FlatStyle.Flat;
            btnCollection.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            btnCollection.ForeColor = Color.White;
            btnCollection.Dock = DockStyle.Fill;
            btnCollection.Location = new Point(215, 5);
            btnCollection.Margin = new Padding(5);
            btnCollection.Name = "btnCollection";
            btnCollection.Size = new Size(200, 56);
            btnCollection.TabIndex = 4;
            btnCollection.Text = "COLLECTION";
            btnCollection.UseVisualStyleBackColor = false;
            btnCollection.Click += new EventHandler(btnCollection_Click);
            // 
            // btnBackToTitle
            // 
            btnBackToTitle.BackColor = Color.FromArgb(40, 48, 70);
            btnBackToTitle.Cursor = Cursors.Hand;
            btnBackToTitle.FlatAppearance.BorderSize = 0;
            btnBackToTitle.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnBackToTitle.FlatStyle = FlatStyle.Flat;
            btnBackToTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            btnBackToTitle.ForeColor = Color.White;
            btnBackToTitle.Dock = DockStyle.Fill;
            btnBackToTitle.Location = new Point(425, 5);
            btnBackToTitle.Margin = new Padding(5);
            btnBackToTitle.Name = "btnBackToTitle";
            btnBackToTitle.Size = new Size(200, 56);
            btnBackToTitle.TabIndex = 5;
            btnBackToTitle.Text = "TITLE SCREEN";
            btnBackToTitle.UseVisualStyleBackColor = false;
            btnBackToTitle.Click += new EventHandler(btnBackToTitle_Click);
            // 
            // btnQuit
            // 
            btnQuit.BackColor = Color.FromArgb(120, 40, 40);
            btnQuit.Cursor = Cursors.Hand;
            btnQuit.FlatAppearance.BorderSize = 0;
            btnQuit.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 55, 55);
            btnQuit.FlatStyle = FlatStyle.Flat;
            btnQuit.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            btnQuit.ForeColor = Color.White;
            btnQuit.Dock = DockStyle.Fill;
            btnQuit.Location = new Point(635, 5);
            btnQuit.Margin = new Padding(5);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(200, 56);
            btnQuit.TabIndex = 6;
            btnQuit.Text = "QUIT GAME";
            btnQuit.UseVisualStyleBackColor = false;
            btnQuit.Click += new EventHandler(btnQuit_Click);
            // 
            // tlpActions
            // 
            tlpActions.ColumnCount = 4;
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.Controls.Add(btnSummon, 0, 0);
            tlpActions.Controls.Add(btnCollection, 1, 0);
            tlpActions.Controls.Add(btnBackToTitle, 2, 0);
            tlpActions.Controls.Add(btnQuit, 3, 0);
            tlpActions.Dock = DockStyle.Fill;
            tlpActions.Location = new Point(25, 462);
            tlpActions.Margin = new Padding(25, 0, 25, 0);
            tlpActions.Name = "tlpActions";
            tlpActions.RowCount = 1;
            tlpActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpActions.Size = new Size(850, 66);
            tlpActions.TabIndex = 4;
            // 
            // ssMain
            // 
            ssMain.BackColor = Color.FromArgb(18, 22, 34);
            ssMain.Dock = DockStyle.Fill;
            ssMain.Items.AddRange(new ToolStripItem[] { tsslMessage, tsslHighest });
            ssMain.Location = new Point(0, 576);
            ssMain.Margin = new Padding(0);
            ssMain.Name = "ssMain";
            ssMain.Size = new Size(900, 24);
            ssMain.SizingGrip = false;
            ssMain.TabIndex = 5;
            ssMain.Text = "ssMain";
            // 
            // tsslMessage
            // 
            tsslMessage.ForeColor = Color.Silver;
            tsslMessage.Name = "tsslMessage";
            tsslMessage.Size = new Size(783, 17);
            tsslMessage.Spring = true;
            tsslMessage.Text = "Welcome back!";
            tsslMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tsslHighest
            // 
            tsslHighest.ForeColor = Color.Silver;
            tsslHighest.Name = "tsslHighest";
            tsslHighest.Size = new Size(102, 17);
            tsslHighest.Text = "Highest floor: 0";
            // 
            // tipMain
            // 
            tipMain.SetToolTip(btnEnterTower, "Bring your team into the next floor of the tower.");
            tipMain.SetToolTip(btnSummon, "Spend gems to summon new units.");
            tipMain.SetToolTip(btnCollection, "Browse every unit and choose who goes on your team.");
            tipMain.SetToolTip(btnBackToTitle, "Go back to the title screen.");
            tipMain.SetToolTip(btnQuit, "Close the game.");
            tipMain.SetToolTip(btnCode, "Enter a code to get free resources.");
            tipMain.SetToolTip(btnMusic, "Turn the background music on or off.");
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 14, 22);
            ClientSize = new Size(900, 600);
            Controls.Add(tlpMain);
            MinimumSize = new Size(760, 580);
            Name = "Form3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Transmogify - Main Menu";
            tlpMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            grpTeam.ResumeLayout(false);
            tlpTeam.ResumeLayout(false);
            pnlTileNext.ResumeLayout(false);
            pnlTileCash.ResumeLayout(false);
            pnlTileReward.ResumeLayout(false);
            pnlTileWaves.ResumeLayout(false);
            tlpTiles.ResumeLayout(false);
            pnlStageLeft.ResumeLayout(false);
            tlpStage.ResumeLayout(false);
            pnlStage.ResumeLayout(false);
            tlpActions.ResumeLayout(false);
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Panel pnlHeader;
        private Label lblGameTitle;
        private Button btnCode;
        private Button btnMusic;
        private Label lblCoins;
        private Label lblGems;
        private GroupBox grpTeam;
        private TableLayoutPanel tlpTeam;
        private UnitSlotControl slot1;
        private UnitSlotControl slot2;
        private UnitSlotControl slot3;
        private UnitSlotControl slot4;
        private UnitSlotControl slot5;
        private UnitSlotControl slot6;
        private Panel pnlStage;
        private TableLayoutPanel tlpStage;
        private Panel pnlStageLeft;
        private Panel pnlStageDivider;
        private Label lblStageCaption;
        private Label lblStageNumber;
        private Label lblBest;
        private TableLayoutPanel tlpTiles;
        private Panel pnlTileWaves;
        private Panel pnlTileWavesAccent;
        private Label lblWavesCaption;
        private Label lblWavesValue;
        private Label lblWavesSub;
        private Panel pnlTileReward;
        private Panel pnlTileRewardAccent;
        private Label lblRewardCaption;
        private Label lblRewardCoins;
        private Label lblRewardGems;
        private Panel pnlTileCash;
        private Panel pnlTileCashAccent;
        private Label lblCashCaption;
        private Label lblCashValue;
        private Label lblCashSub;
        private Panel pnlTileNext;
        private Panel pnlTileNextAccent;
        private Label lblNextCaption;
        private Label lblNextValue;
        private Label lblNextSub;
        private Button btnEnterTower;
        private TableLayoutPanel tlpActions;
        private Button btnSummon;
        private Button btnCollection;
        private Button btnBackToTitle;
        private Button btnQuit;
        private StatusStrip ssMain;
        private ToolStripStatusLabel tsslMessage;
        private ToolStripStatusLabel tsslHighest;
        private ToolTip tipMain;
    }
}