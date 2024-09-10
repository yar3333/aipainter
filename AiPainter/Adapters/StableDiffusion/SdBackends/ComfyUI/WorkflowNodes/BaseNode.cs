using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
abstract class BaseNode
{
    [JsonIgnore] public abstract ComfyUiNodeType NodeType { get; }
    [JsonIgnore] public string Id { get; set; } = "";
}
