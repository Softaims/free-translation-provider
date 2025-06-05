using System;
using System.Windows.Forms;

public partial class OpenAITranslationProviderConfDialog : Form
{
    public string ApiKey { get; set; }

    public OpenAITranslationProviderConfDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.textBoxApiKey = new System.Windows.Forms.TextBox();
        this.labelApiKey = new System.Windows.Forms.Label();
        this.buttonOK = new System.Windows.Forms.Button();
        this.buttonCancel = new System.Windows.Forms.Button();
        this.labelDescription = new System.Windows.Forms.Label();
        this.SuspendLayout();

        // 
        // labelDescription
        // 
        this.labelDescription.AutoSize = true;
        this.labelDescription.Location = new System.Drawing.Point(12, 9);
        this.labelDescription.Name = "labelDescription";
        this.labelDescription.Size = new System.Drawing.Size(300, 13);
        this.labelDescription.TabIndex = 0;
        this.labelDescription.Text = "Enter your OpenAI API key to use OpenAI translation services.";

        // 
        // labelApiKey
        // 
        this.labelApiKey.AutoSize = true;
        this.labelApiKey.Location = new System.Drawing.Point(12, 40);
        this.labelApiKey.Name = "labelApiKey";
        this.labelApiKey.Size = new System.Drawing.Size(73, 13);
        this.labelApiKey.TabIndex = 1;
        this.labelApiKey.Text = "OpenAI API Key:";

        // 
        // textBoxApiKey
        // 
        this.textBoxApiKey.Location = new System.Drawing.Point(15, 56);
        this.textBoxApiKey.Name = "textBoxApiKey";
        this.textBoxApiKey.Size = new System.Drawing.Size(350, 20);
        this.textBoxApiKey.TabIndex = 2;
        this.textBoxApiKey.UseSystemPasswordChar = true;

        // 
        // buttonOK
        // 
        this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.buttonOK.Location = new System.Drawing.Point(209, 90);
        this.buttonOK.Name = "buttonOK";
        this.buttonOK.Size = new System.Drawing.Size(75, 23);
        this.buttonOK.TabIndex = 3;
        this.buttonOK.Text = "OK";
        this.buttonOK.UseVisualStyleBackColor = true;
        this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);

        // 
        // buttonCancel
        // 
        this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.buttonCancel.Location = new System.Drawing.Point(290, 90);
        this.buttonCancel.Name = "buttonCancel";
        this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        this.buttonCancel.TabIndex = 4;
        this.buttonCancel.Text = "Cancel";
        this.buttonCancel.UseVisualStyleBackColor = true;

        // 
        // OpenAITranslationProviderConfDialog
        // 
        this.AcceptButton = this.buttonOK;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.buttonCancel;
        this.ClientSize = new System.Drawing.Size(384, 131);
        this.Controls.Add(this.buttonCancel);
        this.Controls.Add(this.buttonOK);
        this.Controls.Add(this.textBoxApiKey);
        this.Controls.Add(this.labelApiKey);
        this.Controls.Add(this.labelDescription);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "OpenAITranslationProviderConfDialog";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "OpenAI Translation Provider Configuration";
        this.Load += new System.EventHandler(this.OpenAITranslationProviderConfDialog_Load);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TextBox textBoxApiKey;
    private System.Windows.Forms.Label labelApiKey;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelDescription;

    private void OpenAITranslationProviderConfDialog_Load(object sender, EventArgs e)
    {
        textBoxApiKey.Text = ApiKey;
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBoxApiKey.Text))
        {
            MessageBox.Show("Please enter your OpenAI API key.", "API Key Required",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ApiKey = textBoxApiKey.Text.Trim();
        this.Close();
    }
}