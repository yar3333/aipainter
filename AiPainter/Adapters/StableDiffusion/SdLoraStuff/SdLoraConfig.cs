namespace AiPainter.Adapters.StableDiffusion.SdLoraStuff;

class SdLoraConfig
{
    public string homeUrl { get; set; } = "";
    public string downloadUrl { get; set; } = "";
    public string description { get; set; } = "";
    public string prompt { get; set; } = "";

    public bool isNeedAuthToDownload = false;
}
