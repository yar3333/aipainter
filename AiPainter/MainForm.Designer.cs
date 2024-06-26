﻿using AiPainter.Controls;

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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.Timer controlsStateUpdater;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            toolbar = new ToolStrip();
            btClearActiveImage = new ToolStripButton();
            btLoad = new ToolStripButton();
            btSave = new ToolStripButton();
            btSaveAs = new ToolStripButton();
            btCopyToClipboard = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btResizeAndMoveActiveBoxToFitImage = new ToolStripButton();
            sbResize = new ToolStripSplitButton();
            toolStripSeparator3 = new ToolStripSeparator();
            btResetMask = new ToolStripButton();
            btDeAlpha = new ToolStripButton();
            btRestorePrevMask = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btLeft = new ToolStripButton();
            btDown = new ToolStripButton();
            btUp = new ToolStripButton();
            btRight = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            btAbout = new ToolStripButton();
            btRemoveObjectFromImage = new ToolStripButton();
            btRemoveBackgroundFromImage = new ToolStripButton();
            splitContainer = new SplitContainer();
            splitContainer1 = new SplitContainer();
            pictureBox = new SmartPictureBox();
            splitContainer2 = new SplitContainer();
            panel1 = new Panel();
            panStableDiffusion = new Adapters.StableDiffusion.StableDiffusionPanel();
            panGenerationList = new GenerationList();
            hPicScroll = new HScrollBar();
            toolTip = new ToolTip(components);
            updateImageListWorker = new System.ComponentModel.BackgroundWorker();
            contextMenuPreviewImage = new ContextMenuStrip(components);
            contextMenuPreviewImage_MoveToSubfolder = new ToolStripMenuItem();
            contextMenuPreviewImage_MoveToSubfolderAndOpen = new ToolStripMenuItem();
            contextMenuPreviewImage_MoveToParentFolder = new ToolStripMenuItem();
            contextMenuPreviewImage_MoveToParentFolderAndOpen = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            controlsStateUpdater = new System.Windows.Forms.Timer(components);
            toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            panel1.SuspendLayout();
            contextMenuPreviewImage.SuspendLayout();
            SuspendLayout();
            // 
            // controlsStateUpdater
            // 
            controlsStateUpdater.Enabled = true;
            controlsStateUpdater.Interval = 200;
            controlsStateUpdater.Tick += controlsStateUpdater_Tick;
            // 
            // toolbar
            // 
            toolbar.AutoSize = false;
            toolbar.ImageScalingSize = new Size(34, 34);
            toolbar.Items.AddRange(new ToolStripItem[] { btClearActiveImage, btLoad, btSave, btSaveAs, btCopyToClipboard, toolStripSeparator2, btResizeAndMoveActiveBoxToFitImage, sbResize, toolStripSeparator3, btResetMask, btDeAlpha, btRestorePrevMask, toolStripSeparator1, btLeft, btDown, btUp, btRight, toolStripSeparator4, btAbout, btRemoveObjectFromImage, btRemoveBackgroundFromImage });
            toolbar.Location = new Point(0, 0);
            toolbar.Name = "toolbar";
            toolbar.Size = new Size(924, 40);
            toolbar.TabIndex = 0;
            toolbar.Text = "toolStrip1";
            // 
            // btClearActiveImage
            // 
            btClearActiveImage.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btClearActiveImage.Image = (Image)resources.GetObject("btClearActiveImage.Image");
            btClearActiveImage.ImageTransparentColor = Color.Magenta;
            btClearActiveImage.Name = "btClearActiveImage";
            btClearActiveImage.Size = new Size(38, 37);
            btClearActiveImage.Text = "Clear active image";
            btClearActiveImage.Click += btClearActiveImage_Click;
            // 
            // btLoad
            // 
            btLoad.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btLoad.Image = (Image)resources.GetObject("btLoad.Image");
            btLoad.ImageTransparentColor = Color.Magenta;
            btLoad.Name = "btLoad";
            btLoad.Size = new Size(38, 37);
            btLoad.Text = "Load image";
            btLoad.Click += btLoad_Click;
            // 
            // btSave
            // 
            btSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btSave.Image = (Image)resources.GetObject("btSave.Image");
            btSave.ImageTransparentColor = Color.Magenta;
            btSave.Name = "btSave";
            btSave.Size = new Size(38, 37);
            btSave.Text = "Save image (overwrite)";
            btSave.Click += btSave_Click;
            // 
            // btSaveAs
            // 
            btSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btSaveAs.Image = (Image)resources.GetObject("btSaveAs.Image");
            btSaveAs.ImageTransparentColor = Color.Magenta;
            btSaveAs.Name = "btSaveAs";
            btSaveAs.Size = new Size(38, 37);
            btSaveAs.Text = "Save image as...";
            btSaveAs.Click += btSaveAs_Click;
            // 
            // btCopyToClipboard
            // 
            btCopyToClipboard.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btCopyToClipboard.Image = (Image)resources.GetObject("btCopyToClipboard.Image");
            btCopyToClipboard.ImageTransparentColor = Color.Magenta;
            btCopyToClipboard.Name = "btCopyToClipboard";
            btCopyToClipboard.Size = new Size(38, 37);
            btCopyToClipboard.Text = "Copy to clipboard";
            btCopyToClipboard.Click += btCopyToClipboard_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 40);
            // 
            // btResizeAndMoveActiveBoxToFitImage
            // 
            btResizeAndMoveActiveBoxToFitImage.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btResizeAndMoveActiveBoxToFitImage.Image = (Image)resources.GetObject("btResizeAndMoveActiveBoxToFitImage.Image");
            btResizeAndMoveActiveBoxToFitImage.ImageTransparentColor = Color.Magenta;
            btResizeAndMoveActiveBoxToFitImage.Name = "btResizeAndMoveActiveBoxToFitImage";
            btResizeAndMoveActiveBoxToFitImage.Size = new Size(38, 37);
            btResizeAndMoveActiveBoxToFitImage.Text = "Resize and move active zone to fit image";
            btResizeAndMoveActiveBoxToFitImage.Click += btResizeAndMoveActiveBoxToFitImage_Click;
            // 
            // sbResize
            // 
            sbResize.DisplayStyle = ToolStripItemDisplayStyle.Image;
            sbResize.Image = (Image)resources.GetObject("sbResize.Image");
            sbResize.ImageTransparentColor = Color.Magenta;
            sbResize.Name = "sbResize";
            sbResize.Size = new Size(50, 37);
            sbResize.Text = "Resize image";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 40);
            // 
            // btResetMask
            // 
            btResetMask.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btResetMask.Image = (Image)resources.GetObject("btResetMask.Image");
            btResetMask.ImageTransparentColor = Color.Magenta;
            btResetMask.Name = "btResetMask";
            btResetMask.Size = new Size(38, 37);
            btResetMask.Text = "Reset mask";
            btResetMask.Click += btResetMask_Click;
            // 
            // btDeAlpha
            // 
            btDeAlpha.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btDeAlpha.Image = (Image)resources.GetObject("btDeAlpha.Image");
            btDeAlpha.ImageTransparentColor = Color.Magenta;
            btDeAlpha.Name = "btDeAlpha";
            btDeAlpha.Size = new Size(38, 37);
            btDeAlpha.Text = "Transparent to opaque";
            btDeAlpha.Click += btDeAlpha_Click;
            // 
            // btRestorePrevMask
            // 
            btRestorePrevMask.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btRestorePrevMask.Image = (Image)resources.GetObject("btRestorePrevMask.Image");
            btRestorePrevMask.ImageTransparentColor = Color.Magenta;
            btRestorePrevMask.Name = "btRestorePrevMask";
            btRestorePrevMask.Size = new Size(38, 37);
            btRestorePrevMask.Text = "Restore previous mask";
            btRestorePrevMask.Click += btRestorePrevMask_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 40);
            // 
            // btLeft
            // 
            btLeft.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btLeft.Image = (Image)resources.GetObject("btLeft.Image");
            btLeft.ImageTransparentColor = Color.Magenta;
            btLeft.Name = "btLeft";
            btLeft.Size = new Size(38, 37);
            btLeft.Text = "Extend canvas";
            btLeft.Click += btLeft_Click;
            // 
            // btDown
            // 
            btDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btDown.Image = (Image)resources.GetObject("btDown.Image");
            btDown.ImageTransparentColor = Color.Magenta;
            btDown.Name = "btDown";
            btDown.Size = new Size(38, 37);
            btDown.Text = "Extend canvas";
            btDown.Click += btDown_Click;
            // 
            // btUp
            // 
            btUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btUp.Image = (Image)resources.GetObject("btUp.Image");
            btUp.ImageTransparentColor = Color.Magenta;
            btUp.Name = "btUp";
            btUp.Size = new Size(38, 37);
            btUp.Text = "Extend canvas";
            btUp.Click += btUp_Click;
            // 
            // btRight
            // 
            btRight.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btRight.Image = (Image)resources.GetObject("btRight.Image");
            btRight.ImageTransparentColor = Color.Magenta;
            btRight.Name = "btRight";
            btRight.Size = new Size(38, 37);
            btRight.Text = "Extend canvas";
            btRight.Click += btRight_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 40);
            // 
            // btAbout
            // 
            btAbout.Alignment = ToolStripItemAlignment.Right;
            btAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btAbout.Image = (Image)resources.GetObject("btAbout.Image");
            btAbout.ImageTransparentColor = Color.Magenta;
            btAbout.Name = "btAbout";
            btAbout.Size = new Size(38, 37);
            btAbout.Text = "About...";
            btAbout.Click += btAbout_Click;
            // 
            // btRemoveObjectFromImage
            // 
            btRemoveObjectFromImage.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btRemoveObjectFromImage.Image = (Image)resources.GetObject("btRemoveObjectFromImage.Image");
            btRemoveObjectFromImage.ImageTransparentColor = Color.Magenta;
            btRemoveObjectFromImage.Name = "btRemoveObjectFromImage";
            btRemoveObjectFromImage.Size = new Size(38, 37);
            btRemoveObjectFromImage.Text = "toolStripButton1";
            btRemoveObjectFromImage.ToolTipText = "Remove masked object from image";
            btRemoveObjectFromImage.Click += btRemoveObjectFromImage_Click;
            // 
            // btRemoveBackgroundFromImage
            // 
            btRemoveBackgroundFromImage.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btRemoveBackgroundFromImage.Image = (Image)resources.GetObject("btRemoveBackgroundFromImage.Image");
            btRemoveBackgroundFromImage.ImageTransparentColor = Color.Magenta;
            btRemoveBackgroundFromImage.Name = "btRemoveBackgroundFromImage";
            btRemoveBackgroundFromImage.Size = new Size(38, 37);
            btRemoveBackgroundFromImage.Text = "Remove background from image";
            btRemoveBackgroundFromImage.Click += btRemoveBackgroundFromImage_Click;
            // 
            // splitContainer
            // 
            splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer.Location = new Point(0, 40);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Resize += splitContainer_Panel2_Resize;
            splitContainer.Size = new Size(924, 667);
            splitContainer.SplitterDistance = 523;
            splitContainer.TabIndex = 2;
            splitContainer.SplitterMoved += splitContainer_SplitterMoved;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(924, 523);
            splitContainer1.SplitterDistance = 308;
            splitContainer1.TabIndex = 4;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = SystemColors.Window;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Image = null;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(308, 523);
            pictureBox.TabIndex = 3;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(panel1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(panGenerationList);
            splitContainer2.Size = new Size(612, 523);
            splitContainer2.SplitterDistance = 351;
            splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(panStableDiffusion);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(351, 523);
            panel1.TabIndex = 2;
            // 
            // panStableDiffusion
            // 
            panStableDiffusion.AutoSize = true;
            panStableDiffusion.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panStableDiffusion.Dock = DockStyle.Top;
            panStableDiffusion.Location = new Point(0, 0);
            panStableDiffusion.Name = "panStableDiffusion";
            panStableDiffusion.Size = new Size(351, 478);
            panStableDiffusion.TabIndex = 6;
            // 
            // panGenerationList
            // 
            panGenerationList.AutoScroll = true;
            panGenerationList.BackColor = SystemColors.ControlDarkDark;
            panGenerationList.BorderStyle = BorderStyle.FixedSingle;
            panGenerationList.Dock = DockStyle.Fill;
            panGenerationList.Location = new Point(0, 0);
            panGenerationList.Name = "panGenerationList";
            panGenerationList.Size = new Size(257, 523);
            panGenerationList.TabIndex = 0;
            // 
            // hPicScroll
            // 
            hPicScroll.Dock = DockStyle.Bottom;
            hPicScroll.Location = new Point(0, 710);
            hPicScroll.Name = "hPicScroll";
            hPicScroll.Size = new Size(924, 30);
            hPicScroll.TabIndex = 0;
            // 
            // updateImageListWorker
            // 
            updateImageListWorker.DoWork += updateImageListWorker_DoWork;
            // 
            // contextMenuPreviewImage
            // 
            contextMenuPreviewImage.Items.AddRange(new ToolStripItem[] { contextMenuPreviewImage_MoveToSubfolder, contextMenuPreviewImage_MoveToSubfolderAndOpen, toolStripSeparator5, contextMenuPreviewImage_MoveToParentFolder, contextMenuPreviewImage_MoveToParentFolderAndOpen });
            contextMenuPreviewImage.Name = "cmPreviewImageContextMenu";
            contextMenuPreviewImage.Size = new Size(243, 120);
            contextMenuPreviewImage.Closed += contextMenuPreviewImage_Closed;
            // 
            // contextMenuPreviewImage_MoveToSubfolder
            // 
            contextMenuPreviewImage_MoveToSubfolder.Name = "contextMenuPreviewImage_MoveToSubfolder";
            contextMenuPreviewImage_MoveToSubfolder.Size = new Size(242, 22);
            contextMenuPreviewImage_MoveToSubfolder.Text = "Move to subfolder";
            contextMenuPreviewImage_MoveToSubfolder.Click += contextMenuPreviewImage_MoveToSubfolder_Click;
            // 
            // contextMenuPreviewImage_MoveToSubfolderAndOpen
            // 
            contextMenuPreviewImage_MoveToSubfolderAndOpen.Name = "contextMenuPreviewImage_MoveToSubfolderAndOpen";
            contextMenuPreviewImage_MoveToSubfolderAndOpen.Size = new Size(242, 22);
            contextMenuPreviewImage_MoveToSubfolderAndOpen.Text = "Move to subfolder and open";
            contextMenuPreviewImage_MoveToSubfolderAndOpen.Click += contextMenuPreviewImage_MoveToSubfolderAndOpen_Click;
            // 
            // contextMenuPreviewImage_MoveToParentFolder
            // 
            contextMenuPreviewImage_MoveToParentFolder.Name = "contextMenuPreviewImage_MoveToParentFolder";
            contextMenuPreviewImage_MoveToParentFolder.Size = new Size(242, 22);
            contextMenuPreviewImage_MoveToParentFolder.Text = "Move to parent folder";
            contextMenuPreviewImage_MoveToParentFolder.Click += contextMenuPreviewImage_MoveToParentFolder_Click;
            // 
            // contextMenuPreviewImage_MoveToParentFolderAndOpen
            // 
            contextMenuPreviewImage_MoveToParentFolderAndOpen.Name = "contextMenuPreviewImage_MoveToParentFolderAndOpen";
            contextMenuPreviewImage_MoveToParentFolderAndOpen.Size = new Size(242, 22);
            contextMenuPreviewImage_MoveToParentFolderAndOpen.Text = "Move to parent folder and open";
            contextMenuPreviewImage_MoveToParentFolderAndOpen.Click += contextMenuPreviewImage_MoveToParentFolderAndOpen_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(239, 6);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(924, 740);
            Controls.Add(hPicScroll);
            Controls.Add(splitContainer);
            Controls.Add(toolbar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "AiPainter";
            WindowState = FormWindowState.Maximized;
            Load += MainForm_Load;
            toolbar.ResumeLayout(false);
            toolbar.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            contextMenuPreviewImage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ToolStrip toolbar;
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
        private Adapters.StableDiffusion.StableDiffusionPanel panStableDiffusion;
        private ToolStripButton btRestorePrevMask;
        private ToolStripButton btAbout;
        private ToolStripButton btCopyToClipboard;
        private ToolStripButton btResizeAndMoveActiveBoxToFitImage;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton btSave;
        private ToolStripButton btSaveAs;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private System.ComponentModel.BackgroundWorker updateImageListWorker;
        private GenerationList panGenerationList;
        private ToolStripSplitButton sbResize;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton btRemoveObjectFromImage;
        private ToolStripButton btRemoveBackgroundFromImage;
        private ContextMenuStrip contextMenuPreviewImage;
        private ToolStripMenuItem contextMenuPreviewImage_MoveToSubfolderAndOpen;
        private ToolStripMenuItem contextMenuPreviewImage_MoveToSubfolder;
        private ToolStripMenuItem contextMenuPreviewImage_MoveToParentFolder;
        private ToolStripMenuItem contextMenuPreviewImage_MoveToParentFolderAndOpen;
        private ToolStripSeparator toolStripSeparator5;
    }
}