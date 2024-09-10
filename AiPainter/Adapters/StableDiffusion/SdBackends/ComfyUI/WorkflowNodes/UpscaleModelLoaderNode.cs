using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class UpscaleModelLoaderNode : BaseNode
{
    public string model_name { get; set; } = "";

    [JsonIgnore] public object[] Output_upscale_model => new object[] { Id, 0 };
}