namespace AiPainter;

class Config
{
    public string OutputFolder { get; set; } = "images";

    public bool UseEmbeddedStableDiffusion { get; set; } = true;
    public string StableDiffusionUrl { get; set; } = "http://127.0.0.1:7860/";
    public string StableDiffusionCheckpoint { get; set; } = "";
    public string StableDiffusionVae { get; set; } = "";
    
    public bool UseEmbeddedLamaCleaner { get; set; } = true;
    public string LamaCleanerUrl { get; set; } = "http://127.0.0.1:9595/";

    public List<string> NegativePrompts { get; set; } = new()
    {
        "[deformed | disfigured], poorly drawn, [bad : wrong] anatomy, [extra | missing | floating | disconnected] limb, (mutated hands and fingers), blurry"
    };

    public List<string> ImageSizes { get; set; } = new()
    {
        "512x512",
        "512x768",
        "512x1024",
        "768x512",
        "768x768",
        "768x1024",
        "1024x512",
        "1024x768",
        "1024x1024",
    };

    public string CivitaiApiKey { get; set; } = "";
}
