namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyNamesHelper
{
    public static string GetSamplerName(string humanName)
    {
        switch (humanName)
        {
            case "Euler a": return "euler ancestral";
            case "DPM++ 2M": return "dpmpp_2m";
            case "Heun": return "heun";
            default: return "euler ancestral";
        }
    }
}
