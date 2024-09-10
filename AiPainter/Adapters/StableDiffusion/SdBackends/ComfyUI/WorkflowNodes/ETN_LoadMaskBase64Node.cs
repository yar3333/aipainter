using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class ETN_LoadMaskBase64Node : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.ETN_LoadMaskBase64;
    
    public string mask { get; set; } = "";

    [JsonIgnore] public object[] Output_mask => new object[] { Id, 0 };
}