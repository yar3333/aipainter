using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class ETN_LoadImageBase64Node : BaseNode
{
    public string image { get; set; }

    [JsonIgnore] public object[] Output_image => new object[] { Id, 0 };
    [JsonIgnore] public object[] Output_mask => new object[] { Id, 1 };
}