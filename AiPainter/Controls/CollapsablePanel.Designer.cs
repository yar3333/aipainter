namespace AiPainter.Controls
{
    partial class CollapsablePanel
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
            this.btHeader = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btHeader
            // 
            this.btHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.btHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btHeader.ForeColor = System.Drawing.Color.White;
            this.btHeader.Location = new System.Drawing.Point(0, 0);
            this.btHeader.Margin = new System.Windows.Forms.Padding(0);
            this.btHeader.Name = "btHeader";
            this.btHeader.Size = new System.Drawing.Size(150, 23);
            this.btHeader.TabIndex = 0;
            this.btHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btHeader.UseVisualStyleBackColor = false;
            this.btHeader.Click += new System.EventHandler(this.btHeader_Click);
            // 
            // CollapsablePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btHeader);
            this.Name = "CollapsablePanel";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btHeader;
    }
}
