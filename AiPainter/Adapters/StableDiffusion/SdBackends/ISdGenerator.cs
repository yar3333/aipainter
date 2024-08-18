namespace AiPainter.Adapters.StableDiffusion.SdBackends;

interface ISdGenerator
{
    public Task<bool> RunAsync(SdGenerationParameters sdGenerationParameters);
    public void Cancel();
}