using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class CLIPTextEncodeNode : BaseNode
{
    public object[]? clip { get; set; } // [ "4", 1 ]
    public string text { get; set; } = ""; // "beautiful scenery nature glass bottle landscape, , purple galaxy bottle,",
    
    [JsonIgnore] public object[] Output_conditioning => new object[] { Id, 0 };
}
