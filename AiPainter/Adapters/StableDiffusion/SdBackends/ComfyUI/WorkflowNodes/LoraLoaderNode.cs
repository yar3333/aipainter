using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class LoraLoaderNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.LoraLoader;
    
    public object[]? model { get; set; }
    public object[]? clip { get; set; }

    public string lora_name { get; set; } = "";
    public decimal strength_model { get; set; }
    public decimal strength_clip { get; set; }

    [JsonIgnore] public object[] Output_model => new object[] { Id, 0 };
    [JsonIgnore] public object[] Output_clip => new object[] { Id, 1 };
}