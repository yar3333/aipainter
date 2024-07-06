using System.Diagnostics;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion;

static class StableDiffusionProcess
{
    private static Process? process;
    
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
        
        process = ProcessHelper.RunInBackground
        (
            "run.bat",
            string.Join(' ', new[]
            { 
                "--api",
                uri.Host != "127.0.0.1" && uri.Host.ToLowerInvariant() != "localhost" ? " --listen" : "",
                "--port=" + uri.Port,
                "--ckpt-dir=\"" + SdCheckpointsHelper.BaseDir + "\"",
                "--lora-dir=\"" + SdLoraHelper.BaseDir + "\"",
                "--embeddings-dir=\"" + SdEmbeddingHelper.BaseDir + "\"",
                "--skip-load-model-at-start",
            }.Where(x => !string.IsNullOrEmpty(x))),

            directory: Path.Join(Application.StartupPath, @"external\StableDiffusion"),
            
            logFunc: s => log.WriteLine("[process] " + s),
            
            onExit: code =>
            {
                log.WriteLine("[process] Exit " + code);
            }
        );

        if (process != null) Program.Job.AddProcess(process.Id);
    }

    public static void Stop()
    {
        try { process?.Kill(true); } catch {}
        try { process?.WaitForExit(); } catch {}
    }
}