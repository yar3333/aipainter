using System.Diagnostics;
using System.Text.RegularExpressions;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI;

static class WebUiProcess
{
    private static Process? process;

    public static bool Running { get; private set; }
    public static string ActiveCheckpointFilePath { get; private set; } = "";
    public static string ActiveVaeFilePath { get; private set; } = "";

    public static Action<int>? OnUpscaleProgress = null;

    public static bool IsReady()
    {
        return ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl);
    }

    public static void Start(string? checkpointFilePath, string? vaeFilePath)
    {
        var log = WebUiApiClient.Log;

        if (!Program.Config.UseEmbeddedStableDiffusion || checkpointFilePath == null) return;

        if (ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.StableDiffusionUrl);
            return;
        }

        if (!File.Exists(checkpointFilePath))
        {
            log.WriteLine($"Can't find {checkpointFilePath}.");
            return;
        }

        var uri = new Uri(Program.Config.StableDiffusionUrl);

        Running = true;

        ActiveCheckpointFilePath = checkpointFilePath;
        ActiveVaeFilePath = vaeFilePath ?? "";

        var pathToLoraDir = Path.Join(Application.StartupPath, "stable_diffusion_lora");
        var pathToEmbeddingsDir = Path.Join(Application.StartupPath, "stable_diffusion_embeddings");

        process = ProcessHelper.RunInBackground
        (
            "run.bat",
            string.Join(' ', new[]
            {
                "--api",
                uri.Host != "127.0.0.1" && uri.Host.ToLowerInvariant() != "localhost" ? "--listen" : "",
                "--port=" + uri.Port,
                "--ckpt=\"" + checkpointFilePath + "\"",
                !string.IsNullOrEmpty(vaeFilePath) ? " --vae-path=\"" + vaeFilePath + "\"" : "",
                "--lora-dir=\"" + pathToLoraDir + "\"",
                "--embeddings-dir=\"" + pathToEmbeddingsDir + "\"",
            }.Where(x => !string.IsNullOrEmpty(x))),

            directory: Path.Join(Application.StartupPath, @"external\StableDiffusion"),

            logFunc: s =>
            {
                if (s != null)
                {
                    log.WriteLine("[process] " + s);
                    var m = Regex.Match(s, @"^tiled upscale:\s*(\d+)%");
                    if (m.Success) OnUpscaleProgress?.Invoke(int.Parse(m.Groups[1].Value));
                }
            },

            onExit: code =>
            {
                Running = false;
                log.WriteLine("[process] Exit " + code);
            }
        );

        if (process != null) Program.Job.AddProcess(process.Id);
    }

    public static void Stop()
    {
        Running = false;

        try { process?.Kill(true); } catch { }
        try { process?.WaitForExit(); } catch { }
    }
}