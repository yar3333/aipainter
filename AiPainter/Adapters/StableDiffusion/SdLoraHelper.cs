using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion;

static class SdLoraHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_lora");

    public static string[] GetNames()
    {
        var basePath = BasePath;
        
        return new string[]{}
           .Concat(Directory.GetFiles(basePath, "*.safetensors"))
           .Concat(Directory.GetFiles(basePath, "*.ckpt"))
           .Concat(Directory.GetFiles(basePath, "*.pt"))
           .Concat(Directory.GetFiles(basePath, "*.json"))
           .Select(x => Path.GetFileNameWithoutExtension(x!))
           .Distinct()
           .OrderBy(GetHumanName)
           .ToArray();
    }

    public static string GetPrompt(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";

        var config = GetConfig(name);
        return "<lora:" + name + ":0.9>" + config.prompt;
    }

    public static string GetHumanName(string name)
    {
        return GetConfig(name).name;
    }

    public static string GetDir()
    {
        return BasePath;
    }

    public static string? GetPathToModel(string name)
    {
        var baseFilePath = Path.Combine(BasePath, name) + ".";
        if (File.Exists(baseFilePath + "safetensors")) return baseFilePath + "safetensors";
        if (File.Exists(baseFilePath + "ckpt")) return baseFilePath + "ckpt";
        if (File.Exists(baseFilePath + "pt")) return baseFilePath + "pt";
        return null;
    }

    public static string GetStatus(string name)
    {
        return GetPathToModel(name) != null
                   ? "+" 
                   : !string.IsNullOrEmpty(GetConfig(name).downloadUrl) ? "URL" : "";
    }

    public static SdLoraConfig GetConfig(string name)
    {
        var configFilePath = Path.Combine(BasePath, name + ".json");

        var r = File.Exists(configFilePath) 
                    ? JsonSerializer.Deserialize<SdLoraConfig>(File.ReadAllText(configFilePath)) ?? new SdLoraConfig()
                    : new SdLoraConfig();

        if (string.IsNullOrWhiteSpace(r.name)) r.name = name;

        return r;
    }

    public static bool IsEnabled(string name)
    {
        return !File.Exists(Path.Combine(BasePath, name + ".disabled"));
    }

    public static void SetEnabled(string name, bool enabled)
    {
        var filePath = Path.Combine(BasePath, name + ".disabled");
        if (enabled && File.Exists(filePath)) File.Delete(filePath);
        if (!enabled && !File.Exists(filePath)) File.WriteAllText(filePath, "");
    }
}
