namespace AiPainter.Controls
{
    partial class GenerationListItem
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
            this.tbPrompt = new System.Windows.Forms.TextBox();
            this.pbSteps = new AiPainter.Controls.CustomProgressBar();
            this.pbIterations = new AiPainter.Controls.CustomProgressBar();
            this.numIterations = new System.Windows.Forms.NumericUpDown();
            this.btRemove = new System.Windows.Forms.Button();
            this.btLoadParamsBackToPanel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).BeginInit();
            this.SuspendLayout();
            // 
            // tbPrompt
            // 
            this.tbPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrompt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPrompt.Location = new System.Drawing.Point(50, 7);
            this.tbPrompt.Name = "tbPrompt";
            this.tbPrompt.ReadOnly = true;
            this.tbPrompt.Size = new System.Drawing.Size(268, 23);
            this.tbPrompt.TabIndex = 0;
            // 
            // pbSteps
            // 
            this.pbSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSteps.BackColor = System.Drawing.Color.AliceBlue;
            this.pbSteps.CustomText = null;
            this.pbSteps.Location = new System.Drawing.Point(374, 7);
            this.pbSteps.Name = "pbSteps";
            this.pbSteps.Size = new System.Drawing.Size(100, 23);
            this.pbSteps.TabIndex = 1;
            this.pbSteps.TextColor = System.Drawing.Color.Black;
            // 
            // pbIterations
            // 
            this.pbIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbIterations.BackColor = System.Drawing.Color.AliceBlue;
            this.pbIterations.CustomText = null;
            this.pbIterations.Location = new System.Drawing.Point(480, 7);
            this.pbIterations.Name = "pbIterations";
            this.pbIterations.Size = new System.Drawing.Size(100, 23);
            this.pbIterations.TabIndex = 2;
            this.pbIterations.TextColor = System.Drawing.Color.Black;
            // 
            // numIterations
            // 
            this.numIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numIterations.Location = new System.Drawing.Point(324, 7);
            this.numIterations.Name = "numIterations";
            this.numIterations.Size = new System.Drawing.Size(44, 23);
            this.numIterations.TabIndex = 3;
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btRemove.ForeColor = System.Drawing.Color.Red;
            this.btRemove.Location = new System.Drawing.Point(586, 7);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(31, 23);
            this.btRemove.TabIndex = 4;
            this.btRemove.Text = "X";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // btLoadParamsBackToPanel
            // 
            this.btLoadParamsBackToPanel.Location = new System.Drawing.Point(6, 5);
            this.btLoadParamsBackToPanel.Name = "btLoadParamsBackToPanel";
            this.btLoadParamsBackToPanel.Size = new System.Drawing.Size(38, 25);
            this.btLoadParamsBackToPanel.TabIndex = 5;
            this.btLoadParamsBackToPanel.Text = "<=";
            this.btLoadParamsBackToPanel.UseVisualStyleBackColor = true;
            this.btLoadParamsBackToPanel.Click += new System.EventHandler(this.btLoadParamsBackToPanel_Click);
            // 
            // GenerationListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btLoadParamsBackToPanel);
            this.Controls.Add(this.btRemove);
            this.Controls.Add(this.numIterations);
            this.Controls.Add(this.pbIterations);
            this.Controls.Add(this.pbSteps);
            this.Controls.Add(this.tbPrompt);
            this.Name = "GenerationListItem";
            this.Size = new System.Drawing.Size(620, 42);
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbPrompt;
        private CustomProgressBar pbSteps;
        private CustomProgressBar pbIterations;
        private NumericUpDown numIterations;
        private Button btRemove;
        private Button btLoadParamsBackToPanel;
    }
}
