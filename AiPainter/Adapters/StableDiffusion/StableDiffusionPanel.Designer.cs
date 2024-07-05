namespace AiPainter.Adapters.StableDiffusion
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
            components = new System.ComponentModel.Container();
            collapsablePanel = new Controls.CollapsablePanel();
            cbUseSeed = new CheckBox();
            label1 = new Label();
            label5 = new Label();
            ddSampler = new ComboBox();
            btStyles = new Button();
            btEmbeddings = new Button();
            btNegativeEmbeddings = new Button();
            btNegativePromptHistory = new Button();
            btLoras = new Button();
            btCheckpoint = new Button();
            label4 = new Label();
            ddImageSize = new ComboBox();
            ddCheckpoint = new ComboBox();
            tbNegative = new TextBox();
            cbUseInitImage = new CheckBox();
            btReset = new Button();
            tbPrompt = new TextBox();
            numIterations = new NumericUpDown();
            btGenerate = new Button();
            numSteps = new NumericUpDown();
            tbSeed = new TextBox();
            numCfgScale = new NumericUpDown();
            label3 = new Label();
            trackBarChangesLevel = new TrackBar();
            trackBarSeedVariationStrength = new TrackBar();
            toolTip = new ToolTip(components);
            cmCheckpointMenu = new ContextMenuStrip(components);
            cmLorasMenu = new ContextMenuStrip(components);
            cmNegativePromptHistoryMenu = new ContextMenuStrip(components);
            cmEmbeddingsMenu = new ContextMenuStrip(components);
            cmNegativeEmbeddingsMenu = new ContextMenuStrip(components);
            collapsablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIterations).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSteps).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCfgScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarChangesLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarSeedVariationStrength).BeginInit();
            SuspendLayout();
            // 
            // collapsablePanel
            // 
            collapsablePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            collapsablePanel.BackColor = SystemColors.Control;
            collapsablePanel.Caption = "StableDiffusion";
            collapsablePanel.Controls.Add(cbUseSeed);
            collapsablePanel.Controls.Add(label1);
            collapsablePanel.Controls.Add(label5);
            collapsablePanel.Controls.Add(ddSampler);
            collapsablePanel.Controls.Add(btStyles);
            collapsablePanel.Controls.Add(btEmbeddings);
            collapsablePanel.Controls.Add(btNegativeEmbeddings);
            collapsablePanel.Controls.Add(btNegativePromptHistory);
            collapsablePanel.Controls.Add(btLoras);
            collapsablePanel.Controls.Add(btCheckpoint);
            collapsablePanel.Controls.Add(label4);
            collapsablePanel.Controls.Add(ddImageSize);
            collapsablePanel.Controls.Add(ddCheckpoint);
            collapsablePanel.Controls.Add(tbNegative);
            collapsablePanel.Controls.Add(cbUseInitImage);
            collapsablePanel.Controls.Add(btReset);
            collapsablePanel.Controls.Add(tbPrompt);
            collapsablePanel.Controls.Add(numIterations);
            collapsablePanel.Controls.Add(btGenerate);
            collapsablePanel.Controls.Add(numSteps);
            collapsablePanel.Controls.Add(tbSeed);
            collapsablePanel.Controls.Add(numCfgScale);
            collapsablePanel.Controls.Add(label3);
            collapsablePanel.Controls.Add(trackBarChangesLevel);
            collapsablePanel.Controls.Add(trackBarSeedVariationStrength);
            collapsablePanel.IsCollapsed = false;
            collapsablePanel.Location = new Point(0, 0);
            collapsablePanel.Name = "collapsablePanel";
            collapsablePanel.Size = new Size(426, 549);
            collapsablePanel.TabIndex = 4;
            collapsablePanel.Load += collapsablePanel_Load;
            collapsablePanel.Resize += collapsablePanel_Resize;
            // 
            // cbUseSeed
            // 
            cbUseSeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cbUseSeed.AutoSize = true;
            cbUseSeed.Location = new Point(3, 465);
            cbUseSeed.Name = "cbUseSeed";
            cbUseSeed.Size = new Size(51, 19);
            cbUseSeed.TabIndex = 29;
            cbUseSeed.Text = "Seed";
            toolTip.SetToolTip(cbUseSeed, "Positive integer");
            cbUseSeed.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.Location = new Point(216, 388);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 26;
            label1.Text = "Size";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.Location = new Point(3, 427);
            label5.Name = "label5";
            label5.Size = new Size(50, 15);
            label5.TabIndex = 26;
            label5.Text = "Sampler";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddSampler
            // 
            ddSampler.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ddSampler.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSampler.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddSampler.FormattingEnabled = true;
            ddSampler.Items.AddRange(new object[] { "512x512", "768x768", "512x768", "768x512", "640x640" });
            ddSampler.Location = new Point(59, 422);
            ddSampler.Name = "ddSampler";
            ddSampler.Size = new Size(162, 25);
            ddSampler.TabIndex = 25;
            toolTip.SetToolTip(ddSampler, "Euler a (default, use 50+ detail level), DPM++ 2M (use 20+ detail level), Heun (useful for postprocessing)");
            // 
            // btStyles
            // 
            btStyles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btStyles.BackColor = Color.White;
            btStyles.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btStyles.Location = new Point(367, 121);
            btStyles.Name = "btStyles";
            btStyles.Size = new Size(56, 25);
            btStyles.TabIndex = 24;
            btStyles.Text = "Styles..";
            toolTip.SetToolTip(btStyles, "Improve your prompt by a style");
            btStyles.UseVisualStyleBackColor = false;
            btStyles.Click += btStyles_Click;
            // 
            // btEmbeddings
            // 
            btEmbeddings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btEmbeddings.BackColor = Color.White;
            btEmbeddings.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btEmbeddings.Location = new Point(367, 90);
            btEmbeddings.Name = "btEmbeddings";
            btEmbeddings.Size = new Size(56, 25);
            btEmbeddings.TabIndex = 24;
            btEmbeddings.Text = "Emb>";
            toolTip.SetToolTip(btEmbeddings, "Embeddings");
            btEmbeddings.UseVisualStyleBackColor = false;
            btEmbeddings.Click += btEmbeddings_Click;
            // 
            // btNegativeEmbeddings
            // 
            btNegativeEmbeddings.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btNegativeEmbeddings.BackColor = Color.White;
            btNegativeEmbeddings.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btNegativeEmbeddings.Location = new Point(367, 306);
            btNegativeEmbeddings.Name = "btNegativeEmbeddings";
            btNegativeEmbeddings.Size = new Size(56, 25);
            btNegativeEmbeddings.TabIndex = 24;
            btNegativeEmbeddings.Text = "Emb>";
            toolTip.SetToolTip(btNegativeEmbeddings, "Negative Embeddings");
            btNegativeEmbeddings.UseVisualStyleBackColor = false;
            btNegativeEmbeddings.Click += btNegativeEmbeddings_Click;
            // 
            // btNegativePromptHistory
            // 
            btNegativePromptHistory.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btNegativePromptHistory.BackColor = Color.White;
            btNegativePromptHistory.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btNegativePromptHistory.Location = new Point(367, 275);
            btNegativePromptHistory.Name = "btNegativePromptHistory";
            btNegativePromptHistory.Size = new Size(56, 25);
            btNegativePromptHistory.TabIndex = 24;
            btNegativePromptHistory.Text = "Hist>";
            toolTip.SetToolTip(btNegativePromptHistory, "Negative Prompts History");
            btNegativePromptHistory.UseVisualStyleBackColor = false;
            btNegativePromptHistory.Click += btNegativePromptHistory_Click;
            // 
            // btLoras
            // 
            btLoras.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btLoras.BackColor = Color.White;
            btLoras.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btLoras.Location = new Point(367, 59);
            btLoras.Name = "btLoras";
            btLoras.Size = new Size(56, 25);
            btLoras.TabIndex = 24;
            btLoras.Text = "LoRA>";
            toolTip.SetToolTip(btLoras, "LoRA models");
            btLoras.UseVisualStyleBackColor = false;
            btLoras.Click += btLoras_Click;
            // 
            // btCheckpoint
            // 
            btCheckpoint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btCheckpoint.BackColor = Color.White;
            btCheckpoint.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btCheckpoint.Location = new Point(391, 28);
            btCheckpoint.Name = "btCheckpoint";
            btCheckpoint.Size = new Size(32, 25);
            btCheckpoint.TabIndex = 24;
            btCheckpoint.Text = ">>";
            btCheckpoint.UseVisualStyleBackColor = false;
            btCheckpoint.Click += btCheckpoint_Click;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.Location = new Point(227, 427);
            label4.Name = "label4";
            label4.Size = new Size(78, 15);
            label4.TabIndex = 23;
            label4.Text = "Detail level";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddImageSize
            // 
            ddImageSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ddImageSize.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            ddImageSize.FormattingEnabled = true;
            ddImageSize.Location = new Point(272, 381);
            ddImageSize.Name = "ddImageSize";
            ddImageSize.Size = new Size(151, 28);
            ddImageSize.TabIndex = 22;
            ddImageSize.Text = "512x512";
            toolTip.SetToolTip(ddImageSize, "Generated image size. Can be changed in \"Config.json\" file");
            // 
            // ddCheckpoint
            // 
            ddCheckpoint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ddCheckpoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ddCheckpoint.FormattingEnabled = true;
            ddCheckpoint.Location = new Point(3, 29);
            ddCheckpoint.Name = "ddCheckpoint";
            ddCheckpoint.Size = new Size(382, 23);
            ddCheckpoint.TabIndex = 18;
            toolTip.SetToolTip(ddCheckpoint, "Active checkpoint (image generation model)");
            ddCheckpoint.DropDown += ddCheckpoint_DropDown;
            // 
            // tbNegative
            // 
            tbNegative.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbNegative.BackColor = Color.FromArgb(255, 242, 242);
            tbNegative.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            tbNegative.Location = new Point(3, 275);
            tbNegative.Multiline = true;
            tbNegative.Name = "tbNegative";
            tbNegative.PlaceholderText = "Negative prompt (don't want to get)";
            tbNegative.ScrollBars = ScrollBars.Vertical;
            tbNegative.Size = new Size(358, 57);
            tbNegative.TabIndex = 17;
            // 
            // cbUseInitImage
            // 
            cbUseInitImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cbUseInitImage.Location = new Point(3, 346);
            cbUseInitImage.Name = "cbUseInitImage";
            cbUseInitImage.Size = new Size(186, 19);
            cbUseInitImage.TabIndex = 12;
            cbUseInitImage.Text = "Use active image as start point";
            cbUseInitImage.UseVisualStyleBackColor = true;
            // 
            // btReset
            // 
            btReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btReset.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btReset.Location = new Point(327, 498);
            btReset.Name = "btReset";
            btReset.Size = new Size(96, 39);
            btReset.TabIndex = 11;
            btReset.Text = "Reset";
            toolTip.SetToolTip(btReset, "Reset values to defaut state");
            btReset.UseVisualStyleBackColor = true;
            btReset.Click += btReset_Click;
            // 
            // tbPrompt
            // 
            tbPrompt.AcceptsReturn = true;
            tbPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbPrompt.BackColor = Color.FromArgb(233, 255, 232);
            tbPrompt.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            tbPrompt.Location = new Point(3, 59);
            tbPrompt.Multiline = true;
            tbPrompt.Name = "tbPrompt";
            tbPrompt.PlaceholderText = "Prompt (describe desired picture)";
            tbPrompt.ScrollBars = ScrollBars.Vertical;
            tbPrompt.Size = new Size(358, 210);
            tbPrompt.TabIndex = 0;
            // 
            // numIterations
            // 
            numIterations.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numIterations.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numIterations.Location = new Point(3, 505);
            numIterations.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numIterations.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numIterations.Name = "numIterations";
            numIterations.Size = new Size(59, 27);
            numIterations.TabIndex = 1;
            numIterations.TextAlign = HorizontalAlignment.Right;
            toolTip.SetToolTip(numIterations, "Images to generate");
            numIterations.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btGenerate
            // 
            btGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btGenerate.BackColor = Color.FromArgb(60, 138, 64);
            btGenerate.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btGenerate.ForeColor = Color.White;
            btGenerate.Location = new Point(68, 498);
            btGenerate.Name = "btGenerate";
            btGenerate.Size = new Size(254, 39);
            btGenerate.TabIndex = 8;
            btGenerate.Text = "Add to generation queue";
            btGenerate.UseVisualStyleBackColor = false;
            btGenerate.Click += btGenerate_Click;
            // 
            // numSteps
            // 
            numSteps.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numSteps.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numSteps.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            numSteps.Location = new Point(311, 421);
            numSteps.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numSteps.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSteps.Name = "numSteps";
            numSteps.Size = new Size(48, 27);
            numSteps.TabIndex = 1;
            toolTip.SetToolTip(numSteps, "1..200 (original name: steps)");
            numSteps.Value = new decimal(new int[] { 35, 0, 0, 0 });
            // 
            // tbSeed
            // 
            tbSeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbSeed.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            tbSeed.Location = new Point(60, 459);
            tbSeed.Name = "tbSeed";
            tbSeed.Size = new Size(123, 27);
            tbSeed.TabIndex = 7;
            toolTip.SetToolTip(tbSeed, "Positive integer");
            // 
            // numCfgScale
            // 
            numCfgScale.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numCfgScale.DecimalPlaces = 1;
            numCfgScale.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numCfgScale.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numCfgScale.Location = new Point(126, 382);
            numCfgScale.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numCfgScale.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numCfgScale.Name = "numCfgScale";
            numCfgScale.Size = new Size(62, 27);
            numCfgScale.TabIndex = 3;
            toolTip.SetToolTip(numCfgScale, "How hard to follow the text prompt (original name: cfg scale)");
            numCfgScale.Value = new decimal(new int[] { 7, 0, 0, 0 });
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(3, 388);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 4;
            label3.Text = "Relevance to prompt";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trackBarChangesLevel
            // 
            trackBarChangesLevel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            trackBarChangesLevel.Location = new Point(186, 346);
            trackBarChangesLevel.Maximum = 100;
            trackBarChangesLevel.Name = "trackBarChangesLevel";
            trackBarChangesLevel.Size = new Size(237, 45);
            trackBarChangesLevel.TabIndex = 27;
            trackBarChangesLevel.TickStyle = TickStyle.None;
            toolTip.SetToolTip(trackBarChangesLevel, "Changes level");
            trackBarChangesLevel.Value = 100;
            // 
            // trackBarSeedVariationStrength
            // 
            trackBarSeedVariationStrength.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            trackBarSeedVariationStrength.Location = new Point(186, 462);
            trackBarSeedVariationStrength.Maximum = 100;
            trackBarSeedVariationStrength.Name = "trackBarSeedVariationStrength";
            trackBarSeedVariationStrength.Size = new Size(237, 45);
            trackBarSeedVariationStrength.TabIndex = 27;
            trackBarSeedVariationStrength.TickStyle = TickStyle.None;
            toolTip.SetToolTip(trackBarSeedVariationStrength, "Seed variation strength");
            // 
            // cmCheckpointMenu
            // 
            cmCheckpointMenu.Name = "contextMenuCheckpoint";
            cmCheckpointMenu.Size = new Size(61, 4);
            // 
            // cmLorasMenu
            // 
            cmLorasMenu.MinimumSize = new Size(220, 0);
            cmLorasMenu.Name = "contextMenuCheckpoint";
            cmLorasMenu.Size = new Size(220, 4);
            // 
            // cmNegativePromptHistoryMenu
            // 
            cmNegativePromptHistoryMenu.MinimumSize = new Size(200, 0);
            cmNegativePromptHistoryMenu.Name = "contextMenuCheckpoint";
            cmNegativePromptHistoryMenu.Size = new Size(200, 4);
            // 
            // cmEmbeddingsMenu
            // 
            cmEmbeddingsMenu.MinimumSize = new Size(220, 0);
            cmEmbeddingsMenu.Name = "contextMenuCheckpoint";
            cmEmbeddingsMenu.Size = new Size(220, 4);
            // 
            // cmNegativeEmbeddingsMenu
            // 
            cmNegativeEmbeddingsMenu.MinimumSize = new Size(200, 0);
            cmNegativeEmbeddingsMenu.Name = "contextMenuCheckpoint";
            cmNegativeEmbeddingsMenu.Size = new Size(200, 4);
            // 
            // StableDiffusionPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(collapsablePanel);
            MinimumSize = new Size(426, 0);
            Name = "StableDiffusionPanel";
            Size = new Size(426, 552);
            collapsablePanel.ResumeLayout(false);
            collapsablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIterations).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSteps).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCfgScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarChangesLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarSeedVariationStrength).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Controls.CollapsablePanel collapsablePanel;
        public CheckBox cbUseInitImage;
        private Button btReset;
        public TextBox tbPrompt;
        public NumericUpDown numIterations;
        private Button btGenerate;
        public NumericUpDown numSteps;
        public TextBox tbSeed;
        public NumericUpDown numCfgScale;
        private Label label3;
        private ToolTip toolTip;
        public TextBox tbNegative;
        public ComboBox ddCheckpoint;
        public ComboBox ddImageSize;
        private Label label4;
        private Button btCheckpoint;
        private ContextMenuStrip cmCheckpointMenu;
        public ComboBox ddSampler;
        private Label label5;
        public TrackBar trackBarChangesLevel;
        public TrackBar trackBarSeedVariationStrength;
        public CheckBox cbUseSeed;
        private Button btNegativePromptHistory;
        private Button btLoras;
        private ContextMenuStrip cmLorasMenu;
        private ContextMenuStrip cmNegativePromptHistoryMenu;
        private Label label1;
        private Button btNegativeEmbeddings;
        private Button btEmbeddings;
        private ContextMenuStrip cmEmbeddingsMenu;
        private ContextMenuStrip cmNegativeEmbeddingsMenu;
        private Button btStyles;
    }
}
