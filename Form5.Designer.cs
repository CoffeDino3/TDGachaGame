namespace TDGachaGame
{
    partial class Form5
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
            lblPrompt = new Label();
            txtCode = new TextBox();
            lblResult = new Label();
            btnRedeem = new Button();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblPrompt.ForeColor = Color.White;
            lblPrompt.Location = new Point(20, 16);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(320, 24);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "Enter a code for free resources:";
            // 
            // txtCode
            // 
            txtCode.BackColor = Color.FromArgb(24, 28, 40);
            txtCode.BorderStyle = BorderStyle.FixedSingle;
            txtCode.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtCode.ForeColor = Color.White;
            txtCode.Location = new Point(20, 46);
            txtCode.MaxLength = 32;
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(320, 29);
            txtCode.TabIndex = 1;
            // 
            // lblResult
            // 
            lblResult.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblResult.ForeColor = Color.Silver;
            lblResult.Location = new Point(20, 82);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(320, 22);
            lblResult.TabIndex = 2;
            // 
            // btnRedeem
            // 
            btnRedeem.BackColor = Color.FromArgb(46, 160, 90);
            btnRedeem.Cursor = Cursors.Hand;
            btnRedeem.FlatAppearance.BorderSize = 0;
            btnRedeem.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 190, 110);
            btnRedeem.FlatStyle = FlatStyle.Flat;
            btnRedeem.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnRedeem.ForeColor = Color.White;
            btnRedeem.Location = new Point(20, 116);
            btnRedeem.Name = "btnRedeem";
            btnRedeem.Size = new Size(150, 36);
            btnRedeem.TabIndex = 3;
            btnRedeem.Text = "REDEEM";
            btnRedeem.UseVisualStyleBackColor = false;
            btnRedeem.Click += new EventHandler(btnRedeem_Click);
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(40, 48, 70);
            btnClose.Cursor = Cursors.Hand;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 70, 100);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(190, 116);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(150, 36);
            btnClose.TabIndex = 4;
            btnClose.Text = "CLOSE";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // Form5
            // 
            AcceptButton = btnRedeem;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 14, 22);
            CancelButton = btnClose;
            ClientSize = new Size(360, 170);
            Controls.Add(lblPrompt);
            Controls.Add(txtCode);
            Controls.Add(lblResult);
            Controls.Add(btnRedeem);
            Controls.Add(btnClose);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form5";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transmogify - Enter Code";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPrompt;
        private TextBox txtCode;
        private Label lblResult;
        private Button btnRedeem;
        private Button btnClose;
    }
}