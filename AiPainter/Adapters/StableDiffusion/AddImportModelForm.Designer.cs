namespace AiPainter.Adapters.StableDiffusion
{
    partial class AddImportModelForm
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
            label2 = new Label();
            tbUrl = new TextBox();
            btImport = new Button();
            tabs = new TabControl();
            tabCheckpoint = new TabPage();
            label16 = new Label();
            tbCheckpointSuggestedPrompt = new TextBox();
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
            tbCheckpointRequiredPrompt = new TextBox();
            tbCheckpointName = new TextBox();
            tabLora = new TabPage();
            label11 = new Label();
            tbLoraSuggestedPrompt = new TextBox();
            btLoraOk = new Button();
            label1 = new Label();
            label12 = new Label();
            labLoraNameError = new Label();
            label14 = new Label();
            label15 = new Label();
            tbLoraDownloadUrl = new TextBox();
            tbLoraDescription = new TextBox();
            tbLoraRequiredPrompt = new TextBox();
            tbLoraName = new TextBox();
            tabEmbedding = new TabPage();
            cbEmbeddingIsNegative = new CheckBox();
            btEmbeddingOk = new Button();
            label17 = new Label();
            labEmbeddingNameError = new Label();
            label19 = new Label();
            label20 = new Label();
            tbEmbeddingDownloadUrl = new TextBox();
            tbEmbeddingDescription = new TextBox();
            tbEmbeddingName = new TextBox();
            toolTip1 = new ToolTip(components);
            labUrlError = new Label();
            linkLabel1 = new LinkLabel();
            tabs.SuspendLayout();
            tabCheckpoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCheckpointClipSkip).BeginInit();
            tabLora.SuspendLayout();
            tabEmbedding.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 20);
            label2.Name = "label2";
            label2.Size = new Size(81, 15);
            label2.TabIndex = 13;
            label2.Text = "URL to import";
            toolTip1.SetToolTip(label2, "URL to import checkpoint or LoRA");
            // 
            // tbUrl
            // 
            tbUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbUrl.BackColor = Color.FromArgb(226, 241, 227);
            tbUrl.Location = new Point(99, 17);
            tbUrl.Name = "tbUrl";
            tbUrl.PlaceholderText = "https://civitai.com/models/352581/vixons-pony-styles";
            tbUrl.Size = new Size(460, 23);
            tbUrl.TabIndex = 14;
            // 
            // btImport
            // 
            btImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btImport.Location = new Point(565, 16);
            btImport.Name = "btImport";
            btImport.Size = new Size(75, 26);
            btImport.TabIndex = 16;
            btImport.Text = "Import";
            btImport.UseVisualStyleBackColor = true;
            btImport.Click += btImport_Click;
            // 
            // tabs
            // 
            tabs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tabs.Controls.Add(tabCheckpoint);
            tabs.Controls.Add(tabLora);
            tabs.Controls.Add(tabEmbedding);
            tabs.Location = new Point(12, 68);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(776, 380);
            tabs.TabIndex = 17;
            // 
            // tabCheckpoint
            // 
            tabCheckpoint.Controls.Add(label16);
            tabCheckpoint.Controls.Add(tbCheckpointSuggestedPrompt);
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
            tabCheckpoint.Controls.Add(tbCheckpointRequiredPrompt);
            tabCheckpoint.Controls.Add(tbCheckpointName);
            tabCheckpoint.Location = new Point(4, 24);
            tabCheckpoint.Name = "tabCheckpoint";
            tabCheckpoint.Padding = new Padding(3);
            tabCheckpoint.Size = new Size(768, 352);
            tabCheckpoint.TabIndex = 0;
            tabCheckpoint.Text = "Checkpoint (main model)";
            tabCheckpoint.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            label16.Location = new Point(6, 244);
            label16.Name = "label16";
            label16.Size = new Size(158, 15);
            label16.TabIndex = 17;
            label16.Text = "Suggested Prompt";
            label16.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbCheckpointSuggestedPrompt
            // 
            tbCheckpointSuggestedPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointSuggestedPrompt.Location = new Point(170, 241);
            tbCheckpointSuggestedPrompt.Name = "tbCheckpointSuggestedPrompt";
            tbCheckpointSuggestedPrompt.PlaceholderText = "first phrase, second phase...";
            tbCheckpointSuggestedPrompt.Size = new Size(537, 23);
            tbCheckpointSuggestedPrompt.TabIndex = 18;
            // 
            // btCheckpointOk
            // 
            btCheckpointOk.BackColor = Color.FromArgb(226, 241, 227);
            btCheckpointOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btCheckpointOk.Location = new Point(300, 287);
            btCheckpointOk.Name = "btCheckpointOk";
            btCheckpointOk.Size = new Size(179, 48);
            btCheckpointOk.TabIndex = 16;
            btCheckpointOk.Text = "Add Checkpoint";
            btCheckpointOk.UseVisualStyleBackColor = false;
            btCheckpointOk.Click += btCheckpointOk_Click;
            // 
            // numCheckpointClipSkip
            // 
            numCheckpointClipSkip.Location = new Point(652, 207);
            numCheckpointClipSkip.Name = "numCheckpointClipSkip";
            numCheckpointClipSkip.Size = new Size(55, 23);
            numCheckpointClipSkip.TabIndex = 15;
            numCheckpointClipSkip.TextAlign = HorizontalAlignment.Center;
            toolTip1.SetToolTip(numCheckpointClipSkip, "Specify only if need, set to 0 otherwise.");
            // 
            // label8
            // 
            label8.Location = new Point(576, 207);
            label8.Name = "label8";
            label8.Size = new Size(73, 18);
            label8.TabIndex = 13;
            label8.Text = "CLIP SKIP";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.Location = new Point(6, 171);
            label10.Name = "label10";
            label10.Size = new Size(158, 15);
            label10.TabIndex = 13;
            label10.Text = "Description";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.Location = new Point(6, 215);
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
            labCheckpointNameError.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labCheckpointNameError.ForeColor = Color.IndianRed;
            labCheckpointNameError.Location = new Point(523, 27);
            labCheckpointNameError.Name = "labCheckpointNameError";
            labCheckpointNameError.Size = new Size(239, 15);
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
            tbCheckpointDescription.Location = new Point(170, 168);
            tbCheckpointDescription.Name = "tbCheckpointDescription";
            tbCheckpointDescription.Size = new Size(537, 23);
            tbCheckpointDescription.TabIndex = 14;
            // 
            // tbCheckpointRequiredPrompt
            // 
            tbCheckpointRequiredPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbCheckpointRequiredPrompt.Location = new Point(170, 212);
            tbCheckpointRequiredPrompt.Name = "tbCheckpointRequiredPrompt";
            tbCheckpointRequiredPrompt.Size = new Size(397, 23);
            tbCheckpointRequiredPrompt.TabIndex = 14;
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
            tabLora.Controls.Add(label11);
            tabLora.Controls.Add(tbLoraSuggestedPrompt);
            tabLora.Controls.Add(btLoraOk);
            tabLora.Controls.Add(label1);
            tabLora.Controls.Add(label12);
            tabLora.Controls.Add(labLoraNameError);
            tabLora.Controls.Add(label14);
            tabLora.Controls.Add(label15);
            tabLora.Controls.Add(tbLoraDownloadUrl);
            tabLora.Controls.Add(tbLoraDescription);
            tabLora.Controls.Add(tbLoraRequiredPrompt);
            tabLora.Controls.Add(tbLoraName);
            tabLora.Location = new Point(4, 24);
            tabLora.Name = "tabLora";
            tabLora.Padding = new Padding(3);
            tabLora.Size = new Size(768, 352);
            tabLora.TabIndex = 1;
            tabLora.Text = "LoRA (additional model)";
            tabLora.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            label11.Location = new Point(6, 217);
            label11.Name = "label11";
            label11.Size = new Size(158, 15);
            label11.TabIndex = 25;
            label11.Text = "Suggested Prompt";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbLoraSuggestedPrompt
            // 
            tbLoraSuggestedPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbLoraSuggestedPrompt.Location = new Point(170, 214);
            tbLoraSuggestedPrompt.Name = "tbLoraSuggestedPrompt";
            tbLoraSuggestedPrompt.PlaceholderText = "first phrase, second phase...";
            tbLoraSuggestedPrompt.Size = new Size(537, 23);
            tbLoraSuggestedPrompt.TabIndex = 26;
            // 
            // btLoraOk
            // 
            btLoraOk.BackColor = Color.FromArgb(226, 241, 227);
            btLoraOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btLoraOk.Location = new Point(295, 272);
            btLoraOk.Name = "btLoraOk";
            btLoraOk.Size = new Size(179, 48);
            btLoraOk.TabIndex = 24;
            btLoraOk.Text = "Add LoRA";
            btLoraOk.UseVisualStyleBackColor = false;
            btLoraOk.Click += btLoraOk_Click;
            // 
            // label1
            // 
            label1.Location = new Point(6, 142);
            label1.Name = "label1";
            label1.Size = new Size(158, 15);
            label1.TabIndex = 15;
            label1.Text = "Description";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.Location = new Point(6, 188);
            label12.Name = "label12";
            label12.Size = new Size(158, 15);
            label12.TabIndex = 16;
            label12.Text = "Required Prompt";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labLoraNameError
            // 
            labLoraNameError.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labLoraNameError.ForeColor = Color.IndianRed;
            labLoraNameError.Location = new Point(523, 59);
            labLoraNameError.Name = "labLoraNameError";
            labLoraNameError.Size = new Size(239, 15);
            labLoraNameError.TabIndex = 17;
            labLoraNameError.Text = "error";
            // 
            // label14
            // 
            label14.Location = new Point(6, 100);
            label14.Name = "label14";
            label14.Size = new Size(158, 15);
            label14.TabIndex = 18;
            label14.Text = "Download URL";
            label14.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            label15.Location = new Point(104, 59);
            label15.Name = "label15";
            label15.Size = new Size(60, 15);
            label15.TabIndex = 19;
            label15.Text = "Name";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbLoraDownloadUrl
            // 
            tbLoraDownloadUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbLoraDownloadUrl.Location = new Point(170, 97);
            tbLoraDownloadUrl.Name = "tbLoraDownloadUrl";
            tbLoraDownloadUrl.Size = new Size(537, 23);
            tbLoraDownloadUrl.TabIndex = 20;
            // 
            // tbLoraDescription
            // 
            tbLoraDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbLoraDescription.Location = new Point(170, 139);
            tbLoraDescription.Name = "tbLoraDescription";
            tbLoraDescription.Size = new Size(537, 23);
            tbLoraDescription.TabIndex = 21;
            // 
            // tbLoraRequiredPrompt
            // 
            tbLoraRequiredPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbLoraRequiredPrompt.Location = new Point(170, 185);
            tbLoraRequiredPrompt.Name = "tbLoraRequiredPrompt";
            tbLoraRequiredPrompt.Size = new Size(537, 23);
            tbLoraRequiredPrompt.TabIndex = 22;
            // 
            // tbLoraName
            // 
            tbLoraName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbLoraName.Location = new Point(170, 56);
            tbLoraName.Name = "tbLoraName";
            tbLoraName.Size = new Size(347, 23);
            tbLoraName.TabIndex = 23;
            tbLoraName.TextChanged += tbLoraName_TextChanged;
            // 
            // tabEmbedding
            // 
            tabEmbedding.Controls.Add(cbEmbeddingIsNegative);
            tabEmbedding.Controls.Add(btEmbeddingOk);
            tabEmbedding.Controls.Add(label17);
            tabEmbedding.Controls.Add(labEmbeddingNameError);
            tabEmbedding.Controls.Add(label19);
            tabEmbedding.Controls.Add(label20);
            tabEmbedding.Controls.Add(tbEmbeddingDownloadUrl);
            tabEmbedding.Controls.Add(tbEmbeddingDescription);
            tabEmbedding.Controls.Add(tbEmbeddingName);
            tabEmbedding.Location = new Point(4, 24);
            tabEmbedding.Name = "tabEmbedding";
            tabEmbedding.Padding = new Padding(3);
            tabEmbedding.Size = new Size(768, 352);
            tabEmbedding.TabIndex = 2;
            tabEmbedding.Text = "Embedding (textual inversion)";
            tabEmbedding.UseVisualStyleBackColor = true;
            // 
            // cbEmbeddingIsNegative
            // 
            cbEmbeddingIsNegative.AutoSize = true;
            cbEmbeddingIsNegative.Location = new Point(170, 197);
            cbEmbeddingIsNegative.Name = "cbEmbeddingIsNegative";
            cbEmbeddingIsNegative.Size = new Size(195, 19);
            cbEmbeddingIsNegative.TabIndex = 32;
            cbEmbeddingIsNegative.Text = "must be used in negtive prompt";
            cbEmbeddingIsNegative.UseVisualStyleBackColor = true;
            // 
            // btEmbeddingOk
            // 
            btEmbeddingOk.BackColor = Color.FromArgb(226, 241, 227);
            btEmbeddingOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btEmbeddingOk.Location = new Point(290, 245);
            btEmbeddingOk.Name = "btEmbeddingOk";
            btEmbeddingOk.Size = new Size(179, 48);
            btEmbeddingOk.TabIndex = 31;
            btEmbeddingOk.Text = "Add Embedding";
            btEmbeddingOk.UseVisualStyleBackColor = false;
            btEmbeddingOk.Click += btEmbeddingOk_Click;
            // 
            // label17
            // 
            label17.Location = new Point(6, 155);
            label17.Name = "label17";
            label17.Size = new Size(158, 15);
            label17.TabIndex = 24;
            label17.Text = "Description";
            label17.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labEmbeddingNameError
            // 
            labEmbeddingNameError.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labEmbeddingNameError.ForeColor = Color.IndianRed;
            labEmbeddingNameError.Location = new Point(523, 72);
            labEmbeddingNameError.Name = "labEmbeddingNameError";
            labEmbeddingNameError.Size = new Size(239, 15);
            labEmbeddingNameError.TabIndex = 25;
            labEmbeddingNameError.Text = "error";
            // 
            // label19
            // 
            label19.Location = new Point(6, 113);
            label19.Name = "label19";
            label19.Size = new Size(158, 15);
            label19.TabIndex = 26;
            label19.Text = "Download URL";
            label19.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            label20.Location = new Point(104, 72);
            label20.Name = "label20";
            label20.Size = new Size(60, 15);
            label20.TabIndex = 27;
            label20.Text = "Name";
            label20.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbEmbeddingDownloadUrl
            // 
            tbEmbeddingDownloadUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbEmbeddingDownloadUrl.Location = new Point(170, 110);
            tbEmbeddingDownloadUrl.Name = "tbEmbeddingDownloadUrl";
            tbEmbeddingDownloadUrl.Size = new Size(537, 23);
            tbEmbeddingDownloadUrl.TabIndex = 28;
            // 
            // tbEmbeddingDescription
            // 
            tbEmbeddingDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbEmbeddingDescription.Location = new Point(170, 152);
            tbEmbeddingDescription.Name = "tbEmbeddingDescription";
            tbEmbeddingDescription.Size = new Size(537, 23);
            tbEmbeddingDescription.TabIndex = 29;
            // 
            // tbEmbeddingName
            // 
            tbEmbeddingName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbEmbeddingName.Location = new Point(170, 69);
            tbEmbeddingName.Name = "tbEmbeddingName";
            tbEmbeddingName.Size = new Size(347, 23);
            tbEmbeddingName.TabIndex = 30;
            tbEmbeddingName.TextChanged += tbEmbeddingName_TextChanged;
            // 
            // labUrlError
            // 
            labUrlError.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labUrlError.ForeColor = Color.IndianRed;
            labUrlError.Location = new Point(99, 43);
            labUrlError.Name = "labUrlError";
            labUrlError.Size = new Size(460, 15);
            labUrlError.TabIndex = 13;
            labUrlError.Text = "error";
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            linkLabel1.Location = new Point(646, 18);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(138, 23);
            linkLabel1.TabIndex = 18;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "civitai.com";
            linkLabel1.TextAlign = ContentAlignment.MiddleRight;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked_1;
            // 
            // AddImportModelForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 473);
            Controls.Add(linkLabel1);
            Controls.Add(tabs);
            Controls.Add(btImport);
            Controls.Add(tbUrl);
            Controls.Add(label2);
            Controls.Add(labUrlError);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "AddImportModelForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add/Import Checkpoint, LoRA or Embedding model";
            Load += ImportFromCivitaiForm_Load;
            tabs.ResumeLayout(false);
            tabCheckpoint.ResumeLayout(false);
            tabCheckpoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCheckpointClipSkip).EndInit();
            tabLora.ResumeLayout(false);
            tabLora.PerformLayout();
            tabEmbedding.ResumeLayout(false);
            tabEmbedding.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private TextBox tbUrl;
        private Button btImport;
        public TabControl tabs;
        public TabPage tabCheckpoint;
        private Label label7;
        private Label label6;
        private Label label5;
        private TextBox tbCheckpointVaeUrl;
        private Label label4;
        private TextBox tbCheckpointInpaintUrl;
        private Label label3;
        private TextBox tbCheckpointMainUrl;
        private TextBox tbCheckpointRequiredPrompt;
        private TextBox tbCheckpointName;
        public TabPage tabLora;
        private Label label8;
        private Button btCheckpointOk;
        private NumericUpDown numCheckpointClipSkip;
        private Label label9;
        private ToolTip toolTip1;
        private Label label10;
        private TextBox tbCheckpointDescription;
        private Label labCheckpointNameError;
        private Label label11;
        private Label labUrlError;
        private LinkLabel linkLabel1;
        private Label label1;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private TextBox tbLoraDownloadUrl;
        private TextBox tbLoraDescription;
        private TextBox tbLoraRequiredPrompt;
        private TextBox tbLoraName;
        private Button btLoraOk;
        private Label labLoraNameError;
        private Label label16;
        private TextBox tbCheckpointSuggestedPrompt;
        private TextBox tbLoraSuggestedPrompt;
        public TabPage tabEmbedding;
        private Button btEmbeddingOk;
        private Label label17;
        private Label labEmbeddingNameError;
        private Label label19;
        private Label label20;
        private TextBox tbEmbeddingDownloadUrl;
        private TextBox tbEmbeddingDescription;
        private TextBox tbEmbeddingName;
        private CheckBox cbEmbeddingIsNegative;
    }
}