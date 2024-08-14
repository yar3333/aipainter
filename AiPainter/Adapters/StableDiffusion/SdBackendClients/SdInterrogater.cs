namespace AiPainter.Adapters.StableDiffusion.SdBackendClients;

static class SdInterrogater
{
    public static async Task<string?> RunAsync(Bitmap image)
    {
        return await WebUI.WebUiInterrogater.RunAsync(image);
    }
}
