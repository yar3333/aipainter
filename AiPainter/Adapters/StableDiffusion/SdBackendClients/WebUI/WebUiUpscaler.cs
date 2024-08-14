using AiPainter.Adapters.StableDiffusion.SdBackendClients.WebUI.SdApiClientStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackendClients.WebUI;

static class WebUiUpscaler
{
    public static async Task<Bitmap?> RunAsync(string upscaler, int resizeFactor, string imageBase64, Action<int> progressPercent, CancellationTokenSource cancellationTokenSource)
    {
        var cancelCalled = false;
                
        var request = new SdExtraImageRequest
        {
            upscaler_1 = upscaler,
            upscaling_resize = resizeFactor,
            image = imageBase64,
        };
        var r = await SdApiClient.extraImageAsync(request, percent =>
        {
            progressPercent(percent);
            if (!cancelCalled && cancellationTokenSource.IsCancellationRequested)
            {
                cancelCalled = true;
                SdApiClient.Cancel();
            }
        });

        return !cancellationTokenSource.IsCancellationRequested && r?.image != null
                ? BitmapTools.FromBase64(r.image)
                : null;
    }
}
