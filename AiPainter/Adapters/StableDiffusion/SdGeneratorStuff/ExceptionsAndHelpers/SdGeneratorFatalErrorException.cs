namespace AiPainter.Adapters.StableDiffusion.SdGeneratorStuff.ExceptionsAndHelpers;

class SdGeneratorFatalErrorException : Exception
{
    public SdGeneratorFatalErrorException(string message)
        : base(message)
    {
    }
}