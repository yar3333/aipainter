using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class VAEDecodeNode : BaseNode
{
    public object[]? samples { get; set; } // [ "3", 0 ],
    public object[]? vae { get; set; } // [ "4", 2 ]

    [JsonIgnore] public object[] Output_image => new object[] { Id, 0 };
}
