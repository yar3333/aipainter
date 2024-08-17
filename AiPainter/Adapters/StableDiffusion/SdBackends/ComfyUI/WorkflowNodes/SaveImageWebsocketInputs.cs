namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class SaveImageWebsocketInputs : ComfyUiNodeInputs
{
    public object[] images { get; set; } // [ "8", 0 ],
}
