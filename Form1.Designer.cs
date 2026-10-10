namespace TDGachaGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DoubleBufferedPanel gamePanel;
        private System.Windows.Forms.Timer gameTimer;
        private Panel pnlResult;
        private Label lblResultTitle;
        private Label lblResultDetail;
        private Button btnReturnToMenu;

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
            gamePanel = new DoubleBufferedPanel();
            gameTimer = new System.Windows.Forms.Timer(components);
            pnlResult = new Panel();
            lblResultTitle = new Label();
            lblResultDetail = new Label();
            btnReturnToMenu = new Button();
            gamePanel.SuspendLayout();
            pnlResult.SuspendLayout();
            SuspendLayout();
            // 
            // gamePanel
            // 
            gamePanel.BackColor = Color.Black;
            gamePanel.Controls.Add(pnlResult);
            gamePanel.Dock = DockStyle.Fill;
            gamePanel.Location = new Point(0, 0);
            gamePanel.Name = "gamePanel";
            gamePanel.Size = new Size(900, 700);
            gamePanel.TabIndex = 0;
            gamePanel.TabStop = true;
            gamePanel.Paint += new PaintEventHandler(GamePanel_Paint);
            gamePanel.MouseClick += new MouseEventHandler(GamePanel_MouseClick);
            gamePanel.MouseDown += new MouseEventHandler(GamePanel_MouseDown);
            gamePanel.MouseEnter += new EventHandler(GamePanel_MouseEnter);
            gamePanel.MouseLeave += new EventHandler(GamePanel_MouseLeave);
            gamePanel.MouseMove += new MouseEventHandler(GamePanel_MouseMove);
            gamePanel.MouseUp += new MouseEventHandler(GamePanel_MouseUp);
            gamePanel.MouseWheel += new MouseEventHandler(GamePanel_MouseWheel);
            gamePanel.Resize += new EventHandler(GamePanel_Resize);
            // 
            // pnlResult
            // 
            pnlResult.BackColor = Color.FromArgb(30, 24, 24);
            pnlResult.BorderStyle = BorderStyle.FixedSingle;
            pnlResult.Controls.Add(btnReturnToMenu);
            pnlResult.Controls.Add(lblResultDetail);
            pnlResult.Controls.Add(lblResultTitle);
            pnlResult.Location = new Point(260, 250);
            pnlResult.Name = "pnlResult";
            pnlResult.Size = new Size(380, 200);
            pnlResult.TabIndex = 0;
            pnlResult.Visible = false;
            // 
            // lblResultTitle
            // 
            lblResultTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblResultTitle.ForeColor = Color.OrangeRed;
            lblResultTitle.Location = new Point(0, 12);
            lblResultTitle.Name = "lblResultTitle";
            lblResultTitle.Size = new Size(378, 56);
            lblResultTitle.TabIndex = 0;
            lblResultTitle.Text = "FLOOR LOST";
            lblResultTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblResultDetail
            // 
            lblResultDetail.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lblResultDetail.ForeColor = Color.White;
            lblResultDetail.Location = new Point(0, 72);
            lblResultDetail.Name = "lblResultDetail";
            lblResultDetail.Size = new Size(378, 60);
            lblResultDetail.TabIndex = 1;
            lblResultDetail.Text = "Result details";
            lblResultDetail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnReturnToMenu
            // 
            btnReturnToMenu.BackColor = Color.FromArgb(46, 160, 90);
            btnReturnToMenu.Cursor = Cursors.Hand;
            btnReturnToMenu.FlatAppearance.BorderSize = 0;
            btnReturnToMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 190, 110);
            btnReturnToMenu.FlatStyle = FlatStyle.Flat;
            btnReturnToMenu.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnReturnToMenu.ForeColor = Color.White;
            btnReturnToMenu.Location = new Point(100, 144);
            btnReturnToMenu.Name = "btnReturnToMenu";
            btnReturnToMenu.Size = new Size(178, 38);
            btnReturnToMenu.TabIndex = 2;
            btnReturnToMenu.Text = "Return to Menu";
            btnReturnToMenu.UseVisualStyleBackColor = false;
            btnReturnToMenu.Click += new EventHandler(btnReturnToMenu_Click);
            // 
            // gameTimer
            // 
            gameTimer.Enabled = true;
            gameTimer.Interval = 33;
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 700);
            Controls.Add(gamePanel);
            KeyPreview = true;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Transmogify";
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            pnlResult.ResumeLayout(false);
            gamePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}