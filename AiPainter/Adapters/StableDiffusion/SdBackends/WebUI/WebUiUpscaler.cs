using AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI;

static class WebUiUpscaler
{
    public static async Task<Bitmap?> RunAsync(UpscalerType upscaler, int resizeFactor, string imageBase64, Action<int> progressPercent, CancellationTokenSource cancellationTokenSource)
    {
        var cancelCalled = false;
                
        var request = new SdExtraImageRequest
        {
            upscaler_1 = getUpscalerName(upscaler),
            upscaling_resize = resizeFactor,
            image = imageBase64,
        };
        var r = await WebUiApiClient.extraImageAsync(request, percent =>
        {
            progressPercent(percent);
            if (!cancelCalled && cancellationTokenSource.IsCancellationRequested)
            {
                cancelCalled = true;
                WebUiApiClient.Cancel();
            }
        });

        return !cancellationTokenSource.IsCancellationRequested && r?.image != null
                ? BitmapTools.FromDataUri(r.image)
                : null;
    }

    private static string getUpscalerName(UpscalerType upscaler)
    {
        return upscaler switch
        {
            UpscalerType.R_ESRGAN_4x         => "R-ESRGAN 4x+",
            UpscalerType.R_ESRGAN_4x_Anime6B => "R-ESRGAN 4x+ Anime6B",
            _ => throw new ArgumentException("upscaler = " + (int)upscaler)
        };
    }
}
