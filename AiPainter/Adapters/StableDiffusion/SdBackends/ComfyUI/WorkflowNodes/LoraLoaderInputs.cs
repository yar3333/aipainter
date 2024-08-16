namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class LoraLoaderInputs : IComfyUiNodeInputs
{
    public object[] model { get; set; }
    public object[] clip { get; set; }

    public string lora_name { get; set; }
    public decimal strength_model { get; set; }
    public decimal strength_clip { get; set; }
}