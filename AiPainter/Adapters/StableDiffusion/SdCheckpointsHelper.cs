namespace AiPainter.Adapters.StableDiffusion;

static class SdCheckpointsHelper
{
    private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_checkpoints");

    public static string[] GetNames()
    {
        var basePath = BasePath;
        return Directory.GetFiles(basePath, "*.ckpt", SearchOption.AllDirectories)
                        .Select(x => x.Substring(basePath.Length).TrimStart('\\'))
                        .OrderBy(x => x)
                        .ToArray();
    }

    public static long GetSize(string name)
    {
        return new FileInfo(GetPath(name)).Length;
    }

    public static string GetPath(string name)
    {
        return Path.Combine(BasePath, name);
    }
}
