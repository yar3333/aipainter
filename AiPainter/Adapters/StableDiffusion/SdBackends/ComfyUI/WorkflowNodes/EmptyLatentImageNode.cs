namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class EmptyLatentImageNode : BaseNode
{
    public int width { get; set; } // 512,
    public int height { get; set; } // 512,
    public int batch_size { get; set; } // 1
}
