namespace TDGachaGame
{
    partial class Form6
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
            btnCode = new Button();
            btnMusic = new Button();
            lblGems = new Label();
            lblSlots = new Label();
            lblTitle = new Label();
            display = new GachaDisplayControl();
            pnlReveal = new Panel();
            lblUnitName = new Label();
            lblRarity = new Label();
            lblChance = new Label();
            grpResults = new GroupBox();
            flowResults = new FlowLayoutPanel();
            tlpPulls = new TableLayoutPanel();
            btnPull1 = new Button();
            btnPull5 = new Button();
            btnPull10 = new Button();
            btnPullMax = new Button();
            pnlFooter = new Panel();
            lblStatus = new Label();
            pbReveal = new ProgressBar();
            btnOdds = new Button();
            btnSkip = new Button();
            btnBack = new Button();
            tipMain = new ToolTip(components);
            tmrAnim = new System.Windows.Forms.Timer(components);
            tlpMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlReveal.SuspendLayout();
            grpResults.SuspendLayout();
            tlpPulls.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(pnlHeader, 0, 0);
            tlpMain.Controls.Add(display, 0, 1);
            tlpMain.Controls.Add(pnlReveal, 0, 2);
            tlpMain.Controls.Add(grpResults, 0, 3);
            tlpMain.Controls.Add(tlpPulls, 0, 4);
            tlpMain.Controls.Add(pnlFooter, 0, 5);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Margin = new Padding(0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 6;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpMain.Size = new Size(900, 720);
            tlpMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(18, 22, 34);
            pnlHeader.Controls.Add(btnCode);
            pnlHeader.Controls.Add(btnMusic);
            pnlHeader.Controls.Add(lblGems);
            pnlHeader.Controls.Add(lblSlots);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(900, 64);
            pnlHeader.TabIndex = 0;
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
            btnCode.Location = new Point(360, 17);
            btnCode.Name = "btnCode";
            btnCode.Size = new Size(80, 30);
            btnCode.TabIndex = 5;
            btnCode.Text = "CODE";
            btnCode.UseVisualStyleBackColor = false;
            btnCode.Click += new EventHandler(btnCode_Click);
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
            btnMusic.Location = new Point(448, 17);
            btnMusic.Name = "btnMusic";
            btnMusic.Size = new Size(42, 30);
            btnMusic.TabIndex = 4;
            btnMusic.TabStop = false;
            btnMusic.Text = "\U0001F50A";
            btnMusic.UseVisualStyleBackColor = false;
            btnMusic.Click += new EventHandler(btnMusic_Click);
            // 
            // lblGems
            // 
            lblGems.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblGems.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblGems.ForeColor = Color.Cyan;
            lblGems.Location = new Point(700, 17);
            lblGems.Name = "lblGems";
            lblGems.Size = new Size(180, 30);
            lblGems.TabIndex = 3;
            lblGems.Text = "Gems: 0";
            lblGems.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSlots
            // 
            lblSlots.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSlots.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblSlots.ForeColor = Color.Silver;
            lblSlots.Location = new Point(500, 17);
            lblSlots.Name = "lblSlots";
            lblSlots.Size = new Size(190, 30);
            lblSlots.TabIndex = 2;
            lblSlots.Text = "Collection: 0 / 100";
            lblSlots.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(130, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Summon";
            // 
            // display
            // 
            display.BackColor = Color.FromArgb(10, 12, 20);
            display.Dock = DockStyle.Fill;
            display.Location = new Point(0, 64);
            display.Margin = new Padding(0);
            display.Name = "display";
            display.Size = new Size(900, 300);
            display.TabIndex = 1;
            // 
            // pnlReveal
            // 
            pnlReveal.BackColor = Color.FromArgb(10, 12, 20);
            pnlReveal.Controls.Add(lblChance);
            pnlReveal.Controls.Add(lblRarity);
            pnlReveal.Controls.Add(lblUnitName);
            pnlReveal.Dock = DockStyle.Fill;
            pnlReveal.Location = new Point(0, 368);
            pnlReveal.Margin = new Padding(0);
            pnlReveal.Name = "pnlReveal";
            pnlReveal.Size = new Size(900, 88);
            pnlReveal.TabIndex = 2;
            // 
            // lblUnitName
            // 
            lblUnitName.Dock = DockStyle.Top;
            lblUnitName.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblUnitName.ForeColor = Color.White;
            lblUnitName.Location = new Point(0, 0);
            lblUnitName.Name = "lblUnitName";
            lblUnitName.Size = new Size(900, 38);
            lblUnitName.TabIndex = 0;
            lblUnitName.Text = "Pick a summon below";
            lblUnitName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRarity
            // 
            lblRarity.Dock = DockStyle.Top;
            lblRarity.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblRarity.ForeColor = Color.Silver;
            lblRarity.Location = new Point(0, 38);
            lblRarity.Name = "lblRarity";
            lblRarity.Size = new Size(900, 28);
            lblRarity.TabIndex = 1;
            lblRarity.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblChance
            // 
            lblChance.Dock = DockStyle.Top;
            lblChance.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblChance.ForeColor = Color.Silver;
            lblChance.Location = new Point(0, 66);
            lblChance.Name = "lblChance";
            lblChance.Size = new Size(900, 22);
            lblChance.TabIndex = 2;
            lblChance.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpResults
            // 
            grpResults.Controls.Add(flowResults);
            grpResults.Dock = DockStyle.Fill;
            grpResults.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpResults.ForeColor = Color.White;
            grpResults.Location = new Point(20, 460);
            grpResults.Margin = new Padding(20, 4, 20, 2);
            grpResults.Name = "grpResults";
            grpResults.Padding = new Padding(8, 4, 8, 8);
            grpResults.Size = new Size(860, 136);
            grpResults.TabIndex = 3;
            grpResults.TabStop = false;
            grpResults.Text = "Summon results";
            // 
            // flowResults
            // 
            flowResults.AutoScroll = true;
            flowResults.BackColor = Color.FromArgb(12, 14, 22);
            flowResults.Dock = DockStyle.Fill;
            flowResults.Location = new Point(8, 24);
            flowResults.Name = "flowResults";
            flowResults.Padding = new Padding(2);
            flowResults.Size = new Size(844, 118);
            flowResults.TabIndex = 0;
            // 
            // tlpPulls
            // 
            tlpPulls.ColumnCount = 4;
            tlpPulls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpPulls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpPulls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpPulls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpPulls.Controls.Add(btnPull1, 0, 0);
            tlpPulls.Controls.Add(btnPull5, 1, 0);
            tlpPulls.Controls.Add(btnPull10, 2, 0);
            tlpPulls.Controls.Add(btnPullMax, 3, 0);
            tlpPulls.Dock = DockStyle.Fill;
            tlpPulls.Location = new Point(15, 598);
            tlpPulls.Margin = new Padding(15, 2, 15, 2);
            tlpPulls.Name = "tlpPulls";
            tlpPulls.RowCount = 1;
            tlpPulls.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPulls.Size = new Size(870, 72);
            tlpPulls.TabIndex = 4;
            // 
            // btnPull1
            // 
            btnPull1.BackColor = Color.FromArgb(52, 96, 190);
            btnPull1.Cursor = Cursors.Hand;
            btnPull1.Dock = DockStyle.Fill;
            btnPull1.FlatAppearance.BorderSize = 0;
            btnPull1.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 120, 225);
            btnPull1.FlatStyle = FlatStyle.Flat;
            btnPull1.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnPull1.ForeColor = Color.White;
            btnPull1.Location = new Point(5, 5);
            btnPull1.Margin = new Padding(5);
            btnPull1.Name = "btnPull1";
            btnPull1.Size = new Size(207, 62);
            btnPull1.TabIndex = 0;
            btnPull1.Text = "SUMMON x1\r\n50 gems";
            btnPull1.UseVisualStyleBackColor = false;
            btnPull1.Click += new EventHandler(btnPull1_Click);
            // 
            // btnPull5
            // 
            btnPull5.BackColor = Color.FromArgb(52, 96, 190);
            btnPull5.Cursor = Cursors.Hand;
            btnPull5.Dock = DockStyle.Fill;
            btnPull5.FlatAppearance.BorderSize = 0;
            btnPull5.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 120, 225);
            btnPull5.FlatStyle = FlatStyle.Flat;
            btnPull5.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnPull5.ForeColor = Color.White;
            btnPull5.Location = new Point(222, 5);
            btnPull5.Margin = new Padding(5);
            btnPull5.Name = "btnPull5";
            btnPull5.Size = new Size(207, 62);
            btnPull5.TabIndex = 1;
            btnPull5.Text = "SUMMON x5\r\n250 gems";
            btnPull5.UseVisualStyleBackColor = false;
            btnPull5.Click += new EventHandler(btnPull5_Click);
            // 
            // btnPull10
            // 
            btnPull10.BackColor = Color.FromArgb(52, 96, 190);
            btnPull10.Cursor = Cursors.Hand;
            btnPull10.Dock = DockStyle.Fill;
            btnPull10.FlatAppearance.BorderSize = 0;
            btnPull10.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 120, 225);
            btnPull10.FlatStyle = FlatStyle.Flat;
            btnPull10.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnPull10.ForeColor = Color.White;
            btnPull10.Location = new Point(439, 5);
            btnPull10.Margin = new Padding(5);
            btnPull10.Name = "btnPull10";
            btnPull10.Size = new Size(207, 62);
            btnPull10.TabIndex = 2;
            btnPull10.Text = "SUMMON x10\r\n500 gems";
            btnPull10.UseVisualStyleBackColor = false;
            btnPull10.Click += new EventHandler(btnPull10_Click);
            // 
            // btnPullMax
            // 
            btnPullMax.BackColor = Color.FromArgb(176, 124, 32);
            btnPullMax.Cursor = Cursors.Hand;
            btnPullMax.Dock = DockStyle.Fill;
            btnPullMax.FlatAppearance.BorderSize = 0;
            btnPullMax.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 150, 45);
            btnPullMax.FlatStyle = FlatStyle.Flat;
            btnPullMax.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnPullMax.ForeColor = Color.White;
            btnPullMax.Location = new Point(656, 5);
            btnPullMax.Margin = new Padding(5);
            btnPullMax.Name = "btnPullMax";
            btnPullMax.Size = new Size(209, 62);
            btnPullMax.TabIndex = 3;
            btnPullMax.Text = "SUMMON MAX";
            btnPullMax.UseVisualStyleBackColor = false;
            btnPullMax.Click += new EventHandler(btnPullMax_Click);
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(18, 22, 34);
            pnlFooter.Controls.Add(lblStatus);
            pnlFooter.Controls.Add(pbReveal);
            pnlFooter.Controls.Add(btnOdds);
            pnlFooter.Controls.Add(btnSkip);
            pnlFooter.Controls.Add(btnBack);
            pnlFooter.Dock = DockStyle.Fill;
            pnlFooter.Location = new Point(0, 672);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(900, 48);
            pnlFooter.TabIndex = 5;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.OrangeRed;
            lblStatus.Location = new Point(20, 4);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(430, 40);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pbReveal
            // 
            pbReveal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbReveal.Location = new Point(460, 16);
            pbReveal.Name = "pbReveal";
            pbReveal.Size = new Size(110, 16);
            pbReveal.Style = ProgressBarStyle.Continuous;
            pbReveal.TabIndex = 1;
            pbReveal.Visible = false;
            // 
            // btnOdds
            // 
            btnOdds.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOdds.BackColor = Color.FromArgb(40, 48, 70);
            btnOdds.Cursor = Cursors.Hand;
            btnOdds.FlatAppearance.BorderSize = 0;
            btnOdds.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnOdds.FlatStyle = FlatStyle.Flat;
            btnOdds.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnOdds.ForeColor = Color.White;
            btnOdds.Location = new Point(580, 8);
            btnOdds.Name = "btnOdds";
            btnOdds.Size = new Size(90, 32);
            btnOdds.TabIndex = 2;
            btnOdds.Text = "ODDS";
            btnOdds.UseVisualStyleBackColor = false;
            btnOdds.Click += new EventHandler(btnOdds_Click);
            // 
            // btnSkip
            // 
            btnSkip.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSkip.BackColor = Color.FromArgb(40, 48, 70);
            btnSkip.Cursor = Cursors.Hand;
            btnSkip.Enabled = false;
            btnSkip.FlatAppearance.BorderSize = 0;
            btnSkip.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnSkip.FlatStyle = FlatStyle.Flat;
            btnSkip.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnSkip.ForeColor = Color.White;
            btnSkip.Location = new Point(680, 8);
            btnSkip.Name = "btnSkip";
            btnSkip.Size = new Size(90, 32);
            btnSkip.TabIndex = 3;
            btnSkip.Text = "SKIP";
            btnSkip.UseVisualStyleBackColor = false;
            btnSkip.Click += new EventHandler(btnSkip_Click);
            // 
            // btnBack
            // 
            btnBack.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBack.BackColor = Color.FromArgb(40, 48, 70);
            btnBack.Cursor = Cursors.Hand;
            btnBack.DialogResult = DialogResult.Cancel;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(780, 8);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(100, 32);
            btnBack.TabIndex = 4;
            btnBack.Text = "BACK";
            btnBack.UseVisualStyleBackColor = false;
            // 
            // tipMain
            // 
            tipMain.SetToolTip(btnPull1, "Spend 50 gems for one summon.");
            tipMain.SetToolTip(btnPull5, "Spend 250 gems for five summons.");
            tipMain.SetToolTip(btnPull10, "Spend 500 gems for ten summons.");
            tipMain.SetToolTip(btnPullMax, "Summon as many times as your gems allow (up to 50). Cancelled if the collection can't fit them all.");
            tipMain.SetToolTip(btnOdds, "See the summon odds for the stage you are on.");
            tipMain.SetToolTip(btnSkip, "Skip the animation and see every result right away.");
            tipMain.SetToolTip(btnBack, "Go back to the main menu.");
            tipMain.SetToolTip(btnCode, "Enter a code to get free resources.");
            tipMain.SetToolTip(btnMusic, "Turn the background music on or off.");
            // 
            // tmrAnim
            // 
            tmrAnim.Enabled = true;
            tmrAnim.Interval = 16;
            tmrAnim.Tick += new EventHandler(tmrAnim_Tick);
            // 
            // Form6
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 14, 22);
            CancelButton = btnBack;
            ClientSize = new Size(900, 720);
            Controls.Add(tlpMain);
            MinimumSize = new Size(780, 640);
            Name = "Form6";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transmogify - Summon";
            tlpMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlReveal.ResumeLayout(false);
            grpResults.ResumeLayout(false);
            tlpPulls.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Panel pnlHeader;
        private Button btnCode;
        private Button btnMusic;
        private Label lblGems;
        private Label lblSlots;
        private Label lblTitle;
        private GachaDisplayControl display;
        private Panel pnlReveal;
        private Label lblUnitName;
        private Label lblRarity;
        private Label lblChance;
        private GroupBox grpResults;
        private FlowLayoutPanel flowResults;
        private TableLayoutPanel tlpPulls;
        private Button btnPull1;
        private Button btnPull5;
        private Button btnPull10;
        private Button btnPullMax;
        private Panel pnlFooter;
        private Label lblStatus;
        private ProgressBar pbReveal;
        private Button btnOdds;
        private Button btnSkip;
        private Button btnBack;
        private ToolTip tipMain;
        private System.Windows.Forms.Timer tmrAnim;
    }
}