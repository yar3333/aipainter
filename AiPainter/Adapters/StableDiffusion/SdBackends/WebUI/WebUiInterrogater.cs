using AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI;

static class WebUiInterrogater
{
    public static async Task<string?> RunAsync(Bitmap image)
    {
        return await WebUiApiClient.interrogateAsync(new SdInterrogateRequest
        {
            image = BitmapTools.GetBase64String(image),
            //model = "model_base_caption_capfilt_large",
        });
    }
}
