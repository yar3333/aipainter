namespace AiPainter.Controls;

[Serializable]
class SdGenerationParameters
{
    public string checkpoint { get; set; } = null!;
    public string prompt { get; set; } = null!;
    public string negative { get; set; } = null!;
    public int steps { get; set; }
    public decimal cfgScale { get; set; }
    
    public long seed { get; set; }
    public decimal seedVariationStrength { get; set; }
    
    public string[] modifiers { get; set; } = null!;
    public int width { get; set; } = 512;
    public int height { get; set; } = 512;
    public string sampler { get; set; } = "Euler a";
    public decimal changesLevel { get; set; }

    public SdGenerationParameters ShallowCopy()
    {
        return (SdGenerationParameters)MemberwiseClone();
    }
}