using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyNodeOutputHelper
{
    public static JsonArray CheckpointLoaderSimple_clip(string nodeId)
    {
        return new JsonArray(JsonValue.Create(nodeId), JsonValue.Create(1));
    }    
    
    public static JsonArray CheckpointLoaderSimple_vae(string nodeId)
    {
        return new JsonArray(JsonValue.Create(nodeId), JsonValue.Create(2));
    }
}
