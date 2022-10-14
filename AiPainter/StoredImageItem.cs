using AiPainter.Helpers;

namespace AiPainter;

class StoredImageItem
{
    public readonly string FilePath;
    
    private DateTime changeTime;
    public DateTime ChangeTime => changeTime;
    
    private Bitmap? bitmap;
    public Bitmap? Bitmap
    {
        get
        {
            if (bitmap != null) return bitmap;
            try
            {
                using var src = (Bitmap)Image.FromFile(FilePath);
                if (src == null || src.Width == 0 || src.Height == 0) throw new Exception();
                changeTime = File.GetLastWriteTime(FilePath);
                return bitmap = BitmapTools.Clone(src);
            }
            catch
            {
                return null;
            }
        }
    }

    public StoredImageItem(string filePath)
    {
        FilePath = filePath;
        changeTime = File.GetLastWriteTime(FilePath);
    }

    public bool Update()
    {
        if (bitmap == null) return false;

        var newChTime = File.GetLastWriteTime(FilePath);
        if (newChTime != changeTime)
        {
            bitmap = null;
            changeTime = newChTime;
            return true;
        }
        return false;
    }
}