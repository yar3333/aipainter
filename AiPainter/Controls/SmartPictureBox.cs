using AiPainter.Helpers;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
        
        private static readonly Primitive UNDO_DELIMITER = new() { Kind = PrimitiveKind.UndoDelimiter };

        private List<Primitive> primitives = new();
        private readonly List<Primitive[]> redoPrimitiveBlocks = new();

        private Primitive? lastPrim => primitives.LastOrDefault();

        private static readonly HatchBrush primBrush = new(HatchStyle.Percent50, Color.Red, Color.Transparent);
        
        private static readonly HatchBrush cursorBrush = new(HatchStyle.Percent75, Color.LightCoral, Color.Transparent);

        private Point? cursorPt;
        
        private decimal zoom = 1;

        private readonly HatchBrush whiteGrayCheckesBrush = new(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.White);
        private readonly Pen activeBoxPen = new(Color.Red, 3);

        private Mode mode = Mode.NOTHING;
        private Point movingStartPoint;

        public Bitmap? Image { get; set; }

        private int globalX;
        private int globalY;

        public Rectangle ActiveBox = new(0, 0, 512, 512);

        public bool HasMask => primitives.Any();
        public bool HasPrevMask => oldPrimitives.Any();

        private bool isCursorVisible = true;

        private Primitive[] oldPrimitives = {};

        private bool mouseInPictureBox;
        private bool ctrlPressed;
        
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

                m.Scale((float)zoom, (float)zoom, MatrixOrder.Append);

                m.Translate
                (
                    // ReSharper disable once PossibleLossOfFraction
                    (ClientSize.Width - ActiveBox.Width) / 2,
                    // ReSharper disable once PossibleLossOfFraction
                    (ClientSize.Height - ActiveBox.Height) / 2,
                    MatrixOrder.Append
                );

                m.Translate(globalX, globalY, MatrixOrder.Append);
                
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

        public Bitmap GetMaskedImageCropped(Color back, byte alpha)
        {
            var bmp = BitmapTools.GetCropped(Image!, ActiveBox, back);
            MaskHelper.DrawAlpha(-ActiveBox.X, -ActiveBox.Y, bmp, primitives, alpha);
            return bmp;
        }

        public Bitmap? GetMaskCropped(Color backColor, Color maskColor)
        {
            if (!primitives.Any()) return null;

            var bmp = new Bitmap(ActiveBox.Width, ActiveBox.Height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            using var backBrush = new SolidBrush(backColor);
            using var maskBrush = new SolidBrush(maskColor);
            g.FillRectangle(backBrush, 0, 0, bmp.Width, bmp.Height);
            MaskHelper.DrawPrimitives(-ActiveBox.X, -ActiveBox.Y, g, maskBrush, primitives);
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
            zoom = 1;
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

            switch (ctrlPressed)
            {
                case false:
                {
                    var center = new Point(ClientSize / 2);
                    var oldCenter = getTransformedMousePos(center);
                    zoom = e.Delta > 0 ? zoom * 1.1m : zoom / 1.1m;
                    var newCenter = getTransformedMousePos(center);
                    globalX += (int)Math.Round((newCenter.X - oldCenter.X) * zoom);
                    globalY += (int)Math.Round((newCenter.Y - oldCenter.Y) * zoom);
                    break;
                }

                case true:
                {
                    var center = new Point(ClientSize / 2);
                    var oldCenter = getTransformedMousePos(center);

                    var inc = (int)Math.Round(ACTIVE_BOX_EXTEND_SIZE * Math.Sign(e.Delta) / zoom);

                    var newActiveBoxW = Math.Max(ACTIVE_BOX_EXTEND_SIZE, ActiveBox.Width  + inc);
                    var newActiveBoxH = Math.Max(ACTIVE_BOX_EXTEND_SIZE, ActiveBox.Height + inc);

                    var dx = newActiveBoxW - ActiveBox.Width;
                    var dy = newActiveBoxH - ActiveBox.Height;

                    ActiveBox.Width  += dx;
                    ActiveBox.Height += dy;

                    ActiveBox.X -= dx >> 1;
                    ActiveBox.Y -= dy >> 1;

                    var newCenter = getTransformedMousePos(center);

                    globalX += (int)Math.Round((newCenter.X - oldCenter.X) * zoom);
                    globalY += (int)Math.Round((newCenter.Y - oldCenter.Y) * zoom);

                    movingStartPoint = getTransformedMousePos(e.Location);

                    break;
                }
            }

            Refresh();
        }

        private void SmartPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Transform = Transform;

            if (Image != null)
            {
                e.Graphics.FillRectangle(whiteGrayCheckesBrush, 0, 0, Image.Width, Image.Height);
                e.Graphics.DrawImage(Image, 0, 0);
            }

            activeBoxPen.Width = 3 / (float)zoom;
            e.Graphics.DrawRectangle
            (
                activeBoxPen,
                ActiveBox.X - 2 / (float)zoom, 
                ActiveBox.Y - 2 / (float)zoom, 
                ActiveBox.Width + 3 / (float)zoom, 
                ActiveBox.Height + 3 / (float)zoom
            );

            MaskHelper.DrawPrimitives(0, 0, e.Graphics, primBrush, primitives);

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
            if (!Enabled) return;
            
            Capture = true;

            if (e.Button == MouseButtons.Left && !ctrlPressed) maskingMouseDown(e.Location);
            if (e.Button == MouseButtons.Left &&  ctrlPressed) activeBoxMovingMouseDown(e.Location);
            if (e.Button == MouseButtons.Right) globalMovingMouseDown(e.Location);
        }

        private void SmartPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            manageCursor(e.Location);
            
            if (!Enabled) return;

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

            if (mode == Mode.ACTIVE_BOX_MOVING)
            {
                if (Math.Abs(ActiveBox.X) < 10)
                {
                    moveActiveBoxOnFreezedImage(-ActiveBox.X, 0);
                }
                if (Math.Abs(ActiveBox.Y) < 10)
                {
                    moveActiveBoxOnFreezedImage(0, -ActiveBox.Y);
                }
                if (Math.Abs(Image.Width - (ActiveBox.X + ActiveBox.Width)) < 10)
                {
                    moveActiveBoxOnFreezedImage(Image.Width - (ActiveBox.X + ActiveBox.Width), 0);
                }
                if (Math.Abs(Image.Height - (ActiveBox.Y + ActiveBox.Height)) < 10)
                {
                    moveActiveBoxOnFreezedImage(0, Image.Height - (ActiveBox.Y + ActiveBox.Height));
                }
            }

            mode = Mode.NOTHING;
            
            Refresh();
        }

        private void moveActiveBoxOnFreezedImage(int dx, int dy)
        {
            globalX -= (int)Math.Round(dx * zoom);
            ActiveBox.X += dx;

            globalY -= (int)Math.Round(dy * zoom);
            ActiveBox.Y += dy;
        }

        private void maskingMouseDown(Point loc)
        {
            if (Image == null) return;

            var pt = getTransformedMousePos(loc);
            
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
            if (Image == null) return;

            var pt = getTransformedMousePos(loc);

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
            if (Image == null) return;
            
            mode = Mode.ACTIVE_BOX_MOVING;
            movingStartPoint = getTransformedMousePos(loc);
            manageCursor(loc);
        }

        private void activeBoxMovingMouseMove(Point loc)
        {
            if (Image == null) return;

            var tranLoc = getTransformedMousePos(loc);

            ActiveBox.X += tranLoc.X - movingStartPoint.X;
            ActiveBox.Y += tranLoc.Y - movingStartPoint.Y;
            
            movingStartPoint = getTransformedMousePos(loc);
            
            Refresh();
        }

        private void globalMovingMouseDown(Point loc)
        {
            if (Image == null) return;
            
            mode = Mode.GLOBAL_MOVING;
            movingStartPoint = loc;
            manageCursor(loc);
        }

        private void globalMovingMouseMove(Point loc)
        {
            if (Image == null) return;

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
        
        private Point getTransformedPoint(Point point)
        {
            var points = new[] { point };
            Transform.TransformPoints(points);
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

        private void SmartPictureBox_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        public void ProcessKeys(Message msg)
        {
            if (msg.Msg == 0x100 && msg.WParam == (IntPtr)0x11) // WM_KEYDOWN + Ctrl
            {
                ctrlPressed = true;
            }
            if (msg.Msg == 0x101 && msg.WParam == (IntPtr)0x11) // WM_KEYUP + Ctrl
            {
                ctrlPressed = false;
            }
        }

        public Primitive[] SaveMask()
        {
            return primitives.ToArray();
        }

        public void LoadMask(Primitive[] data)
        {
            oldPrimitives = primitives.ToArray();
            redoPrimitiveBlocks.Clear();
            primitives = data.ToList();
        }
    }
}
