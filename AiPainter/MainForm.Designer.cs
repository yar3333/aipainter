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
            this.checkPortsWorker = new System.ComponentModel.BackgroundWorker();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.btClearActiveImage = new System.Windows.Forms.ToolStripButton();
            this.btLoad = new System.Windows.Forms.ToolStripButton();
            this.btSavePng = new System.Windows.Forms.ToolStripButton();
            this.btSaveJpeg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
            this.pictureBox = new AiPainter.Controls.SmartPictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panRemBg = new AiPainter.Adapters.RemBg.RemBgPanel();
            this.panLamaCleaner = new AiPainter.Adapters.LamaCleaner.LamaCleanerPanel();
            this.panInvokeAi = new AiPainter.Adapters.InvokeAi.InvokeAiPanel();
            this.hPicScroll = new System.Windows.Forms.HScrollBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            controlsStateUpdater = new System.Windows.Forms.Timer(this.components);
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlsStateUpdater
            // 
            controlsStateUpdater.Enabled = true;
            controlsStateUpdater.Interval = 200;
            controlsStateUpdater.Tick += new System.EventHandler(this.controlsStateUpdater_Tick);
            // 
            // checkPortsWorker
            // 
            this.checkPortsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkPortsWorker_DoWork);
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
            this.btSaveJpeg.ToolTipText = "Save image";
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
            this.splitContainer.Panel1.Controls.Add(this.pictureBox);
            this.splitContainer.Panel1.Controls.Add(this.panel1);
            this.splitContainer.Size = new System.Drawing.Size(924, 667);
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
            this.pictureBox.Size = new System.Drawing.Size(588, 523);
            this.pictureBox.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.panRemBg);
            this.panel1.Controls.Add(this.panLamaCleaner);
            this.panel1.Controls.Add(this.panInvokeAi);
            this.panel1.Location = new System.Drawing.Point(590, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 523);
            this.panel1.TabIndex = 2;
            // 
            // panRemBg
            // 
            this.panRemBg.AutoSize = true;
            this.panRemBg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panRemBg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panRemBg.Location = new System.Drawing.Point(0, 432);
            this.panRemBg.Name = "panRemBg";
            this.panRemBg.Size = new System.Drawing.Size(334, 80);
            this.panRemBg.TabIndex = 8;
            // 
            // panLamaCleaner
            // 
            this.panLamaCleaner.AutoSize = true;
            this.panLamaCleaner.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panLamaCleaner.Dock = System.Windows.Forms.DockStyle.Top;
            this.panLamaCleaner.Location = new System.Drawing.Point(0, 352);
            this.panLamaCleaner.Name = "panLamaCleaner";
            this.panLamaCleaner.Size = new System.Drawing.Size(334, 80);
            this.panLamaCleaner.TabIndex = 7;
            // 
            // panInvokeAi
            // 
            this.panInvokeAi.AutoSize = true;
            this.panInvokeAi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panInvokeAi.Dock = System.Windows.Forms.DockStyle.Top;
            this.panInvokeAi.Location = new System.Drawing.Point(0, 0);
            this.panInvokeAi.Name = "panInvokeAi";
            this.panInvokeAi.Size = new System.Drawing.Size(334, 352);
            this.panInvokeAi.TabIndex = 6;
            // 
            // hPicScroll
            // 
            this.hPicScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hPicScroll.Location = new System.Drawing.Point(0, 710);
            this.hPicScroll.Name = "hPicScroll";
            this.hPicScroll.Size = new System.Drawing.Size(924, 30);
            this.hPicScroll.TabIndex = 0;
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
        private Adapters.InvokeAi.InvokeAiPanel panInvokeAi;
        private Adapters.LamaCleaner.LamaCleanerPanel panLamaCleaner;
        private Adapters.RemBg.RemBgPanel panRemBg;
        private System.ComponentModel.BackgroundWorker checkPortsWorker;
        private ToolStripButton btRestorePrevMask;
        private ToolStripButton btAbout;
    }
}