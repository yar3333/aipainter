using AiPainter.Helpers;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace AiPainter.Controls
{
    public partial class SmartPictureBox : UserControl
    {
        private const int VIEWPORT_WIDTH  = 512;
        private const int VIEWPORT_HEIGHT = 512;
        
        private const int PEN_SIZE = 48;
        
        private static readonly float[] zoomLevels = { 1.0f/16, 1.0f/8, 1.0f/4, 1.0f/2, 1, 2, 4, 8, 16 };
        
        private static readonly Primitive UNDO_DELIMITER = new() { Kind = PrimitiveKind.UndoDelimiter };

        private List<Primitive> primitives = new();
        private readonly List<Primitive[]> redoPrimitiveBlocks = new();

        private Primitive? lastPrim => primitives.LastOrDefault();

        private static readonly HatchBrush primBrush = new(HatchStyle.Percent75, Color.Red, Color.Transparent);
        private static readonly Pen primPen = PenTools.CreateRoundPen(primBrush);
        
        private static readonly HatchBrush cursorBrush = new(HatchStyle.Percent75, Color.LightCoral, Color.Transparent);

        private Point? cursorPt;
        
        private int zoomIndex = 4;
        private float zoom => zoomLevels[zoomIndex];

        private readonly HatchBrush gridBrush = new(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.White);
        private readonly Pen borderPen = new(Color.Red, 3);

        private bool modeMaskDrawing;
        private bool modeViewportMoving;
        private Point viewportMovingStart;

        public Bitmap? Image { get; set; }

        public int ViewDeltaX;
        public int ViewDeltaY;

        public bool HasMask => primitives.Any();

        public SmartPictureBox()
        {
            InitializeComponent();

            MouseWheel += MainForm_MouseWheel;
        }
        
        private int getActivePenSize()
        {
            return (int)Math.Round(PEN_SIZE / zoom);
        }

        private Matrix Transform
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

        public Bitmap? GetImageWithMaskToTransparent()
        {
            return getImageWithMaskToTransparent(false, out _);
        }
        
        public Bitmap? GetImageWithMaskToTransparentCroppedToViewport(out bool wasCropped)
        {
            return getImageWithMaskToTransparent(true, out wasCropped);
        }

        private Bitmap? getImageWithMaskToTransparent(bool cropToViewport, out bool wasCropped)
        {
            wasCropped = false;
            
            if (Image == null) return null;

            var bmp = BitmapTools.Clone(Image)!;

            var cenX = 0;//VIEWPORT_WIDTH  / 2 - ViewDeltaX;
            var cenY = 0;//VIEWPORT_HEIGHT / 2 - ViewDeltaY;

            MaskHelper.DrawAlpha(cenX, cenY, bmp, primitives);

            if (!cropToViewport || (Width <= VIEWPORT_WIDTH && Height <= VIEWPORT_HEIGHT)) return bmp;
            
            wasCropped = true;
            return BitmapTools.GetCropped
            (
                bmp, 
                -ViewDeltaX, 
                -ViewDeltaY,
                VIEWPORT_WIDTH, 
                VIEWPORT_HEIGHT
            );
        }

        public void Undo()
        {
            if (!primitives.Any()) return;
                
            var i = primitives.FindLastIndex(x => x == UNDO_DELIMITER);
            redoPrimitiveBlocks.Add(primitives.GetRange(i, primitives.Count - i).ToArray());
            primitives = primitives.GetRange(0, i);
            
            Refresh();
        }

        public void Redo()
        {
            if (!redoPrimitiveBlocks.Any()) return;

            var redoBlock = redoPrimitiveBlocks.Last();
            redoPrimitiveBlocks.RemoveAt(redoPrimitiveBlocks.Count - 1);
            primitives.AddRange(redoBlock);

            Refresh();
        }

        public void ResetMask()
        {
            primitives.Clear();
            redoPrimitiveBlocks.Clear();
            Invalidate();
        }

        public void ResetView()
        {
            ViewDeltaX = 0;
            ViewDeltaY = 0;
            zoomIndex = Array.IndexOf(zoomLevels, 1.0f);
            Invalidate();
        }

        private void MainForm_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (zoomIndex > 0) zoomIndex--;
            }
            else
            {
                if (zoomIndex < zoomLevels.Length - 1) zoomIndex++;
            }
            Refresh();
        }

        private void SmartPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Transform = Transform;

            if (Image != null)
            {
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
            }
            
            e.Graphics.DrawRectangle
            (
                borderPen,
                -2, 
                -2, 
                VIEWPORT_WIDTH  + 3, 
                VIEWPORT_HEIGHT + 3
            );

            MaskHelper.DrawPrimitives(ViewDeltaX, ViewDeltaY, e.Graphics, primPen, primBrush, primitives);

            drawCursor(e.Graphics);
        }

        private void drawCursor(Graphics g)
        {
            if (cursorPt == null) return;

            var penSize = getActivePenSize();
            g.FillEllipse
            (
                cursorBrush,
                cursorPt.Value.X - penSize / 2,
                cursorPt.Value.Y - penSize / 2,
                penSize,
                penSize
            );
        }

        private void SmartPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var loc = getTransformedMousePos(e.Location);

            if (e.Button == MouseButtons.Left) mouseLeftDown(loc);
            if (e.Button == MouseButtons.Right) mouseRightDown(loc);
        }

        private void mouseLeftDown(Point loc)
        {
            loc = new Point(loc.X - ViewDeltaX, loc.Y - ViewDeltaY);
            
            modeMaskDrawing = true;
            Capture = true;

            if (lastPrim != UNDO_DELIMITER) primitives.Add(UNDO_DELIMITER);

            primitives.Add
            (
                new Primitive
                {
                    Kind = PrimitiveKind.Line,
                    Pt0 = loc,
                    Pt1 = loc,
                    PenSize = getActivePenSize(),
                }
            );

            Refresh();
        }

        private void mouseRightDown(Point loc)
        {
            modeViewportMoving = true;
            viewportMovingStart = loc;
        }

        private void SmartPictureBox_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void SmartPictureBox_MouseLeave(object sender, EventArgs e)
        {
            cursorPt = null;
            Refresh();
            Cursor.Show();
        }

        private void SmartPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            var loc = getTransformedMousePos(e.Location);

            cursorPt = !modeMaskDrawing && !modeViewportMoving ? loc : null;

            if (modeMaskDrawing) mouseLeftMove(loc);
            if (modeViewportMoving) mouseRightMove(loc);

            Refresh();
        }

        private void SmartPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Capture = false;
            modeViewportMoving = false;
            modeMaskDrawing = false;
            Refresh();
        }

        private void mouseLeftMove(Point loc)
        {
            loc = new Point(loc.X - ViewDeltaX, loc.Y - ViewDeltaY);

            switch (lastPrim.Kind)
            {
                case PrimitiveKind.Line:
                    lastPrim.Pt1 = loc;
                    primitives.Add
                    (
                        new Primitive
                        {
                            Kind = PrimitiveKind.Line,
                            Pt0 = loc,
                            Pt1 = loc,
                            PenSize = getActivePenSize(),
                        }
                    );
                    break;
            }
        }

        private void mouseRightMove(Point loc)
        {
            ViewDeltaX += loc.X - viewportMovingStart.X;
            ViewDeltaY += loc.Y - viewportMovingStart.Y;
            viewportMovingStart = loc;
            Refresh();
        }

        private Point getTransformedMousePos(Point point)
        {
            var transform = Transform;
            transform.Invert();
            
            var points = new[] { point };
            transform.TransformPoints(points);
            return points[0];
        }
    }
}
