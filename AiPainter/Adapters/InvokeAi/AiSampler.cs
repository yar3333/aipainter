using System.Text.Json.Serialization;

namespace AiPainter.Adapters.InvokeAi;

[JsonConverter(typeof(JsonStringEnumConverter))]
enum AiSampler
{
    ddim,
    plms,
    k_lms,
    k_dpm_2,
    k_dpm_2_a,
    k_euler,
    k_heun,
    k_euler_a
}