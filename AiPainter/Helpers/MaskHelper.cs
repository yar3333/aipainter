using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiPainter.Helpers;

static class MaskHelper
{
    public static void DrawPrimitives(Point cen, Graphics g, Pen pen, Brush brush, List<Primitive> primitives)
    {
        foreach (var p in primitives)
        {
            switch (p.Kind)
            {
                case PrimitiveKind.Line:
                    if (p.Pt0 != p.Pt1)
                    {
                        pen.Width = p.PenSize;
                        g.DrawLine(pen, cen.X + p.Pt0.X, cen.Y + p.Pt0.Y, cen.X + p.Pt1.X, cen.Y + p.Pt1.Y);
                    }
                    else
                    {
                        g.FillEllipse
                        (
                            brush,
                            cen.X + p.Pt0.X - p.PenSize / 2,
                            cen.Y + p.Pt0.Y - p.PenSize / 2,
                            p.PenSize,
                            p.PenSize
                        );
                    }
                    break;
            }
        }
    }

    public static void DrawAlpha(Point cen, Bitmap bmp, List<Primitive> primitives)
    {
        var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

        foreach (var p in primitives)
        {
            switch (p.Kind)
            {
                case PrimitiveKind.Line:
                    var r = p.PenSize / 2;
                    double x = cen.X + p.Pt0.X;
                    double y = cen.Y + p.Pt0.Y;
                    double dx = p.Pt1.X - p.Pt0.X;
                    double dy = p.Pt1.Y - p.Pt0.Y;
                    var len = Math.Sqrt(dx * dx + dy * dy);
                    var steps = Math.Max(1, (int)len / 2);
                    dx /= steps;
                    dy /= steps;
                    for (var i = 0; i < steps; i++)
                    {
                        BitmapTools.DrawAlphaCirle(data, (int)x, (int)y, r);
                        x += dx;
                        y += dy;
                    }
                    break;
            }
        }

        bmp.UnlockBits(data);
    }
}