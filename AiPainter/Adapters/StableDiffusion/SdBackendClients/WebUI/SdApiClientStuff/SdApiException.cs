namespace AiPainter.Adapters.StableDiffusion.SdBackendClients.WebUI.SdApiClientStuff;

class SdApiException : Exception
{
    public SdApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}