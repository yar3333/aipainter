using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion;

static class SdLoraHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_lora");

    public static string[] GetNames()
    {
        var basePath = BasePath;
        
        return new string[]{}
               .Concat(Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
               .Concat(Directory.GetFiles(basePath, "*.safetensors", SearchOption.AllDirectories))
               .Select(x => x.Substring(basePath.Length).TrimStart('\\'))
               .OrderBy(GetHumanName))
               .ToArray();
    }

    public static string GetPrompt(string? name)
    {
        if (string.IsNullOrEmpty(name)) return "";

        var config = GetConfig(name);
        return "<lora:" + Path.GetFileNameWithoutExtension(name) + ":0.9>" + config.prompt;
    }

    public static string GetHumanName(string? name)
    {
        return string.IsNullOrEmpty(name) ? "" : GetConfig(name).name;
    }

    public static SdLoraConfig GetConfig(string name)
    {
        var configFilePath = Path.Combine(BasePath, Path.GetFileNameWithoutExtension(name) + ".json");

        var r = File.Exists(configFilePath) 
                    ? JsonSerializer.Deserialize<SdLoraConfig>(File.ReadAllText(configFilePath)) ?? new SdLoraConfig()
                    : new SdLoraConfig();

        if (string.IsNullOrWhiteSpace(r.name)) r.name = Path.GetFileNameWithoutExtension(name);

        return r;
    }
}
