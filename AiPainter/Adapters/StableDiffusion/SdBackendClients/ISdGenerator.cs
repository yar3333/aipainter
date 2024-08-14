namespace AiPainter.Adapters.StableDiffusion.SdBackendClients;

interface ISdGenerator
{
    public Task<bool> RunAsync();
    public void Cancel();
}