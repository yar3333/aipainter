using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using AiPainter.Helpers;

namespace AiPainter.Adapters.LamaCleaner;

static class LamaCleanerClient
{
    public static readonly Log Log = new("lama-cleaner");

    public static async Task<Bitmap?> RunAsync(Bitmap image, Bitmap mask)
    {
        var httpClient = new HttpClient(new LoggerHttpClientHandler(Log))
        {
            BaseAddress = new Uri(Program.Config.LamaCleanerUrl),
        };

        using var memBufferImage = new MemoryStream();
        image.Save(memBufferImage, ImageFormat.Png);
        var fileContent1 = new ByteArrayContent(memBufferImage.ToArray());
        fileContent1.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        fileContent1.Headers.Add("Content-Disposition", "form-data; name=\"image\"; filename=\"image.png\"");

        using var memBufferMask = new MemoryStream();
        mask.Save(memBufferMask, ImageFormat.Png);
        var fileContent2 = new ByteArrayContent(memBufferMask.ToArray());
        fileContent2.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        fileContent2.Headers.Add("Content-Disposition", "form-data; name=\"mask\"; filename=\"mask.png\"");

        using var multipartFormContent = new MultipartFormDataContent
        {
            { fileContent1, "image", "inpaint.png" },
            { fileContent2, "mask", "blob" },

            { "zitsWireframe", "true" },

            { "ldmSteps", "25" },
            { "ldmSampler", "plms" },

            { "hdStrategy", "Original" }, // Original
            { "hdStrategyCropMargin", "128" },
            { "hdStrategyCropTrigerSize", "2048" },
            { "hdStrategyResizeLimit", "2048" },
            { "sizeLimit", image.Width.ToString() },

            { "prompt", "" },

            { "useCroper", "false" },
            { "croperX", "0" },
            { "croperY", "0" },
            { "croperWidth", "512" },
            { "croperHeight", "512" },

            { "sdMaskBlur", "5" },
            { "sdStrength", "0.75" },
            { "sdSteps", "50" },
            { "sdGuidanceScale", "7.5" },
            { "sdSampler", "ddim" },
            { "sdSeed", "42" },

            { "cv2Radius", "5" },
            { "cv2Flag", "INPAINT_NS" },
        };
        var response = await httpClient.PostAsync("/inpaint", multipartFormContent);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return BitmapTools.RestoreAlpha(image, BitmapTools.ResizeIfNeed
        (
            (Bitmap)Image.FromStream(await response.Content.ReadAsStreamAsync()),
            image.Width,
            image.Height
        )!);
    }
}