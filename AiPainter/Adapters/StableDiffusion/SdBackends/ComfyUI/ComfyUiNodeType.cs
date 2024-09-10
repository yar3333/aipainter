using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

[JsonConverter(typeof(JsonStringEnumConverter))]
enum ComfyUiNodeType
{
    CheckpointLoaderSimple,
    CLIP_Interrogator,
    CLIPSetLastLayer,
    CLIPTextEncode,
    EmptyLatentImage,
    ETN_LoadImageBase64,
    ETN_LoadMaskBase64,
    FluxGuidance,
    ImageUpscaleWithModel,
    InpaintModelConditioning,
    KSampler,
    LoraLoader,
    SaveImage,
    SaveImageWebsocket,
    ShowText,
    UpscaleModelLoader,
    VAEDecode,
    VAEEncodeForInpaint,
    VAELoader,
}
