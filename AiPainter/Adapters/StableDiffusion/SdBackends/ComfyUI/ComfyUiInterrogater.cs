using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.SdApiClientStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyUiInterrogater
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
