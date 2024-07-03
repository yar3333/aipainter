namespace AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

[Serializable]
class SdGenerationProgess
{
    public decimal progress { get; set; }
    public decimal eta_relative { get; set; }
    public SdGenerationProgessState? state { get; set; } = null!;
    string? current_image { get; set; }
}

[Serializable]
class SdGenerationProgessState
{
    public bool skipped { get; set; }
    public bool interrupted { get; set; }
    public string? job { get; set; }
    public int job_count { get; set; }
    public int job_no { get; set; }
    public int sampling_step { get; set; }
    public int sampling_steps { get; set; }
}
