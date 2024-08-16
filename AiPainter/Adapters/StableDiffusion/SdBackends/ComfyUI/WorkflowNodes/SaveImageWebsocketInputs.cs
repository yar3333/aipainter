namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class SaveImageWebsocketInputs : IComfyNodeInputs
{
    public object[] images { get; set; } // [ "8", 0 ],
}
