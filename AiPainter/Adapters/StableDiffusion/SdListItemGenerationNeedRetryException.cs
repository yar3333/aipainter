namespace AiPainter.Adapters.StableDiffusion;

class SdListItemGenerationNeedRetryException : Exception
{
    public SdListItemGenerationNeedRetryException() {}
    public SdListItemGenerationNeedRetryException(string message) : base(message) {}
}