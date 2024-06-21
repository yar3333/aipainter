using System.Globalization;
using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion;

static class SdLoraHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_lora");

    public static string[] GetNames()
    {
        var basePath = BasePath;
        
        return new []{ "" }
               .Concat(Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
               .Concat(Directory.GetFiles(basePath, "*.safetensors", SearchOption.AllDirectories))
               .Select(x => x.Substring(basePath.Length).TrimStart('\\'))
               .OrderBy(GetHumanName))
               .ToArray();
    }

    public static string GetPrompt(string? name, double? weight)
    {
        if (string.IsNullOrEmpty(name)) return "";

        var config = GetConfig(name);
        return config.prompt
            + "<lora:" + (!string.IsNullOrEmpty(config.name) ? config.name : Path.GetFileNameWithoutExtension(name))
                 + ":" + (weight ?? config.weight).ToString(CultureInfo.InvariantCulture) + ">";
    }

    public static string GetHumanName(string? name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        
        // ReSharper disable once StringIndexOfIsCultureSpecific.1
        var n = name.IndexOf(".");
        if (n > 0) name = name.Substring(0, n);
        return name;
    }

    public static SdLoraConfig GetConfig(string name)
    {
        var configFilePath = Path.Combine(BasePath, Path.GetFileNameWithoutExtension(name) + ".json");

        return File.Exists(configFilePath)
                   ? JsonSerializer.Deserialize<SdLoraConfig>(File.ReadAllText(configFilePath)) ?? new SdLoraConfig()
                   : new SdLoraConfig();
    }
}
