namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class SaveImageWebsocketInputs : IComfyUiNodeInputs
{
    public object[] images { get; set; } // [ "8", 0 ],
}
