﻿namespace AiPainter.Adapters.StableDiffusion
{
    partial class StableDiffusionPanel
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
            this.collapsablePanel = new AiPainter.Controls.CollapsablePanel();
            this.tbNegative = new System.Windows.Forms.TextBox();
            this.pbIterations = new AiPainter.Controls.CustomProgressBar();
            this.pbSteps = new AiPainter.Controls.CustomProgressBar();
            this.cbUseInitImage = new System.Windows.Forms.CheckBox();
            this.btReset = new System.Windows.Forms.Button();
            this.tbPrompt = new System.Windows.Forms.TextBox();
            this.numIterations = new System.Windows.Forms.NumericUpDown();
            this.btGenerate = new System.Windows.Forms.Button();
            this.numSteps = new System.Windows.Forms.NumericUpDown();
            this.tbSeed = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numCfgScale = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.collapsablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCfgScale)).BeginInit();
            this.SuspendLayout();
            // 
            // collapsablePanel
            // 
            this.collapsablePanel.BackColor = System.Drawing.SystemColors.Control;
            this.collapsablePanel.Caption = "StableDiffusion";
            this.collapsablePanel.Controls.Add(this.tbNegative);
            this.collapsablePanel.Controls.Add(this.pbIterations);
            this.collapsablePanel.Controls.Add(this.pbSteps);
            this.collapsablePanel.Controls.Add(this.cbUseInitImage);
            this.collapsablePanel.Controls.Add(this.btReset);
            this.collapsablePanel.Controls.Add(this.tbPrompt);
            this.collapsablePanel.Controls.Add(this.numIterations);
            this.collapsablePanel.Controls.Add(this.btGenerate);
            this.collapsablePanel.Controls.Add(this.numSteps);
            this.collapsablePanel.Controls.Add(this.tbSeed);
            this.collapsablePanel.Controls.Add(this.label1);
            this.collapsablePanel.Controls.Add(this.label2);
            this.collapsablePanel.Controls.Add(this.numCfgScale);
            this.collapsablePanel.Controls.Add(this.label3);
            this.collapsablePanel.IsCollapsed = false;
            this.collapsablePanel.Location = new System.Drawing.Point(0, 0);
            this.collapsablePanel.Name = "collapsablePanel";
            this.collapsablePanel.Size = new System.Drawing.Size(334, 422);
            this.collapsablePanel.TabIndex = 4;
            // 
            // tbNegative
            // 
            this.tbNegative.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNegative.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbNegative.Location = new System.Drawing.Point(7, 153);
            this.tbNegative.Multiline = true;
            this.tbNegative.Name = "tbNegative";
            this.tbNegative.PlaceholderText = "Negative (what you don\'t want to see)";
            this.tbNegative.Size = new System.Drawing.Size(327, 56);
            this.tbNegative.TabIndex = 17;
            // 
            // pbIterations
            // 
            this.pbIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbIterations.BackColor = System.Drawing.Color.AliceBlue;
            this.pbIterations.CustomText = "";
            this.pbIterations.Location = new System.Drawing.Point(5, 382);
            this.pbIterations.Name = "pbIterations";
            this.pbIterations.Size = new System.Drawing.Size(324, 23);
            this.pbIterations.TabIndex = 16;
            this.pbIterations.TextColor = System.Drawing.Color.Black;
            this.toolTip.SetToolTip(this.pbIterations, "Generated images count (iterations)");
            // 
            // pbSteps
            // 
            this.pbSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbSteps.BackColor = System.Drawing.Color.AliceBlue;
            this.pbSteps.CustomText = "";
            this.pbSteps.Location = new System.Drawing.Point(5, 355);
            this.pbSteps.Name = "pbSteps";
            this.pbSteps.Size = new System.Drawing.Size(324, 23);
            this.pbSteps.TabIndex = 15;
            this.pbSteps.TextColor = System.Drawing.Color.Black;
            this.toolTip.SetToolTip(this.pbSteps, "Detail level for current image (steps)");
            // 
            // cbUseInitImage
            // 
            this.cbUseInitImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbUseInitImage.AutoSize = true;
            this.cbUseInitImage.Location = new System.Drawing.Point(8, 215);
            this.cbUseInitImage.Name = "cbUseInitImage";
            this.cbUseInitImage.Size = new System.Drawing.Size(186, 19);
            this.cbUseInitImage.TabIndex = 12;
            this.cbUseInitImage.Text = "Use active image as start point";
            this.cbUseInitImage.UseVisualStyleBackColor = true;
            // 
            // btReset
            // 
            this.btReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btReset.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btReset.Location = new System.Drawing.Point(240, 310);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(90, 39);
            this.btReset.TabIndex = 11;
            this.btReset.Text = "Reset";
            this.toolTip.SetToolTip(this.btReset, "Reset values to defaut state");
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // tbPrompt
            // 
            this.tbPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrompt.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbPrompt.Location = new System.Drawing.Point(3, 29);
            this.tbPrompt.Multiline = true;
            this.tbPrompt.Name = "tbPrompt";
            this.tbPrompt.PlaceholderText = "Prompt (describe desired picture)";
            this.tbPrompt.Size = new System.Drawing.Size(327, 118);
            this.tbPrompt.TabIndex = 0;
            // 
            // numIterations
            // 
            this.numIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numIterations.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numIterations.Location = new System.Drawing.Point(87, 244);
            this.numIterations.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIterations.Name = "numIterations";
            this.numIterations.Size = new System.Drawing.Size(101, 27);
            this.numIterations.TabIndex = 1;
            this.toolTip.SetToolTip(this.numIterations, "Count of images to generate (original name: iterations)");
            this.numIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btGenerate
            // 
            this.btGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btGenerate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btGenerate.Location = new System.Drawing.Point(3, 310);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(232, 39);
            this.btGenerate.TabIndex = 8;
            this.btGenerate.Text = "Generate";
            this.btGenerate.UseVisualStyleBackColor = true;
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // numSteps
            // 
            this.numSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numSteps.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numSteps.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSteps.Location = new System.Drawing.Point(267, 244);
            this.numSteps.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSteps.Name = "numSteps";
            this.numSteps.Size = new System.Drawing.Size(62, 27);
            this.numSteps.TabIndex = 1;
            this.toolTip.SetToolTip(this.numSteps, "1..200 (original name: steps)");
            this.numSteps.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // tbSeed
            // 
            this.tbSeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbSeed.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbSeed.Location = new System.Drawing.Point(197, 277);
            this.tbSeed.Name = "tbSeed";
            this.tbSeed.PlaceholderText = "Seed...";
            this.tbSeed.Size = new System.Drawing.Size(132, 27);
            this.tbSeed.TabIndex = 7;
            this.toolTip.SetToolTip(this.tbSeed, "Random seed (positive integer)");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Images count";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 250);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Detail level";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numCfgScale
            // 
            this.numCfgScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numCfgScale.DecimalPlaces = 1;
            this.numCfgScale.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numCfgScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numCfgScale.Location = new System.Drawing.Point(126, 277);
            this.numCfgScale.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numCfgScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numCfgScale.Name = "numCfgScale";
            this.numCfgScale.Size = new System.Drawing.Size(62, 27);
            this.numCfgScale.TabIndex = 3;
            this.toolTip.SetToolTip(this.numCfgScale, "How hard to follow the text prompt (original name: cfg scale)");
            this.numCfgScale.Value = new decimal(new int[] {
            75,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 283);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Relevance to prompt";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StableDiffusionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.collapsablePanel);
            this.Name = "StableDiffusionPanel";
            this.Size = new System.Drawing.Size(337, 425);
            this.collapsablePanel.ResumeLayout(false);
            this.collapsablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCfgScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CollapsablePanel collapsablePanel;
        private CheckBox cbUseInitImage;
        private Button btReset;
        private TextBox tbPrompt;
        private NumericUpDown numIterations;
        private Button btGenerate;
        private NumericUpDown numSteps;
        private TextBox tbSeed;
        private Label label1;
        private Label label2;
        private NumericUpDown numCfgScale;
        private Label label3;
        private Controls.CustomProgressBar pbIterations;
        private Controls.CustomProgressBar pbSteps;
        private ToolTip toolTip;
        private TextBox tbNegative;
    }
}