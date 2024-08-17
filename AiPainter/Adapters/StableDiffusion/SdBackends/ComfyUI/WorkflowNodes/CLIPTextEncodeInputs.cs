namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class CLIPTextEncodeInputs : ComfyUiNodeInputs
{
    public string text { get; set; } // "beautiful scenery nature glass bottle landscape, , purple galaxy bottle,",
    public object[] clip { get; set; } // [ "4", 1 ]
}
