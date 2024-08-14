namespace AiPainter.Adapters.StableDiffusion.SdBackendClients;

class SdGeneratorFatalErrorException : Exception
{
    public SdGeneratorFatalErrorException(string message)
        : base(message)
    {
    }
}