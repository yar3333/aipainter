using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class CheckpointLoaderSimpleNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.CheckpointLoaderSimple;
    
    public string ckpt_name { get; set; } = ""; // "StableDiffusion-v1.5\\v1-5-pruned-emaonly.safetensors"

    [JsonIgnore] public object[] Output_model => new object[] { Id, 0 };
    [JsonIgnore] public object[] Output_clip => new object[] { Id, 1 };
    [JsonIgnore] public object[] Output_vae => new object[] { Id, 2 };
}