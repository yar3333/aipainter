namespace AiPainter.Adapters.StableDiffusion;

class SdLoraConfig
{
    public string name { get; set; } = "";
    public string prompt { get; set; } = "";
    public double weight { get; set; } = 0.95;
}
