using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;

[Serializable]
class SdGenerationResponse
{
    public string[]? images { get; set; } = null; // data:image/png;base64,iVBOR....
    public string info { get; set; } = null!;
    public SdGenerationInfo infoParsed => JsonSerializer.Deserialize<SdGenerationInfo>(info)!;
}

[Serializable]
class SdGenerationInfo
{
    public string prompt { get; set; } = "";
    public string[] all_prompts { get; set; } = { };
    public string negative_prompt { get; set; } = "";
    public long seed { get; set; }
    public long[] all_seeds { get; set; } = { };
    public long subseed { get; set; }
    public long[] all_subseeds { get; set; } = { };
    public decimal subseed_strength { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int sampler_index { get; set; }
    public string sampler { get; set; } = "";
    public decimal cfg_scale { get; set; }
    public int steps { get; set; }
    public int batch_size { get; set; }
    public bool restore_faces { get; set; }
    public string? face_restoration_model { get; set; }
    public string? sd_model_hash { get; set; }
    public int seed_resize_from_w { get; set; }
    public int seed_resize_from_h { get; set; }
    public decimal denoising_strength { get; set; }
    public JsonObject? extra_generation_params { get; set; }
    public int index_of_first_image { get; set; }
}
