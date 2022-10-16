namespace AiPainter.Adapters.InvokeAi
{
    partial class InvokeAiPanel
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
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numImg2img = new System.Windows.Forms.NumericUpDown();
            this.numCfgScale = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numGfpGan = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.collapsablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numImg2img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCfgScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGfpGan)).BeginInit();
            this.SuspendLayout();
            // 
            // collapsablePanel
            // 
            this.collapsablePanel.BackColor = System.Drawing.SystemColors.Control;
            this.collapsablePanel.Caption = "InvokeAI (StableDiffusion)";
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
            this.collapsablePanel.Controls.Add(this.label5);
            this.collapsablePanel.Controls.Add(this.label2);
            this.collapsablePanel.Controls.Add(this.numImg2img);
            this.collapsablePanel.Controls.Add(this.numCfgScale);
            this.collapsablePanel.Controls.Add(this.label4);
            this.collapsablePanel.Controls.Add(this.numGfpGan);
            this.collapsablePanel.Controls.Add(this.label3);
            this.collapsablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsablePanel.IsCollapsed = false;
            this.collapsablePanel.Location = new System.Drawing.Point(0, 0);
            this.collapsablePanel.Name = "collapsablePanel";
            this.collapsablePanel.Size = new System.Drawing.Size(334, 349);
            this.collapsablePanel.TabIndex = 4;
            // 
            // pbIterations
            // 
            this.pbIterations.CustomText = null;
            this.pbIterations.Location = new System.Drawing.Point(5, 309);
            this.pbIterations.Name = "pbIterations";
            this.pbIterations.Size = new System.Drawing.Size(324, 23);
            this.pbIterations.TabIndex = 16;
            this.pbIterations.TextColor = System.Drawing.Color.Black;
            // 
            // pbSteps
            // 
            this.pbSteps.CustomText = null;
            this.pbSteps.Location = new System.Drawing.Point(5, 282);
            this.pbSteps.Name = "pbSteps";
            this.pbSteps.Size = new System.Drawing.Size(324, 23);
            this.pbSteps.TabIndex = 15;
            this.pbSteps.TextColor = System.Drawing.Color.Black;
            // 
            // cbUseInitImage
            // 
            this.cbUseInitImage.AutoSize = true;
            this.cbUseInitImage.Location = new System.Drawing.Point(8, 107);
            this.cbUseInitImage.Name = "cbUseInitImage";
            this.cbUseInitImage.Size = new System.Drawing.Size(186, 19);
            this.cbUseInitImage.TabIndex = 12;
            this.cbUseInitImage.Text = "Use active image as start point";
            this.cbUseInitImage.UseVisualStyleBackColor = true;
            // 
            // btReset
            // 
            this.btReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btReset.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btReset.Location = new System.Drawing.Point(240, 237);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(90, 39);
            this.btReset.TabIndex = 11;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // tbPrompt
            // 
            this.tbPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrompt.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbPrompt.Location = new System.Drawing.Point(3, 29);
            this.tbPrompt.Multiline = true;
            this.tbPrompt.Name = "tbPrompt";
            this.tbPrompt.PlaceholderText = "Prompt (describe desired picture)";
            this.tbPrompt.Size = new System.Drawing.Size(327, 66);
            this.tbPrompt.TabIndex = 0;
            // 
            // numIterations
            // 
            this.numIterations.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numIterations.Location = new System.Drawing.Point(48, 136);
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
            this.numIterations.Size = new System.Drawing.Size(90, 27);
            this.numIterations.TabIndex = 1;
            this.numIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btGenerate
            // 
            this.btGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btGenerate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btGenerate.Location = new System.Drawing.Point(3, 237);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(232, 39);
            this.btGenerate.TabIndex = 8;
            this.btGenerate.Text = "Generate";
            this.btGenerate.UseVisualStyleBackColor = true;
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // numSteps
            // 
            this.numSteps.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numSteps.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSteps.Location = new System.Drawing.Point(267, 136);
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
            this.numSteps.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // tbSeed
            // 
            this.tbSeed.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbSeed.Location = new System.Drawing.Point(3, 203);
            this.tbSeed.Name = "tbSeed";
            this.tbSeed.PlaceholderText = "Seed...";
            this.tbSeed.Size = new System.Drawing.Size(326, 27);
            this.tbSeed.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Count";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(206, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Changes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Detail level";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numImg2img
            // 
            this.numImg2img.DecimalPlaces = 2;
            this.numImg2img.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numImg2img.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numImg2img.Location = new System.Drawing.Point(267, 102);
            this.numImg2img.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.numImg2img.Name = "numImg2img";
            this.numImg2img.Size = new System.Drawing.Size(62, 27);
            this.numImg2img.TabIndex = 5;
            this.numImg2img.Value = new decimal(new int[] {
            75,
            0,
            0,
            131072});
            // 
            // numCfgScale
            // 
            this.numCfgScale.DecimalPlaces = 1;
            this.numCfgScale.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numCfgScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numCfgScale.Location = new System.Drawing.Point(267, 169);
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
            this.numCfgScale.Value = new decimal(new int[] {
            75,
            0,
            0,
            65536});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Face fix";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numGfpGan
            // 
            this.numGfpGan.DecimalPlaces = 1;
            this.numGfpGan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numGfpGan.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numGfpGan.Location = new System.Drawing.Point(58, 169);
            this.numGfpGan.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGfpGan.Name = "numGfpGan";
            this.numGfpGan.Size = new System.Drawing.Size(62, 27);
            this.numGfpGan.TabIndex = 3;
            this.numGfpGan.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Relevance to prompt";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // InvokeAiPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.collapsablePanel);
            this.Name = "InvokeAiPanel";
            this.Size = new System.Drawing.Size(334, 349);
            this.collapsablePanel.ResumeLayout(false);
            this.collapsablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numImg2img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCfgScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGfpGan)).EndInit();
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
        private Label label5;
        private Label label2;
        private NumericUpDown numImg2img;
        private NumericUpDown numCfgScale;
        private Label label4;
        private NumericUpDown numGfpGan;
        private Label label3;
        private Controls.CustomProgressBar pbIterations;
        private Controls.CustomProgressBar pbSteps;
    }
}
