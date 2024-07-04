using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;

static class SdCheckpointsHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_checkpoints");

    private static readonly Dictionary<string, SdCheckpointConfig> configCache = new();

    public static string[] GetNames(string nameToEnsureExists)
    {
        var basePath = BasePath;

        var r = !string.IsNullOrEmpty(nameToEnsureExists)
                    ? new[] { nameToEnsureExists }
                    : new string[] { };

        return r.Concat(Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(basePath, "*.safetensors", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(basePath, "config.json", SearchOption.AllDirectories))
                .Select(x => Path.GetDirectoryName(x.Substring(basePath.Length).TrimStart('\\'))!))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();
    }

    public static ListItem[] GetListItems(string nameToEnsureExists)
    {
        return GetNames(nameToEnsureExists).Where(x => x == nameToEnsureExists || GetPathToMainCheckpoint(x) != null && IsEnabled(x))
                                           .Select(x => new ListItem { Value = x, Text = getHumanName(x) })
                                           .ToArray();
    }

    public static string? GetPathToMainCheckpoint(string name)
    {
        if (!Directory.Exists(GetDirPath(name))) return null;

        var models = Directory.GetFiles(GetDirPath(name), "*.ckpt")
             .Concat(Directory.GetFiles(GetDirPath(name), "*.safetensors"))
             .ToArray();

        return models.Where(x => !Path.GetFileNameWithoutExtension(x).ToLowerInvariant().Contains("inpaint")
                              && !IsFilePathLikeVae(x))
                     .Min();
    }

    public static string? GetPathToInpaintCheckpoint(string name)
    {
        if (!Directory.Exists(GetDirPath(name))) return null;

        var models = Directory.GetFiles(GetDirPath(name), "*.ckpt")
             .Concat(Directory.GetFiles(GetDirPath(name), "*.safetensors"))
             .ToArray();

        return models.Where(x => Path.GetFileNameWithoutExtension(x).ToLowerInvariant().Contains("inpaint")
                              && !IsFilePathLikeVae(x))
                     .Min();
    }

    public static string? GetPathToVae(string name)
    {
        if (!Directory.Exists(GetDirPath(name))) return null;

        var models = Directory.GetFiles(GetDirPath(name), "*.ckpt")
             .Concat(Directory.GetFiles(GetDirPath(name), "*.safetensors"))
             .Concat(Directory.GetFiles(GetDirPath(name), "*.pt"))
             .ToArray();

        return models.Where(IsFilePathLikeVae).Min();
    }

    public static string GetDirPath(string name)
    {
        return Path.Combine(BasePath, name);
    }

    static string getHumanName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "<Select Checkpoint>";
        var path = GetPathToMainCheckpoint(name);
        var size = path != null ? new FileInfo(path).Length : 0;
        return name + " (" + Math.Round(size / 1024.0 / 1024 / 1024, 1) + " GB)";
    }

    public static SdCheckpointConfig GetConfig(string name)
    {
        if (configCache.ContainsKey(name)) return configCache[name];

        var configFilePath = Path.Combine(GetDirPath(name), "config.json");

        var r = File.Exists(configFilePath)
                   ? JsonSerializer.Deserialize<SdCheckpointConfig>(File.ReadAllText(configFilePath)) ?? new SdCheckpointConfig()
                   : new SdCheckpointConfig();

        configCache[name] = r;

        return r;
    }

    public static string GetStatusMain(string name)
    {
        var file = GetPathToMainCheckpoint(name);
        var url = GetConfig(name).mainCheckpointUrl;
        return file != null ? "+" : !string.IsNullOrWhiteSpace(url) ? "URL" : "";

    }

    public static string GetStatusInpaint(string name)
    {
        var file = GetPathToInpaintCheckpoint(name);
        var url = GetConfig(name).inpaintCheckpointUrl;
        return file != null ? "+" : !string.IsNullOrWhiteSpace(url) ? "URL" : "";
    }

    public static string GetStatusVae(string name)
    {
        var file = GetPathToVae(name);
        var url = GetConfig(name).vaeUrl;
        return file != null ? "+" : !string.IsNullOrWhiteSpace(url) ? "URL" : "";
    }

    public static bool IsEnabled(string name)
    {
        return !File.Exists(Path.Combine(GetDirPath(name), "_disabled"));
    }

    public static void SetEnabled(string name, bool enabled)
    {
        var filePath = Path.Combine(GetDirPath(name), "_disabled");
        if (enabled && File.Exists(filePath)) File.Delete(filePath);
        if (!enabled && !File.Exists(filePath)) File.WriteAllText(filePath, "");
    }

    public static bool IsFilePathLikeVae(string filePath)
    {
        return Regex.IsMatch(Path.GetFileNameWithoutExtension(filePath), @"(?:\b|_)vae(?:\b|_)", RegexOptions.IgnoreCase);
    }

    public static bool SaveConfig(string name, SdCheckpointConfig config)
    {
        var configFilePath = Path.Combine(GetDirPath(name), "config.json");
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
        catch (Exception e)
        {
            return false;
        }
    }
}
