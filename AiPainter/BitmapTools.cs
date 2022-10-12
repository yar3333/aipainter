using System.Drawing.Imaging;

namespace AiPainter;

public static class BitmapTools
{
    private static readonly Random rnd = new();

    public static Bitmap? Load(string filePath)
    {
        using var src = (Bitmap)Image.FromFile(filePath);
        return Clone(src);
    }

    public static Bitmap? Clone(Bitmap? src)
    {
        if (src == null) return null;

        var dst = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);

        var srcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly,  PixelFormat.Format32bppArgb);
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
    
    public static void DrawAlphaCirle(BitmapData data, int cx, int cy, int r)
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
                    *p = 0;
                }
            }
        }
    }

    public static Bitmap? GetCropped(Bitmap? src, int x, int y, int width, int height)
    {
        if (src == null) return null;

        var dst = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        
        var srcData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly,  PixelFormat.Format32bppArgb);
        var dstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        var rWidth  = Math.Min(width,  src.Width  - x);
        var rHeight = Math.Min(height, src.Height - y);
            
        unsafe
        {
            var pOrgSrc = (byte*)srcData.Scan0.ToPointer();
            var pOrgDst = (byte*)dstData.Scan0.ToPointer();

            for (var h = 0; h < rHeight; h++)
            {
                var pSrc = pOrgSrc + srcData.Stride * (y + h) + x * 4;
                var pDst = pOrgDst + dstData.Stride * (y + h);
                for (var w = 0; w < rWidth * 4; w++)
                {
                    *pDst = *pSrc;
                    pSrc++;
                    pDst++;
                }
            }
        }

        dst.UnlockBits(dstData);
        src.UnlockBits(srcData);

        return dst;
    }

    public static Bitmap ResizeIfNeed(Bitmap image, int width, int height)
    {
        if (image.Width == width && image.Height == height) return image;

        var res = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(res);
        g.DrawImage(image, 0, 0, res.Width, res.Height);
        return res;
    }
}