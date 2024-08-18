using System.Drawing.Imaging;
using AiPainter.Controls;
using AiPainter.Helpers;
using AiPainter.Adapters.StableDiffusion.SdBackends;

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
        this.imageBase64 = BitmapTools.ToDataUri(image);
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

                var image = await SdBackend.Instance.UpscaleAsync
                (
                    upscaler, 
                    resizeFactor, 
                    imageBase64, 
                    percent => dialog.ProgressValue = percent, 
                    cancellationTokenSource
                );
                image?.Save(ResultFilePath, ImageFormat.Png);

            }, cancellationTokenSource.Token);
        });
    }
}
