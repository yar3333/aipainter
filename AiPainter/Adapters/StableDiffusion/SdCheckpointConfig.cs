namespace AiPainter.Adapters.StableDiffusion;

class SdCheckpointConfig
{
    public string? prompt { get; set; }
    public string? vae { get; set; }
    public string? homeUrl { get; set; }
    public string? downloadUrl { get; set; }
    public string? downloadInpaintUrl { get; set; }
    public string? downloadVaeUrl { get; set; }
    public string? description { get; set; }
}
