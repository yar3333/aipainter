namespace AiPainter.Adapters.StableDiffusion.SdBackendClients;

static class SdUpscaler
{
    public static async Task<Bitmap?> RunAsync(string upscaler, int resizeFactor, string imageBase64, Action<int> progressPercent, CancellationTokenSource cancellationTokenSource)
    {
        return await WebUI.WebUiUpscaler.RunAsync(upscaler, resizeFactor, imageBase64, progressPercent, cancellationTokenSource);
    }
}
