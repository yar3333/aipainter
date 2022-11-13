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
            this.lvModifiers = new System.Windows.Forms.ListView();
            this.lvSelected = new System.Windows.Forms.ListView();
            this.lbCategory = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvModifiers
            // 
            this.lvModifiers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvModifiers.Location = new System.Drawing.Point(158, 27);
            this.lvModifiers.MultiSelect = false;
            this.lvModifiers.Name = "lvModifiers";
            this.lvModifiers.Size = new System.Drawing.Size(548, 527);
            this.lvModifiers.TabIndex = 0;
            this.lvModifiers.UseCompatibleStateImageBehavior = false;
            this.lvModifiers.DoubleClick += new System.EventHandler(this.lvModifiers_DoubleClick);
            // 
            // lvSelected
            // 
            this.lvSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSelected.Location = new System.Drawing.Point(712, 27);
            this.lvSelected.MultiSelect = false;
            this.lvSelected.Name = "lvSelected";
            this.lvSelected.Size = new System.Drawing.Size(350, 527);
            this.lvSelected.TabIndex = 0;
            this.lvSelected.UseCompatibleStateImageBehavior = false;
            this.lvSelected.DoubleClick += new System.EventHandler(this.lvSelected_DoubleClick);
            // 
            // lbCategory
            // 
            this.lbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbCategory.FormattingEnabled = true;
            this.lbCategory.ItemHeight = 15;
            this.lbCategory.Location = new System.Drawing.Point(12, 27);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(140, 529);
            this.lbCategory.TabIndex = 1;
            this.lbCategory.SelectedIndexChanged += new System.EventHandler(this.lbCategory_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Category";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Modifiers";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(712, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Selected";
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(346, 563);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(172, 40);
            this.btOk.TabIndex = 3;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(562, 563);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(172, 40);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // SdModifiersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 613);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbCategory);
            this.Controls.Add(this.lvSelected);
            this.Controls.Add(this.lvModifiers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SdModifiersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "StableDiffusion modifiers";
            this.Load += new System.EventHandler(this.SdModifiersForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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