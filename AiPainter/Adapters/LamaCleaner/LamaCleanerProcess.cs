using System.Diagnostics;

namespace AiPainter.Adapters.LamaCleaner;

static class LamaCleanerProcess
{
    public static Process? Start()
    {
        var log = LamaCleanerClient.Log;

        if (Program.Config.UseExternalLamaCleaner) return null;

        return ProcessRunner.RunInBackground
        (
            Path.Join("main", "main.exe"),
            "--model=lama"
          + " --device=cpu"
          + " --port=" + +new Uri(Program.Config.LamaCleanerUrl).Port,
            directory: Path.Join(Application.StartupPath, "external", "lama-cleaner"),
            logFunc: s => log.WriteLine("[process] " + s),
            onExit: code => log.WriteLine("[process] Exit " + code)
        );
    }
}