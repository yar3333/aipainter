using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class SaveImageWebsocketInputs : IComfyNodeInputs
{
    public JsonValue[] images { get; set; } // [ "8", 0 ],
}
