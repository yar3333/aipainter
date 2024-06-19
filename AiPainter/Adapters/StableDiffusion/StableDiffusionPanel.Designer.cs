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
            label4 = new Label();
            ddlSize = new ComboBox();
            ddLora = new ComboBox();
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
            label1 = new Label();
            label2 = new Label();
            numCfgScale = new NumericUpDown();
            label3 = new Label();
            toolTip = new ToolTip(components);
            collapsablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIterations).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSteps).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCfgScale).BeginInit();
            SuspendLayout();
            // 
            // collapsablePanel
            // 
            collapsablePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            collapsablePanel.BackColor = SystemColors.Control;
            collapsablePanel.Caption = "StableDiffusion";
            collapsablePanel.Controls.Add(label4);
            collapsablePanel.Controls.Add(ddlSize);
            collapsablePanel.Controls.Add(ddLora);
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
            collapsablePanel.Controls.Add(label1);
            collapsablePanel.Controls.Add(label2);
            collapsablePanel.Controls.Add(numCfgScale);
            collapsablePanel.Controls.Add(label3);
            collapsablePanel.IsCollapsed = false;
            collapsablePanel.Location = new Point(0, 0);
            collapsablePanel.Name = "collapsablePanel";
            collapsablePanel.Size = new Size(348, 438);
            collapsablePanel.TabIndex = 4;
            collapsablePanel.Load += collapsablePanel_Load;
            collapsablePanel.Resize += collapsablePanel_Resize;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(152, 272);
            label4.Name = "label4";
            label4.Size = new Size(64, 15);
            label4.TabIndex = 23;
            label4.Text = "Detail level";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddlSize
            // 
            ddlSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ddlSize.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlSize.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlSize.FormattingEnabled = true;
            ddlSize.Items.AddRange(new object[] { "512x512", "768x768", "512x768", "768x512", "640x640" });
            ddlSize.Location = new Point(269, 267);
            ddlSize.Name = "ddlSize";
            ddlSize.Size = new Size(72, 25);
            ddlSize.TabIndex = 22;
            toolTip.SetToolTip(ddlSize, "Generated image size.");
            // 
            // ddLora
            // 
            ddLora.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ddLora.DropDownStyle = ComboBoxStyle.DropDownList;
            ddLora.FormattingEnabled = true;
            ddLora.Location = new Point(3, 56);
            ddLora.Name = "ddLora";
            ddLora.Size = new Size(342, 23);
            ddLora.TabIndex = 21;
            toolTip.SetToolTip(ddLora, "Active StableDiffusion LoRA (put files into `stable_diffusion_lora` folder).");
            // 
            // lbModifiers
            // 
            lbModifiers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbModifiers.BackColor = Color.FromArgb(224, 224, 224);
            lbModifiers.FormattingEnabled = true;
            lbModifiers.ItemHeight = 15;
            lbModifiers.Location = new Point(3, 332);
            lbModifiers.Name = "lbModifiers";
            lbModifiers.SelectionMode = SelectionMode.None;
            lbModifiers.Size = new Size(342, 49);
            lbModifiers.TabIndex = 20;
            toolTip.SetToolTip(lbModifiers, "Modifiers (style names, will be added to prompt)");
            lbModifiers.Click += lbModifiers_Click;
            // 
            // ddInpaintingFill
            // 
            ddInpaintingFill.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ddInpaintingFill.DropDownStyle = ComboBoxStyle.DropDownList;
            ddInpaintingFill.FormattingEnabled = true;
            ddInpaintingFill.Location = new Point(197, 233);
            ddInpaintingFill.Name = "ddInpaintingFill";
            ddInpaintingFill.Size = new Size(144, 23);
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
            ddCheckpoint.Size = new Size(342, 23);
            ddCheckpoint.TabIndex = 18;
            toolTip.SetToolTip(ddCheckpoint, "Active StableDiffusion checkpoint (weights). Just download additional *.ckpt files and put them into `stable_diffusion_checkpoints` folder.");
            ddCheckpoint.SelectedIndexChanged += ddCheckpoint_SelectedIndexChanged;
            // 
            // tbNegative
            // 
            tbNegative.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbNegative.BackColor = Color.FromArgb(255, 242, 242);
            tbNegative.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            tbNegative.Location = new Point(3, 183);
            tbNegative.Multiline = true;
            tbNegative.Name = "tbNegative";
            tbNegative.PlaceholderText = "Negative prompt (don't want to get)";
            tbNegative.Size = new Size(342, 48);
            tbNegative.TabIndex = 17;
            // 
            // cbUseInitImage
            // 
            cbUseInitImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cbUseInitImage.AutoSize = true;
            cbUseInitImage.Location = new Point(8, 237);
            cbUseInitImage.Name = "cbUseInitImage";
            cbUseInitImage.Size = new Size(186, 19);
            cbUseInitImage.TabIndex = 12;
            cbUseInitImage.Text = "Use active image as start point";
            cbUseInitImage.UseVisualStyleBackColor = true;
            // 
            // btReset
            // 
            btReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btReset.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            btReset.Location = new Point(249, 387);
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
            tbPrompt.Location = new Point(3, 85);
            tbPrompt.Multiline = true;
            tbPrompt.Name = "tbPrompt";
            tbPrompt.PlaceholderText = "Prompt (describe desired picture)";
            tbPrompt.ScrollBars = ScrollBars.Vertical;
            tbPrompt.Size = new Size(342, 92);
            tbPrompt.TabIndex = 0;
            // 
            // numIterations
            // 
            numIterations.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numIterations.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numIterations.Location = new Point(87, 266);
            numIterations.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numIterations.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numIterations.Name = "numIterations";
            numIterations.Size = new Size(59, 27);
            numIterations.TabIndex = 1;
            toolTip.SetToolTip(numIterations, "Count of images to generate (original name: iterations)");
            numIterations.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btGenerate
            // 
            btGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btGenerate.BackColor = Color.FromArgb(60, 138, 64);
            btGenerate.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            btGenerate.ForeColor = Color.White;
            btGenerate.Location = new Point(1, 387);
            btGenerate.Name = "btGenerate";
            btGenerate.Size = new Size(243, 39);
            btGenerate.TabIndex = 8;
            btGenerate.Text = "Add to generation queue";
            btGenerate.UseVisualStyleBackColor = false;
            btGenerate.Click += btGenerate_Click;
            // 
            // numSteps
            // 
            numSteps.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numSteps.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numSteps.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numSteps.Location = new Point(219, 266);
            numSteps.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numSteps.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSteps.Name = "numSteps";
            numSteps.Size = new Size(44, 27);
            numSteps.TabIndex = 1;
            toolTip.SetToolTip(numSteps, "1..200 (original name: steps)");
            numSteps.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // tbSeed
            // 
            tbSeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbSeed.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            tbSeed.Location = new Point(230, 299);
            tbSeed.Name = "tbSeed";
            tbSeed.Size = new Size(111, 27);
            tbSeed.TabIndex = 7;
            toolTip.SetToolTip(tbSeed, "Random seed (positive integer)");
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(2, 272);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 2;
            label1.Text = "Images count";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(194, 306);
            label2.Name = "label2";
            label2.Size = new Size(32, 15);
            label2.TabIndex = 2;
            label2.Text = "Seed";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numCfgScale
            // 
            numCfgScale.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numCfgScale.DecimalPlaces = 1;
            numCfgScale.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numCfgScale.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numCfgScale.Location = new Point(126, 299);
            numCfgScale.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numCfgScale.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numCfgScale.Name = "numCfgScale";
            numCfgScale.Size = new Size(62, 27);
            numCfgScale.TabIndex = 3;
            toolTip.SetToolTip(numCfgScale, "How hard to follow the text prompt (original name: cfg scale)");
            numCfgScale.Value = new decimal(new int[] { 75, 0, 0, 65536 });
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(3, 305);
            label3.Name = "label3";
            label3.Size = new Size(117, 15);
            label3.TabIndex = 4;
            label3.Text = "Relevance to prompt";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // StableDiffusionPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(collapsablePanel);
            Name = "StableDiffusionPanel";
            Size = new Size(348, 441);
            collapsablePanel.ResumeLayout(false);
            collapsablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIterations).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSteps).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCfgScale).EndInit();
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
        private Label label1;
        private Label label2;
        public NumericUpDown numCfgScale;
        private Label label3;
        private ToolTip toolTip;
        public TextBox tbNegative;
        public ComboBox ddCheckpoint;
        public ComboBox ddInpaintingFill;
        private ListBox lbModifiers;
        public ComboBox ddLora;
        public ComboBox ddlSize;
        private Label label4;
    }
}
