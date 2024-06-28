namespace AiPainter.Controls
{
    partial class SmartImagePreview
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
            pictureBox = new PictureBox();
            btRemove = new ImageButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btRemove).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Cursor = Cursors.UpArrow;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(250, 243);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.DoubleClick += pictureBox_DoubleClick;
            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseEnter += pictureBox_MouseEnter;
            pictureBox.MouseLeave += pictureBox_MouseLeave;
            pictureBox.MouseMove += pictureBox_MouseMove;
            pictureBox.MouseUp += pictureBox_MouseUp;
            // 
            // btRemove
            // 
            btRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btRemove.BackgroundImageLayout = ImageLayout.None;
            btRemove.Image = Properties.Resources.remove;
            btRemove.Location = new Point(219, 3);
            btRemove.Name = "btRemove";
            btRemove.Size = new Size(28, 28);
            btRemove.SizeMode = PictureBoxSizeMode.Zoom;
            btRemove.TabIndex = 3;
            btRemove.TabStop = false;
            btRemove.Visible = false;
            btRemove.Click += btRemove_Click;
            // 
            // SmartImagePreview
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btRemove);
            Controls.Add(pictureBox);
            Name = "SmartImagePreview";
            Size = new Size(250, 243);
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)btRemove).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox;
        private ImageButton btRemove;
    }
}
