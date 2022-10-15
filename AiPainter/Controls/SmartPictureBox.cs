using System.Drawing.Drawing2D;
using System.Numerics;

namespace AiPainter.Controls
{
    public partial class SmartPictureBox : UserControl
    {
        private const int VIEWPORT_WIDTH  = 512;
        private const int VIEWPORT_HEIGHT = 512;

        public Bitmap? Image { get; set; }
        
        public int ViewDeltaX { get; set; }
        public int ViewDeltaY { get; set; }

        public Point ViewDelta => new(ViewDeltaX, ViewDeltaY);

        private readonly HatchBrush gridBrush = new(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.White);
        private readonly Pen borderPen = new(Color.Red, 3);

        public Matrix Transform
        {
            get
            {
                var m = new Matrix(Matrix3x2.Identity);
                m.Translate( ClientSize.Width / 2,  ClientSize.Height / 2);
                m.Scale(zoom, zoom);
                m.Translate(-ClientSize.Width / 2, -ClientSize.Height / 2);
                m.Translate
                (
                    // ReSharper disable once PossibleLossOfFraction
                    (ClientSize.Width - VIEWPORT_WIDTH) / 2,
                    // ReSharper disable once PossibleLossOfFraction
                    (ClientSize.Height - VIEWPORT_HEIGHT) / 2
                );
                return m;
            }
        }

        private float zoom = 1.0f;
        public float Zoom
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

            e.Graphics.Transform = Transform;

            e.Graphics.FillRectangle
            (
                gridBrush, 
                0, 
                0, 
                VIEWPORT_WIDTH, 
                VIEWPORT_HEIGHT
            );
            
            e.Graphics.FillRectangle
            (
                gridBrush, 
                ViewDeltaX, 
                ViewDeltaY, 
                Image.Width, 
                Image.Height
            );

            e.Graphics.DrawImage
            (
                Image, 
                ViewDeltaX, 
                ViewDeltaY,
                Image.Width,
                Image.Height
            );
            
            e.Graphics.DrawRectangle
            (
                borderPen,
                -2, 
                -2, 
                VIEWPORT_WIDTH  + 3, 
                VIEWPORT_HEIGHT + 3
            );
        }
    }
}
