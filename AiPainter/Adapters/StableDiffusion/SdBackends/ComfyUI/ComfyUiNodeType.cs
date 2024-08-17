using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

[JsonConverter(typeof(JsonStringEnumConverter))]
enum ComfyUiNodeType
{
    KSampler,
    CheckpointLoaderSimple,
    EmptyLatentImage,
    CLIPTextEncode,
    CLIPSetLastLayer,
    VAEDecode,
    SaveImage,
    SaveImageWebsocket,
    LoraLoader,
}
