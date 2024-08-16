namespace AiPainter.Adapters.StableDiffusion.SdBackends;

static class SdGeneratorHelper
{
    public static void SaveMain(Log log, SdGenerationParameters sdGenerationParameters, long seed, string destDir, Bitmap resultImage)
    {
        try
        {
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            var destImageFilePath = Path.Combine(destDir, DateTime.UtcNow.Ticks / 10000 + ".png");
            SdPngHelper.Save(resultImage, sdGenerationParameters, seed, destImageFilePath);
            resultImage.Dispose();
        }
        catch (Exception e)
        {
            log.WriteLine(e.ToString());
        }
    }
}