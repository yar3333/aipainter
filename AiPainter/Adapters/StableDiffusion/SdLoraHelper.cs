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
               .OrderBy(x => x))
               .ToArray();
    }

    public static string GetPrompt(string? name)
    {
        if (string.IsNullOrEmpty(name)) return "";

        var loraTextFilePath = Path.Combine(BasePath, Path.GetFileNameWithoutExtension(name)) + ".txt";
        return File.Exists(loraTextFilePath)
                ? File.ReadAllText(loraTextFilePath).Trim()
                : "<lora:" + Path.GetFileNameWithoutExtension(name) + ":0.95>";
    }
}
