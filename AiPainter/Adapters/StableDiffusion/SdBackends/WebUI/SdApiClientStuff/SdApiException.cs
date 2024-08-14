namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;

class SdApiException : Exception
{
    public SdApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}