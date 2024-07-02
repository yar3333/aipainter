namespace AiPainter.Adapters.StableDiffusion;

class SdCheckpointConfig
{
    public string? prompt { get; set; }
    public string? homeUrl { get; set; }
    public string? mainCheckpointUrl { get; set; }
    public string? inpaintCheckpointUrl { get; set; }
    public string? vaeUrl { get; set; }
    public string? description { get; set; }
}
