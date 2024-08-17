namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class CheckpointLoaderSimpleInputs : ComfyUiNodeInputs
{
    public string ckpt_name { get; set; } // "StableDiffusion-v1.5\\v1-5-pruned-emaonly.safetensors"
}