using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class WorkflowHelper
{
    private class NativeNode
    {
        public object inputs { get; set; }
        public ComfyUiNodeType class_type { get; set; }
        public Dictionary<string, string>? _meta { get; set; } // { "title": "KSampler" }

        public NativeNode(BaseNode inputs)
        {
            this.class_type = inputs.NodeType;
            this.inputs = inputs;
            this._meta = new Dictionary<string, string> { { "title", class_type.ToString() } };
        }
    }

    public static List<BaseNode> DeserializeWorkflow(string jsonStr)
    {
        var r = new List<BaseNode>();

        var workflow = JsonSerializer.Deserialize<JsonObject>(jsonStr)!;
        foreach (var item in workflow)
        {
            r.Add(DeserializeNode(workflow, item.Key));
        }

        return r;
    }

    public static BaseNode DeserializeNode(JsonObject workflow, string nodeId)
    {
        var item = workflow[nodeId]!.AsObject();

        var classTypeStr = item["class_type"]!.ToString();
        if (!Enum.TryParse<ComfyUiNodeType>(classTypeStr, out var classType)) throw new InvalidDataException(classTypeStr);
        
        BaseNode r;
        switch (classType)
        {
            case ComfyUiNodeType.CheckpointLoaderSimple:
                r = item["inputs"].Deserialize<CheckpointLoaderSimpleNode>()!;
                break;

            case ComfyUiNodeType.CLIPSetLastLayer:
                r = item["inputs"].Deserialize<CLIPSetLastLayerNode>()!;
                break;

            case ComfyUiNodeType.CLIPTextEncode:
                r = item["inputs"].Deserialize<CLIPTextEncodeNode>()!;
                break;

            case ComfyUiNodeType.EmptyLatentImage:
                r = item["inputs"].Deserialize<EmptyLatentImageNode>()!;
                break;

            case ComfyUiNodeType.ETN_LoadImageBase64:
                r = item["inputs"].Deserialize<ETN_LoadImageBase64Node>()!;
                break;

            case ComfyUiNodeType.ETN_LoadMaskBase64:
                r = item["inputs"].Deserialize<ETN_LoadMaskBase64Node>()!;
                break;

            case ComfyUiNodeType.FluxGuidance:
                r = item["inputs"].Deserialize<FluxGuidanceNode>()!;
                break;

            case ComfyUiNodeType.ImageUpscaleWithModel:
                r = item["inputs"].Deserialize<ImageUpscaleWithModelNode>()!;
                break;

            case ComfyUiNodeType.InpaintModelConditioning:
                r = item["inputs"].Deserialize<InpaintModelConditioningNode>()!;
                break;

            case ComfyUiNodeType.KSampler:
                r = item["inputs"].Deserialize<KSamplerNode>()!;
                break;

            case ComfyUiNodeType.LoraLoader:
                r = item["inputs"].Deserialize<LoraLoaderNode>()!;
                break;

            case ComfyUiNodeType.SaveImage:
                r = item["inputs"].Deserialize<SaveImageNode>()!;
                break;

            case ComfyUiNodeType.SaveImageWebsocket:
                r = item["inputs"].Deserialize<SaveImageWebsocketNode>()!;
                break;

            case ComfyUiNodeType.UpscaleModelLoader:
                r = item["inputs"].Deserialize<UpscaleModelLoaderNode>()!;
                break;

            case ComfyUiNodeType.VAEDecode:
                r = item["inputs"].Deserialize<VAEDecodeNode>()!;
                break;

            case ComfyUiNodeType.VAEEncodeForInpaint:
                r = item["inputs"].Deserialize<VAEEncodeForInpaintNode>()!;
                break;

            case ComfyUiNodeType.VAELoader:
                r = item["inputs"].Deserialize<VAELoaderNode>()!;
                break;

            default:
                throw new NotImplementedException(classTypeStr);
        }

        r.Id = nodeId;

        return r;
    }

    public static string SerializeWorkflow(List<BaseNode> workflow)
    {
        var nativeWorkflow = new Dictionary<string, NativeNode>();
        foreach (var node in workflow)
        {
            nativeWorkflow[node.Id] = new NativeNode(node);
        }
        return JsonSerializer.Serialize(nativeWorkflow, Program.DefaultJsonSerializerOptions);
    }
}