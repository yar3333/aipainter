namespace AiPainter.Adapters.InvokeAi;

class AiImageInfo
{
    public string? prompt { get; set; }
    public int iterations { get; set; } = 1;
    public int steps { get; set; } = 50;
    public decimal cfg_scale { get; set; } = 7.5m; // кол-во итераций уточнения изображения
    public AiSampler sampler_name { get; set; } = AiSampler.k_lms;
    public int width { get; set; } = 512;
    public int height { get; set; } = 512;
    public long seed { get; set; } = -1;
    public decimal variation_amount { get; set; } = 0m;
    public string with_variations { get; set; } = "";
    public string? initimg { get; set; } // data:image/png;base64,iVBORw0KGgoAA....
    public decimal strength { get; set; } = 0.75m; // img2img
    public string fit { get; set; } = "on"; // fit to width/height for img2img
    public decimal gfpgan_strength { get; set; } = 0.8m;
    public string upscale_level { get; set; } = "";
    public decimal upscale_strength { get; set; } = 0.75m;
    public string url { get; set; } = "";
}
