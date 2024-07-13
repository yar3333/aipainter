namespace AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

[Serializable]
class SdInterrogateRequest
{
    public string image { get; set; }
    public string model { get; set; } // clip
}

[Serializable]
class SdInterrogateResponse
{
    public string caption { get; set; }
}