using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.LamaCleaner;

static class LamaCleanerProcess
{
    private static Process? process;
    
    public static bool Loading { get; private set; }

    public static void Start()
    {
        var log = LamaCleanerClient.Log;

        if (!Program.Config.UseEmbeddedLamaCleaner) return;

        if (ProcessHelper.IsPortOpen(Program.Config.LamaCleanerUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.LamaCleanerUrl);
            return;
        }

        Loading = true;

        process = ProcessHelper.RunInBackground
        (
            Path.Join("main", "aipainter_lamacleaner.exe"),
                  "--model=lama"
                + " --device=cpu"
                + " --port=" + +new Uri(Program.Config.LamaCleanerUrl).Port,
            directory: Path.Join(Application.StartupPath, @"external\lama-cleaner"),
            env: new Dictionary<string, string?>
            {
                { "TORCH_HOME", Path.Join(Application.StartupPath, @"external\lama-cleaner\stuff\models") }
            },
            logFunc: s => log.WriteLine("[process] " + s),
            onExit: code =>
            {
                Loading = false;
                log.WriteLine("[process] Exit " + code);
            }
        );
    }

    public static void Stop()
    {
        try { process?.Kill(true); } catch {}
    }
}