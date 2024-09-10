using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class FluxGuidanceNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.FluxGuidance;
    
    public decimal guidance { get; set; }
    public object[]? conditioning { get; set; }
    
    [JsonIgnore] public object[] Output_conditioning => new object[] { Id, 0 };
}
