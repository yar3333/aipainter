using System.Text.RegularExpressions;

namespace AiPainter.Adapters.StableDiffusion.SdBackends;

static class SdGeneratorHelper
{
    public static void SaveMain(SdGenerationParameters sdGenerationParameters, long seed, string destDir, Bitmap resultImage)
    {
        if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
        var destImageFilePath = Path.Combine(destDir, DateTime.UtcNow.Ticks / 10000 + ".png");
        SdPngHelper.Save(resultImage, sdGenerationParameters, seed, destImageFilePath);
        resultImage.Dispose();
    }

    public static void SaveInpaint(SdGenerationParameters sdGenerationParameters, long seed, string originalFilePath, Bitmap image)
    {
        var basePath = Path.Join(Path.GetDirectoryName(originalFilePath), Path.GetFileNameWithoutExtension(originalFilePath));
        var matches = Regex.Matches(basePath, @"-aip(\d+)$");

        var n = 1;
        if (matches.Count == 1)
        {
            n = int.Parse(matches[0].Groups[1].Value) + 1;
            basePath = basePath.Substring(0, basePath.Length - matches[0].Groups[0].Value.Length);
        }

        string resultFilePath;
        for (; ; )
        {
            resultFilePath = basePath + "-aip" + n.ToString("D3") + ".png";
            if (!File.Exists(resultFilePath)) break;
            n++;
        }

        SdPngHelper.Save(image, sdGenerationParameters, seed, resultFilePath);
    }
}