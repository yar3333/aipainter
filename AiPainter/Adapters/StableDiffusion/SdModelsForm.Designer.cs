namespace AiPainter.Adapters.StableDiffusion
{
    partial class SdModelsForm
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
            btOk = new Button();
            lvModels = new ListView();
            linkLabel1 = new LinkLabel();
            tbCivitaiApiKey = new TextBox();
            label1 = new Label();
            btImportFromCivitai = new Button();
            label2 = new Label();
            tbSearch = new TextBox();
            SuspendLayout();
            // 
            // btOk
            // 
            btOk.Anchor = AnchorStyles.Bottom;
            btOk.DialogResult = DialogResult.OK;
            btOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btOk.Location = new Point(453, 529);
            btOk.Name = "btOk";
            btOk.Size = new Size(172, 40);
            btOk.TabIndex = 5;
            btOk.Text = "OK";
            btOk.UseVisualStyleBackColor = true;
            // 
            // lvModels
            // 
            lvModels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvModels.CheckBoxes = true;
            lvModels.FullRowSelect = true;
            lvModels.GridLines = true;
            lvModels.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvModels.Location = new Point(12, 91);
            lvModels.MultiSelect = false;
            lvModels.Name = "lvModels";
            lvModels.Size = new Size(1050, 426);
            lvModels.TabIndex = 6;
            lvModels.UseCompatibleStateImageBehavior = false;
            lvModels.View = View.Details;
            lvModels.ItemChecked += lvModels_ItemChecked;
            lvModels.MouseClick += lvModels_MouseClick;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(554, 17);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(91, 15);
            linkLabel1.TabIndex = 12;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "visit Civitai.com";
            linkLabel1.UseMnemonic = false;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // tbCivitaiApiKey
            // 
            tbCivitaiApiKey.Location = new Point(336, 12);
            tbCivitaiApiKey.Name = "tbCivitaiApiKey";
            tbCivitaiApiKey.PlaceholderText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            tbCivitaiApiKey.Size = new Size(212, 23);
            tbCivitaiApiKey.TabIndex = 11;
            tbCivitaiApiKey.TextChanged += tbCivitaiApiKey_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(318, 15);
            label1.TabIndex = 10;
            label1.Text = "Downloading some models require API key for Civitai.com:";
            // 
            // btImportFromCivitai
            // 
            btImportFromCivitai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btImportFromCivitai.Location = new Point(968, 9);
            btImportFromCivitai.Name = "btImportFromCivitai";
            btImportFromCivitai.Size = new Size(94, 30);
            btImportFromCivitai.TabIndex = 13;
            btImportFromCivitai.Text = "Import...";
            btImportFromCivitai.UseVisualStyleBackColor = true;
            btImportFromCivitai.Click += btImportFromCivitai_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 10;
            label2.Text = "Search:";
            // 
            // tbSearch
            // 
            tbSearch.Location = new Point(63, 52);
            tbSearch.Name = "tbSearch";
            tbSearch.PlaceholderText = "words to filter list";
            tbSearch.Size = new Size(999, 23);
            tbSearch.TabIndex = 11;
            tbSearch.TextChanged += tbSearch_TextChanged;
            // 
            // SdModelsForm
            // 
            AcceptButton = btOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 581);
            Controls.Add(btImportFromCivitai);
            Controls.Add(linkLabel1);
            Controls.Add(tbSearch);
            Controls.Add(tbCivitaiApiKey);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lvModels);
            Controls.Add(btOk);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "SdModelsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Stable Diffusion Checkpoints (image generation models)";
            FormClosed += SdModelsForm_FormClosed;
            Load += SdModelsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btOk;
        private ListView lvModels;
        private LinkLabel linkLabel1;
        private TextBox tbCivitaiApiKey;
        private Label label1;
        private Button btImportFromCivitai;
        private System.Windows.Forms.Timer updateTimer;
        private Label label2;
        private TextBox tbSearch;
    }
}