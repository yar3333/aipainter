using System.Drawing.Imaging;
using AiPainter.Controls;
using AiPainter.Helpers;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

namespace AiPainter;

public class UpscaleForm
{
    private readonly string imageBase64; 
    private readonly string upscaler;
    private readonly int resizeFactor;
    
    public readonly string ResultFilePath;
    
    private readonly WaitingDialog dialog;

    public UpscaleForm(Bitmap image, string upscaler, int resizeFactor, string resultFilePath)
    {
        this.imageBase64 = BitmapTools.GetBase64String(image);
        this.upscaler = upscaler;
        this.resizeFactor = resizeFactor;
        this.ResultFilePath = resultFilePath;

        dialog = new WaitingDialog("Upscale image", "Upscale " + resizeFactor + "x using " + upscaler);
    }

    public DialogResult ShowDialog(IWin32Window parent)
    {
        return dialog.ShowDialog(parent, async cancellationTokenSource =>
        {
            var canceled = false;
            
            var request = new SdExtraImageRequest
            {
                upscaler_1 = upscaler,
                upscaling_resize = resizeFactor,
                image = imageBase64,
            };
            var r = await SdApiClient.extraImageAsync(request, percent =>
            {
                dialog.ProgressValue = percent;
                if (!canceled && cancellationTokenSource.IsCancellationRequested)
                {
                    canceled = true;
                    SdApiClient.Cancel();
                }
            });
            
            if (!cancellationTokenSource.IsCancellationRequested && r?.image != null)
            {
                var image = BitmapTools.FromBase64(r.image);
                image.Save(ResultFilePath, ImageFormat.Png);
            }
        });
    }
}
