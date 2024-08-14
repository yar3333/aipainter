using AiPainter.Adapters.StableDiffusion.SdBackendClients.WebUI.SdApiClientStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackendClients.WebUI;

static class WebUiInterrogater
{
    public static async Task<string?> RunAsync(Bitmap image)
    {
        return await SdApiClient.interrogateAsync(new SdInterrogateRequest
        {
            image = BitmapTools.GetBase64String(image),
            //model = "model_base_caption_capfilt_large",
        });
    }
}
