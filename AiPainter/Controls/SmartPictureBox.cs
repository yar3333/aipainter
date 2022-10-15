using System.Drawing.Drawing2D;

namespace AiPainter.Controls
{
    public partial class SmartPictureBox : UserControl
    {
        private const int VIEWPORT_WIDTH  = 512;
        private const int VIEWPORT_HEIGHT = 512;

        public Bitmap? Image { get; set; }
        
        public int ViewDeltaX { get; set; }
        public int ViewDeltaY { get; set; }

        private readonly HatchBrush gridBrush = new(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.White);
        private readonly Pen borderPen = new(Color.Red, 3);

        private double zoom = 1.0;
        public double Zoom
        {
            get => zoom;
            set
            {
                zoom = value;
                Invalidate();
            }
        }

        public SmartPictureBox()
        {
            InitializeComponent();
        }

        private void SmartPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (Image == null) return;

            var cen = new Point(ClientSize / 2);

            var vX = cen.X - zoomed(VIEWPORT_WIDTH)  / 2;
            var vY = cen.Y - zoomed(VIEWPORT_HEIGHT) / 2;
            
            e.Graphics.FillRectangle
            (
                gridBrush, 
                vX, 
                vY, 
                zoomed(VIEWPORT_WIDTH), 
                zoomed(VIEWPORT_HEIGHT)
            );
            
            e.Graphics.FillRectangle
            (
                gridBrush, 
                vX + zoomed(ViewDeltaX), 
                vY + zoomed(ViewDeltaY), 
                zoomed(Image.Width), 
                zoomed(Image.Height)
            );

            e.Graphics.DrawImage
            (
                Image, 
                vX + zoomed(ViewDeltaX), 
                vY + zoomed(ViewDeltaY),
                zoomed(Image.Width),
                zoomed(Image.Height)
            );
            
            e.Graphics.DrawRectangle
            (
                borderPen,
                vX - 2, 
                vY - 2, 
                zoomed(VIEWPORT_WIDTH)  + 3, 
                zoomed(VIEWPORT_HEIGHT) + 3
            );
        }

        private int zoomed(int a) => (int)Math.Round(a  * zoom);
    }
}
