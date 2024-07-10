using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;

static class SdEmbeddingHelper
{
    static string BaseDir => Path.Join(Application.StartupPath, "stable_diffusion_embeddings");

    public static string[] GetNames()
    {
        var basePath = BaseDir;

        return new string[] { }
           .Concat(Directory.GetFiles(basePath, "*.safetensors"))
           .Concat(Directory.GetFiles(basePath, "*.ckpt"))
           .Concat(Directory.GetFiles(basePath, "*.pt"))
           .Concat(Directory.GetFiles(basePath, "*.json"))
           .Select(x => Path.GetFileNameWithoutExtension(x!))
           .Distinct()
           .OrderBy(GetHumanName)
           .ToArray();
    }

    public static string GetHumanName(string name)
    {
        return name;
    }

    public static string GetDir()
    {
        return BaseDir;
    }

    public static string? GetPathToModel(string name)
    {
        var baseFilePath = Path.Combine(BaseDir, name) + ".";
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

    public static SdEmbeddingConfig GetConfig(string name)
    {
        var configFilePath = Path.Combine(BaseDir, name + ".json");

        var r = File.Exists(configFilePath)
                    ? JsonSerializer.Deserialize<SdEmbeddingConfig>(File.ReadAllText(configFilePath)) ?? new SdEmbeddingConfig()
                    : new SdEmbeddingConfig();

        return r;
    }

    public static bool IsEnabled(string name)
    {
        return !File.Exists(Path.Combine(BaseDir, name + ".disabled"));
    }

    public static void SetEnabled(string name, bool enabled)
    {
        var filePath = Path.Combine(BaseDir, name + ".disabled");
        if (enabled && File.Exists(filePath)) File.Delete(filePath);
        if (!enabled && !File.Exists(filePath)) File.WriteAllText(filePath, "");
    }

    public static bool SaveConfig(string name, SdEmbeddingConfig config)
    {
        var configFilePath = Path.Combine(BaseDir, name + ".json");
        if (!Directory.Exists(Path.GetDirectoryName(configFilePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(configFilePath)!);
        }
        
        try
        {
            File.WriteAllText(configFilePath, JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true,
            }));
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsConfigExist(string name)
    {
        var configFilePath = Path.Combine(BaseDir, name + ".json");
        return File.Exists(configFilePath);
    }
}
