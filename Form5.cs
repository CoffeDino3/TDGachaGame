using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace TDGachaGame
{
    public partial class Form5 : Form
    {
        private readonly PlayerProfile profile;

        public event EventHandler Redeemed;

        public Form5() : this(new PlayerProfile())
        {
        }

        public Form5(PlayerProfile profile)
        {
            this.profile = profile;
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtCode.Focus();
        }

        private void btnRedeem_Click(object sender, EventArgs e)
        {
            bool ok = RedeemCodes.TryRedeem(profile, txtCode.Text, out string message);

            lblResult.Text = message;
            lblResult.ForeColor = ok ? Color.LimeGreen : Color.OrangeRed;

            if (ok)
            {
                txtCode.Clear();
                Redeemed?.Invoke(this, EventArgs.Empty);
            }

            txtCode.Focus();
        }
    }
}