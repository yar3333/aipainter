namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

enum ComfyNodeType
{
    KSampler,
    CheckpointLoaderSimple,
    EmptyLatentImage,
    CLIPTextEncode,
    VAEDecode,
    SaveImage,
    SaveImageWebsocket,
}
