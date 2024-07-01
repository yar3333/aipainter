using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion;

static class SdVaeHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_vae");

    public static string[] GetNames(string nameToEnsureExists)
    {
        var basePath = BasePath;
        
        return new[] { nameToEnsureExists }
                .Concat(Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(basePath, "*.safetensors", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(basePath, "*.pt", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(basePath, "config.json", SearchOption.AllDirectories))
                .Select(x => Path.GetDirectoryName(x.Substring(basePath.Length).TrimStart('\\'))!))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();
    }

    public static ListItem[] GetListItems(string nameToEnsureExists)
    {
        return GetNames(nameToEnsureExists).Where(x => GetPathToVae(x) != null && IsEnabled(x))
                                           .Select(x => new ListItem { Value = x, Text = getHumanName(x) })
                                           .ToArray();
    }

    public static string? GetPathToVae(string name)
    {
        if (string.IsNullOrEmpty(name) || !Directory.Exists(GetDirPath(name))) return null;

        var models = Directory.GetFiles(GetDirPath(name), "*.ckpt")
             .Concat(Directory.GetFiles(GetDirPath(name), "*.safetensors"))
             .Concat(Directory.GetFiles(GetDirPath(name), "*.pt"))
             .ToArray();

        return models.Min();
    }

    public static string GetDirPath(string name)
    {
        return Path.Combine(BasePath, name);
    }

    static string getHumanName(string name)
    {
        var path = GetPathToVae(name);
        var size = path != null ? new FileInfo(path).Length : 0;
        return name + " (" + Math.Round(size / 1024.0 / 1024, 1) + " MB)";
    }

    public static SdVaeConfig GetConfig(string name)
    {
        var configFilePath = Path.Combine(GetDirPath(name), "config.json");

        return File.Exists(configFilePath)
                   ? JsonSerializer.Deserialize<SdVaeConfig>(File.ReadAllText(configFilePath)) ?? new SdVaeConfig()
                   : new SdVaeConfig();
    }

    public static string GetStatus(string name)
    {
        var file = GetPathToVae(name);
        var url = GetConfig(name).downloadUrl;
        return file != null ? "file" : (!string.IsNullOrWhiteSpace(url) ? "URL" : "-");
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
}
