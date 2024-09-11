using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion
{
    partial class SdListItemGeneration
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
            components = new System.ComponentModel.Container();
            tbPrompt = new TextBox();
            pbSteps = new CustomProgressBar();
            pbIterations = new CustomProgressBar();
            numIterations = new NumericUpDown();
            btRemove = new Button();
            btLoadParamsBackToPanel = new Button();
            toolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)numIterations).BeginInit();
            SuspendLayout();
            // 
            // tbPrompt
            // 
            tbPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbPrompt.BorderStyle = BorderStyle.FixedSingle;
            tbPrompt.Location = new Point(50, 7);
            tbPrompt.Name = "tbPrompt";
            tbPrompt.ReadOnly = true;
            tbPrompt.Size = new Size(264, 23);
            tbPrompt.TabIndex = 0;
            // 
            // pbSteps
            // 
            pbSteps.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbSteps.BackColor = Color.AliceBlue;
            pbSteps.CustomText = null;
            pbSteps.Location = new Point(374, 7);
            pbSteps.Name = "pbSteps";
            pbSteps.Size = new Size(100, 23);
            pbSteps.TabIndex = 1;
            pbSteps.TextColor = Color.Black;
            toolTip.SetToolTip(pbSteps, "Steps");
            // 
            // pbIterations
            // 
            pbIterations.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbIterations.BackColor = Color.AliceBlue;
            pbIterations.CustomText = null;
            pbIterations.Location = new Point(480, 7);
            pbIterations.Name = "pbIterations";
            pbIterations.Size = new Size(100, 23);
            pbIterations.TabIndex = 2;
            pbIterations.TextColor = Color.Black;
            toolTip.SetToolTip(pbIterations, "Iterations");
            // 
            // numIterations
            // 
            numIterations.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numIterations.Location = new Point(320, 7);
            numIterations.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numIterations.Name = "numIterations";
            numIterations.Size = new Size(48, 23);
            numIterations.TabIndex = 3;
            toolTip.SetToolTip(numIterations, "Images count need to generate");
            numIterations.ValueChanged += numIterations_ValueChanged;
            // 
            // btRemove
            // 
            btRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btRemove.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btRemove.ForeColor = Color.Red;
            btRemove.Location = new Point(586, 7);
            btRemove.Name = "btRemove";
            btRemove.Size = new Size(31, 23);
            btRemove.TabIndex = 4;
            btRemove.Text = "X";
            toolTip.SetToolTip(btRemove, "Cancel");
            btRemove.UseVisualStyleBackColor = true;
            btRemove.Click += btRemove_Click;
            // 
            // btLoadParamsBackToPanel
            // 
            btLoadParamsBackToPanel.Location = new Point(6, 6);
            btLoadParamsBackToPanel.Name = "btLoadParamsBackToPanel";
            btLoadParamsBackToPanel.Size = new Size(38, 25);
            btLoadParamsBackToPanel.TabIndex = 5;
            btLoadParamsBackToPanel.Text = "<=";
            toolTip.SetToolTip(btLoadParamsBackToPanel, "Load parameters into panel");
            btLoadParamsBackToPanel.UseVisualStyleBackColor = true;
            btLoadParamsBackToPanel.Click += btLoadParamsBackToPanel_Click;
            // 
            // GenerationListItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(btLoadParamsBackToPanel);
            Controls.Add(btRemove);
            Controls.Add(numIterations);
            Controls.Add(pbIterations);
            Controls.Add(pbSteps);
            Controls.Add(tbPrompt);
            Name = "GenerationListItem";
            Size = new Size(620, 42);
            ((System.ComponentModel.ISupportInitialize)numIterations).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbPrompt;
        private CustomProgressBar pbSteps;
        private CustomProgressBar pbIterations;
        private NumericUpDown numIterations;
        private Button btRemove;
        private Button btLoadParamsBackToPanel;
        private ToolTip toolTip;
    }
}
