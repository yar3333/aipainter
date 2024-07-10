﻿namespace AiPainter.Adapters.StableDiffusion.SdLoraStuff
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
            components = new System.ComponentModel.Container();
            btOk = new Button();
            lvModels = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            label1 = new Label();
            tbCivitaiApiKey = new TextBox();
            linkLabel1 = new LinkLabel();
            btImportFromCivitai = new Button();
            updateTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // btOk
            // 
            btOk.Anchor = AnchorStyles.Bottom;
            btOk.DialogResult = DialogResult.OK;
            btOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btOk.Location = new Point(485, 571);
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
            lvModels.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            lvModels.FullRowSelect = true;
            lvModels.GridLines = true;
            lvModels.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvModels.Location = new Point(12, 50);
            lvModels.MultiSelect = false;
            lvModels.Name = "lvModels";
            lvModels.Size = new Size(1114, 509);
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
            columnHeader3.Width = 330;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Description";
            columnHeader4.Width = 430;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Link";
            columnHeader5.Width = 190;
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
            // btImportFromCivitai
            // 
            btImportFromCivitai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btImportFromCivitai.Location = new Point(1032, 10);
            btImportFromCivitai.Name = "btImportFromCivitai";
            btImportFromCivitai.Size = new Size(94, 30);
            btImportFromCivitai.TabIndex = 14;
            btImportFromCivitai.Text = "Import...";
            btImportFromCivitai.UseVisualStyleBackColor = true;
            btImportFromCivitai.Click += btImportFromCivitai_Click;
            // 
            // updateTimer
            // 
            updateTimer.Enabled = true;
            updateTimer.Interval = 1000;
            updateTimer.Tick += updateTimer_Tick;
            // 
            // SdLorasForm
            // 
            AcceptButton = btOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1138, 623);
            Controls.Add(btImportFromCivitai);
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
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Label label1;
        private TextBox tbCivitaiApiKey;
        private LinkLabel linkLabel1;
        private Button btImportFromCivitai;
        private ColumnHeader columnHeader5;
        private System.Windows.Forms.Timer updateTimer;
    }
}