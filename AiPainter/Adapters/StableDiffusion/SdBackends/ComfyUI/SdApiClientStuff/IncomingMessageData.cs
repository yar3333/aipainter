namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.SdApiClientStuff;

class IncomingMessageData
{
    public string prompt_id { get; set; }
    public string? node { get; set; }
}