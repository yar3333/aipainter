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
            ACTIVE_BOX_MOVING,
            GLOBAL_MOVING,
        }

        private const int ACTIVE_BOX_EXTEND_SIZE = 16;
        
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

        private readonly HatchBrush whiteGrayCheckesBrush = new(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.White);
        private readonly Pen activeBoxPen = new(Color.Red, 3);

        private Mode mode = Mode.NOTHING;
        private Point movingStartPoint;

        public Bitmap? Image { get; set; }

        private int globalX;
        private int globalY;
        
        public Rectangle ActiveBox = new(0, 0, 512, 512);

        public bool HasMask => primitives.Any();

        private bool isCursorVisible = true;

        private Primitive[] oldPrimitives = {};

        private bool mouseInPictureBox;
        
        public SmartPictureBox()
        {
            InitializeComponent();

            MouseWheel += SmartPictureBox_MouseWheel;
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

                m.Translate(globalX, globalY);

                m.Translate( ClientSize.Width / 2,  ClientSize.Height / 2);
                m.Scale(zoom, zoom);
                m.Translate(-ClientSize.Width / 2, -ClientSize.Height / 2);
                m.Translate
                (
                    // ReSharper disable once PossibleLossOfFraction
                    (ClientSize.Width - ActiveBox.Width) / 2,
                    // ReSharper disable once PossibleLossOfFraction
                    (ClientSize.Height - ActiveBox.Height) / 2
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
            if (Image == null) return null;

            var bmp = BitmapTools.Clone(Image)!;
            MaskHelper.DrawAlpha(bmp, primitives, alpha);
            return bmp;
        }

        public Bitmap? GetMaskCropped(int x,int y, int w, int h, Color backColor, Color maskColor)
        {
            if (Image == null) return null;

            var bmp = new Bitmap(w, h, Image.PixelFormat);
            using var g = Graphics.FromImage(bmp);
            using var backBrush = new SolidBrush(backColor);
            using var maskBrush = new SolidBrush(maskColor);
            g.FillRectangle(backBrush, 0, 0, bmp.Width, bmp.Height);
            MaskHelper.DrawPrimitives(-x, -y, g, maskBrush, primitives);
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
            if (primitives.Any()) oldPrimitives = primitives.ToArray();
            primitives.Clear();
            redoPrimitiveBlocks.Clear();
            Invalidate();
        }

        public void ResetView()
        {
            ActiveBox.X = 0;
            ActiveBox.Y = 0;
            zoomIndex = Array.IndexOf(zoomLevels, 1.0f);
            Invalidate();
        }

        private void manageCursor(Point loc)
        {
            if (!mouseInPictureBox || !Enabled || Image == null)
            {
                if (!isCursorVisible) { Cursor.Show(); isCursorVisible = true; }
                if (cursorPt != null) { cursorPt = null; Refresh(); }
                return;
            }

            switch (mode)
            {
                case Mode.NOTHING:
                    if (isCursorVisible) { Cursor.Hide(); isCursorVisible = false; }
                    if (cursorPt != loc) { cursorPt = loc; Refresh(); }
                    break;

                case Mode.MASKING:
                    if (isCursorVisible) { Cursor.Hide(); isCursorVisible = false; }
                    if (cursorPt != null) { cursorPt = null; Refresh(); }
                    break;

                case Mode.ACTIVE_BOX_MOVING:
                case Mode.GLOBAL_MOVING:
                    if (!isCursorVisible) { Cursor.Show(); isCursorVisible = true; }
                    if (cursorPt != null) { cursorPt = null; Refresh(); }
                    break;
            }
        }

        private void SmartPictureBox_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (Image == null) return;

            switch (mode)
            {
                case Mode.NOTHING:
                    zoomIndex = Math.Clamp(zoomIndex + Math.Sign(e.Delta), 0, zoomLevels.Length - 1);
                    break;

                case Mode.ACTIVE_BOX_MOVING:
                    var newActiveBoxW = Math.Max(ACTIVE_BOX_EXTEND_SIZE, ActiveBox.Width + Math.Sign(e.Delta) * ACTIVE_BOX_EXTEND_SIZE);
                    var newActiveBoxH = Math.Max(ACTIVE_BOX_EXTEND_SIZE, ActiveBox.Height + Math.Sign(e.Delta) * ACTIVE_BOX_EXTEND_SIZE);

                    var dx = newActiveBoxW - ActiveBox.Width;
                    var dy = newActiveBoxH - ActiveBox.Height;

                    ActiveBox.Width += dx;
                    ActiveBox.Height += dy;

                    ActiveBox.X += dx >> 1;
                    ActiveBox.Y += dy >> 1;

                    movingStartPoint = getTransformedMousePos(e.Location);

                    break;
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
                    whiteGrayCheckesBrush, 
                    ActiveBox.X, 
                    ActiveBox.Y, 
                    Image.Width, 
                    Image.Height
                );

                e.Graphics.DrawImage
                (
                    Image, 
                    ActiveBox.X, 
                    ActiveBox.Y,
                    Image.Width,
                    Image.Height
                );
            }

            activeBoxPen.Width = 3 / zoom;
            e.Graphics.DrawRectangle
            (
                activeBoxPen,
                -2 / zoom, 
                -2 / zoom, 
                ActiveBox.Width + 3 / zoom, 
                ActiveBox.Height + 3 / zoom
            );

            MaskHelper.DrawPrimitives(ActiveBox.X, ActiveBox.Y, e.Graphics, primBrush, primitives);

            drawCursor(e.Graphics);
        }

        private void drawCursor(Graphics g)
        {
            if (cursorPt == null || Image == null) return;

            var loc = getTransformedMousePos(cursorPt.Value);

            var penSize = getActivePenSize();
            g.FillEllipse
            (
                cursorBrush,
                loc.X - penSize / 2,
                loc.Y - penSize / 2,
                penSize,
                penSize
            );
        }

        private void SmartPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = true;
            
            if (Image == null) return;

            if (e.Button == MouseButtons.Left) maskingMouseDown(e.Location);
            if (e.Button == MouseButtons.Right) activeBoxMovingMouseDown(e.Location);
            if (e.Button == MouseButtons.Middle) globalMovingMouseDown(e.Location);
        }

        private void SmartPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            manageCursor(e.Location);
            
            if (!Enabled || Image == null) return;

            switch (mode)
            {
                case Mode.MASKING:
                    maskingMouseMove(e.Location);
                    break;

                case Mode.ACTIVE_BOX_MOVING:
                    activeBoxMovingMouseMove(e.Location);
                    break;

                case Mode.GLOBAL_MOVING:
                    globalMovingMouseMove(e.Location);
                    break;
            }
        }

        private void SmartPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Capture = false;
            
            if (Image == null) return;

            mode = Mode.NOTHING;
            ActiveBox.X = (int)Math.Round(ActiveBox.X / 16.0) * 16;
            ActiveBox.Y = (int)Math.Round(ActiveBox.Y / 16.0) * 16;
            Refresh();
        }

        private void maskingMouseDown(Point loc)
        {
            if (!Enabled) return;

            var pt = getTransformedMousePos(loc);
            pt.X -= ActiveBox.X;
            pt.Y -= ActiveBox.Y;
            
            mode = Mode.MASKING;

            if (lastPrim != UNDO_DELIMITER) primitives.Add(UNDO_DELIMITER);

            primitives.Add
            (
                new Primitive
                {
                    Kind = PrimitiveKind.Line,
                    Pt0 = pt,
                    Pt1 = pt,
                    PenSize = getActivePenSize(),
                }
            );

            manageCursor(loc);
        }

        private void maskingMouseMove(Point loc)
        {
            if (!Enabled) return;

            var pt = getTransformedMousePos(loc);
            pt.X -= ActiveBox.X;
            pt.Y -= ActiveBox.Y;

            switch (lastPrim!.Kind)
            {
                case PrimitiveKind.Line:
                    lastPrim.Pt1 = pt;
                    primitives.Add
                    (
                        new Primitive
                        {
                            Kind = PrimitiveKind.Line,
                            Pt0 = pt,
                            Pt1 = pt,
                            PenSize = getActivePenSize(),
                        }
                    );
                    break;
            }

            Refresh();
        }

        private void activeBoxMovingMouseDown(Point loc)
        {
            mode = Mode.ACTIVE_BOX_MOVING;
            movingStartPoint = getTransformedMousePos(loc);
            manageCursor(loc);
        }

        private void activeBoxMovingMouseMove(Point loc)
        {
            loc = getTransformedMousePos(loc);

            ActiveBox.X += loc.X - movingStartPoint.X;
            ActiveBox.Y += loc.Y - movingStartPoint.Y;
            movingStartPoint = loc;
            Refresh();
        }

        private void globalMovingMouseDown(Point loc)
        {
            mode = Mode.GLOBAL_MOVING;
            movingStartPoint = loc;
            manageCursor(loc);
        }

        private void globalMovingMouseMove(Point loc)
        {
            globalX += loc.X - movingStartPoint.X;
            globalY += loc.Y - movingStartPoint.Y;
            movingStartPoint = loc;
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

        private void SmartPictureBox_MouseEnter(object sender, EventArgs e)
        {
            mouseInPictureBox = true;
        }

        private void SmartPictureBox_MouseLeave(object sender, EventArgs e)
        {
            mouseInPictureBox = false;
            manageCursor(Point.Empty);
        }
    }
}
