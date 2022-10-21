using AiPainter.Helpers;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace AiPainter.Controls
{
    public partial class SmartPictureBox : UserControl
    {
        enum Mode
        {
            NOTHING,
            MASKING,
            RED_BOX_MOVING
        }

        private const int VIEWPORT_WIDTH  = 512;
        private const int VIEWPORT_HEIGHT = 512;
        
        private const int PEN_SIZE = 48;
        
        private static readonly float[] zoomLevels = { 1.0f/16, 1.0f/8, 1.0f/4, 1.0f/2, 1, 2, 4, 8, 16 };
        
        private static readonly Primitive UNDO_DELIMITER = new() { Kind = PrimitiveKind.UndoDelimiter };

        private List<Primitive> primitives = new();
        private readonly List<Primitive[]> redoPrimitiveBlocks = new();

        private Primitive? lastPrim => primitives.LastOrDefault();

        private static readonly HatchBrush primBrush = new(HatchStyle.Percent50, Color.Red, Color.Transparent);
        
        private static readonly HatchBrush cursorBrush = new(HatchStyle.Percent75, Color.LightCoral, Color.Transparent);

        private Point? cursorPt;
        
        private int zoomIndex = 4;
        private float zoom => zoomLevels[zoomIndex];

        private readonly HatchBrush gridBrush = new(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.White);
        private readonly Pen borderPen = new(Color.Red, 3);

        private Mode mode = Mode.NOTHING;
        private Point viewportMovingStart;

        public Bitmap? Image { get; set; }
        
        public int RedBoxDx;
        public int RedBoxDy;

        public bool HasMask => primitives.Any();

        private bool isCursorVisible = true;

        private Primitive[] oldPrimitives = {};
        
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

        public void ShiftMask(int dx, int dy)
        {
            foreach (var p in primitives)
            {
                p.Pt0 = new Point(p.Pt0.X + dx, p.Pt0.Y + dy);
                p.Pt1 = new Point(p.Pt1.X + dx, p.Pt1.Y + dy);
            }
        }

        public void AddBoxToMask(int x, int y, int width, int height)
        {
            if (lastPrim != UNDO_DELIMITER) primitives.Add(UNDO_DELIMITER);
            primitives.Add(new Primitive
            {
                Kind = PrimitiveKind.Box,
                Pt0 = new Point(x, y),
                Pt1 = new Point(x + width, y + height)
            });
        }

        public Bitmap? GetMaskedImage(byte alpha)
        {
            return getMaskedImage(alpha, false, out _);
        }
        
        public Bitmap? GetMaskedImageCroppedToViewport(byte alpha, out bool wasCropped)
        {
            return getMaskedImage(alpha, true, out wasCropped);
        }

        private Bitmap? getMaskedImage(byte alpha, bool cropToViewport, out bool wasCropped)
        {
            wasCropped = false;
            
            if (Image == null) return null;

            var bmp = BitmapTools.Clone(Image)!;

            MaskHelper.DrawAlpha(bmp, primitives, alpha);

            if (!cropToViewport || (Width <= VIEWPORT_WIDTH && Height <= VIEWPORT_HEIGHT)) return bmp;
            
            wasCropped = true;
            return BitmapTools.GetCropped
            (
                bmp, 
                -Math.Min(0, RedBoxDx), 
                -Math.Min(0, RedBoxDy),
                VIEWPORT_WIDTH,
                VIEWPORT_HEIGHT,
                Color.Transparent
            );
        }

        public Bitmap? GetMaskAsWhiteOnBlack()
        {
            if (Image == null) return null;

            var bmp = new Bitmap(Image.Width, Image.Height, Image.PixelFormat);
            using var g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
            MaskHelper.DrawPrimitives(0, 0, g, Brushes.White, primitives);
            return bmp;
        }

        public void RestorePreviousMask()
        {
            var t = primitives;
            primitives = oldPrimitives.ToList();
            oldPrimitives = t.ToArray();
            Refresh();
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
            oldPrimitives = primitives.ToArray();
            primitives.Clear();
            redoPrimitiveBlocks.Clear();
            Invalidate();
        }

        public void ResetView()
        {
            RedBoxDx = 0;
            RedBoxDy = 0;
            zoomIndex = Array.IndexOf(zoomLevels, 1.0f);
            Invalidate();
        }

        private void SmartPictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (Enabled && Image != null && cursorPt == null && isCursorVisible)
            {
                Cursor.Hide();
                isCursorVisible = false;
            }
        }

        private void SmartPictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (Enabled && Image != null && cursorPt != null && !isCursorVisible)
            {
                Cursor.Show();
                isCursorVisible = true;
            }
            cursorPt = null;
            Refresh();
        }

        private void MainForm_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (Image == null) return;

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
                    RedBoxDx, 
                    RedBoxDy, 
                    Image.Width, 
                    Image.Height
                );

                e.Graphics.DrawImage
                (
                    Image, 
                    RedBoxDx, 
                    RedBoxDy,
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

            MaskHelper.DrawPrimitives(RedBoxDx, RedBoxDy, e.Graphics, primBrush, primitives);

            drawCursor(e.Graphics);
        }

        private void drawCursor(Graphics g)
        {
            if (cursorPt == null || Image == null) return;

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
            if (Image == null) return;

            var loc = getTransformedMousePos(e.Location);

            if (e.Button == MouseButtons.Left) maskingMouseDown(loc);
            if (e.Button == MouseButtons.Right) redBoxMovingMouseDown(loc);
        }

        private void SmartPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Image == null) return;

            var loc = getTransformedMousePos(e.Location);

            cursorPt = mode == Mode.NOTHING ? loc : null;

            switch (mode)
            {
                case Mode.MASKING:
                    maskingMouseMove(loc);
                    break;

                case Mode.RED_BOX_MOVING:
                    redBoxMovingMouseMove(loc);
                    break;
            }
        }

        private void SmartPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (Image == null) return;

            Capture = false;
            mode = Mode.NOTHING;
            RedBoxDx = (int)Math.Round(RedBoxDx / 16.0) * 16;
            RedBoxDy = (int)Math.Round(RedBoxDy / 16.0) * 16;
            Refresh();
        }

        private void maskingMouseDown(Point loc)
        {
            if (!Enabled) return;

            loc = new Point(loc.X - RedBoxDx, loc.Y - RedBoxDy);
            
            mode = Mode.MASKING;
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

        private void maskingMouseMove(Point loc)
        {
            if (!Enabled) return;

            loc = new Point(loc.X - RedBoxDx, loc.Y - RedBoxDy);

            switch (lastPrim!.Kind)
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

            Refresh();
        }

        private void redBoxMovingMouseDown(Point loc)
        {
            mode = Mode.RED_BOX_MOVING;
            viewportMovingStart = loc;
        }

        private void redBoxMovingMouseMove(Point loc)
        {
            RedBoxDx += loc.X - viewportMovingStart.X;
            RedBoxDy += loc.Y - viewportMovingStart.Y;
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
