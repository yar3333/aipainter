using System.Drawing.Imaging;
using System.Xml.Linq;

namespace AiPainter.Helpers;

public static class BitmapTools
{
    public static Bitmap? Load(string filePath)
    {
        using var src = (Bitmap)Image.FromFile(filePath);
        return Clone(src);
    }

    public static Bitmap? Clone(Bitmap? src)
    {
        if (src == null) return null;

        var dst = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);

        var srcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var dstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        unsafe
        {
            var pSrc = (byte*)srcData.Scan0.ToPointer();
            var pDst = (byte*)dstData.Scan0.ToPointer();
            var pSrcEnd = pSrc + srcData.Height * srcData.Stride;
            while (pSrc < pSrcEnd)
            {
                *pDst = *pSrc;
                pSrc++;
                pDst++;
            }
        }

        dst.UnlockBits(dstData);
        src.UnlockBits(srcData);

        return dst;
    }

    public static string? GetBase64String(Image? image)
    {
        if (image == null) return null;

        using var m = new MemoryStream();
        image.Save(m, ImageFormat.Png);
        return "data:image/png;base64," + Convert.ToBase64String(m.ToArray());
    }

    public static void DeAlpha(Bitmap? bmp)
    {
        if (bmp == null) return;

        var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        unsafe
        {
            var scan0 = (byte*)data.Scan0.ToPointer();

            for (var y = 0; y < bmp.Height; y++)
            {
                var p = scan0 + y * data.Stride + 3;
                var end = p + bmp.Width * 4;
                while (p < end)
                {
                    *p = 255;
                    p += 4;
                }
            }
        }

        bmp.UnlockBits(data);
    }

    public static void DrawAlphaCirle(BitmapData data, int cx, int cy, int r, byte alpha)
    {
        unsafe
        {
            var scan0 = (byte*)data.Scan0.ToPointer();

            for (var y = -r; y < r; y++)
            {
                for (var x = -r; x < r; x++)
                {
                    if (x * x + y * y > r * r) continue;

                    var rx = cx + x;
                    var ry = cy + y;
                    if (rx < 0 || ry < 0 || rx >= data.Width || ry >= data.Height) continue;
                    var p = scan0 + ry * data.Stride + rx * 4 + 3;
                    *p = alpha;
                }
            }
        }
    }

    public static void DrawAlphaBox(BitmapData data, int x1, int y1, int width, int height, byte alpha)
    {
        var x2 = Math.Min(x1 + width,  data.Width);
        var y2 = Math.Min(y1 + height, data.Height);
        
        unsafe
        {
            var scan0 = (byte*)data.Scan0.ToPointer();

            for (var y = y1; y < y2; y++)
            {
                var p = scan0 + y * data.Stride + x1 * 4 + 3;
                for (var x = x1; x < x2; x++)
                {
                    *p = alpha;
                    p += 4;
                }
            }
        }
    }

    public static Bitmap? GetCropped(Bitmap? src, int x, int y, int width, int height, Color backColor)
    {
        if (src == null) return null;

        var dst = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        FillBox(dst, 0, 0, dst.Width, dst.Height, backColor);
        DrawBitmapAtPos(src, dst, -x, -y);

        return dst;
    }

    public static void FillBox(Bitmap dst, int x0, int y0, int w, int h, Color color)
    {
        var dstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        unsafe
        {
            var pOrgDst = (byte*)dstData.Scan0.ToPointer();

            for (var y = 0; y < h; y++)
            {
                var pDst = pOrgDst + dstData.Stride * (y0 + y) + x0 * 4;
                for (var x = 0; x < w; x++)
                {
                    pDst[0] = color.A;
                    pDst[1] = color.R;
                    pDst[2] = color.G;
                    pDst[3] = color.B;
                    pDst+=4;
                }
            }
        }

        dst.UnlockBits(dstData);
    }

    public static void DrawBitmapAtPos(Bitmap src, Bitmap dst, int dstX, int dstY)
    {
        var srcX = dstX >= 0 ? 0 : -dstX;
        var srcY = dstY >= 0 ? 0 : -dstY;

        dstX = Math.Max(0, dstX);
        dstY = Math.Max(0, dstY);

        var srcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var dstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        var w = Math.Min(dst.Width  - dstX, src.Width  - srcX);
        var h = Math.Min(dst.Height - dstY, src.Height - srcY);

        unsafe
        {
            var pOrgSrc = (byte*)srcData.Scan0.ToPointer();
            var pOrgDst = (byte*)dstData.Scan0.ToPointer();

            for (var y = 0; y < h; y++)
            {
                var pSrc = pOrgSrc + srcData.Stride * (srcY + y) + srcX * 4;
                var pDst = pOrgDst + dstData.Stride * (dstY + y) + dstX * 4;
                for (var x = 0; x < w * 4; x++)
                {
                    *pDst = *pSrc;
                    pSrc++;
                    pDst++;
                }
            }
        }

        dst.UnlockBits(dstData);
        src.UnlockBits(srcData);
    }

    public static Bitmap ResizeIfNeed(Bitmap image, int width, int height)
    {
        if (image.Width == width && image.Height == height) return image;

        var res = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(res);
        g.DrawImage(image, 0, 0, res.Width, res.Height);
        return res;
    }

    public static Bitmap ShrinkIfNeed(Bitmap image, int maxWidth, int maxHeight)
    {
        if (image.Width <= maxWidth && image.Height <= maxHeight) return image;

        var k = Math.Min((double)maxWidth / image.Width, (double)maxHeight / image.Height);
        return ResizeIfNeed(image, (int)Math.Round(image.Width * k), (int)Math.Round(image.Height * k));
    }

    public static bool HasAlpha(Bitmap bmp)
    {
        var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        unsafe
        {
            var scan0 = (byte*)data.Scan0.ToPointer();

            for (var y = 0; y < bmp.Height; y++)
            {
                var p = scan0 + y * data.Stride + 3;
                var end = p + bmp.Width * 4;
                while (p < end)
                {
                    if (*p != 255)
                    {
                        bmp.UnlockBits(data);
                        return true;
                    }
                    p += 4;
                }
            }
        }

        bmp.UnlockBits(data);
        return false;
    }

    public static Bitmap RestoreAlpha(Bitmap src, Bitmap dst)
    {
        var srcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly,  PixelFormat.Format32bppArgb);
        var dstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        unsafe
        {
            var srcScan0 = (byte*)srcData.Scan0.ToPointer();
            var dstScan0 = (byte*)dstData.Scan0.ToPointer();

            for (var y = 0; y < src.Height; y++)
            {
                var pSrc = srcScan0 + y * srcData.Stride + 3;
                var pDst = dstScan0 + y * dstData.Stride + 3;
                var pSrcEnd = pSrc + srcData.Stride;
                while (pSrc < pSrcEnd)
                {
                    *pDst = *pSrc;
                    pSrc+=4;
                    pDst+=4;
                }
            }

        }

        dst.UnlockBits(dstData);
        src.UnlockBits(srcData);

        return dst;
    }
}