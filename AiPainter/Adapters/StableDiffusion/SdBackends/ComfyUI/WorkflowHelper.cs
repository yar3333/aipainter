using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class WorkflowHelper
{
    private class NativeNode<T> where T : ComfyUiNodeInputs
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
    }

    /*public static List<ComfyUiNodeInputs> DeserializeWorkflow(string jsonStr)
    {
        var r = new List<ComfyUiNodeInputs>();

        var workflow = JsonSerializer.Deserialize<JsonObject>(jsonStr)!;
        foreach (var item in workflow)
        {
            r.Add(DeserializeNode(workflow, item.Key));
        }

        return r;
    }*/

    public static ComfyUiNodeInputs DeserializeNode(JsonObject workflow, string nodeId)
    {
        var item = workflow[nodeId]!.AsObject();

        ComfyUiNodeInputs r;
        switch (Enum.Parse<ComfyUiNodeType>(item["class_type"]!.ToString()))
        {
            case ComfyUiNodeType.CLIPTextEncode:
                r = item["inputs"].Deserialize<CLIPTextEncodeInputs>()!;
                break;

            case ComfyUiNodeType.CLIPSetLastLayer:
                r = item["inputs"].Deserialize<CLIPSetLastLayerInputs>()!;
                break;

            case ComfyUiNodeType.CheckpointLoaderSimple:
                r = item["inputs"].Deserialize<CheckpointLoaderSimpleInputs>()!;
                break;

            case ComfyUiNodeType.EmptyLatentImage:
                r = item["inputs"].Deserialize<EmptyLatentImageInputs>()!;
                break;

            case ComfyUiNodeType.KSampler:
                r = item["inputs"].Deserialize<KSamplerInputs>()!;
                break;

            case ComfyUiNodeType.LoraLoader:
                r = item["inputs"].Deserialize<LoraLoaderInputs>()!;
                break;

            case ComfyUiNodeType.SaveImage:
                r = item["inputs"].Deserialize<SaveImageInputs>()!;
                break;

            case ComfyUiNodeType.VAEDecode:
                r = item["inputs"].Deserialize<VAEDecodeInputs>()!;
                break;

            case ComfyUiNodeType.SaveImageWebsocket:
                throw new NotImplementedException();

            default:
                throw new NotImplementedException();
        }
        r.Id = nodeId;
        return r;
    }

    public static void SerializeNode<T>(T node, JsonObject workflow) where T : ComfyUiNodeInputs
    {
        var wrapper = new NativeNode<T>(node);
        workflow[node.Id] = JsonSerializer.SerializeToNode(wrapper);
    }
}