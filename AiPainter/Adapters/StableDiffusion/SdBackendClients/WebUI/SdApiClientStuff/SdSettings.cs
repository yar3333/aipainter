namespace AiPainter.Adapters.StableDiffusion.SdBackendClients.WebUI.SdApiClientStuff;

class SdSettings
{
    public string? sd_model_checkpoint { get; set; }
    public string? sd_vae { get; set; }
    public int? CLIP_stop_at_last_layers { get; set; }

    public SdSettings Clone()
    {
        return (SdSettings)MemberwiseClone();
    }
}