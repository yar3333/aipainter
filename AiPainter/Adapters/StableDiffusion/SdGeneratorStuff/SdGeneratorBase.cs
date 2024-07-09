using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

namespace AiPainter.Adapters.StableDiffusion.SdGeneratorStuff;

abstract class SdGeneratorBase
{
    public abstract Task<bool> RunAsync();

    public void Cancel()
    {
        Task.Run(SdApiClient.Cancel);
    }
}