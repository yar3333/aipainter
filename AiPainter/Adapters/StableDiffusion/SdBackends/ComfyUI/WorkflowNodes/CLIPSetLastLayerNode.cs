using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class CLIPSetLastLayerNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.CLIPSetLastLayer;

    public object[]? clip { get; set; }
    
    public int stop_at_clip_layer { get; set; }

    [JsonIgnore] public object[] Output_clip => new object[] { Id, 0 };
}