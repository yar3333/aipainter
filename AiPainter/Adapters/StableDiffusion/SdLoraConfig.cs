namespace AiPainter.Adapters.StableDiffusion;

class SdLoraConfig
{
    public string prompt { get; set; } = "";
    public string homeUrl { get; set; } = "";
    public string downloadUrl { get; set; } = "";

    public bool isNeedAuthToDownload = false;
}
