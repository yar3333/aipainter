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
    public string[] modifiers { get; set; } = null!;
    public string loraPrompt { get; set; } = null!;

    public SdGenerationParameters ShallowCopy()
    {
        return (SdGenerationParameters)MemberwiseClone();
    }
}