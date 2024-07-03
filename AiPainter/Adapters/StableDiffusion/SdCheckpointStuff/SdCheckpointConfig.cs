using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

namespace AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;

class SdCheckpointConfig
{
    public string? prompt { get; set; }
    public string? homeUrl { get; set; }
    public string? mainCheckpointUrl { get; set; }
    public string? inpaintCheckpointUrl { get; set; }
    public string? vaeUrl { get; set; }
    public string? description { get; set; }
    public SdSettings? overrideSettings { get; set; }

    public bool isNeedAuthToDownload = false;
}
