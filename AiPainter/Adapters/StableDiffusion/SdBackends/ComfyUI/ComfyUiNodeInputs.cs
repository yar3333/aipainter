using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiNodeInputs
{
    [JsonIgnore]
    public string Id { get; set; }
}
