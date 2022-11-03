namespace AiPainter.Adapters.LamaCleaner
{
    partial class LamaCleanerPanel
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
            this.btInpaint = new System.Windows.Forms.Button();
            this.collapsablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // collapsablePanel
            // 
            this.collapsablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsablePanel.BackColor = System.Drawing.SystemColors.Control;
            this.collapsablePanel.Caption = "lama-cleaner";
            this.collapsablePanel.Controls.Add(this.btInpaint);
            this.collapsablePanel.IsCollapsed = false;
            this.collapsablePanel.Location = new System.Drawing.Point(0, 0);
            this.collapsablePanel.Name = "collapsablePanel";
            this.collapsablePanel.Size = new System.Drawing.Size(292, 77);
            this.collapsablePanel.TabIndex = 5;
            // 
            // btInpaint
            // 
            this.btInpaint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btInpaint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btInpaint.Location = new System.Drawing.Point(3, 27);
            this.btInpaint.Name = "btInpaint";
            this.btInpaint.Size = new System.Drawing.Size(286, 39);
            this.btInpaint.TabIndex = 12;
            this.btInpaint.Text = "Clean masked area";
            this.btInpaint.UseVisualStyleBackColor = true;
            this.btInpaint.Click += new System.EventHandler(this.btInpaint_Click);
            // 
            // LamaCleanerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.collapsablePanel);
            this.Name = "LamaCleanerPanel";
            this.Size = new System.Drawing.Size(292, 80);
            this.collapsablePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CollapsablePanel collapsablePanel;
        private Button btInpaint;
    }
}
