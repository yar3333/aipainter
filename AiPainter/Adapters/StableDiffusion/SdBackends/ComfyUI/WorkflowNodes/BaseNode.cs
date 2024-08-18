using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class BaseNode
{
    [JsonIgnore] public string Id { get; set; }
}
