namespace AiPainter.Controls;

partial class FolderNameDialog
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
        label1 = new Label();
        btOk = new Button();
        cbFolderName = new ComboBox();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(36, 48);
        label1.Name = "label1";
        label1.Size = new Size(73, 15);
        label1.TabIndex = 0;
        label1.Text = "Folder name";
        // 
        // btOk
        // 
        btOk.DialogResult = DialogResult.OK;
        btOk.Location = new Point(181, 97);
        btOk.Name = "btOk";
        btOk.Size = new Size(147, 36);
        btOk.TabIndex = 2;
        btOk.Text = "OK";
        btOk.UseVisualStyleBackColor = true;
        btOk.Click += btOk_Click;
        // 
        // cbFolderName
        // 
        cbFolderName.FormattingEnabled = true;
        cbFolderName.Location = new Point(115, 45);
        cbFolderName.Name = "cbFolderName";
        cbFolderName.Size = new Size(329, 23);
        cbFolderName.TabIndex = 3;
        // 
        // FolderNameDialog
        // 
        AcceptButton = btOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(508, 154);
        Controls.Add(cbFolderName);
        Controls.Add(btOk);
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FolderNameDialog";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "NewFolderDialog";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private Button btOk;
    private ComboBox cbFolderName;
}