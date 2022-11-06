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
            System.Windows.Forms.Timer controlsStateUpdater;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.btClearActiveImage = new System.Windows.Forms.ToolStripButton();
            this.btLoad = new System.Windows.Forms.ToolStripButton();
            this.btSave = new System.Windows.Forms.ToolStripButton();
            this.btSaveAs = new System.Windows.Forms.ToolStripButton();
            this.btSavePng = new System.Windows.Forms.ToolStripButton();
            this.btSaveJpeg = new System.Windows.Forms.ToolStripButton();
            this.btCopyToClipboard = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btResizeAndMoveActiveBoxToFitImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btResetMask = new System.Windows.Forms.ToolStripButton();
            this.btDeAlpha = new System.Windows.Forms.ToolStripButton();
            this.btRestorePrevMask = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btLeft = new System.Windows.Forms.ToolStripButton();
            this.btDown = new System.Windows.Forms.ToolStripButton();
            this.btUp = new System.Windows.Forms.ToolStripButton();
            this.btRight = new System.Windows.Forms.ToolStripButton();
            this.btAbout = new System.Windows.Forms.ToolStripButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new AiPainter.Controls.SmartPictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panRemBg = new AiPainter.Adapters.RemBg.RemBgPanel();
            this.panLamaCleaner = new AiPainter.Adapters.LamaCleaner.LamaCleanerPanel();
            this.panStableDiffusion = new AiPainter.Adapters.StableDiffusion.StableDiffusionPanel();
            this.hPicScroll = new System.Windows.Forms.HScrollBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.updateImageListWorker = new System.ComponentModel.BackgroundWorker();
            controlsStateUpdater = new System.Windows.Forms.Timer(this.components);
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlsStateUpdater
            // 
            controlsStateUpdater.Enabled = true;
            controlsStateUpdater.Interval = 200;
            controlsStateUpdater.Tick += new System.EventHandler(this.controlsStateUpdater_Tick);
            // 
            // toolbar
            // 
            this.toolbar.AutoSize = false;
            this.toolbar.ImageScalingSize = new System.Drawing.Size(34, 34);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btClearActiveImage,
            this.btLoad,
            this.btSave,
            this.btSaveAs,
            this.btSavePng,
            this.btSaveJpeg,
            this.btCopyToClipboard,
            this.toolStripSeparator2,
            this.btResizeAndMoveActiveBoxToFitImage,
            this.toolStripSeparator3,
            this.btResetMask,
            this.btDeAlpha,
            this.btRestorePrevMask,
            this.toolStripSeparator1,
            this.btLeft,
            this.btDown,
            this.btUp,
            this.btRight,
            this.btAbout});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(924, 40);
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
            // btSave
            // 
            this.btSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSave.Image = ((System.Drawing.Image)(resources.GetObject("btSave.Image")));
            this.btSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(38, 37);
            this.btSave.Text = "Save image (overwrite)";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btSaveAs
            // 
            this.btSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btSaveAs.Image")));
            this.btSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSaveAs.Name = "btSaveAs";
            this.btSaveAs.Size = new System.Drawing.Size(38, 37);
            this.btSaveAs.Text = "Save image as...";
            this.btSaveAs.Click += new System.EventHandler(this.btSaveAs_Click);
            // 
            // btSavePng
            // 
            this.btSavePng.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSavePng.Image = ((System.Drawing.Image)(resources.GetObject("btSavePng.Image")));
            this.btSavePng.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSavePng.Name = "btSavePng";
            this.btSavePng.Size = new System.Drawing.Size(38, 37);
            this.btSavePng.Text = "Save image as PNG (auto name)";
            this.btSavePng.Click += new System.EventHandler(this.btSavePng_Click);
            // 
            // btSaveJpeg
            // 
            this.btSaveJpeg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSaveJpeg.Image = ((System.Drawing.Image)(resources.GetObject("btSaveJpeg.Image")));
            this.btSaveJpeg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSaveJpeg.Name = "btSaveJpeg";
            this.btSaveJpeg.Size = new System.Drawing.Size(38, 37);
            this.btSaveJpeg.Text = "Save image as JPG (auto name)";
            this.btSaveJpeg.ToolTipText = "Save image";
            this.btSaveJpeg.Click += new System.EventHandler(this.btSaveJpeg_Click);
            // 
            // btCopyToClipboard
            // 
            this.btCopyToClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btCopyToClipboard.Image = ((System.Drawing.Image)(resources.GetObject("btCopyToClipboard.Image")));
            this.btCopyToClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btCopyToClipboard.Name = "btCopyToClipboard";
            this.btCopyToClipboard.Size = new System.Drawing.Size(38, 37);
            this.btCopyToClipboard.Text = "Copy to clipboard";
            this.btCopyToClipboard.Click += new System.EventHandler(this.btCopyToClipboard_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 40);
            // 
            // btResizeAndMoveActiveBoxToFitImage
            // 
            this.btResizeAndMoveActiveBoxToFitImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btResizeAndMoveActiveBoxToFitImage.Image = ((System.Drawing.Image)(resources.GetObject("btResizeAndMoveActiveBoxToFitImage.Image")));
            this.btResizeAndMoveActiveBoxToFitImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btResizeAndMoveActiveBoxToFitImage.Name = "btResizeAndMoveActiveBoxToFitImage";
            this.btResizeAndMoveActiveBoxToFitImage.Size = new System.Drawing.Size(38, 37);
            this.btResizeAndMoveActiveBoxToFitImage.Text = "Resize and move active zone to fit image";
            this.btResizeAndMoveActiveBoxToFitImage.Click += new System.EventHandler(this.btResizeAndMoveActiveBoxToFitImage_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 40);
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
            // btDeAlpha
            // 
            this.btDeAlpha.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDeAlpha.Image = ((System.Drawing.Image)(resources.GetObject("btDeAlpha.Image")));
            this.btDeAlpha.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDeAlpha.Name = "btDeAlpha";
            this.btDeAlpha.Size = new System.Drawing.Size(38, 37);
            this.btDeAlpha.Text = "Transparent to opaque";
            this.btDeAlpha.Click += new System.EventHandler(this.btDeAlpha_Click);
            // 
            // btRestorePrevMask
            // 
            this.btRestorePrevMask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRestorePrevMask.Image = ((System.Drawing.Image)(resources.GetObject("btRestorePrevMask.Image")));
            this.btRestorePrevMask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRestorePrevMask.Name = "btRestorePrevMask";
            this.btRestorePrevMask.Size = new System.Drawing.Size(38, 37);
            this.btRestorePrevMask.Text = "Restore previous mask";
            this.btRestorePrevMask.Click += new System.EventHandler(this.btRestorePrevMask_Click);
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
            // btAbout
            // 
            this.btAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAbout.Image = ((System.Drawing.Image)(resources.GetObject("btAbout.Image")));
            this.btAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAbout.Name = "btAbout";
            this.btAbout.Size = new System.Drawing.Size(38, 37);
            this.btAbout.Text = "About...";
            this.btAbout.Click += new System.EventHandler(this.btAbout_Click);
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
            this.splitContainer.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainer.Size = new System.Drawing.Size(924, 667);
            this.splitContainer.SplitterDistance = 523;
            this.splitContainer.TabIndex = 2;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(924, 523);
            this.splitContainer1.SplitterDistance = 308;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Image = null;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(308, 523);
            this.pictureBox.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(612, 523);
            this.splitContainer2.SplitterDistance = 404;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.panRemBg);
            this.panel1.Controls.Add(this.panLamaCleaner);
            this.panel1.Controls.Add(this.panStableDiffusion);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(404, 523);
            this.panel1.TabIndex = 2;
            // 
            // panRemBg
            // 
            this.panRemBg.AutoSize = true;
            this.panRemBg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panRemBg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panRemBg.Location = new System.Drawing.Point(0, 510);
            this.panRemBg.Name = "panRemBg";
            this.panRemBg.Size = new System.Drawing.Size(387, 80);
            this.panRemBg.TabIndex = 8;
            // 
            // panLamaCleaner
            // 
            this.panLamaCleaner.AutoSize = true;
            this.panLamaCleaner.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panLamaCleaner.Dock = System.Windows.Forms.DockStyle.Top;
            this.panLamaCleaner.Location = new System.Drawing.Point(0, 430);
            this.panLamaCleaner.Name = "panLamaCleaner";
            this.panLamaCleaner.Size = new System.Drawing.Size(387, 80);
            this.panLamaCleaner.TabIndex = 7;
            // 
            // panStableDiffusion
            // 
            this.panStableDiffusion.AutoSize = true;
            this.panStableDiffusion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panStableDiffusion.Dock = System.Windows.Forms.DockStyle.Top;
            this.panStableDiffusion.Location = new System.Drawing.Point(0, 0);
            this.panStableDiffusion.Name = "panStableDiffusion";
            this.panStableDiffusion.Size = new System.Drawing.Size(387, 430);
            this.panStableDiffusion.TabIndex = 6;
            // 
            // hPicScroll
            // 
            this.hPicScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hPicScroll.Location = new System.Drawing.Point(0, 710);
            this.hPicScroll.Name = "hPicScroll";
            this.hPicScroll.Size = new System.Drawing.Size(924, 30);
            this.hPicScroll.TabIndex = 0;
            // 
            // updateImageListWorker
            // 
            this.updateImageListWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateImageListWorker_DoWork);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(924, 740);
            this.Controls.Add(this.hPicScroll);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "AiPainter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private ToolStripButton btClearActiveImage;
        private ToolStripButton btLeft;
        private ToolStripButton btDown;
        private ToolStripButton btUp;
        private ToolStripButton btRight;
        private ToolStripButton btResetMask;
        private ToolTip toolTip;
        private SmartPictureBox pictureBox;
        private ToolStripButton btSaveJpeg;
        private Adapters.StableDiffusion.StableDiffusionPanel panStableDiffusion;
        private Adapters.LamaCleaner.LamaCleanerPanel panLamaCleaner;
        private Adapters.RemBg.RemBgPanel panRemBg;
        private ToolStripButton btRestorePrevMask;
        private ToolStripButton btAbout;
        private ToolStripButton btCopyToClipboard;
        private ToolStripButton btResizeAndMoveActiveBoxToFitImage;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton btSave;
        private ToolStripButton btSaveAs;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private GenerationList panGenerationList;
        private System.ComponentModel.BackgroundWorker updateImageListWorker;
    }
}