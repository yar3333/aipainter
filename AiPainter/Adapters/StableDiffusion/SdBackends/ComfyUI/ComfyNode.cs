using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyNode
{
    public IComfyNodeInputs inputs { get; set; }
    public ComfyNodeType class_type { get; set; }
    public Dictionary<string, string>? _meta { get; set; } // { "title": "KSampler" }

    private ComfyNode(ComfyNodeType class_type, IComfyNodeInputs inputs)
    {
        this.class_type = class_type;
        this.inputs = inputs;
    }

    private static ComfyNodeType getTypeByInputs(IComfyNodeInputs inputs)
    {
        if (inputs is CheckpointLoaderSimpleInputs) return ComfyNodeType.CheckpointLoaderSimple;
        if (inputs is CLIPTextEncodeInputs) return ComfyNodeType.CLIPTextEncode;
        if (inputs is EmptyLatentImageInputs) return ComfyNodeType.EmptyLatentImage;
        if (inputs is KSamplerInputs) return ComfyNodeType.KSampler;
        if (inputs is LoraLoaderInputs) return ComfyNodeType.LoraLoader;
        if (inputs is SaveImageInputs) return ComfyNodeType.SaveImage;
        if (inputs is SaveImageWebsocketInputs) return ComfyNodeType.SaveImageWebsocket;
        if (inputs is VAEDecodeInputs) return ComfyNodeType.VAEDecode;
        throw new ArgumentException();
    }

    public static ComfyNode Create<Inputs>(Inputs inputs) where Inputs : IComfyNodeInputs
    {
        return new ComfyNode(getTypeByInputs(inputs), inputs);
    }
}
