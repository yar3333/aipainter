﻿namespace AiPainter.Adapters.StableDiffusion
{
    partial class SdCheckpointsForm
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
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            bwDownloading = new System.ComponentModel.BackgroundWorker();
            columnHeader4 = new ColumnHeader();
            SuspendLayout();
            // 
            // btOk
            // 
            btOk.Anchor = AnchorStyles.Bottom;
            btOk.DialogResult = DialogResult.OK;
            btOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btOk.Location = new Point(316, 398);
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
            lvModels.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6 });
            lvModels.FullRowSelect = true;
            lvModels.GridLines = true;
            lvModels.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvModels.Location = new Point(12, 12);
            lvModels.MultiSelect = false;
            lvModels.Name = "lvModels";
            lvModels.Size = new Size(776, 374);
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
            columnHeader2.Text = "Main file";
            columnHeader2.TextAlign = HorizontalAlignment.Center;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Inpaint file";
            columnHeader3.TextAlign = HorizontalAlignment.Center;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Name";
            columnHeader5.Width = 180;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Link";
            columnHeader6.Width = 350;
            // 
            // bwDownloading
            // 
            bwDownloading.WorkerSupportsCancellation = true;
            bwDownloading.DoWork += bwDownloading_DoWork;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "VAE file";
            columnHeader4.TextAlign = HorizontalAlignment.Center;
            // 
            // SdCheckpointsForm
            // 
            AcceptButton = btOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(lvModels);
            Controls.Add(btOk);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "SdCheckpointsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Stable Diffusion Checkpoints";
            Load += SdModelsForm_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button btOk;
        private ListView lvModels;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private System.ComponentModel.BackgroundWorker bwDownloading;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
    }
}