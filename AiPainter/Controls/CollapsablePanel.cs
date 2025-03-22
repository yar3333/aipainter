// ReSharper disable LocalizableElement

using System.ComponentModel;
using System.Windows.Forms.Design;

namespace AiPainter.Controls
{
    [Designer(typeof(ParentControlDesigner))]
    public partial class CollapsablePanel : UserControl
    {
        private int height;

        private bool isCollapsed;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCollapsed
        {
            get => isCollapsed;
            set
            {
                if (value == isCollapsed)
                    return;
                isCollapsed = value;

                if (isCollapsed)
                {
                    height = Height;
                    Height = Margin.Top + btHeader.Height + Margin.Bottom;
                }
                else
                {
                    Height = height;
                }

                Caption = caption;
            }
        }

        private string caption = "Caption";

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Caption
        {
            get => caption;
            set
            {
                caption = value;

                if (isCollapsed)
                {
                    btHeader.Text = "> " + Caption;
                }
                else
                {
                    btHeader.Text = "v " + Caption;
                }
            }
        }

        public CollapsablePanel()
        {
            InitializeComponent();
        }

        private void btHeader_Click(object sender, EventArgs e)
        {
            IsCollapsed = !IsCollapsed;
        }
    }
}
