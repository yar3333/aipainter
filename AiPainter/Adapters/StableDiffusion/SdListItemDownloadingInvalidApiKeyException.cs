namespace AiPainter.Adapters.StableDiffusion;

class SdListItemDownloadingInvalidApiKeyException : Exception
{
    public SdListItemDownloadingInvalidApiKeyException() : base("Invalid API key") {}
}