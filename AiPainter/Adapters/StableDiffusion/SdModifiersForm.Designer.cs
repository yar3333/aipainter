namespace AiPainter.Adapters.StableDiffusion
{
    partial class SdModifiersForm
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
            lvModifiers = new ListView();
            lvSelected = new ListView();
            lbCategory = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btOk = new Button();
            btCancel = new Button();
            SuspendLayout();
            // 
            // lvModifiers
            // 
            lvModifiers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvModifiers.Location = new Point(158, 27);
            lvModifiers.MultiSelect = false;
            lvModifiers.Name = "lvModifiers";
            lvModifiers.Size = new Size(548, 527);
            lvModifiers.TabIndex = 0;
            lvModifiers.UseCompatibleStateImageBehavior = false;
            lvModifiers.DoubleClick += lvModifiers_DoubleClick;
            // 
            // lvSelected
            // 
            lvSelected.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lvSelected.Location = new Point(712, 27);
            lvSelected.MultiSelect = false;
            lvSelected.Name = "lvSelected";
            lvSelected.Size = new Size(350, 527);
            lvSelected.TabIndex = 0;
            lvSelected.UseCompatibleStateImageBehavior = false;
            lvSelected.DoubleClick += lvSelected_DoubleClick;
            // 
            // lbCategory
            // 
            lbCategory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lbCategory.FormattingEnabled = true;
            lbCategory.ItemHeight = 15;
            lbCategory.Location = new Point(12, 27);
            lbCategory.Name = "lbCategory";
            lbCategory.Size = new Size(140, 529);
            lbCategory.TabIndex = 1;
            lbCategory.SelectedIndexChanged += lbCategory_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 2;
            label1.Text = "Category";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(158, 9);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 2;
            label2.Text = "Modifiers";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(712, 9);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 2;
            label3.Text = "Selected";
            // 
            // btOk
            // 
            btOk.Anchor = AnchorStyles.Bottom;
            btOk.DialogResult = DialogResult.OK;
            btOk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btOk.Location = new Point(346, 563);
            btOk.Name = "btOk";
            btOk.Size = new Size(172, 40);
            btOk.TabIndex = 3;
            btOk.Text = "OK";
            btOk.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            btCancel.Anchor = AnchorStyles.Bottom;
            btCancel.DialogResult = DialogResult.Cancel;
            btCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btCancel.Location = new Point(562, 563);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(172, 40);
            btCancel.TabIndex = 3;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            // 
            // SdModifiersForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 613);
            Controls.Add(btCancel);
            Controls.Add(btOk);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lbCategory);
            Controls.Add(lvSelected);
            Controls.Add(lvModifiers);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "SdModifiersForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "StableDiffusion modifiers";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lvModifiers;
        private ListView lvSelected;
        private ListBox lbCategory;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btOk;
        private Button btCancel;
    }
}