namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyNodeOutputHelper
{
    public static object[] CheckpointLoaderSimple_model(string nodeId)
    {
        return new object[] { nodeId, 0 };
    }       
    
    public static object[] CheckpointLoaderSimple_clip(string nodeId)
    {
        return new object[] { nodeId, 1 };
    }    
    
    public static object[] CheckpointLoaderSimple_vae(string nodeId)
    {
        return new object[] { nodeId, 2 };
    }
    
    public static object[] VAELoader_vae(string nodeId)
    {
        return new object[] { nodeId, 0 };
    }

    public static object[] LoraLoader_model(string nodeId)
    {
        return new object[] { nodeId, 0 };
    }

    public static object[] LoraLoader_clip(string nodeId)
    {
        return new object[] { nodeId, 1 };
    }
}
