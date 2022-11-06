namespace AiPainter.Adapters.RemBg
{
    partial class RemBgPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.collapsablePanel = new AiPainter.Controls.CollapsablePanel();
            this.btRemoveBackground = new System.Windows.Forms.Button();
            this.portCheckWorker = new System.ComponentModel.BackgroundWorker();
            this.collapsablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // collapsablePanel
            // 
            this.collapsablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsablePanel.BackColor = System.Drawing.SystemColors.Control;
            this.collapsablePanel.Caption = "rembg";
            this.collapsablePanel.Controls.Add(this.btRemoveBackground);
            this.collapsablePanel.IsCollapsed = false;
            this.collapsablePanel.Location = new System.Drawing.Point(0, 0);
            this.collapsablePanel.Name = "collapsablePanel";
            this.collapsablePanel.Size = new System.Drawing.Size(292, 77);
            this.collapsablePanel.TabIndex = 6;
            // 
            // btRemoveBackground
            // 
            this.btRemoveBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemoveBackground.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btRemoveBackground.Location = new System.Drawing.Point(3, 26);
            this.btRemoveBackground.Name = "btRemoveBackground";
            this.btRemoveBackground.Size = new System.Drawing.Size(286, 39);
            this.btRemoveBackground.TabIndex = 13;
            this.btRemoveBackground.Text = "Remove background";
            this.btRemoveBackground.UseVisualStyleBackColor = true;
            this.btRemoveBackground.Click += new System.EventHandler(this.btRemBgRemoveBackground_Click);
            // 
            // portCheckWorker
            // 
            this.portCheckWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.portCheckWorker_DoWork);
            // 
            // RemBgPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.collapsablePanel);
            this.Name = "RemBgPanel";
            this.Size = new System.Drawing.Size(292, 80);
            this.collapsablePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CollapsablePanel collapsablePanel;
        private Button btRemoveBackground;
        private System.ComponentModel.BackgroundWorker portCheckWorker;
    }
}
