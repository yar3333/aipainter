using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class WorkflowHelper
{
    /*private class NativeNode<T> where T : ComfyUiNodeInputs
    {
        public T inputs { get; set; }
        public ComfyUiNodeType class_type { get; set; }
        public Dictionary<string, string>? _meta { get; set; } // { "title": "KSampler" }

        public NativeNode(T inputs)
        {
            this.class_type = getTypeByInputs(inputs);
            this.inputs = inputs;
            this._meta = new Dictionary<string, string> { { "title", class_type.ToString() } };
        }

        private static ComfyUiNodeType getTypeByInputs(ComfyUiNodeInputs inputs)
        {
            if (inputs is CheckpointLoaderSimpleInputs) return ComfyUiNodeType.CheckpointLoaderSimple;
            if (inputs is CLIPTextEncodeInputs) return ComfyUiNodeType.CLIPTextEncode;
            if (inputs is CLIPSetLastLayerInputs) return ComfyUiNodeType.CLIPSetLastLayer;
            if (inputs is EmptyLatentImageInputs) return ComfyUiNodeType.EmptyLatentImage;
            if (inputs is KSamplerInputs) return ComfyUiNodeType.KSampler;
            if (inputs is LoraLoaderInputs) return ComfyUiNodeType.LoraLoader;
            if (inputs is SaveImageInputs) return ComfyUiNodeType.SaveImage;
            if (inputs is SaveImageWebsocketInputs) return ComfyUiNodeType.SaveImageWebsocket;
            if (inputs is VAEDecodeInputs) return ComfyUiNodeType.VAEDecode;
            throw new ArgumentException();
        }
    }*/   
    
    private class NativeNode2
    {
        public object inputs { get; set; }
        public ComfyUiNodeType class_type { get; set; }
        public Dictionary<string, string>? _meta { get; set; } // { "title": "KSampler" }

        public NativeNode2(BaseNode inputs)
        {
            this.class_type = getTypeByInputs(inputs);
            this.inputs = inputs;
            this._meta = new Dictionary<string, string> { { "title", class_type.ToString() } };
        }

        private static ComfyUiNodeType getTypeByInputs(BaseNode inputs)
        {
            if (inputs is CheckpointLoaderSimpleNode) return ComfyUiNodeType.CheckpointLoaderSimple;
            if (inputs is CLIPTextEncodeNode) return ComfyUiNodeType.CLIPTextEncode;
            if (inputs is CLIPSetLastLayerNode) return ComfyUiNodeType.CLIPSetLastLayer;
            if (inputs is EmptyLatentImageNode) return ComfyUiNodeType.EmptyLatentImage;
            if (inputs is KSamplerNode) return ComfyUiNodeType.KSampler;
            if (inputs is LoraLoaderNode) return ComfyUiNodeType.LoraLoader;
            if (inputs is SaveImageNode) return ComfyUiNodeType.SaveImage;
            if (inputs is SaveImageWebsocketNode) return ComfyUiNodeType.SaveImageWebsocket;
            if (inputs is VAEDecodeNode) return ComfyUiNodeType.VAEDecode;
            throw new ArgumentException();
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
            case ComfyUiNodeType.CLIPTextEncode:
                r = item["inputs"].Deserialize<CLIPTextEncodeNode>()!;
                break;

            case ComfyUiNodeType.CLIPSetLastLayer:
                r = item["inputs"].Deserialize<CLIPSetLastLayerNode>()!;
                break;

            case ComfyUiNodeType.CheckpointLoaderSimple:
                r = item["inputs"].Deserialize<CheckpointLoaderSimpleNode>()!;
                break;

            case ComfyUiNodeType.EmptyLatentImage:
                r = item["inputs"].Deserialize<EmptyLatentImageNode>()!;
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

            case ComfyUiNodeType.VAEDecode:
                r = item["inputs"].Deserialize<VAEDecodeNode>()!;
                break;

            case ComfyUiNodeType.SaveImageWebsocket:
                r = item["inputs"].Deserialize<SaveImageWebsocketNode>()!;
                break;

            default:
                throw new NotImplementedException(classTypeStr);
        }

        r.Id = nodeId;

        return r;
    }

    /*public static void SerializeNode<T>(T node, JsonObject workflow) where T : ComfyUiNodeInputs
    {
        var wrapper = new NativeNode<T>(node);
        workflow[node.Id] = JsonSerializer.SerializeToNode(wrapper);
    }*/

    public static string SerializeWorkflow(List<BaseNode> workflow)
    {
        var nativeWorkflow = new Dictionary<string, NativeNode2>();
        foreach (var node in workflow)
        {
            nativeWorkflow[node.Id] = new NativeNode2(node);
        }
        return JsonSerializer.Serialize(nativeWorkflow, Program.DefaultJsonSerializerOptions);
    }
}