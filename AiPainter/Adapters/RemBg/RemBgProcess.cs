using System.Diagnostics;

namespace AiPainter.Adapters.RemBg;

static class RemBgProcess
{
    public static Process? Start()
    {
        var log = RemBgClient.Log;

        if (Program.Config.UseExternalRemBg) return null;

        var baseDir = Path.Join(Application.StartupPath, "external", "rembg");

        return ProcessRunner.RunInBackground
        (
            Path.Join("main", "main.exe"),
            "s --port=" + new Uri(Program.Config.RemBgUrl).Port,
            directory: baseDir,
            env: new Dictionary<string, string>
            {
                {
                    "PATH",
                    Path.Join(baseDir, @"cudnn-windows-x86_64-8.4.1.50_cuda11.6\bin") + ";"
                      + Path.Join(baseDir, @"TensorRT-8.4.2.4\lib") + ";"
                      + Environment.GetEnvironmentVariable("PATH")
                }
            },
            logFunc: s => log.WriteLine("[process] " + s),
            onExit: code => log.WriteLine("[process] Exit " + code)
        );
    }
}