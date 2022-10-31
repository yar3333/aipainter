namespace AiPainter.Adapters.InvokeAi;

class AiGenerationParameters
{
    public string prompt { get; set; } = "";
    public int iterations { get; set; } = 1;
    public int steps { get; set; } = 50;
    public decimal cfg_scale { get; set; } = 7.5m;

    public int width { get; set; } = 512;
    public int height { get; set; } = 512;
    public AiSampler sampler_name { get; set; } = AiSampler.k_lms;
    
    public uint seed { get; set; } // from 0 to 4 294 967 295
    public decimal threshold { get; set; } = 0; // seed
    public decimal perlin { get; set; } = 0; // seed
    
    public decimal variation_amount { get; set; } = 0;
    
    public bool seamless { get; set; } = false;
    public bool hires_fix { get; set; } = false;
    
    public string? init_img { get; set; }
    public decimal strength { get; set; } = 0.75m;
    public bool fit { get; set; } = true;

    public bool progress_images { get; set; } = false;
}

public class AiGfpganParameters
{
    public decimal strength { get; set; } = 0.8m;
}

/*
 ["generateImage",
{
    "prompt":"woman in red dress",
    "iterations":1,
    "steps":50,
    "cfg_scale":7.5,
    "threshold":0,
    "perlin":0,
    "height":512,
    "width":512,
    "sampler_name":
    "k_lms","seed":2996532007,
    "seamless":false,
    "hires_fix":false,
    "progress_images":false,
    "init_img":"outputs/000003.2998758527.png",
    "strength":0.75,
    "fit":true,
    "variation_amount":0
},
false,
{"strength":0.8}]
*/
