using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class ShowTextNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.ShowText;
    
    public object[]? text { get; set; }

    [JsonIgnore] public object[] Output_text => new object[] { Id, 0 };
}