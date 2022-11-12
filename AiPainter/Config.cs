namespace AiPainter;

class Config
{
    public string OutputFolder { get; set; } = "images";

    public bool UseEmbeddedStableDiffusion { get; set; } = true;
    public string StableDiffusionUrl { get; set; } = "http://127.0.0.1:7860/";
    public string StableDiffusionCheckpoint { get; set; } = "sd-v1-5-pruned-emaonly.ckpt";
    
    public bool UseEmbeddedLamaCleaner { get; set; } = true;
    public string LamaCleanerUrl { get; set; } = "http://127.0.0.1:9595/";

    public bool UseEmbeddedRemBg { get; set; } = true;
    public string RemBgUrl { get; set; } = "http://127.0.0.1:9696/";
}
