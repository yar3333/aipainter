namespace AiPainter.Controls
{
    partial class GenerationList
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
            this.components = new System.ComponentModel.Container();
            this.stateManager = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // stateManager
            // 
            this.stateManager.Enabled = true;
            this.stateManager.Tick += new System.EventHandler(this.stateManager_Tick);
            // 
            // GenerationList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Name = "GenerationList";
            this.Size = new System.Drawing.Size(225, 269);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer stateManager;
    }
}
