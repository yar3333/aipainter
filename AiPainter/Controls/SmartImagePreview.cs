namespace AiPainter.Controls
{
    public partial class SmartImagePreview : UserControl
    {
        public string FilePath { get; set; } = null!;

        public Image Image
        {
            get => pictureBox.Image;
            set => pictureBox.Image = value;
        }

        public Action? OnImageClick;
        public Action? OnImageRemove;
        
        public SmartImagePreview()
        {
            InitializeComponent();
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (btRemove.Bounds.Contains(e.Location))
            {
                OnImageRemove?.Invoke();
            }
            else
            {
                OnImageClick?.Invoke();
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            OnImageRemove?.Invoke();
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            btRemove.Visible = true;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            btRemove.Visible = false;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor = btRemove.Bounds.Contains(e.Location) ? Cursors.Arrow : Cursors.UpArrow;
        }
    }
}
