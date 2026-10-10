namespace TDGachaGame
{
    partial class Form7
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
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblCoins = new Label();
            pnlSide = new Panel();
            slotTarget = new UnitSlotControl();
            lblTargetName = new Label();
            lblLevel = new Label();
            pbExp = new ProgressBar();
            lblExp = new Label();
            grpStats = new GroupBox();
            tlpStats = new TableLayoutPanel();
            lblStatName1 = new Label();
            lblStatValue1 = new Label();
            lblStatName2 = new Label();
            lblStatValue2 = new Label();
            lblStatName3 = new Label();
            lblStatValue3 = new Label();
            lblStatName4 = new Label();
            lblStatValue4 = new Label();
            btnLevelUp = new Button();
            grpMerge = new GroupBox();
            rdoPickTarget = new RadioButton();
            rdoPickFodder = new RadioButton();
            lblMergeInfo = new Label();
            btnMerge = new Button();
            btnClearPicks = new Button();
            lblStatus = new Label();
            btnClose = new Button();
            pnlToolbar = new Panel();
            lblShow = new Label();
            cmbFilter = new ComboBox();
            lblHint = new Label();
            flowUnits = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlSide.SuspendLayout();
            grpStats.SuspendLayout();
            tlpStats.SuspendLayout();
            grpMerge.SuspendLayout();
            pnlToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(18, 22, 34);
            pnlHeader.Controls.Add(lblCoins);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1040, 64);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(200, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Upgrade Units";
            // 
            // lblCoins
            // 
            lblCoins.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCoins.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCoins.ForeColor = Color.Gold;
            lblCoins.Location = new Point(640, 17);
            lblCoins.Name = "lblCoins";
            lblCoins.Size = new Size(380, 30);
            lblCoins.TabIndex = 1;
            lblCoins.Text = "Coins: 0";
            lblCoins.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlSide
            // 
            pnlSide.BackColor = Color.FromArgb(18, 22, 34);
            pnlSide.Controls.Add(btnClose);
            pnlSide.Controls.Add(lblStatus);
            pnlSide.Controls.Add(grpMerge);
            pnlSide.Controls.Add(btnLevelUp);
            pnlSide.Controls.Add(grpStats);
            pnlSide.Controls.Add(lblExp);
            pnlSide.Controls.Add(pbExp);
            pnlSide.Controls.Add(lblLevel);
            pnlSide.Controls.Add(lblTargetName);
            pnlSide.Controls.Add(slotTarget);
            pnlSide.Dock = DockStyle.Right;
            pnlSide.Location = new Point(700, 64);
            pnlSide.Name = "pnlSide";
            pnlSide.Size = new Size(340, 616);
            pnlSide.TabIndex = 2;
            // 
            // slotTarget
            // 
            slotTarget.BackColor = Color.FromArgb(24, 28, 40);
            slotTarget.Location = new Point(20, 16);
            slotTarget.Name = "slotTarget";
            slotTarget.Size = new Size(130, 150);
            slotTarget.SlotNumber = 1;
            slotTarget.ShowSlotNumber = false;
            slotTarget.EmptyText = "No unit";
            slotTarget.TabIndex = 3;
            slotTarget.TabStop = false;
            // 
            // lblTargetName
            // 
            lblTargetName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblTargetName.ForeColor = Color.White;
            lblTargetName.Location = new Point(162, 16);
            lblTargetName.Name = "lblTargetName";
            lblTargetName.Size = new Size(170, 26);
            lblTargetName.TabIndex = 4;
            lblTargetName.Text = "No unit picked";
            lblTargetName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLevel
            // 
            lblLevel.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblLevel.ForeColor = Color.Gold;
            lblLevel.Location = new Point(162, 46);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(170, 24);
            lblLevel.TabIndex = 5;
            lblLevel.Text = "Level -";
            lblLevel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pbExp
            // 
            pbExp.Location = new Point(162, 82);
            pbExp.Name = "pbExp";
            pbExp.Size = new Size(166, 18);
            pbExp.Style = ProgressBarStyle.Continuous;
            pbExp.TabIndex = 6;
            // 
            // lblExp
            // 
            lblExp.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblExp.ForeColor = Color.Silver;
            lblExp.Location = new Point(162, 106);
            lblExp.Name = "lblExp";
            lblExp.Size = new Size(170, 22);
            lblExp.TabIndex = 7;
            lblExp.Text = "";
            lblExp.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpStats
            // 
            grpStats.Controls.Add(tlpStats);
            grpStats.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpStats.ForeColor = Color.White;
            grpStats.Location = new Point(20, 178);
            grpStats.Name = "grpStats";
            grpStats.Padding = new Padding(8, 6, 8, 8);
            grpStats.Size = new Size(308, 112);
            grpStats.TabIndex = 8;
            grpStats.TabStop = false;
            grpStats.Text = "Stats  (now  ->  next level)";
            // 
            // tlpStats
            // 
            tlpStats.ColumnCount = 2;
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpStats.Controls.Add(lblStatName1, 0, 0);
            tlpStats.Controls.Add(lblStatName2, 0, 1);
            tlpStats.Controls.Add(lblStatName3, 0, 2);
            tlpStats.Controls.Add(lblStatName4, 0, 3);
            tlpStats.Controls.Add(lblStatValue1, 1, 0);
            tlpStats.Controls.Add(lblStatValue2, 1, 1);
            tlpStats.Controls.Add(lblStatValue3, 1, 2);
            tlpStats.Controls.Add(lblStatValue4, 1, 3);
            tlpStats.Dock = DockStyle.Fill;
            tlpStats.Location = new Point(8, 24);
            tlpStats.Name = "tlpStats";
            tlpStats.RowCount = 4;
            tlpStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpStats.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpStats.Size = new Size(292, 80);
            tlpStats.TabIndex = 9;
            // 
            // lblStatName1
            // 
            lblStatName1.Dock = DockStyle.Fill;
            lblStatName1.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatName1.ForeColor = Color.Silver;
            lblStatName1.Location = new Point(0, 0);
            lblStatName1.Name = "lblStatName1";
            lblStatName1.Size = new Size(100, 24);
            lblStatName1.TabIndex = 10;
            lblStatName1.Text = "";
            lblStatName1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStatValue1
            // 
            lblStatValue1.Dock = DockStyle.Fill;
            lblStatValue1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatValue1.ForeColor = Color.White;
            lblStatValue1.Location = new Point(0, 0);
            lblStatValue1.Name = "lblStatValue1";
            lblStatValue1.Size = new Size(150, 24);
            lblStatValue1.TabIndex = 11;
            lblStatValue1.Text = "";
            lblStatValue1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStatName2
            // 
            lblStatName2.Dock = DockStyle.Fill;
            lblStatName2.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatName2.ForeColor = Color.Silver;
            lblStatName2.Location = new Point(0, 0);
            lblStatName2.Name = "lblStatName2";
            lblStatName2.Size = new Size(100, 24);
            lblStatName2.TabIndex = 12;
            lblStatName2.Text = "";
            lblStatName2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStatValue2
            // 
            lblStatValue2.Dock = DockStyle.Fill;
            lblStatValue2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatValue2.ForeColor = Color.White;
            lblStatValue2.Location = new Point(0, 0);
            lblStatValue2.Name = "lblStatValue2";
            lblStatValue2.Size = new Size(150, 24);
            lblStatValue2.TabIndex = 13;
            lblStatValue2.Text = "";
            lblStatValue2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStatName3
            // 
            lblStatName3.Dock = DockStyle.Fill;
            lblStatName3.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatName3.ForeColor = Color.Silver;
            lblStatName3.Location = new Point(0, 0);
            lblStatName3.Name = "lblStatName3";
            lblStatName3.Size = new Size(100, 24);
            lblStatName3.TabIndex = 14;
            lblStatName3.Text = "";
            lblStatName3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStatValue3
            // 
            lblStatValue3.Dock = DockStyle.Fill;
            lblStatValue3.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatValue3.ForeColor = Color.White;
            lblStatValue3.Location = new Point(0, 0);
            lblStatValue3.Name = "lblStatValue3";
            lblStatValue3.Size = new Size(150, 24);
            lblStatValue3.TabIndex = 15;
            lblStatValue3.Text = "";
            lblStatValue3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStatName4
            // 
            lblStatName4.Dock = DockStyle.Fill;
            lblStatName4.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatName4.ForeColor = Color.Silver;
            lblStatName4.Location = new Point(0, 0);
            lblStatName4.Name = "lblStatName4";
            lblStatName4.Size = new Size(100, 24);
            lblStatName4.TabIndex = 16;
            lblStatName4.Text = "";
            lblStatName4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStatValue4
            // 
            lblStatValue4.Dock = DockStyle.Fill;
            lblStatValue4.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatValue4.ForeColor = Color.White;
            lblStatValue4.Location = new Point(0, 0);
            lblStatValue4.Name = "lblStatValue4";
            lblStatValue4.Size = new Size(150, 24);
            lblStatValue4.TabIndex = 17;
            lblStatValue4.Text = "";
            lblStatValue4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnLevelUp
            // 
            btnLevelUp.BackColor = Color.FromArgb(46, 160, 90);
            btnLevelUp.Cursor = Cursors.Hand;
            btnLevelUp.Enabled = false;
            btnLevelUp.FlatAppearance.BorderSize = 0;
            btnLevelUp.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 190, 110);
            btnLevelUp.FlatStyle = FlatStyle.Flat;
            btnLevelUp.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnLevelUp.ForeColor = Color.White;
            btnLevelUp.Location = new Point(20, 298);
            btnLevelUp.Name = "btnLevelUp";
            btnLevelUp.Size = new Size(308, 40);
            btnLevelUp.TabIndex = 18;
            btnLevelUp.Text = "LEVEL UP";
            btnLevelUp.UseVisualStyleBackColor = false;
            btnLevelUp.Click += new EventHandler(btnLevelUp_Click);
            // 
            // grpMerge
            // 
            grpMerge.Controls.Add(btnClearPicks);
            grpMerge.Controls.Add(btnMerge);
            grpMerge.Controls.Add(lblMergeInfo);
            grpMerge.Controls.Add(rdoPickFodder);
            grpMerge.Controls.Add(rdoPickTarget);
            grpMerge.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpMerge.ForeColor = Color.White;
            grpMerge.Location = new Point(20, 346);
            grpMerge.Name = "grpMerge";
            grpMerge.Padding = new Padding(8, 6, 8, 8);
            grpMerge.Size = new Size(308, 166);
            grpMerge.TabIndex = 19;
            grpMerge.TabStop = false;
            grpMerge.Text = "Merge units into it";
            // 
            // rdoPickTarget
            // 
            rdoPickTarget.AutoSize = true;
            rdoPickTarget.Checked = true;
            rdoPickTarget.Cursor = Cursors.Hand;
            rdoPickTarget.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            rdoPickTarget.ForeColor = Color.White;
            rdoPickTarget.Location = new Point(12, 26);
            rdoPickTarget.Name = "rdoPickTarget";
            rdoPickTarget.Size = new Size(200, 21);
            rdoPickTarget.TabIndex = 20;
            rdoPickTarget.TabStop = true;
            rdoPickTarget.Text = "1. Pick the unit to upgrade";
            rdoPickTarget.UseVisualStyleBackColor = true;
            // 
            // rdoPickFodder
            // 
            rdoPickFodder.AutoSize = true;
            rdoPickFodder.Cursor = Cursors.Hand;
            rdoPickFodder.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            rdoPickFodder.ForeColor = Color.White;
            rdoPickFodder.Location = new Point(12, 50);
            rdoPickFodder.Name = "rdoPickFodder";
            rdoPickFodder.Size = new Size(200, 21);
            rdoPickFodder.TabIndex = 21;
            rdoPickFodder.Text = "2. Pick the units to merge";
            rdoPickFodder.UseVisualStyleBackColor = true;
            // 
            // lblMergeInfo
            // 
            lblMergeInfo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblMergeInfo.ForeColor = Color.Gainsboro;
            lblMergeInfo.Location = new Point(12, 80);
            lblMergeInfo.Name = "lblMergeInfo";
            lblMergeInfo.Size = new Size(284, 48);
            lblMergeInfo.TabIndex = 22;
            lblMergeInfo.Text = "Pick a unit to upgrade first.";
            lblMergeInfo.TextAlign = ContentAlignment.TopLeft;
            // 
            // btnMerge
            // 
            btnMerge.BackColor = Color.FromArgb(190, 120, 30);
            btnMerge.Cursor = Cursors.Hand;
            btnMerge.Enabled = false;
            btnMerge.FlatAppearance.BorderSize = 0;
            btnMerge.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 146, 46);
            btnMerge.FlatStyle = FlatStyle.Flat;
            btnMerge.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnMerge.ForeColor = Color.White;
            btnMerge.Location = new Point(12, 128);
            btnMerge.Name = "btnMerge";
            btnMerge.Size = new Size(170, 32);
            btnMerge.TabIndex = 23;
            btnMerge.Text = "MERGE";
            btnMerge.UseVisualStyleBackColor = false;
            btnMerge.Click += new EventHandler(btnMerge_Click);
            // 
            // btnClearPicks
            // 
            btnClearPicks.BackColor = Color.FromArgb(40, 48, 70);
            btnClearPicks.Cursor = Cursors.Hand;
            btnClearPicks.FlatAppearance.BorderSize = 0;
            btnClearPicks.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnClearPicks.FlatStyle = FlatStyle.Flat;
            btnClearPicks.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnClearPicks.ForeColor = Color.White;
            btnClearPicks.Location = new Point(190, 128);
            btnClearPicks.Name = "btnClearPicks";
            btnClearPicks.Size = new Size(106, 32);
            btnClearPicks.TabIndex = 24;
            btnClearPicks.Text = "Clear picks";
            btnClearPicks.UseVisualStyleBackColor = false;
            btnClearPicks.Click += new EventHandler(btnClearPicks_Click);
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.Gainsboro;
            lblStatus.Location = new Point(20, 518);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(308, 44);
            lblStatus.TabIndex = 25;
            lblStatus.Text = "";
            lblStatus.TextAlign = ContentAlignment.TopLeft;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(40, 48, 70);
            btnClose.Cursor = Cursors.Hand;
            btnClose.DialogResult = DialogResult.OK;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(20, 566);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(308, 38);
            btnClose.TabIndex = 26;
            btnClose.Text = "Done";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(lblHint);
            pnlToolbar.Controls.Add(cmbFilter);
            pnlToolbar.Controls.Add(lblShow);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 64);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(700, 46);
            pnlToolbar.TabIndex = 27;
            // 
            // lblShow
            // 
            lblShow.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblShow.ForeColor = Color.Silver;
            lblShow.Location = new Point(20, 12);
            lblShow.Name = "lblShow";
            lblShow.Size = new Size(45, 19);
            lblShow.TabIndex = 28;
            lblShow.Text = "Show:";
            lblShow.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbFilter
            // 
            cmbFilter.BackColor = Color.FromArgb(40, 48, 70);
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.FlatStyle = FlatStyle.Flat;
            cmbFilter.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cmbFilter.ForeColor = Color.White;
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(70, 9);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(130, 25);
            cmbFilter.TabIndex = 29;
            cmbFilter.SelectedIndexChanged += new EventHandler(cmbFilter_SelectedIndexChanged);
            // 
            // lblHint
            // 
            lblHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblHint.AutoEllipsis = true;
            lblHint.Font = new Font("Segoe UI", 10F, FontStyle.Italic, GraphicsUnit.Point);
            lblHint.ForeColor = Color.Gainsboro;
            lblHint.Location = new Point(216, 8);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(470, 28);
            lblHint.TabIndex = 30;
            lblHint.Text = "Click a unit to pick it.";
            lblHint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowUnits
            // 
            flowUnits.AutoScroll = true;
            flowUnits.BackColor = Color.FromArgb(12, 14, 22);
            flowUnits.Dock = DockStyle.Fill;
            flowUnits.Location = new Point(0, 110);
            flowUnits.Margin = new Padding(0);
            flowUnits.Name = "flowUnits";
            flowUnits.Padding = new Padding(14, 8, 8, 8);
            flowUnits.Size = new Size(700, 570);
            flowUnits.TabIndex = 31;
            // 
            // Form7
            // 
            AcceptButton = btnClose;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 14, 22);
            CancelButton = btnClose;
            ClientSize = new Size(1040, 680);
            Controls.Add(flowUnits);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlSide);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(1000, 720);
            Name = "Form7";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transmogify - Upgrade Units";
            pnlToolbar.ResumeLayout(false);
            grpMerge.ResumeLayout(false);
            tlpStats.ResumeLayout(false);
            grpStats.ResumeLayout(false);
            pnlSide.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblCoins;
        private Panel pnlSide;
        private UnitSlotControl slotTarget;
        private Label lblTargetName;
        private Label lblLevel;
        private ProgressBar pbExp;
        private Label lblExp;
        private GroupBox grpStats;
        private TableLayoutPanel tlpStats;
        private Label lblStatName1;
        private Label lblStatValue1;
        private Label lblStatName2;
        private Label lblStatValue2;
        private Label lblStatName3;
        private Label lblStatValue3;
        private Label lblStatName4;
        private Label lblStatValue4;
        private Button btnLevelUp;
        private GroupBox grpMerge;
        private RadioButton rdoPickTarget;
        private RadioButton rdoPickFodder;
        private Label lblMergeInfo;
        private Button btnMerge;
        private Button btnClearPicks;
        private Label lblStatus;
        private Button btnClose;
        private Panel pnlToolbar;
        private Label lblShow;
        private ComboBox cmbFilter;
        private Label lblHint;
        private FlowLayoutPanel flowUnits;
    }
}