using System;
using System.Drawing;
using System.Windows.Forms;

namespace FreeTranslateProvider
{
    public partial class FreeTranslateCredentialsDialog : Form
    {
        public string ApiKey { get; private set; }

        private TextBox txtApiKey;
        private Button btnOK;
        private Button btnCancel;
        private Label lblApiKey;
        private Label lblInfo;

        public FreeTranslateCredentialsDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Free Translate Provider Credentials";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Info label
            lblInfo = new Label
            {
                Text = "Enter your MyMemory API key to increase translation limits.\nLeave empty to use free tier (1000 words/day).",
                Location = new Point(20, 20),
                Size = new Size(350, 40),
                AutoSize = false
            };
            this.Controls.Add(lblInfo);

            // API Key
            lblApiKey = new Label
            {
                Text = "API Key:",
                Location = new Point(20, 80),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblApiKey);

            txtApiKey = new TextBox
            {
                Location = new Point(110, 80),
                Size = new Size(240, 23),
                UseSystemPasswordChar = true
            };
            this.Controls.Add(txtApiKey);

            // Buttons
            btnOK = new Button
            {
                Text = "OK",
                Location = new Point(195, 120),
                Size = new Size(75, 23),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(275, 120),
                Size = new Size(75, 23),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            ApiKey = txtApiKey.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}