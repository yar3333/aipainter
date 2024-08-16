using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class VAEDecodeInputs : IComfyNodeInputs
{
    public JsonValue[] samples { get; set; } // [ "3", 0 ],
    public JsonValue[] vae { get; set; } // [ "4", 2 ]
}
