using System.Drawing.Drawing2D;

namespace AiPainter
{
    public partial class SmartPictureBox : UserControl
    {
        private const int VIEWPORT_WIDTH = 512;
        private const int VIEWPORT_HEIGHT = 512;

        public Bitmap? Image { get; set; }
        
        public int ViewportDeltaX { get; set; }
        public int ViewportDeltaY { get; set; }

        private readonly HatchBrush gridBrush;
        private readonly Pen borderPen;
        
        public SmartPictureBox()
        {
            InitializeComponent();

            // ReSharper disable once VirtualMemberCallInConstructor
            DoubleBuffered = true;
            
            gridBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.White);
            borderPen = new Pen(Color.Red, 3);
        }

        private void SmartPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (Image == null) return;

            var cen = new Point(ClientSize / 2);

            var vX = cen.X - VIEWPORT_WIDTH  / 2;
            var vY = cen.Y - VIEWPORT_HEIGHT / 2;

            e.Graphics.FillRectangle(gridBrush, vX, vY, VIEWPORT_WIDTH, VIEWPORT_HEIGHT);
            e.Graphics.FillRectangle(gridBrush, vX + ViewportDeltaX, vY + ViewportDeltaY, Image.Width, Image.Height);

            e.Graphics.DrawImage(Image, vX + ViewportDeltaX, vY + ViewportDeltaY);
            
            e.Graphics.DrawRectangle
            (
                borderPen,
                vX - 2, 
                vY - 2, 
                VIEWPORT_WIDTH  + 3, 
                VIEWPORT_HEIGHT + 3
            );
        }
    }
}
