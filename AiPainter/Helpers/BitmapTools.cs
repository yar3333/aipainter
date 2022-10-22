using System.Drawing.Imaging;

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
            var pOrgDst = (int*)dstData.Scan0.ToPointer();
            var stride = dstData.Stride >> 2;
            var value = color.ToArgb();

            for (var y = 0; y < h; y++)
            {
                var pDst = pOrgDst + stride * (y0 + y) + x0;
                var pEnd = pDst + w;
                while (pDst < pEnd)
                {
                    *pDst = value;
                    pDst++;
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

        var srcStride = srcData.Stride >> 2;
        var dstStride = dstData.Stride >> 2;

        var w = Math.Min(dst.Width  - dstX, src.Width  - srcX);
        var h = Math.Min(dst.Height - dstY, src.Height - srcY);

        unsafe
        {
            var pOrgSrc = (int*)srcData.Scan0.ToPointer();
            var pOrgDst = (int*)dstData.Scan0.ToPointer();

            for (var y = 0; y < h; y++)
            {
                var pSrc = pOrgSrc + srcStride * (srcY + y) + srcX;
                var pDst = pOrgDst + dstStride * (dstY + y) + dstX;
                for (var x = 0; x < w; x++)
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

    public static Bitmap? ResizeIfNeed(Bitmap? image, int width, int height)
    {
        if (image == null) return null;
        if (image.Width == width && image.Height == height) return image;
        return Resize(image, width, height);
    }

    public static Bitmap ShrinkIfNeed(Bitmap image, int maxWidth, int maxHeight)
    {
        if (image.Width <= maxWidth && image.Height <= maxHeight) return image;

        var k = Math.Min((double)maxWidth / image.Width, (double)maxHeight / image.Height);
        return Resize(image, (int)Math.Round(image.Width * k), (int)Math.Round(image.Height * k));
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

    public static Bitmap Resize(Bitmap image, int width, int height)
    {
        var dst = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        drawImageResizedInner(image, dst, new Rectangle(0, 0, width, height), 128);
        return dst;
    }

    private static void drawImageResizedInner(Bitmap src, Bitmap dst, Rectangle dstRect, int alphaLimit = -1)
    {
        var srcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly,  PixelFormat.Format32bppArgb);
        //var dstData = dst.LockBits(dstRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        var dstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        var srcStride = srcData.Stride >> 2;
        var dstStride = dstData.Stride >> 2;

        unsafe
        {
            var srcScan0 = (uint*)srcData.Scan0.ToPointer();
            var dstScan0 = (uint*)dstData.Scan0.ToPointer();
            
            var xRatio = (double)(src.Width - 1) / dstRect.Width;
            var yRatio = (double)(src.Height - 1) / dstRect.Height;

            for (var i = 0; i < dstRect.Height; i++)
            {
                var pDst = dstScan0 + (dstRect.Y + i) * dstStride + dstRect.X;
                for (var j = 0; j < dstRect.Width; j++)
                {
                    var x = (int)(xRatio * j);
                    var y = (int)(yRatio * i);
                    var xDiff = xRatio * j - x;
                    var yDiff = yRatio * i - y;
                    var pSrc = srcScan0 + y * srcStride + x;              
                    var a = pSrc[0];
                    var b = pSrc[1];
                    var c = pSrc[srcStride];
                    var d = pSrc[srcStride + 1];

                    var blue = (a & 0xff) * (1 - xDiff) * (1 - yDiff) + (b & 0xff) * xDiff * (1 - yDiff) +
                               (c & 0xff) * yDiff * (1 - xDiff) + (d & 0xff) * (xDiff * yDiff);

                    var green = ((a >> 8) & 0xff) * (1 - xDiff) * (1 - yDiff) + ((b >> 8) & 0xff) * xDiff * (1 - yDiff) +
                                ((c >> 8) & 0xff) * yDiff * (1 - xDiff) + ((d >> 8) & 0xff) * (xDiff * yDiff);

                    var red = ((a >> 16) & 0xff) * (1 - xDiff) * (1 - yDiff) + ((b >> 16) & 0xff) * xDiff * (1 - yDiff) +
                              ((c >> 16) & 0xff) * yDiff * (1 - xDiff) + ((d >> 16) & 0xff) * (xDiff * yDiff);

                    var alpha = ((a >> 24) & 0xff) * (1 - xDiff) * (1 - yDiff) + ((b >> 24) & 0xff) * xDiff * (1 - yDiff) +
                                ((c >> 24) & 0xff) * yDiff * (1 - xDiff) + ((d >> 24) & 0xff) * (xDiff * yDiff);

                    if (alphaLimit >= 0)
                    {
                        alpha = alpha < alphaLimit ? 0 : 255;
                    }

                    *pDst = (((uint)alpha << 24) & 0xff000000)
                          | (((uint)red << 16) & 0xff0000) 
                          | (((uint)green << 8) & 0xff00) 
                          | (uint)blue;

                    pDst++;
                }
            }
        }

        dst.UnlockBits(dstData);
        src.UnlockBits(srcData);
    }
}