using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class InpaintModelConditioningNode : BaseNode
{
    public object[]? positive { get; set; }
    public object[]? negative { get; set; }
    public object[]? vae { get; set; }
    public object[]? pixels { get; set; }
    public object[]? mask { get; set; }

    [JsonIgnore] public object[] Output_positive => new object[] { Id, 0 };
    [JsonIgnore] public object[] Output_negative => new object[] { Id, 1 };
    [JsonIgnore] public object[] Output_latent => new object[] { Id, 2 };
}