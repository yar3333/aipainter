namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class CLIPSetLastLayerInputs : ComfyUiNodeInputs
{
    public object[] clip { get; set; }
    public int stop_at_clip_layer { get; set; }
}