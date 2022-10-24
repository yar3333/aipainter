using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.RemBg;

static class RemBgProcess
{
    public static bool Loading { get; private set; }

    public static Process? Start()
    {
        var log = RemBgClient.Log;

        if (!Program.Config.UseEmbeddedRemBg) return null;

        if (ProcessHelper.IsPortOpen(Program.Config.RemBgUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.RemBgUrl);
            return null;
        }
        
        Loading = true;
        return ProcessHelper.RunInBackground
        (
            Path.Join("aipainter_rembg", "aipainter_rembg.exe"),
            "s --port=" + new Uri(Program.Config.RemBgUrl).Port,
            directory: Path.Join(Application.StartupPath, @"external\rembg"),
            env: new Dictionary<string, string>
            {
                {
                    "PATH",
                      Path.Join(Application.StartupPath, @"external\_stuff\cudnn-windows-x86_64-8.4.1.50_cuda11.6\bin") + ";"
                    + Path.Join(Application.StartupPath, @"external\_stuff\TensorRT-8.4.2.4\lib") + ";"
                    + Path.Join(Application.StartupPath, @"external\_stuff\NVIDIA GPU Computing Toolkit_CUDA_v11.4\bin") + ";"
                    + Environment.GetEnvironmentVariable("PATH")
                },
                {
                    "U2NET_HOME", Path.Join(Application.StartupPath, @"external\rembg\stuff\.u2net")
                },
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