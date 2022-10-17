using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.LamaCleaner;

static class LamaCleanerProcess
{
    public static Process? Start()
    {
        var log = LamaCleanerClient.Log;

        if (Program.Config.UseExternalLamaCleaner) return null;

        if (ProcessHelper.IsPortOpen(Program.Config.LamaCleanerUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.LamaCleanerUrl);
            return null;
        }
        
        return ProcessHelper.RunInBackground
        (
            Path.Join("main", "aipainter_lamacleaner.exe"),
            "--model=lama"
                + " --device=cpu"
                + " --port=" + +new Uri(Program.Config.LamaCleanerUrl).Port,
            directory: Path.Join(Application.StartupPath, "external", "lama-cleaner"),
            logFunc: s => log.WriteLine("[process] " + s),
            onExit: code => log.WriteLine("[process] Exit " + code)
        );
    }
}