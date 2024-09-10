using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class ImageUpscaleWithModelNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.ImageUpscaleWithModel;
    
    public object[]? upscale_model { get; set; }
    public object[]? image { get; set; }

    [JsonIgnore] public object[] Output_image => new object[] { Id, 0 };
}