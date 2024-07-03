namespace AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

[Serializable]
class SdImg2ImgRequest : SdBaseGenerationRequest
{
    public string[] init_images { get; set; } = { };

    // 0 - just resize, 1 - crop & keep ratio, 2 - fill & keep ratio
    public int resize_mode { get; set; }

    public string? mask { get; set; }
    public int mask_blur { get; set; } = 4;

    // https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Features#masked-content
    public SdInpaintingFill inpainting_fill { get; set; }
    public bool inpaint_full_res { get; set; }
    public int inpaint_full_res_padding { get; set; }
    public int inpainting_mask_invert { get; set; }

    public bool include_init_images { get; set; }

    public SdImg2ImgRequest Clone()
    {
        var r = (SdImg2ImgRequest)MemberwiseClone();
        r.override_settings = r.override_settings?.Clone();
        return r;
    }
}