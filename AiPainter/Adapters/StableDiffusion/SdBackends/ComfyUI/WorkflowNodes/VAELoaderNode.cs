using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class VAELoaderNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.VAELoader;
    
    public string vae_name { get; set; } = "";

    [JsonIgnore] public object[] Output_vae => new object[] { Id, 0 };
}