using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class SaveImageInputs : IComfyNodeInputs
{
    public string filename_prefix { get; set; } // "ComfyUI",
    public JsonValue[] images { get; set; } // [ "8", 0 ]
}
