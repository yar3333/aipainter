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
            label5 = new Label();
            ddlSampler = new ComboBox();
            btContextMenuCheckpoint = new Button();
            label4 = new Label();
            ddlImageSize = new ComboBox();
            lbModifiers = new ListBox();
            ddInpaintingFill = new ComboBox();
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
            contextMenuCheckpoint = new ContextMenuStrip(components);
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
            collapsablePanel.Controls.Add(label5);
            collapsablePanel.Controls.Add(ddlSampler);
            collapsablePanel.Controls.Add(btContextMenuCheckpoint);
            collapsablePanel.Controls.Add(label4);
            collapsablePanel.Controls.Add(ddlImageSize);
            collapsablePanel.Controls.Add(lbModifiers);
            collapsablePanel.Controls.Add(ddInpaintingFill);
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
            collapsablePanel.Size = new Size(348, 549);
            collapsablePanel.TabIndex = 4;
            collapsablePanel.Load += collapsablePanel_Load;
            collapsablePanel.Resize += collapsablePanel_Resize;
            // 
            // cbUseSeed
            // 
            cbUseSeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cbUseSeed.AutoSize = true;
            cbUseSeed.Location = new Point(3, 393);
            cbUseSeed.Name = "cbUseSeed";
            cbUseSeed.Size = new Size(51, 19);
            cbUseSeed.TabIndex = 29;
            cbUseSeed.Text = "Seed";
            toolTip.SetToolTip(cbUseSeed, "Positive integer");
            cbUseSeed.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(3, 355);
            label5.Name = "label5";
            label5.Size = new Size(50, 15);
            label5.TabIndex = 26;
            label5.Text = "Sampler";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddlSampler
            // 
            ddlSampler.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ddlSampler.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlSampler.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlSampler.FormattingEnabled = true;
            ddlSampler.Items.AddRange(new object[] { "512x512", "768x768", "512x768", "768x512", "640x640" });
            ddlSampler.Location = new Point(59, 350);
            ddlSampler.Name = "ddlSampler";
            ddlSampler.Size = new Size(162, 25);
            ddlSampler.TabIndex = 25;
            toolTip.SetToolTip(ddlSampler, "Euler a (default, use 50+ detail level), DPM++ 2M (use 20+ detail level), Heun (useful for postprocessing)");
            // 
            // btContextMenuCheckpoint
            // 
            btContextMenuCheckpoint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btContextMenuCheckpoint.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btContextMenuCheckpoint.Location = new Point(313, 28);
            btContextMenuCheckpoint.Name = "btContextMenuCheckpoint";
            btContextMenuCheckpoint.Size = new Size(32, 25);
            btContextMenuCheckpoint.TabIndex = 24;
            btContextMenuCheckpoint.Text = ">>";
            btContextMenuCheckpoint.UseVisualStyleBackColor = true;
            btContextMenuCheckpoint.Click += btContextMenuCheckpoint_Click;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(227, 355);
            label4.Name = "label4";
            label4.Size = new Size(64, 15);
            label4.TabIndex = 23;
            label4.Text = "Detail level";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddlImageSize
            // 
            ddlImageSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ddlImageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlImageSize.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlImageSize.FormattingEnabled = true;
            ddlImageSize.Location = new Point(194, 311);
            ddlImageSize.Name = "ddlImageSize";
            ddlImageSize.Size = new Size(151, 25);
            ddlImageSize.TabIndex = 22;
            toolTip.SetToolTip(ddlImageSize, "Generated image size. Can be changed in \"Config.json\" file");
            // 
            // lbModifiers
            // 
            lbModifiers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbModifiers.BackColor = Color.FromArgb(224, 224, 224);
            lbModifiers.FormattingEnabled = true;
            lbModifiers.ItemHeight = 15;
            lbModifiers.Location = new Point(3, 428);
            lbModifiers.Name = "lbModifiers";
            lbModifiers.SelectionMode = SelectionMode.None;
            lbModifiers.Size = new Size(342, 64);
            lbModifiers.TabIndex = 20;
            toolTip.SetToolTip(lbModifiers, "Modifiers (style names, will be added to prompt)");
            lbModifiers.Click += lbModifiers_Click;
            // 
            // ddInpaintingFill
            // 
            ddInpaintingFill.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ddInpaintingFill.DropDownStyle = ComboBoxStyle.DropDownList;
            ddInpaintingFill.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            ddInpaintingFill.FormattingEnabled = true;
            ddInpaintingFill.Location = new Point(192, 252);
            ddInpaintingFill.Name = "ddInpaintingFill";
            ddInpaintingFill.Size = new Size(153, 25);
            ddInpaintingFill.TabIndex = 19;
            toolTip.SetToolTip(ddInpaintingFill, "Inpaint type (preparing masked area)");
            // 
            // ddCheckpoint
            // 
            ddCheckpoint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ddCheckpoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ddCheckpoint.FormattingEnabled = true;
            ddCheckpoint.Location = new Point(3, 29);
            ddCheckpoint.Name = "ddCheckpoint";
            ddCheckpoint.Size = new Size(304, 23);
            ddCheckpoint.TabIndex = 18;
            toolTip.SetToolTip(ddCheckpoint, "Active StableDiffusion checkpoint (weights). Just download additional *.ckpt files and put them into `stable_diffusion_checkpoints` folder.");
            ddCheckpoint.SelectedIndexChanged += ddCheckpoint_SelectedIndexChanged;
            // 
            // tbNegative
            // 
            tbNegative.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbNegative.BackColor = Color.FromArgb(255, 242, 242);
            tbNegative.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            tbNegative.Location = new Point(3, 194);
            tbNegative.Multiline = true;
            tbNegative.Name = "tbNegative";
            tbNegative.PlaceholderText = "Negative prompt (don't want to get)";
            tbNegative.ScrollBars = ScrollBars.Vertical;
            tbNegative.Size = new Size(342, 48);
            tbNegative.TabIndex = 17;
            // 
            // cbUseInitImage
            // 
            cbUseInitImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cbUseInitImage.AutoSize = true;
            cbUseInitImage.Location = new Point(3, 256);
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
            btReset.Location = new Point(249, 498);
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
            tbPrompt.Size = new Size(342, 129);
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
            btGenerate.Size = new Size(176, 39);
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
            numSteps.Location = new Point(297, 349);
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
            tbSeed.Location = new Point(60, 387);
            tbSeed.Name = "tbSeed";
            tbSeed.Size = new Size(105, 27);
            tbSeed.TabIndex = 7;
            toolTip.SetToolTip(tbSeed, "Positive integer");
            // 
            // numCfgScale
            // 
            numCfgScale.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numCfgScale.DecimalPlaces = 1;
            numCfgScale.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numCfgScale.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numCfgScale.Location = new Point(126, 310);
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
            label3.Location = new Point(3, 316);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 4;
            label3.Text = "Relevance to prompt";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trackBarChangesLevel
            // 
            trackBarChangesLevel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            trackBarChangesLevel.Location = new Point(0, 279);
            trackBarChangesLevel.Maximum = 100;
            trackBarChangesLevel.Name = "trackBarChangesLevel";
            trackBarChangesLevel.Size = new Size(348, 45);
            trackBarChangesLevel.TabIndex = 27;
            trackBarChangesLevel.TickStyle = TickStyle.None;
            toolTip.SetToolTip(trackBarChangesLevel, "Changes level");
            // 
            // trackBarSeedVariationStrength
            // 
            trackBarSeedVariationStrength.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            trackBarSeedVariationStrength.Location = new Point(171, 390);
            trackBarSeedVariationStrength.Maximum = 100;
            trackBarSeedVariationStrength.Name = "trackBarSeedVariationStrength";
            trackBarSeedVariationStrength.Size = new Size(174, 45);
            trackBarSeedVariationStrength.TabIndex = 27;
            trackBarSeedVariationStrength.TickStyle = TickStyle.None;
            toolTip.SetToolTip(trackBarSeedVariationStrength, "Seed variation strength");
            // 
            // contextMenuCheckpoint
            // 
            contextMenuCheckpoint.Name = "contextMenuCheckpoint";
            contextMenuCheckpoint.Size = new Size(61, 4);
            // 
            // StableDiffusionPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(collapsablePanel);
            Name = "StableDiffusionPanel";
            Size = new Size(348, 552);
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
        public ComboBox ddInpaintingFill;
        private ListBox lbModifiers;
        public ComboBox ddlImageSize;
        private Label label4;
        private Button btContextMenuCheckpoint;
        private ContextMenuStrip contextMenuCheckpoint;
        public ComboBox ddlSampler;
        private Label label5;
        public TrackBar trackBarChangesLevel;
        public TrackBar trackBarSeedVariationStrength;
        public CheckBox cbUseSeed;
    }
}
