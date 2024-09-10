namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiBackend : ISdBackend
{
    public void ProcessStart(string? checkpointFilePath, string? vaeFilePath)
    {
        ComfyUiProcess.Start();
    }

    public void ProcessStop()
    {
        ComfyUiProcess.Stop();
    }

    public ISdGenerator CreateGeneratorMain(SdGenerationListItem control, string destDir)
    {
        return new ComfyUiGeneratorMain(control, destDir);
    }

    public ISdGenerator CreateGeneratorInpaint(SdGenerationListItem control, Bitmap originalImage, Rectangle activeBox, Bitmap? croppedMask, string originalFilePath)
    {
        return new ComfyUiGeneratorInpaint
        (
            control,
            originalImage,
            activeBox,
            croppedMask,
            originalFilePath
        );
    }

    public async Task<Bitmap?> UpscaleAsync(UpscalerType upscaler, int resizeFactor, string imageBase64, Action<int> progressPercent, CancellationTokenSource cancellationTokenSource)
    {
        return await ComfyUiUpscaler.RunAsync(upscaler, resizeFactor, imageBase64, progressPercent, cancellationTokenSource);
    }

    public async Task<string?> InterrogateAsync(Bitmap image)
    {
        return await ComfyUiInterrogater.RunAsync(image);
    }
}
