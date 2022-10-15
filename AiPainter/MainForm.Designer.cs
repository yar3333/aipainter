using AiPainter.Controls;

namespace AiPainter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.btClearActiveImage = new System.Windows.Forms.ToolStripButton();
            this.btLoad = new System.Windows.Forms.ToolStripButton();
            this.btSavePng = new System.Windows.Forms.ToolStripButton();
            this.btSaveJpeg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btResetMask = new System.Windows.Forms.ToolStripButton();
            this.btApplyAlphaMask = new System.Windows.Forms.ToolStripButton();
            this.btDeAlpha = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btLeft = new System.Windows.Forms.ToolStripButton();
            this.btDown = new System.Windows.Forms.ToolStripButton();
            this.btUp = new System.Windows.Forms.ToolStripButton();
            this.btRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new AiPainter.Controls.SmartPictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.collapsablePanel3 = new AiPainter.Controls.CollapsablePanel();
            this.btRemBgRemoveBackground = new System.Windows.Forms.Button();
            this.collapsablePanel2 = new AiPainter.Controls.CollapsablePanel();
            this.btLamaCleanerInpaint = new System.Windows.Forms.Button();
            this.collapsablePanel1 = new AiPainter.Controls.CollapsablePanel();
            this.cbInvokeAiUseInitImage = new System.Windows.Forms.CheckBox();
            this.btInvokeAiReset = new System.Windows.Forms.Button();
            this.pbInvokeAiSteps = new System.Windows.Forms.ProgressBar();
            this.tbPrompt = new System.Windows.Forms.TextBox();
            this.pbInvokeAiIterations = new System.Windows.Forms.ProgressBar();
            this.numInvokeAiIterations = new System.Windows.Forms.NumericUpDown();
            this.btInvokeAiGenerate = new System.Windows.Forms.Button();
            this.numInvokeAiSteps = new System.Windows.Forms.NumericUpDown();
            this.tbInvokeAiSeed = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numInvokeAiImg2img = new System.Windows.Forms.NumericUpDown();
            this.numInvokeAiCfgScale = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numInvokeAiGfpGan = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.hPicScroll = new System.Windows.Forms.HScrollBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.collapsablePanel3.SuspendLayout();
            this.collapsablePanel2.SuspendLayout();
            this.collapsablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiImg2img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiCfgScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiGfpGan)).BeginInit();
            this.SuspendLayout();
            // 
            // toolbar
            // 
            this.toolbar.AutoSize = false;
            this.toolbar.ImageScalingSize = new System.Drawing.Size(34, 34);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btClearActiveImage,
            this.btLoad,
            this.btSavePng,
            this.btSaveJpeg,
            this.toolStripSeparator2,
            this.btResetMask,
            this.btApplyAlphaMask,
            this.btDeAlpha,
            this.toolStripSeparator1,
            this.btLeft,
            this.btDown,
            this.btUp,
            this.btRight,
            this.toolStripSeparator3});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(1010, 40);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            // 
            // btClearActiveImage
            // 
            this.btClearActiveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btClearActiveImage.Image = ((System.Drawing.Image)(resources.GetObject("btClearActiveImage.Image")));
            this.btClearActiveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btClearActiveImage.Name = "btClearActiveImage";
            this.btClearActiveImage.Size = new System.Drawing.Size(38, 37);
            this.btClearActiveImage.Text = "Clear active image";
            this.btClearActiveImage.Click += new System.EventHandler(this.btClearActiveImage_Click);
            // 
            // btLoad
            // 
            this.btLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btLoad.Image = ((System.Drawing.Image)(resources.GetObject("btLoad.Image")));
            this.btLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(38, 37);
            this.btLoad.Text = "Load image";
            this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
            // 
            // btSavePng
            // 
            this.btSavePng.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSavePng.Image = ((System.Drawing.Image)(resources.GetObject("btSavePng.Image")));
            this.btSavePng.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSavePng.Name = "btSavePng";
            this.btSavePng.Size = new System.Drawing.Size(38, 37);
            this.btSavePng.Text = "Save image";
            this.btSavePng.Click += new System.EventHandler(this.btSavePng_Click);
            // 
            // btSaveJpeg
            // 
            this.btSaveJpeg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSaveJpeg.Image = ((System.Drawing.Image)(resources.GetObject("btSaveJpeg.Image")));
            this.btSaveJpeg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSaveJpeg.Name = "btSaveJpeg";
            this.btSaveJpeg.Size = new System.Drawing.Size(38, 37);
            this.btSaveJpeg.Text = "toolStripButton1";
            this.btSaveJpeg.Click += new System.EventHandler(this.btSaveJpeg_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 40);
            // 
            // btResetMask
            // 
            this.btResetMask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btResetMask.Image = ((System.Drawing.Image)(resources.GetObject("btResetMask.Image")));
            this.btResetMask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btResetMask.Name = "btResetMask";
            this.btResetMask.Size = new System.Drawing.Size(38, 37);
            this.btResetMask.Text = "Reset mask";
            this.btResetMask.Click += new System.EventHandler(this.btResetMask_Click);
            // 
            // btApplyAlphaMask
            // 
            this.btApplyAlphaMask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btApplyAlphaMask.Image = ((System.Drawing.Image)(resources.GetObject("btApplyAlphaMask.Image")));
            this.btApplyAlphaMask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btApplyAlphaMask.Name = "btApplyAlphaMask";
            this.btApplyAlphaMask.Size = new System.Drawing.Size(38, 37);
            this.btApplyAlphaMask.Text = "Apply mask (add alpha)";
            this.btApplyAlphaMask.Click += new System.EventHandler(this.btApplyAlphaMask_Click);
            // 
            // btDeAlpha
            // 
            this.btDeAlpha.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDeAlpha.Image = ((System.Drawing.Image)(resources.GetObject("btDeAlpha.Image")));
            this.btDeAlpha.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDeAlpha.Name = "btDeAlpha";
            this.btDeAlpha.Size = new System.Drawing.Size(38, 37);
            this.btDeAlpha.Text = "Alpha to opaque";
            this.btDeAlpha.Click += new System.EventHandler(this.btDeAlpha_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // btLeft
            // 
            this.btLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btLeft.Image = ((System.Drawing.Image)(resources.GetObject("btLeft.Image")));
            this.btLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btLeft.Name = "btLeft";
            this.btLeft.Size = new System.Drawing.Size(38, 37);
            this.btLeft.Text = "Extend canvas";
            this.btLeft.Click += new System.EventHandler(this.btLeft_Click);
            // 
            // btDown
            // 
            this.btDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
            this.btDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(38, 37);
            this.btDown.Text = "Extend canvas";
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btUp
            // 
            this.btUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btUp.Image = ((System.Drawing.Image)(resources.GetObject("btUp.Image")));
            this.btUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(38, 37);
            this.btUp.Text = "Extend canvas";
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // btRight
            // 
            this.btRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRight.Image = ((System.Drawing.Image)(resources.GetObject("btRight.Image")));
            this.btRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRight.Name = "btRight";
            this.btRight.Size = new System.Drawing.Size(38, 37);
            this.btRight.Text = "Extend canvas";
            this.btRight.Click += new System.EventHandler(this.btRight_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 40);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(0, 40);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pictureBox);
            this.splitContainer.Panel1.Controls.Add(this.panel1);
            this.splitContainer.Size = new System.Drawing.Size(1010, 667);
            this.splitContainer.SplitterDistance = 523;
            this.splitContainer.TabIndex = 2;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.Image = null;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(674, 523);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.ViewDeltaX = 0;
            this.pictureBox.ViewDeltaY = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.collapsablePanel3);
            this.panel1.Controls.Add(this.collapsablePanel2);
            this.panel1.Controls.Add(this.collapsablePanel1);
            this.panel1.Location = new System.Drawing.Point(676, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 523);
            this.panel1.TabIndex = 2;
            // 
            // collapsablePanel3
            // 
            this.collapsablePanel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.collapsablePanel3.Caption = "rembg";
            this.collapsablePanel3.Controls.Add(this.btRemBgRemoveBackground);
            this.collapsablePanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsablePanel3.IsCollapsed = false;
            this.collapsablePanel3.Location = new System.Drawing.Point(0, 411);
            this.collapsablePanel3.Name = "collapsablePanel3";
            this.collapsablePanel3.Size = new System.Drawing.Size(334, 79);
            this.collapsablePanel3.TabIndex = 5;
            // 
            // btRemBgRemoveBackground
            // 
            this.btRemBgRemoveBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemBgRemoveBackground.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btRemBgRemoveBackground.Location = new System.Drawing.Point(3, 26);
            this.btRemBgRemoveBackground.Name = "btRemBgRemoveBackground";
            this.btRemBgRemoveBackground.Size = new System.Drawing.Size(327, 39);
            this.btRemBgRemoveBackground.TabIndex = 13;
            this.btRemBgRemoveBackground.Text = "Remove background";
            this.toolTip.SetToolTip(this.btRemBgRemoveBackground, "Start generation (StableDiffusion)");
            this.btRemBgRemoveBackground.UseVisualStyleBackColor = true;
            this.btRemBgRemoveBackground.Click += new System.EventHandler(this.btRemBgRemoveBackground_Click);
            // 
            // collapsablePanel2
            // 
            this.collapsablePanel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.collapsablePanel2.Caption = "lama-cleaner";
            this.collapsablePanel2.Controls.Add(this.btLamaCleanerInpaint);
            this.collapsablePanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsablePanel2.IsCollapsed = false;
            this.collapsablePanel2.Location = new System.Drawing.Point(0, 332);
            this.collapsablePanel2.Name = "collapsablePanel2";
            this.collapsablePanel2.Size = new System.Drawing.Size(334, 79);
            this.collapsablePanel2.TabIndex = 4;
            // 
            // btLamaCleanerInpaint
            // 
            this.btLamaCleanerInpaint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btLamaCleanerInpaint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btLamaCleanerInpaint.Location = new System.Drawing.Point(4, 27);
            this.btLamaCleanerInpaint.Name = "btLamaCleanerInpaint";
            this.btLamaCleanerInpaint.Size = new System.Drawing.Size(327, 39);
            this.btLamaCleanerInpaint.TabIndex = 12;
            this.btLamaCleanerInpaint.Text = "Inpaint";
            this.toolTip.SetToolTip(this.btLamaCleanerInpaint, "Start generation (StableDiffusion)");
            this.btLamaCleanerInpaint.UseVisualStyleBackColor = true;
            this.btLamaCleanerInpaint.Click += new System.EventHandler(this.btLamaCleanerInpaint_Click);
            // 
            // collapsablePanel1
            // 
            this.collapsablePanel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.collapsablePanel1.Caption = "InvokeAI (StableDiffusion)";
            this.collapsablePanel1.Controls.Add(this.cbInvokeAiUseInitImage);
            this.collapsablePanel1.Controls.Add(this.btInvokeAiReset);
            this.collapsablePanel1.Controls.Add(this.pbInvokeAiSteps);
            this.collapsablePanel1.Controls.Add(this.tbPrompt);
            this.collapsablePanel1.Controls.Add(this.pbInvokeAiIterations);
            this.collapsablePanel1.Controls.Add(this.numInvokeAiIterations);
            this.collapsablePanel1.Controls.Add(this.btInvokeAiGenerate);
            this.collapsablePanel1.Controls.Add(this.numInvokeAiSteps);
            this.collapsablePanel1.Controls.Add(this.tbInvokeAiSeed);
            this.collapsablePanel1.Controls.Add(this.label1);
            this.collapsablePanel1.Controls.Add(this.label5);
            this.collapsablePanel1.Controls.Add(this.label2);
            this.collapsablePanel1.Controls.Add(this.numInvokeAiImg2img);
            this.collapsablePanel1.Controls.Add(this.numInvokeAiCfgScale);
            this.collapsablePanel1.Controls.Add(this.label4);
            this.collapsablePanel1.Controls.Add(this.numInvokeAiGfpGan);
            this.collapsablePanel1.Controls.Add(this.label3);
            this.collapsablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsablePanel1.IsCollapsed = false;
            this.collapsablePanel1.Location = new System.Drawing.Point(0, 0);
            this.collapsablePanel1.Name = "collapsablePanel1";
            this.collapsablePanel1.Size = new System.Drawing.Size(334, 332);
            this.collapsablePanel1.TabIndex = 3;
            // 
            // cbInvokeAiUseInitImage
            // 
            this.cbInvokeAiUseInitImage.AutoSize = true;
            this.cbInvokeAiUseInitImage.Location = new System.Drawing.Point(4, 101);
            this.cbInvokeAiUseInitImage.Name = "cbInvokeAiUseInitImage";
            this.cbInvokeAiUseInitImage.Size = new System.Drawing.Size(186, 19);
            this.cbInvokeAiUseInitImage.TabIndex = 12;
            this.cbInvokeAiUseInitImage.Text = "Use active image as start point";
            this.cbInvokeAiUseInitImage.UseVisualStyleBackColor = true;
            // 
            // btInvokeAiReset
            // 
            this.btInvokeAiReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btInvokeAiReset.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btInvokeAiReset.Location = new System.Drawing.Point(241, 237);
            this.btInvokeAiReset.Name = "btInvokeAiReset";
            this.btInvokeAiReset.Size = new System.Drawing.Size(90, 39);
            this.btInvokeAiReset.TabIndex = 11;
            this.btInvokeAiReset.Text = "Reset";
            this.btInvokeAiReset.UseVisualStyleBackColor = true;
            this.btInvokeAiReset.Click += new System.EventHandler(this.btInvokeAiReset_Click);
            // 
            // pbInvokeAiSteps
            // 
            this.pbInvokeAiSteps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInvokeAiSteps.Location = new System.Drawing.Point(4, 282);
            this.pbInvokeAiSteps.Name = "pbInvokeAiSteps";
            this.pbInvokeAiSteps.Size = new System.Drawing.Size(327, 17);
            this.pbInvokeAiSteps.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbInvokeAiSteps.TabIndex = 10;
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
            this.tbPrompt.Size = new System.Drawing.Size(328, 66);
            this.tbPrompt.TabIndex = 0;
            // 
            // pbInvokeAiIterations
            // 
            this.pbInvokeAiIterations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInvokeAiIterations.Location = new System.Drawing.Point(4, 301);
            this.pbInvokeAiIterations.Name = "pbInvokeAiIterations";
            this.pbInvokeAiIterations.Size = new System.Drawing.Size(327, 17);
            this.pbInvokeAiIterations.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbInvokeAiIterations.TabIndex = 9;
            // 
            // numInvokeAiIterations
            // 
            this.numInvokeAiIterations.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numInvokeAiIterations.Location = new System.Drawing.Point(64, 127);
            this.numInvokeAiIterations.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numInvokeAiIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInvokeAiIterations.Name = "numInvokeAiIterations";
            this.numInvokeAiIterations.Size = new System.Drawing.Size(90, 27);
            this.numInvokeAiIterations.TabIndex = 1;
            this.toolTip.SetToolTip(this.numInvokeAiIterations, "Count of images to generate");
            this.numInvokeAiIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btInvokeAiGenerate
            // 
            this.btInvokeAiGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btInvokeAiGenerate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btInvokeAiGenerate.Location = new System.Drawing.Point(3, 237);
            this.btInvokeAiGenerate.Name = "btInvokeAiGenerate";
            this.btInvokeAiGenerate.Size = new System.Drawing.Size(234, 39);
            this.btInvokeAiGenerate.TabIndex = 8;
            this.btInvokeAiGenerate.Text = "Generate";
            this.toolTip.SetToolTip(this.btInvokeAiGenerate, "Start generation (StableDiffusion)");
            this.btInvokeAiGenerate.UseVisualStyleBackColor = true;
            this.btInvokeAiGenerate.Click += new System.EventHandler(this.btInvokeAiGenerate_Click);
            // 
            // numInvokeAiSteps
            // 
            this.numInvokeAiSteps.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numInvokeAiSteps.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numInvokeAiSteps.Location = new System.Drawing.Point(258, 127);
            this.numInvokeAiSteps.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numInvokeAiSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInvokeAiSteps.Name = "numInvokeAiSteps";
            this.numInvokeAiSteps.Size = new System.Drawing.Size(62, 27);
            this.numInvokeAiSteps.TabIndex = 1;
            this.toolTip.SetToolTip(this.numInvokeAiSteps, "Quality (1..200)");
            this.numInvokeAiSteps.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // tbInvokeAiSeed
            // 
            this.tbInvokeAiSeed.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbInvokeAiSeed.Location = new System.Drawing.Point(3, 203);
            this.tbInvokeAiSeed.Name = "tbInvokeAiSeed";
            this.tbInvokeAiSeed.PlaceholderText = "Seed...";
            this.tbInvokeAiSeed.Size = new System.Drawing.Size(194, 27);
            this.tbInvokeAiSeed.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Iterations";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "img2img";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Steps";
            // 
            // numInvokeAiImg2img
            // 
            this.numInvokeAiImg2img.DecimalPlaces = 2;
            this.numInvokeAiImg2img.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numInvokeAiImg2img.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numInvokeAiImg2img.Location = new System.Drawing.Point(64, 165);
            this.numInvokeAiImg2img.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.numInvokeAiImg2img.Name = "numInvokeAiImg2img";
            this.numInvokeAiImg2img.Size = new System.Drawing.Size(62, 27);
            this.numInvokeAiImg2img.TabIndex = 5;
            this.toolTip.SetToolTip(this.numInvokeAiImg2img, "Changes strength for case with initial image");
            this.numInvokeAiImg2img.Value = new decimal(new int[] {
            75,
            0,
            0,
            131072});
            // 
            // numInvokeAiCfgScale
            // 
            this.numInvokeAiCfgScale.DecimalPlaces = 1;
            this.numInvokeAiCfgScale.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numInvokeAiCfgScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numInvokeAiCfgScale.Location = new System.Drawing.Point(258, 165);
            this.numInvokeAiCfgScale.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numInvokeAiCfgScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numInvokeAiCfgScale.Name = "numInvokeAiCfgScale";
            this.numInvokeAiCfgScale.Size = new System.Drawing.Size(62, 27);
            this.numInvokeAiCfgScale.TabIndex = 3;
            this.toolTip.SetToolTip(this.numInvokeAiCfgScale, "How hard to follow the text prompt");
            this.numInvokeAiCfgScale.Value = new decimal(new int[] {
            75,
            0,
            0,
            65536});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "gfpgan";
            // 
            // numInvokeAiGfpGan
            // 
            this.numInvokeAiGfpGan.DecimalPlaces = 1;
            this.numInvokeAiGfpGan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numInvokeAiGfpGan.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numInvokeAiGfpGan.Location = new System.Drawing.Point(258, 204);
            this.numInvokeAiGfpGan.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInvokeAiGfpGan.Name = "numInvokeAiGfpGan";
            this.numInvokeAiGfpGan.Size = new System.Drawing.Size(62, 27);
            this.numInvokeAiGfpGan.TabIndex = 3;
            this.toolTip.SetToolTip(this.numInvokeAiGfpGan, "Face fix strength");
            this.numInvokeAiGfpGan.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Cfg Scale";
            // 
            // hPicScroll
            // 
            this.hPicScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hPicScroll.Location = new System.Drawing.Point(0, 710);
            this.hPicScroll.Name = "hPicScroll";
            this.hPicScroll.Size = new System.Drawing.Size(1010, 30);
            this.hPicScroll.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1010, 740);
            this.Controls.Add(this.hPicScroll);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "SyImageEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.collapsablePanel3.ResumeLayout(false);
            this.collapsablePanel2.ResumeLayout(false);
            this.collapsablePanel1.ResumeLayout(false);
            this.collapsablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiImg2img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiCfgScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInvokeAiGfpGan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStrip toolbar;
        private ToolStripButton btSavePng;
        private ToolStripButton btDeAlpha;
        private ToolStripButton btLoad;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator1;
        private SplitContainer splitContainer;
        private HScrollBar hPicScroll;
        private Panel panel1;
        private TextBox tbPrompt;
        private Label label2;
        private Label label1;
        private NumericUpDown numInvokeAiSteps;
        private NumericUpDown numInvokeAiIterations;
        private Label label3;
        private NumericUpDown numInvokeAiCfgScale;
        private Label label4;
        private NumericUpDown numInvokeAiGfpGan;
        private Label label5;
        private NumericUpDown numInvokeAiImg2img;
        private TextBox tbInvokeAiSeed;
        private Button btInvokeAiGenerate;
        private ToolStripButton btClearActiveImage;
        private ProgressBar pbInvokeAiSteps;
        private ProgressBar pbInvokeAiIterations;
        private Button btInvokeAiReset;
        private ToolStripButton btLeft;
        private ToolStripButton btDown;
        private ToolStripButton btUp;
        private ToolStripButton btRight;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton btResetMask;
        private ToolTip toolTip;
        private ToolStripButton btApplyAlphaMask;
        private CollapsablePanel collapsablePanel1;
        private CollapsablePanel collapsablePanel2;
        private Button btLamaCleanerInpaint;
        private CheckBox cbInvokeAiUseInitImage;
        private SmartPictureBox pictureBox;
        private CollapsablePanel collapsablePanel3;
        private Button btRemBgRemoveBackground;
        private ToolStripButton btSaveJpeg;
    }
}