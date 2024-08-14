namespace AiPainter.Adapters.StableDiffusion.SdBackendClients.WebUI.SdApiClientStuff;

[Serializable]
class SdTxt2ImgRequest : SdBaseGenerationRequest
{
    public bool enable_hr { get; set; } = false;
    public int firstphase_width { get; set; } = 0;
    public int firstphase_height { get; set; } = 0;

    public override SdTxt2ImgRequest Clone()
    {
        var r = (SdTxt2ImgRequest)MemberwiseClone();
        r.override_settings = r.override_settings?.Clone();
        return r;
    }
}