namespace AiPainter
{
    partial class AboutDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btOk = new Button();
            rtbText = new RichTextBox();
            SuspendLayout();
            // 
            // btOk
            // 
            btOk.Anchor = AnchorStyles.Bottom;
            btOk.Location = new Point(218, 360);
            btOk.Name = "btOk";
            btOk.Size = new Size(260, 45);
            btOk.TabIndex = 0;
            btOk.Text = "OK";
            btOk.UseVisualStyleBackColor = true;
            btOk.Click += btOk_Click;
            // 
            // rtbText
            // 
            rtbText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbText.BackColor = SystemColors.Control;
            rtbText.BorderStyle = BorderStyle.None;
            rtbText.Location = new Point(12, 12);
            rtbText.Name = "rtbText";
            rtbText.Size = new Size(663, 324);
            rtbText.TabIndex = 1;
            rtbText.Text = "About text";
            rtbText.LinkClicked += rtbText_LinkClicked;
            // 
            // AboutDialog
            // 
            AcceptButton = btOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(687, 427);
            Controls.Add(rtbText);
            Controls.Add(btOk);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About application";
            ResumeLayout(false);
        }

        #endregion

        private Button btOk;
        private RichTextBox rtbText;
    }
}