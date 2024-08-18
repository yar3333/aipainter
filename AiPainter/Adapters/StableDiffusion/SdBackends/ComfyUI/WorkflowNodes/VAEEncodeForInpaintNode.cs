using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class VAEEncodeForInpaintNode : BaseNode
{
    public object[] pixels { get; set; }
    public object[] vae { get; set; }
    public object[] mask { get; set; }

    public int grow_mask_by { get; set; } // 6

    [JsonIgnore] public object[] Output_latent => new object[] { Id, 0 };
}