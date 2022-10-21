﻿using AiPainter.Helpers;
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
            RED_BOX_MOVING,
            GLOBAL_MOVING,
        }

        private const int RED_BOX_EXTEND_SIZE = 16;
        
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
        private readonly Pen redBoxPen = new(Color.Red, 3);

        private Mode mode = Mode.NOTHING;
        private Point movingStartPoint;

        public Bitmap? Image { get; set; }

        private int globalX;
        private int globalY;
        
        public int RedBoxX;
        public int RedBoxY;
        public int RedBoxW = 512;
        public int RedBoxH = 512;

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
                    (ClientSize.Width - RedBoxW) / 2,
                    // ReSharper disable once PossibleLossOfFraction
                    (ClientSize.Height - RedBoxH) / 2
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
            if (primitives.Any()) oldPrimitives = primitives.ToArray();
            primitives.Clear();
            redoPrimitiveBlocks.Clear();
            Invalidate();
        }

        public void ResetView()
        {
            RedBoxX = 0;
            RedBoxY = 0;
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

                case Mode.RED_BOX_MOVING:
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

                case Mode.RED_BOX_MOVING:
                    var newRedBoxW = Math.Max(RED_BOX_EXTEND_SIZE, RedBoxW + Math.Sign(e.Delta) * RED_BOX_EXTEND_SIZE);
                    var newRedBoxH = Math.Max(RED_BOX_EXTEND_SIZE, RedBoxH + Math.Sign(e.Delta) * RED_BOX_EXTEND_SIZE);

                    var dx = newRedBoxW - RedBoxW;
                    var dy = newRedBoxH - RedBoxH;

                    RedBoxW += dx;
                    RedBoxH += dy;

                    RedBoxX += dx >> 1;
                    RedBoxY += dy >> 1;

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
                    RedBoxX, 
                    RedBoxY, 
                    Image.Width, 
                    Image.Height
                );

                e.Graphics.DrawImage
                (
                    Image, 
                    RedBoxX, 
                    RedBoxY,
                    Image.Width,
                    Image.Height
                );
            }

            redBoxPen.Width = 3 / zoom;
            e.Graphics.DrawRectangle
            (
                redBoxPen,
                -2 / zoom, 
                -2 / zoom, 
                RedBoxW + 3 / zoom, 
                RedBoxH + 3 / zoom
            );

            MaskHelper.DrawPrimitives(RedBoxX, RedBoxY, e.Graphics, primBrush, primitives);

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
            if (e.Button == MouseButtons.Right) redBoxMovingMouseDown(e.Location);
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

                case Mode.RED_BOX_MOVING:
                    redBoxMovingMouseMove(e.Location);
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
            RedBoxX = (int)Math.Round(RedBoxX / 16.0) * 16;
            RedBoxY = (int)Math.Round(RedBoxY / 16.0) * 16;
            Refresh();
        }

        private void maskingMouseDown(Point loc)
        {
            if (!Enabled) return;

            var pt = getTransformedMousePos(loc);
            pt.X -= RedBoxX;
            pt.Y -= RedBoxY;
            
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
            pt.X -= RedBoxX;
            pt.Y -= RedBoxY;

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

        private void redBoxMovingMouseDown(Point loc)
        {
            mode = Mode.RED_BOX_MOVING;
            movingStartPoint = getTransformedMousePos(loc);
            manageCursor(loc);
        }

        private void redBoxMovingMouseMove(Point loc)
        {
            loc = getTransformedMousePos(loc);

            RedBoxX += loc.X - movingStartPoint.X;
            RedBoxY += loc.Y - movingStartPoint.Y;
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
