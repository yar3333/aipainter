namespace AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

[Serializable]
class SdExtraImageRequest
{
    // https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Features/e85e54f5b069d8937951b1dbce593a4331cdd357#resizing
    public int resize_mode { get; set; } 
    
    public bool show_extras_results { get; set; } = true;
    
    public decimal gfpgan_visibility { get; set; }
    public decimal codeformer_visibility { get; set; }
    public decimal codeformer_weight { get; set; }
    
    public int upscaling_resize { get; set; } = 2;
    public int upscaling_resize_w { get; set; } = 512;
    public int upscaling_resize_h { get; set; } = 512;
    
    public bool upscaling_crop { get; set; } = true;
    
    // It is useful for blending different upscaler models at once.
    // I use ESRGAN_4x for detail and SwinIR_4x (with visibility 0.2-0.5) for smoothness.
    public string? upscaler_1 { get; set; }
    public string? upscaler_2 { get; set; }
    
    public decimal extras_upscaler_2_visibility { get; set; }
    public bool upscale_first { get; set; } = false;
    
    public string image { get; set; }
}

[Serializable]
class SdExtraImageResponse
{
    public string? html_info { get; set; }
    public string? image { get; set; }
}