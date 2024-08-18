using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion.SdVaeStuff;

static class SdVaeHelper
{
    public static readonly string BaseDir = Path.Join(Application.StartupPath, "stable_diffusion_vae");

    static string[] GetNames()
    {
        var basePath = BaseDir;

        return new[] { "" }
                .Concat(Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(basePath, "*.safetensors", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(basePath, "*.pt", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(basePath, "config.json", SearchOption.AllDirectories))
                .Select(x => Path.GetDirectoryName(x.Substring(basePath.Length).TrimStart('\\'))!))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();
    }

    public static ListItem[] GetListItems()
    {
        // ReSharper disable once CoVariantArrayConversion
        return GetNames().Select(x => new ListItem
                                     {
                                         Text = x != "" ? getHumanName(x) : "Default",
                                         Value = x
                                     })
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
        return Path.Combine(BaseDir, name);
    }

    static string getHumanName(string name)
    {
        return name;
    }

    public static SdVaeConfig GetConfig(string name)
    {
        var configFilePath = Path.Combine(GetDirPath(name), "config.json");

        return File.Exists(configFilePath)
                   ? JsonSerializer.Deserialize<SdVaeConfig>(File.ReadAllText(configFilePath)) ?? new SdVaeConfig()
                   : new SdVaeConfig();
    }
}
