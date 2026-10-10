namespace TDGachaGame
{
    partial class Form2
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
            tlpRoot = new TableLayoutPanel();
            pnlBanner = new DoubleBufferedPanel();
            btnMusic = new Button();
            lblTitle = new Label();
            lblSubtitle = new Label();
            btnStart = new Button();
            btnQuit = new Button();
            lblFooter = new Label();
            tmrBanner = new System.Windows.Forms.Timer(components);
            tlpRoot.SuspendLayout();
            pnlBanner.SuspendLayout();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(pnlBanner, 0, 0);
            tlpRoot.Controls.Add(lblTitle, 0, 1);
            tlpRoot.Controls.Add(lblSubtitle, 0, 2);
            tlpRoot.Controls.Add(btnStart, 0, 3);
            tlpRoot.Controls.Add(btnQuit, 0, 4);
            tlpRoot.Controls.Add(lblFooter, 0, 5);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Margin = new Padding(0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.RowCount = 6;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpRoot.Size = new Size(900, 600);
            tlpRoot.TabIndex = 0;
            // 
            // pnlBanner
            // 
            pnlBanner.BackColor = Color.FromArgb(8, 10, 18);
            pnlBanner.Controls.Add(btnMusic);
            pnlBanner.Dock = DockStyle.Fill;
            pnlBanner.Location = new Point(0, 0);
            pnlBanner.Margin = new Padding(0);
            pnlBanner.Name = "pnlBanner";
            pnlBanner.Size = new Size(900, 294);
            pnlBanner.TabIndex = 0;
            pnlBanner.Paint += pnlBanner_Paint;
            // 
            // btnMusic
            // 
            btnMusic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMusic.BackColor = Color.FromArgb(40, 48, 70);
            btnMusic.Cursor = Cursors.Hand;
            btnMusic.FlatAppearance.BorderSize = 0;
            btnMusic.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnMusic.FlatStyle = FlatStyle.Flat;
            btnMusic.Font = new Font("Segoe UI Emoji", 12F);
            btnMusic.ForeColor = Color.White;
            btnMusic.Location = new Point(848, 10);
            btnMusic.Name = "btnMusic";
            btnMusic.Size = new Size(42, 36);
            btnMusic.TabIndex = 0;
            btnMusic.TabStop = false;
            btnMusic.Text = "🔊";
            btnMusic.UseVisualStyleBackColor = false;
            btnMusic.Click += btnMusic_Click;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 44F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 294);
            lblTitle.Margin = new Padding(0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(900, 84);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Transmogify";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Dock = DockStyle.Fill;
            lblSubtitle.Font = new Font("Segoe UI", 13F, FontStyle.Italic);
            lblSubtitle.ForeColor = Color.Silver;
            lblSubtitle.Location = new Point(0, 378);
            lblSubtitle.Margin = new Padding(0);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(900, 40);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Everything changes shape eventually.";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.None;
            btnStart.BackColor = Color.FromArgb(46, 160, 90);
            btnStart.Cursor = Cursors.Hand;
            btnStart.DialogResult = DialogResult.OK;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 190, 110);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(320, 427);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(260, 66);
            btnStart.TabIndex = 3;
            btnStart.Text = "START";
            btnStart.UseVisualStyleBackColor = false;
            // 
            // btnQuit
            // 
            btnQuit.Anchor = AnchorStyles.None;
            btnQuit.BackColor = Color.FromArgb(120, 40, 40);
            btnQuit.Cursor = Cursors.Hand;
            btnQuit.DialogResult = DialogResult.Cancel;
            btnQuit.FlatAppearance.BorderSize = 0;
            btnQuit.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 55, 55);
            btnQuit.FlatStyle = FlatStyle.Flat;
            btnQuit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnQuit.ForeColor = Color.White;
            btnQuit.Location = new Point(320, 510);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(260, 48);
            btnQuit.TabIndex = 4;
            btnQuit.Text = "QUIT";
            btnQuit.UseVisualStyleBackColor = false;
            // 
            // lblFooter
            // 
            lblFooter.Dock = DockStyle.Fill;
            lblFooter.Font = new Font("Segoe UI", 9F);
            lblFooter.ForeColor = Color.Gray;
            lblFooter.Location = new Point(0, 566);
            lblFooter.Margin = new Padding(0);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(900, 34);
            lblFooter.TabIndex = 5;
            lblFooter.Text = "v0.1";
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tmrBanner
            // 
            tmrBanner.Enabled = true;
            tmrBanner.Interval = 33;
            tmrBanner.Tick += tmrBanner_Tick;
            // 
            // Form2
            // 
            AcceptButton = btnStart;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 14, 22);
            CancelButton = btnQuit;
            ClientSize = new Size(900, 600);
            Controls.Add(tlpRoot);
            MinimumSize = new Size(640, 560);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Transmogify";
            tlpRoot.ResumeLayout(false);
            pnlBanner.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRoot;
        private DoubleBufferedPanel pnlBanner;
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnStart;
        private Button btnQuit;
        private Label lblFooter;
        private Button btnMusic;
        private System.Windows.Forms.Timer tmrBanner;
    }
}