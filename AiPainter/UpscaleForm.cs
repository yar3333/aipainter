using System.Drawing.Imaging;
using AiPainter.Controls;
using AiPainter.Helpers;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

namespace AiPainter;

public class UpscaleForm
{
    private readonly GenerationList generationList;
    private readonly string imageBase64;
    private readonly string upscaler;
    private readonly int resizeFactor;
    public readonly string ResultFilePath;
    
    private readonly WaitingDialog dialog;

    public UpscaleForm(GenerationList generationList, Bitmap image, string upscaler, int resizeFactor, string resultFilePath)
    {
        this.generationList = generationList;
        this.imageBase64 = BitmapTools.GetBase64String(image);
        this.upscaler = upscaler;
        this.resizeFactor = resizeFactor;
        this.ResultFilePath = resultFilePath;

        dialog = new WaitingDialog("Upscale image", "Preparing...");
    }

    public DialogResult ShowDialog(IWin32Window parent)
    {
        return dialog.ShowDialog(parent, async cancellationTokenSource =>
        {
            await generationList.RunGenerationAsSoonAsPossibleAsync(() => dialog.LabelText = "Waiting generation queue paused...", async () =>
            {
                dialog.LabelText = "Upscaling " + resizeFactor + "x using " + upscaler + "...";
            
                var cancelCalled = false;
                
                var request = new SdExtraImageRequest
                {
                    upscaler_1 = upscaler,
                    upscaling_resize = resizeFactor,
                    image = imageBase64,
                };
                var r = await SdApiClient.extraImageAsync(request, percent =>
                {
                    dialog.ProgressValue = percent;
                    if (!cancelCalled && cancellationTokenSource.IsCancellationRequested)
                    {
                        cancelCalled = true;
                        SdApiClient.Cancel();
                    }
                });
            
                if (!cancellationTokenSource.IsCancellationRequested && r?.image != null)
                {
                    var image = BitmapTools.FromBase64(r.image);
                    image.Save(ResultFilePath, ImageFormat.Png);
                }
            }, cancellationTokenSource.Token);
        });
    }
}
