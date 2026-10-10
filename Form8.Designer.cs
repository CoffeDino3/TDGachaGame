namespace TDGachaGame
{
    partial class Form8
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
            lblSelected = new Label();
            lblTotal = new Label();
            lblTeamWarning = new Label();
            btnSelectDuplicates = new Button();
            btnClearSelection = new Button();
            btnSell = new Button();
            lblStatus = new Label();
            btnClose = new Button();
            pnlToolbar = new Panel();
            lblShow = new Label();
            cmbFilter = new ComboBox();
            lblHint = new Label();
            flowUnits = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlSide.SuspendLayout();
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
            lblTitle.Text = "Sell Units";
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
            pnlSide.Controls.Add(btnSell);
            pnlSide.Controls.Add(btnClearSelection);
            pnlSide.Controls.Add(btnSelectDuplicates);
            pnlSide.Controls.Add(lblTeamWarning);
            pnlSide.Controls.Add(lblTotal);
            pnlSide.Controls.Add(lblSelected);
            pnlSide.Dock = DockStyle.Right;
            pnlSide.Location = new Point(740, 64);
            pnlSide.Name = "pnlSide";
            pnlSide.Size = new Size(300, 616);
            pnlSide.TabIndex = 2;
            // 
            // lblSelected
            // 
            lblSelected.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblSelected.ForeColor = Color.White;
            lblSelected.Location = new Point(20, 20);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(262, 26);
            lblSelected.TabIndex = 3;
            lblSelected.Text = "Nothing selected";
            lblSelected.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotal
            // 
            lblTotal.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotal.ForeColor = Color.Gold;
            lblTotal.Location = new Point(20, 50);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(262, 34);
            lblTotal.TabIndex = 4;
            lblTotal.Text = "You get: 0 coins";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTeamWarning
            // 
            lblTeamWarning.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            lblTeamWarning.ForeColor = Color.OrangeRed;
            lblTeamWarning.Location = new Point(20, 90);
            lblTeamWarning.Name = "lblTeamWarning";
            lblTeamWarning.Size = new Size(262, 56);
            lblTeamWarning.TabIndex = 5;
            lblTeamWarning.Text = "";
            lblTeamWarning.TextAlign = ContentAlignment.TopLeft;
            // 
            // btnSelectDuplicates
            // 
            btnSelectDuplicates.BackColor = Color.FromArgb(70, 110, 200);
            btnSelectDuplicates.Cursor = Cursors.Hand;
            btnSelectDuplicates.FlatAppearance.BorderSize = 0;
            btnSelectDuplicates.FlatAppearance.MouseOverBackColor = Color.FromArgb(92, 134, 226);
            btnSelectDuplicates.FlatStyle = FlatStyle.Flat;
            btnSelectDuplicates.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnSelectDuplicates.ForeColor = Color.White;
            btnSelectDuplicates.Location = new Point(20, 156);
            btnSelectDuplicates.Name = "btnSelectDuplicates";
            btnSelectDuplicates.Size = new Size(262, 36);
            btnSelectDuplicates.TabIndex = 6;
            btnSelectDuplicates.Text = "Select duplicates";
            btnSelectDuplicates.UseVisualStyleBackColor = false;
            btnSelectDuplicates.Click += new EventHandler(btnSelectDuplicates_Click);
            // 
            // btnClearSelection
            // 
            btnClearSelection.BackColor = Color.FromArgb(40, 48, 70);
            btnClearSelection.Cursor = Cursors.Hand;
            btnClearSelection.FlatAppearance.BorderSize = 0;
            btnClearSelection.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnClearSelection.FlatStyle = FlatStyle.Flat;
            btnClearSelection.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnClearSelection.ForeColor = Color.White;
            btnClearSelection.Location = new Point(20, 200);
            btnClearSelection.Name = "btnClearSelection";
            btnClearSelection.Size = new Size(262, 36);
            btnClearSelection.TabIndex = 7;
            btnClearSelection.Text = "Clear selection";
            btnClearSelection.UseVisualStyleBackColor = false;
            btnClearSelection.Click += new EventHandler(btnClearSelection_Click);
            // 
            // btnSell
            // 
            btnSell.BackColor = Color.FromArgb(190, 120, 30);
            btnSell.Cursor = Cursors.Hand;
            btnSell.Enabled = false;
            btnSell.FlatAppearance.BorderSize = 0;
            btnSell.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 146, 46);
            btnSell.FlatStyle = FlatStyle.Flat;
            btnSell.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnSell.ForeColor = Color.White;
            btnSell.Location = new Point(20, 252);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(262, 46);
            btnSell.TabIndex = 8;
            btnSell.Text = "SELL SELECTED";
            btnSell.UseVisualStyleBackColor = false;
            btnSell.Click += new EventHandler(btnSell_Click);
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.Gainsboro;
            lblStatus.Location = new Point(20, 312);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(262, 80);
            lblStatus.TabIndex = 9;
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
            btnClose.Size = new Size(262, 38);
            btnClose.TabIndex = 10;
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
            pnlToolbar.TabIndex = 11;
            // 
            // lblShow
            // 
            lblShow.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblShow.ForeColor = Color.Silver;
            lblShow.Location = new Point(20, 12);
            lblShow.Name = "lblShow";
            lblShow.Size = new Size(45, 19);
            lblShow.TabIndex = 12;
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
            cmbFilter.TabIndex = 13;
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
            lblHint.TabIndex = 14;
            lblHint.Text = "Click units to pick them for selling.";
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
            flowUnits.TabIndex = 15;
            // 
            // Form8
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
            MinimumSize = new Size(900, 600);
            Name = "Form8";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transmogify - Sell Units";
            pnlToolbar.ResumeLayout(false);
            pnlSide.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblCoins;
        private Panel pnlSide;
        private Label lblSelected;
        private Label lblTotal;
        private Label lblTeamWarning;
        private Button btnSelectDuplicates;
        private Button btnClearSelection;
        private Button btnSell;
        private Label lblStatus;
        private Button btnClose;
        private Panel pnlToolbar;
        private Label lblShow;
        private ComboBox cmbFilter;
        private Label lblHint;
        private FlowLayoutPanel flowUnits;
    }
}