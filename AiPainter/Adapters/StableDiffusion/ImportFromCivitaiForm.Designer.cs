namespace AiPainter.Adapters.StableDiffusion
{
    partial class ImportFromCivitaiForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            linkLabel1 = new LinkLabel();
            tbCivitaiApiKey = new TextBox();
            label1 = new Label();
            label2 = new Label();
            tbUrl = new TextBox();
            btImport = new Button();
            tabs = new TabControl();
            tabCheckpoint = new TabPage();
            btCheckpointOk = new Button();
            numCheckpointClipSkip = new NumericUpDown();
            label8 = new Label();
            label10 = new Label();
            label7 = new Label();
            label6 = new Label();
            labCheckpointNameError = new Label();
            label5 = new Label();
            label9 = new Label();
            tbCheckpointVaeUrl = new TextBox();
            label4 = new Label();
            tbCheckpointInpaintUrl = new TextBox();
            label3 = new Label();
            tbCheckpointMainUrl = new TextBox();
            tbCheckpointDescription = new TextBox();
            tbCheckpointPrompt = new TextBox();
            tbCheckpointName = new TextBox();
            tabLora = new TabPage();
            toolTip1 = new ToolTip(components);
            tabs.SuspendLayout();
            tabCheckpoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCheckpointClipSkip).BeginInit();
            SuspendLayout();
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(404, 21);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(91, 15);
            linkLabel1.TabIndex = 15;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "visit Civitai.com";
            linkLabel1.UseMnemonic = false;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // tbCivitaiApiKey
            // 
            tbCivitaiApiKey.Location = new Point(186, 18);
            tbCivitaiApiKey.Name = "tbCivitaiApiKey";
            tbCivitaiApiKey.PlaceholderText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            tbCivitaiApiKey.Size = new Size(212, 23);
            tbCivitaiApiKey.TabIndex = 14;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(168, 15);
            label1.TabIndex = 13;
            label1.Text = "Specify API key for Civitai.com";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 68);
            label2.Name = "label2";
            label2.Size = new Size(229, 15);
            label2.TabIndex = 13;
            label2.Text = "Specify URL to import checkpoint or LoRA";
            // 
            // tbUrl
            // 
            tbUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbUrl.Location = new Point(247, 65);
            tbUrl.Name = "tbUrl";
            tbUrl.PlaceholderText = "https://civitai.com/models/352581/vixons-pony-styles";
            tbUrl.Size = new Size(460, 23);
            tbUrl.TabIndex = 14;
            // 
            // btImport
            // 
            btImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btImport.Location = new Point(713, 65);
            btImport.Name = "btImport";
            btImport.Size = new Size(75, 23);
            btImport.TabIndex = 16;
            btImport.Text = "Import";
            btImport.UseVisualStyleBackColor = true;
            btImport.Click += btImport_Click;
            // 
            // tabs
            // 
            tabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabs.Controls.Add(tabCheckpoint);
            tabs.Controls.Add(tabLora);
            tabs.Location = new Point(12, 109);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(776, 349);
            tabs.TabIndex = 17;
            // 
            // tabCheckpoint
            // 
            tabCheckpoint.Controls.Add(btCheckpointOk);
            tabCheckpoint.Controls.Add(numCheckpointClipSkip);
            tabCheckpoint.Controls.Add(label8);
            tabCheckpoint.Controls.Add(label10);
            tabCheckpoint.Controls.Add(label7);
            tabCheckpoint.Controls.Add(label6);
            tabCheckpoint.Controls.Add(labCheckpointNameError);
            tabCheckpoint.Controls.Add(label5);
            tabCheckpoint.Controls.Add(label9);
            tabCheckpoint.Controls.Add(tbCheckpointVaeUrl);
            tabCheckpoint.Controls.Add(label4);
            tabCheckpoint.Controls.Add(tbCheckpointInpaintUrl);
            tabCheckpoint.Controls.Add(label3);
            tabCheckpoint.Controls.Add(tbCheckpointMainUrl);
            tabCheckpoint.Controls.Add(tbCheckpointDescription);
            tabCheckpoint.Controls.Add(tbCheckpointPrompt);
            tabCheckpoint.Controls.Add(tbCheckpointName);
            tabCheckpoint.Location = new Point(4, 24);
            tabCheckpoint.Name = "tabCheckpoint";
            tabCheckpoint.Padding = new Padding(3);
            tabCheckpoint.Size = new Size(768, 321);
            tabCheckpoint.TabIndex = 0;
            tabCheckpoint.Text = "Checkpoint";
            tabCheckpoint.UseVisualStyleBackColor = true;
            // 
            // btCheckpointOk
            // 
            btCheckpointOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btCheckpointOk.Location = new Point(300, 264);
            btCheckpointOk.Name = "btCheckpointOk";
            btCheckpointOk.Size = new Size(179, 35);
            btCheckpointOk.TabIndex = 16;
            btCheckpointOk.Text = "Add Checkpoint";
            btCheckpointOk.UseVisualStyleBackColor = true;
            btCheckpointOk.Click += btCheckpointOk_Click;
            // 
            // numCheckpointClipSkip
            // 
            numCheckpointClipSkip.Location = new Point(652, 169);
            numCheckpointClipSkip.Name = "numCheckpointClipSkip";
            numCheckpointClipSkip.Size = new Size(55, 23);
            numCheckpointClipSkip.TabIndex = 15;
            numCheckpointClipSkip.TextAlign = HorizontalAlignment.Center;
            toolTip1.SetToolTip(numCheckpointClipSkip, "Specify only if need, set to 0 otherwise.");
            // 
            // label8
            // 
            label8.Location = new Point(576, 169);
            label8.Name = "label8";
            label8.Size = new Size(73, 18);
            label8.TabIndex = 13;
            label8.Text = "CLIP SKIP";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.Location = new Point(6, 216);
            label10.Name = "label10";
            label10.Size = new Size(158, 15);
            label10.TabIndex = 13;
            label10.Text = "Description";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.Location = new Point(6, 171);
            label7.Name = "label7";
            label7.Size = new Size(158, 15);
            label7.TabIndex = 13;
            label7.Text = "Required Prompt";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.Location = new Point(6, 129);
            label6.Name = "label6";
            label6.Size = new Size(158, 15);
            label6.TabIndex = 13;
            label6.Text = "VAE URL";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labCheckpointNameError
            // 
            labCheckpointNameError.AutoSize = true;
            labCheckpointNameError.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labCheckpointNameError.ForeColor = Color.IndianRed;
            labCheckpointNameError.Location = new Point(523, 27);
            labCheckpointNameError.Name = "labCheckpointNameError";
            labCheckpointNameError.Size = new Size(36, 15);
            labCheckpointNameError.TabIndex = 13;
            labCheckpointNameError.Text = "error";
            // 
            // label5
            // 
            label5.Location = new Point(6, 98);
            label5.Name = "label5";
            label5.Size = new Size(158, 15);
            label5.TabIndex = 13;
            label5.Text = "Inpaint Checkpoint URL";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(231, 213);
            label9.Name = "label9";
            label9.Size = new Size(0, 15);
            label9.TabIndex = 13;
            // 
            // tbCheckpointVaeUrl
            // 
            tbCheckpointVaeUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointVaeUrl.Location = new Point(170, 126);
            tbCheckpointVaeUrl.Name = "tbCheckpointVaeUrl";
            tbCheckpointVaeUrl.Size = new Size(537, 23);
            tbCheckpointVaeUrl.TabIndex = 14;
            // 
            // label4
            // 
            label4.Location = new Point(6, 68);
            label4.Name = "label4";
            label4.Size = new Size(158, 15);
            label4.TabIndex = 13;
            label4.Text = "Main Checkpoint URL";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbCheckpointInpaintUrl
            // 
            tbCheckpointInpaintUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointInpaintUrl.Location = new Point(170, 95);
            tbCheckpointInpaintUrl.Name = "tbCheckpointInpaintUrl";
            tbCheckpointInpaintUrl.Size = new Size(537, 23);
            tbCheckpointInpaintUrl.TabIndex = 14;
            // 
            // label3
            // 
            label3.Location = new Point(104, 27);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 13;
            label3.Text = "Name";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbCheckpointMainUrl
            // 
            tbCheckpointMainUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointMainUrl.Location = new Point(170, 65);
            tbCheckpointMainUrl.Name = "tbCheckpointMainUrl";
            tbCheckpointMainUrl.Size = new Size(537, 23);
            tbCheckpointMainUrl.TabIndex = 14;
            // 
            // tbCheckpointDescription
            // 
            tbCheckpointDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointDescription.Location = new Point(170, 213);
            tbCheckpointDescription.Name = "tbCheckpointDescription";
            tbCheckpointDescription.Size = new Size(537, 23);
            tbCheckpointDescription.TabIndex = 14;
            // 
            // tbCheckpointPrompt
            // 
            tbCheckpointPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointPrompt.Location = new Point(170, 168);
            tbCheckpointPrompt.Name = "tbCheckpointPrompt";
            tbCheckpointPrompt.Size = new Size(397, 23);
            tbCheckpointPrompt.TabIndex = 14;
            // 
            // tbCheckpointName
            // 
            tbCheckpointName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointName.Location = new Point(170, 24);
            tbCheckpointName.Name = "tbCheckpointName";
            tbCheckpointName.Size = new Size(347, 23);
            tbCheckpointName.TabIndex = 14;
            tbCheckpointName.TextChanged += tbCheckpointName_TextChanged;
            // 
            // tabLora
            // 
            tabLora.Location = new Point(4, 24);
            tabLora.Name = "tabLora";
            tabLora.Padding = new Padding(3);
            tabLora.Size = new Size(768, 321);
            tabLora.TabIndex = 1;
            tabLora.Text = "LoRA";
            tabLora.UseVisualStyleBackColor = true;
            // 
            // ImportFromCivitaiForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 470);
            Controls.Add(tabs);
            Controls.Add(btImport);
            Controls.Add(linkLabel1);
            Controls.Add(tbUrl);
            Controls.Add(tbCivitaiApiKey);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ImportFromCivitaiForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import Checkpoint or LoRA from civitai.com";
            Load += ImportFromCivitaiForm_Load;
            tabs.ResumeLayout(false);
            tabCheckpoint.ResumeLayout(false);
            tabCheckpoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCheckpointClipSkip).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private LinkLabel linkLabel1;
        private TextBox tbCivitaiApiKey;
        private Label label1;
        private Label label2;
        private TextBox tbUrl;
        private Button btImport;
        private TabControl tabs;
        private TabPage tabCheckpoint;
        private Label label7;
        private Label label6;
        private Label label5;
        private TextBox tbCheckpointVaeUrl;
        private Label label4;
        private TextBox tbCheckpointInpaintUrl;
        private Label label3;
        private TextBox tbCheckpointMainUrl;
        private TextBox tbCheckpointPrompt;
        private TextBox tbCheckpointName;
        private TabPage tabLora;
        private Label label8;
        private Button btCheckpointOk;
        private NumericUpDown numCheckpointClipSkip;
        private Label label9;
        private ToolTip toolTip1;
        private Label label10;
        private TextBox tbCheckpointDescription;
        private Label labCheckpointNameError;
    }
}