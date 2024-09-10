using System.Drawing.Imaging;
using AiPainter.Controls;
using AiPainter.Helpers;
using AiPainter.Adapters.StableDiffusion.SdBackends;
using AiPainter.Adapters.StableDiffusion;

namespace AiPainter;

public class UpscaleForm
{
    private readonly GenerationList generationList;
    private readonly string imageBase64;
    private readonly UpscalerType upscaler;
    private readonly int resizeFactor;
    public readonly string ResultFilePath;
    
    private readonly WaitingDialog dialog;

    public UpscaleForm(GenerationList generationList, Bitmap image, UpscalerType upscaler, int resizeFactor, string resultFilePath)
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

                try
                {
                    var image = await SdBackend.Instance.UpscaleAsync
                                (
                                    upscaler, 
                                    resizeFactor, 
                                    imageBase64, 
                                    percent => dialog.ProgressValue = percent, 
                                    cancellationTokenSource
                                );
                    image?.Save(ResultFilePath, ImageFormat.Png);
                }
                catch (Exception e)
                {
                    Program.Log.WriteLine(e.ToString());
                    throw;
                }

            }, cancellationTokenSource.Token);
        });
    }
}
