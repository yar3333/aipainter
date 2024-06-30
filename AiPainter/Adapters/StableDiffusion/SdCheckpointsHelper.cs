using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion;

static class SdCheckpointsHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_checkpoints");
    private static string VaePath => Path.Join(Application.StartupPath, "stable_diffusion_vae");

    public static string[] GetNames(string nameToEnsureExists)
    {
        var basePath = BasePath;
        
        return new[] { nameToEnsureExists }
                .Concat(Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(basePath, "*.safetensors", SearchOption.AllDirectories))
                .Select(x => Path.GetDirectoryName(x.Substring(basePath.Length).TrimStart('\\'))!))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();
    }

    public static ListItem[] GetListItems(string nameToEnsureExists)
    {
        return GetNames(nameToEnsureExists).Select(x => new ListItem { Value = x, Text = getHumanName(x) }).ToArray();
    }

    public static string? GetPathToMainCheckpoint(string name)
    {
        if (!Directory.Exists(getDirPath(name))) return null;

        var models = Directory.GetFiles(getDirPath(name), "*.ckpt")
             .Concat(Directory.GetFiles(getDirPath(name), "*.safetensors"))
             .ToArray();

        if (models.Length == 0) return null;
        if (models.Length == 1) return models[0];

        models = models.Where(x => !x.Contains("inpaint")).ToArray();
        if (models.Length == 1) return models[0];
        
        return null;
    }

    public static string? GetPathToInpaintCheckpoint(string name)
    {
        if (!Directory.Exists(getDirPath(name))) return null;

        var models = Directory.GetFiles(getDirPath(name), "*.ckpt")
             .Concat(Directory.GetFiles(getDirPath(name), "*.safetensors"))
             .ToArray();

        if (models.Length == 0) return null;
        if (models.Length == 1) return models[0];

        models = models.Where(x => x.Contains("inpaint")).ToArray();
        if (models.Length == 1) return models[0];
        
        return null;
    }

    public static string? GetPathToVae(string name)
    {
        var fileName = GetConfig(name).vae;
        return !string.IsNullOrWhiteSpace(fileName) ? Path.Combine(VaePath, fileName) : null;
    }

    static string getDirPath(string name)
    {
        return Path.Combine(BasePath, name);
    }

    static string getHumanName(string name)
    {
        var path = GetPathToMainCheckpoint(name);
        var size = path != null ? new FileInfo(path).Length : 0;
        return name + " (" + Math.Round(size / 1024.0 / 1024 / 1024, 1) + " GB)";
    }

    public static SdCheckpointConfig GetConfig(string name)
    {
        var configFilePath = Path.Combine(getDirPath(name), "config.json");

        return File.Exists(configFilePath!)
                   ? JsonSerializer.Deserialize<SdCheckpointConfig>(File.ReadAllText(configFilePath)) ?? new SdCheckpointConfig()
                   : new SdCheckpointConfig();
    }
}
