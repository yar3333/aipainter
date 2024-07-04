using System.Diagnostics;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion;

static class StableDiffusionProcess
{
    private static Process? process;
    
    public static bool Loading { get; private set; }
    public static string ActiveCheckpointFilePath { get; private set; } = "";
    public static string ActiveVaeFilePath { get; private set; } = "";

    public static bool IsReady()
    {
        return ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl);
    }

    public static void Start(string? checkpointFilePath, string? vaeFilePath)
    {
        var log = SdApiClient.Log;

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

        Loading = true;

        ActiveCheckpointFilePath = checkpointFilePath;
        ActiveVaeFilePath = vaeFilePath ?? "";

        var pathToLoraDir = Path.Join(Application.StartupPath, "stable_diffusion_lora");
        var pathToEmbeddingsDir = Path.Join(Application.StartupPath, "stable_diffusion_embeddings");
        
        process = ProcessHelper.RunInBackground
        (
            "run.bat",
            "--api"
                + (uri.Host != "127.0.0.1" && uri.Host.ToLowerInvariant() != "localhost" ? " --listen" : "")
                + " --port=" + uri.Port
                + " --ckpt=\"" + checkpointFilePath + "\""
                + (!string.IsNullOrEmpty(vaeFilePath) ? " --vae-path=\"" + vaeFilePath + "\"" : "")
                + " --lora-dir=\"" + pathToLoraDir + "\""
                + " --embeddings-dir=\"" + pathToEmbeddingsDir + "\"",
            
            directory: Path.Join(Application.StartupPath, @"external\StableDiffusion"),
            
            logFunc: s => log.WriteLine("[process] " + s),
            
            onExit: code =>
            {
                Loading = false;
                log.WriteLine("[process] Exit " + code);
            }
        );

        if (process != null) Program.Job.AddProcess(process.Id);
    }

    public static void Stop()
    {
        Loading = false;

        try { process?.Kill(true); } catch {}
        try { process?.WaitForExit(); } catch {}
    }
}