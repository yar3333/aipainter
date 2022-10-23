using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.LamaCleaner;

static class LamaCleanerProcess
{
    public static bool Loading { get; private set; }

    public static Process? Start()
    {
        var log = LamaCleanerClient.Log;

        if (!Program.Config.UseEmbeddedLamaCleaner) return null;

        if (ProcessHelper.IsPortOpen(Program.Config.LamaCleanerUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.LamaCleanerUrl);
            return null;
        }

        var baseDir = Path.Join(Application.StartupPath, "external", "lama-cleaner");
        
        Loading = true;
        return ProcessHelper.RunInBackground
        (
            Path.Join("main", "aipainter_lamacleaner.exe"),
            "--model=lama"
                + " --device=cpu"
                + " --port=" + +new Uri(Program.Config.LamaCleanerUrl).Port,
            directory: baseDir,
            env: new Dictionary<string, string>
            {
                { "TORCH_HOME", Path.Join(baseDir, "stuff", "models") }
            },
            logFunc: s => log.WriteLine("[process] " + s),
            onExit: code =>
            {
                Loading = false;
                log.WriteLine("[process] Exit " + code);
            }
        );
    }
}