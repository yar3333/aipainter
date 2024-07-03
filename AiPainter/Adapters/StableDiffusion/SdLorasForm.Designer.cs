namespace AiPainter.Adapters.StableDiffusion
{
    partial class SdLorasForm
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
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            bwDownloading = new System.ComponentModel.BackgroundWorker();
            label1 = new Label();
            tbCivitaiApiKey = new TextBox();
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // btOk
            // 
            btOk.Anchor = AnchorStyles.Bottom;
            btOk.DialogResult = DialogResult.OK;
            btOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btOk.Location = new Point(357, 481);
            btOk.Name = "btOk";
            btOk.Size = new Size(172, 40);
            btOk.TabIndex = 5;
            btOk.Text = "OK";
            btOk.UseVisualStyleBackColor = true;
            btOk.Click += btOk_Click;
            // 
            // lvModels
            // 
            lvModels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvModels.CheckBoxes = true;
            lvModels.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            lvModels.FullRowSelect = true;
            lvModels.GridLines = true;
            lvModels.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvModels.Location = new Point(12, 50);
            lvModels.MultiSelect = false;
            lvModels.Name = "lvModels";
            lvModels.Size = new Size(858, 419);
            lvModels.TabIndex = 6;
            lvModels.UseCompatibleStateImageBehavior = false;
            lvModels.View = View.Details;
            lvModels.ItemChecked += lvModels_ItemChecked;
            lvModels.MouseClick += lvModels_MouseClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Enabled";
            columnHeader1.Width = 35;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "File";
            columnHeader2.TextAlign = HorizontalAlignment.Center;
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Name";
            columnHeader3.Width = 250;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Link";
            columnHeader4.Width = 440;
            // 
            // bwDownloading
            // 
            bwDownloading.WorkerSupportsCancellation = true;
            bwDownloading.DoWork += bwDownloading_DoWork;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(315, 15);
            label1.TabIndex = 7;
            label1.Text = "Downloading some models require API key for Civitai.com";
            // 
            // tbCivitaiApiKey
            // 
            tbCivitaiApiKey.Location = new Point(333, 12);
            tbCivitaiApiKey.Name = "tbCivitaiApiKey";
            tbCivitaiApiKey.PlaceholderText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            tbCivitaiApiKey.Size = new Size(212, 23);
            tbCivitaiApiKey.TabIndex = 8;
            tbCivitaiApiKey.TextChanged += tbCivitaiApiKey_TextChanged;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(551, 15);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(91, 15);
            linkLabel1.TabIndex = 9;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "visit Civitai.com";
            linkLabel1.UseMnemonic = false;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // SdLorasForm
            // 
            AcceptButton = btOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 533);
            ControlBox = false;
            Controls.Add(linkLabel1);
            Controls.Add(tbCivitaiApiKey);
            Controls.Add(label1);
            Controls.Add(lvModels);
            Controls.Add(btOk);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "SdLorasForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Stable Diffusion LoRA (additional mini-checkpoints)";
            Load += SdLorasForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btOk;
        private ListView lvModels;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader4;
        private System.ComponentModel.BackgroundWorker bwDownloading;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Label label1;
        private TextBox tbCivitaiApiKey;
        private LinkLabel linkLabel1;
    }
}