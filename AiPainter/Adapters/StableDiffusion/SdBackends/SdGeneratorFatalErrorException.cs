namespace AiPainter.Adapters.StableDiffusion.SdBackends;

class SdGeneratorFatalErrorException : Exception
{
    public SdGeneratorFatalErrorException(string message)
        : base(message)
    {
    }
}