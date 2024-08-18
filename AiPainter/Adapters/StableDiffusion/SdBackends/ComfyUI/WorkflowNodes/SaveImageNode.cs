namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class SaveImageNode : BaseNode
{
    public object[] images { get; set; } // [ "8", 0 ]

    public string filename_prefix { get; set; } // "ComfyUI",
}
