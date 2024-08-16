using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiNode
{
    public IComfyUiNodeInputs inputs { get; set; }
    public ComfyUiNodeType class_type { get; set; }
    public Dictionary<string, string>? _meta { get; set; } // { "title": "KSampler" }

    private ComfyUiNode(ComfyUiNodeType class_type, IComfyUiNodeInputs inputs)
    {
        this.class_type = class_type;
        this.inputs = inputs;
    }

    private static ComfyUiNodeType getTypeByInputs(IComfyUiNodeInputs inputs)
    {
        if (inputs is CheckpointLoaderSimpleInputs) return ComfyUiNodeType.CheckpointLoaderSimple;
        if (inputs is CLIPTextEncodeInputs) return ComfyUiNodeType.CLIPTextEncode;
        if (inputs is EmptyLatentImageInputs) return ComfyUiNodeType.EmptyLatentImage;
        if (inputs is KSamplerInputs) return ComfyUiNodeType.KSampler;
        if (inputs is LoraLoaderInputs) return ComfyUiNodeType.LoraLoader;
        if (inputs is SaveImageInputs) return ComfyUiNodeType.SaveImage;
        if (inputs is SaveImageWebsocketInputs) return ComfyUiNodeType.SaveImageWebsocket;
        if (inputs is VAEDecodeInputs) return ComfyUiNodeType.VAEDecode;
        throw new ArgumentException();
    }

    public static ComfyUiNode Create<Inputs>(Inputs inputs) where Inputs : IComfyUiNodeInputs
    {
        return new ComfyUiNode(getTypeByInputs(inputs), inputs);
    }
}
