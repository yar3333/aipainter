namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.SdApiClientStuff;

class IncomingMessage
{
    public string type { get; set; }
    public IncomingMessageData data { get; set; }
}