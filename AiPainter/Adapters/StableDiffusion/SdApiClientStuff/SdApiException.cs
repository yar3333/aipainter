namespace AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

class SdApiException : Exception
{
    public SdApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}