namespace AiPainter.Adapters.StableDiffusion.SdBackends;

interface ISdBackend
{
    void ProcessStart(string? checkpointFilePath, string? vaeFilePath);
    void ProcessStop();

    ISdGenerator CreateGeneratorMain(SdGenerationListItem control, string destDir);
    ISdGenerator CreateGeneratorInpaint(SdGenerationListItem control, Bitmap originalImage, Rectangle activeBox, Bitmap? croppedMask, string originalFilePath);

    Task<Bitmap?> UpscaleAsync(string upscaler, int resizeFactor, string imageBase64, Action<int> progressPercent, CancellationTokenSource cancellationTokenSource);
    Task<string?> InterrogateAsync(Bitmap image);
}
