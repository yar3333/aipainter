using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class CLIPSetLastLayerNode : BaseNode
{
    public object[] clip { get; set; }
    
    public int stop_at_clip_layer { get; set; }

    [JsonIgnore] public object[] Output_clip => new object[] { Id, 0 };
}