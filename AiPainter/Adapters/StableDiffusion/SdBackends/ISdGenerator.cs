namespace AiPainter.Adapters.StableDiffusion.SdBackends;

interface ISdGenerator
{
    public Task<bool> RunAsync();
    public void Cancel();
}