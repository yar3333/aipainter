namespace AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;

class SdCheckpointConfig
{
    public string? homeUrl { get; set; }
    public string? mainCheckpointUrl { get; set; }
    public string? inpaintCheckpointUrl { get; set; }
    public string? vaeUrl { get; set; }
    public string? usePredefinedVae { get; set; } // EMA or MSE
    public string? description { get; set; }
    public string? promptRequired { get; set; }
    public string? promptSuggested { get; set; }
    public string? baseModel { get; set; }
    public int? clipSkip { get; set; }
    public string? workflow { get; set; } // regular, flux
}
