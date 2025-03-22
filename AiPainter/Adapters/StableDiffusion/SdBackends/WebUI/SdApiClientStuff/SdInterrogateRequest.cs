namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;

[Serializable]
class SdInterrogateRequest
{
    public required string image { get; set; }
    public string? model { get; set; } // clip
}

[Serializable]
class SdInterrogateResponse
{
    public string caption { get; set; }
}