using System.Reflection;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class WorkflowHelper
{
    [Serializable]
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
        
        var nodeType = getSubclassObjects<BaseNode>().Single(x => x.NodeType == classType).GetType();
        var r = (BaseNode)item["inputs"].Deserialize(nodeType)!;

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

    private static IEnumerable<T> getSubclassObjects<T>(params object[] constructorArgs) where T : class
    {
        var objects = new List<T>();
        var types = Assembly.GetAssembly(typeof(T))!
                            .GetTypes()
                            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));
        foreach (var type in types)
        {
            objects.Add((T)Activator.CreateInstance(type, constructorArgs)!);
        }
        return objects;
    }
}