using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class ETN_LoadMaskBase64Node : BaseNode
{
    public string mask { get; set; }

    [JsonIgnore] public object[] Output_mask => new object[] { Id, 0 };
}