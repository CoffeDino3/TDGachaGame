namespace TDGachaGame
{
    partial class Form4
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
            lblCount = new Label();
            lblTitle = new Label();
            grpTeam = new GroupBox();
            tlpTeam = new TableLayoutPanel();
            pnlToolbar = new Panel();
            lblShow = new Label();
            cmbFilter = new ComboBox();
            lblStatus = new Label();
            btnExpand = new Button();
            btnUpgrade = new Button();
            btnSell = new Button();
            btnClear = new Button();
            btnDone = new Button();
            flowUnits = new FlowLayoutPanel();
            tlpMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            grpTeam.SuspendLayout();
            tlpTeam.SuspendLayout();
            pnlToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(pnlHeader, 0, 0);
            tlpMain.Controls.Add(grpTeam, 0, 1);
            tlpMain.Controls.Add(pnlToolbar, 0, 2);
            tlpMain.Controls.Add(flowUnits, 0, 3);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Margin = new Padding(0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 4;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 168F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.Size = new Size(900, 640);
            tlpMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(18, 22, 34);
            pnlHeader.Controls.Add(lblCount);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(900, 64);
            pnlHeader.TabIndex = 0;
            // 
            // lblCount
            // 
            lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCount.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblCount.ForeColor = Color.Silver;
            lblCount.Location = new Point(600, 17);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(280, 30);
            lblCount.TabIndex = 1;
            lblCount.Text = "0 units";
            lblCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(170, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Collection";
            // 
            // grpTeam
            // 
            grpTeam.Controls.Add(tlpTeam);
            grpTeam.Dock = DockStyle.Fill;
            grpTeam.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            grpTeam.ForeColor = Color.White;
            grpTeam.Location = new Point(20, 74);
            grpTeam.Margin = new Padding(20, 10, 20, 4);
            grpTeam.Name = "grpTeam";
            grpTeam.Padding = new Padding(10, 6, 10, 10);
            grpTeam.Size = new Size(860, 150);
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
            tlpTeam.Dock = DockStyle.Fill;
            tlpTeam.Location = new Point(10, 26);
            tlpTeam.Name = "tlpTeam";
            tlpTeam.RowCount = 1;
            tlpTeam.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTeam.Size = new Size(840, 114);
            tlpTeam.TabIndex = 0;
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(btnExpand);
            pnlToolbar.Controls.Add(btnSell);
            pnlToolbar.Controls.Add(btnUpgrade);
            pnlToolbar.Controls.Add(btnDone);
            pnlToolbar.Controls.Add(btnClear);
            pnlToolbar.Controls.Add(lblStatus);
            pnlToolbar.Controls.Add(cmbFilter);
            pnlToolbar.Controls.Add(lblShow);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(0, 228);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(900, 88);
            pnlToolbar.TabIndex = 2;
            // 
            // lblShow
            // 
            lblShow.AutoSize = true;
            lblShow.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblShow.ForeColor = Color.Silver;
            lblShow.Location = new Point(20, 15);
            lblShow.Name = "lblShow";
            lblShow.Size = new Size(45, 19);
            lblShow.TabIndex = 0;
            lblShow.Text = "Show:";
            // 
            // cmbFilter
            // 
            cmbFilter.BackColor = Color.FromArgb(40, 48, 70);
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.FlatStyle = FlatStyle.Flat;
            cmbFilter.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cmbFilter.ForeColor = Color.White;
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(70, 11);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(130, 25);
            cmbFilter.TabIndex = 1;
            cmbFilter.SelectedIndexChanged += new EventHandler(cmbFilter_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.AutoEllipsis = true;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Italic, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.Gainsboro;
            lblStatus.Location = new Point(20, 50);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(860, 28);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Pick a unit, then a slot.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnExpand
            // 
            btnExpand.BackColor = Color.FromArgb(120, 70, 170);
            btnExpand.Cursor = Cursors.Hand;
            btnExpand.FlatAppearance.BorderSize = 0;
            btnExpand.FlatAppearance.MouseOverBackColor = Color.FromArgb(146, 92, 200);
            btnExpand.FlatStyle = FlatStyle.Flat;
            btnExpand.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnExpand.ForeColor = Color.White;
            btnExpand.Location = new Point(216, 8);
            btnExpand.Name = "btnExpand";
            btnExpand.Size = new Size(210, 32);
            btnExpand.TabIndex = 7;
            btnExpand.Text = "+10 slots  -  100 gems";
            btnExpand.UseVisualStyleBackColor = false;
            btnExpand.Click += new EventHandler(btnExpand_Click);
            // 
            // btnUpgrade
            // 
            btnUpgrade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpgrade.BackColor = Color.FromArgb(70, 110, 200);
            btnUpgrade.Cursor = Cursors.Hand;
            btnUpgrade.FlatAppearance.BorderSize = 0;
            btnUpgrade.FlatAppearance.MouseOverBackColor = Color.FromArgb(92, 134, 226);
            btnUpgrade.FlatStyle = FlatStyle.Flat;
            btnUpgrade.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnUpgrade.ForeColor = Color.White;
            btnUpgrade.Location = new Point(440, 8);
            btnUpgrade.Name = "btnUpgrade";
            btnUpgrade.Size = new Size(140, 32);
            btnUpgrade.TabIndex = 5;
            btnUpgrade.Text = "Upgrade / Merge";
            btnUpgrade.UseVisualStyleBackColor = false;
            btnUpgrade.Click += new EventHandler(btnUpgrade_Click);
            // 
            // btnSell
            // 
            btnSell.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSell.BackColor = Color.FromArgb(190, 120, 30);
            btnSell.Cursor = Cursors.Hand;
            btnSell.FlatAppearance.BorderSize = 0;
            btnSell.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 146, 46);
            btnSell.FlatStyle = FlatStyle.Flat;
            btnSell.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnSell.ForeColor = Color.White;
            btnSell.Location = new Point(588, 8);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(80, 32);
            btnSell.TabIndex = 6;
            btnSell.Text = "Sell";
            btnSell.UseVisualStyleBackColor = false;
            btnSell.Click += new EventHandler(btnSell_Click);
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.BackColor = Color.FromArgb(40, 48, 70);
            btnClear.Cursor = Cursors.Hand;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(676, 8);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 32);
            btnClear.TabIndex = 3;
            btnClear.Text = "Clear team";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += new EventHandler(btnClear_Click);
            // 
            // btnDone
            // 
            btnDone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDone.BackColor = Color.FromArgb(46, 160, 90);
            btnDone.Cursor = Cursors.Hand;
            btnDone.DialogResult = DialogResult.OK;
            btnDone.FlatAppearance.BorderSize = 0;
            btnDone.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 190, 110);
            btnDone.FlatStyle = FlatStyle.Flat;
            btnDone.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnDone.ForeColor = Color.White;
            btnDone.Location = new Point(784, 8);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(96, 32);
            btnDone.TabIndex = 4;
            btnDone.Text = "Done";
            btnDone.UseVisualStyleBackColor = false;
            // 
            // flowUnits
            // 
            flowUnits.AutoScroll = true;
            flowUnits.BackColor = Color.FromArgb(12, 14, 22);
            flowUnits.Dock = DockStyle.Fill;
            flowUnits.Location = new Point(0, 278);
            flowUnits.Margin = new Padding(0);
            flowUnits.Name = "flowUnits";
            flowUnits.Padding = new Padding(14, 8, 8, 8);
            flowUnits.Size = new Size(900, 362);
            flowUnits.TabIndex = 3;
            // 
            // Form4
            // 
            AcceptButton = btnDone;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 14, 22);
            CancelButton = btnDone;
            ClientSize = new Size(900, 640);
            Controls.Add(tlpMain);
            MinimumSize = new Size(760, 560);
            Name = "Form4";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transmogify - Collection";
            tlpMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            grpTeam.ResumeLayout(false);
            tlpTeam.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblCount;
        private GroupBox grpTeam;
        private TableLayoutPanel tlpTeam;
        private Panel pnlToolbar;
        private Label lblShow;
        private ComboBox cmbFilter;
        private Label lblStatus;
        private Button btnExpand;
        private Button btnUpgrade;
        private Button btnSell;
        private Button btnClear;
        private Button btnDone;
        private FlowLayoutPanel flowUnits;
    }
}