using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class EmptyLatentImageNode : BaseNode
{
    public int width { get; set; } // 512,
    public int height { get; set; } // 512,
    public int batch_size { get; set; } // 1

    [JsonIgnore] public object[] Output_latent => new object[] { Id, 0 };
}
