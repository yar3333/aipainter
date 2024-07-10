using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion
{
    partial class SdDownloadingListItem
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
            tbText = new TextBox();
            pbProgress = new CustomProgressBar();
            btRemove = new Button();
            toolTip = new ToolTip(components);
            SuspendLayout();
            // 
            // tbText
            // 
            tbText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbText.BorderStyle = BorderStyle.FixedSingle;
            tbText.Location = new Point(3, 7);
            tbText.Name = "tbText";
            tbText.ReadOnly = true;
            tbText.Size = new Size(311, 23);
            tbText.TabIndex = 0;
            // 
            // pbProgress
            // 
            pbProgress.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbProgress.BackColor = Color.AliceBlue;
            pbProgress.CustomText = null;
            pbProgress.Location = new Point(320, 7);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(260, 23);
            pbProgress.TabIndex = 2;
            pbProgress.TextColor = Color.Black;
            toolTip.SetToolTip(pbProgress, "Iterations");
            // 
            // btRemove
            // 
            btRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btRemove.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btRemove.ForeColor = Color.Red;
            btRemove.Location = new Point(586, 7);
            btRemove.Name = "btRemove";
            btRemove.Size = new Size(31, 23);
            btRemove.TabIndex = 4;
            btRemove.Text = "X";
            toolTip.SetToolTip(btRemove, "Cancel");
            btRemove.UseVisualStyleBackColor = true;
            btRemove.Click += btRemove_Click;
            // 
            // SdDownloadingListItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(btRemove);
            Controls.Add(pbProgress);
            Controls.Add(tbText);
            Name = "SdDownloadingListItem";
            Size = new Size(620, 42);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbText;
        private CustomProgressBar pbProgress;
        private Button btRemove;
        private ToolTip toolTip;
    }
}
