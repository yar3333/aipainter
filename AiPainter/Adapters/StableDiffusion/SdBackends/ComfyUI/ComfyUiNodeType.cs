namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

enum ComfyUiNodeType
{
    KSampler,
    CheckpointLoaderSimple,
    EmptyLatentImage,
    CLIPTextEncode,
    VAEDecode,
    SaveImage,
    SaveImageWebsocket,
    LoraLoader,
}
