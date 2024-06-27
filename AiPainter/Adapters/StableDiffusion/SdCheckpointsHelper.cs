using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion;

static class SdCheckpointsHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_checkpoints");

    static List<string> GetNames(string nameToEnsureExists)
    {
        var basePath = BasePath;
        
        var r = Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
        .Concat(Directory.GetFiles(basePath, "*.safetensors", SearchOption.AllDirectories))
                         .Select(x => x.Substring(basePath.Length).TrimStart('\\'))
                         .OrderBy(GetHumanName)
                         .ToList();

        if (!r.Contains(nameToEnsureExists)) r.Insert(0, nameToEnsureExists);

        return r;
    }

    public static ListItem[] GetListItems(string nameToEnsureExists)
    {
        return GetNames(nameToEnsureExists).Select(x => new ListItem { Value = x, Text = GetHumanName(x) }).ToArray();
    }

    public static string GetPath(string name)
    {
        return Path.Combine(BasePath, name);
    }

    static string GetHumanName(string name)
    {
        // ReSharper disable once StringIndexOfIsCultureSpecific.1
        var n = name.IndexOf(".");
        var shortName = n > 0 ? name.Substring(0, n) : name;

        var parts = shortName.Split('\\');
        if (parts.Length > 1 && parts[^2] == parts[^1])
        {
            shortName = string.Join('\\', parts.Take(parts.Length - 1));
        }

        return shortName + " (" + Math.Round(getSize(name) / 1024.0 / 1024 / 1024, 1) + " GB)";
    }

    public static SdCheckpointConfig GetConfig(string name)
    {
        var dir = Path.GetDirectoryName(GetPath(name));
        var configFilePath = Path.Combine(dir!, "config.json");

        return File.Exists(configFilePath!)
                   ? JsonSerializer.Deserialize<SdCheckpointConfig>(File.ReadAllText(configFilePath)) ?? new SdCheckpointConfig()
                   : new SdCheckpointConfig();
    }

    static long getSize(string name)
    {
        var path = GetPath(name);
        return File.Exists(path) ? new FileInfo(path).Length : 0;
    }
}
