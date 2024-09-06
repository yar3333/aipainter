using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

[JsonConverter(typeof(JsonStringEnumConverter))]
enum ComfyUiNodeType
{
    CheckpointLoaderSimple,
    CLIPSetLastLayer,
    CLIPTextEncode,
    EmptyLatentImage,
    ETN_LoadImageBase64,
    ETN_LoadMaskBase64,
    FluxGuidance,
    InpaintModelConditioning,
    KSampler,
    LoraLoader,
    SaveImage,
    SaveImageWebsocket,
    VAEDecode,
    VAEEncodeForInpaint,
    VAELoader,
}
