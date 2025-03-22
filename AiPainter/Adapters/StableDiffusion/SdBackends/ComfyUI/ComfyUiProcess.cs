using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using AiPainter.Helpers;
using Microsoft.VisualBasic.Logging;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyUiProcess
{
    private static Process? process;

    public static bool Running { get; private set; }

    //public static string ActiveCheckpointFilePath { get; private set; } = "";
    //public static string ActiveVaeFilePath { get; private set; } = "";

    //public static Action<int>? OnUpscaleProgress = null;

    public static bool IsReady()
    {
        return ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl);
    }

    public static void Start()
    {
        var log = ComfyUiApiClient.Log;

        if (!Program.Config.UseEmbeddedStableDiffusion)
            return;

        if (ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.StableDiffusionUrl);
            return;
        }

        var uri = new Uri(Program.Config.StableDiffusionUrl);

        Running = true;

        runProcess(log, uri);

        if (process != null)
        {
            Program.Job.AddProcess(process.Id);
        }
    }

    public static void Stop()
    {
        Running = false;

        try
        {
            process?.Kill(true);
        }
        catch { }
        try
        {
            process?.WaitForExit();
        }
        catch { }
    }

    private static void runProcess(Log log, Uri uri)
    {
        log.WriteLine("GPU Name = " + GpuInfo.GetName());
        log.WriteLine("GPU AdapterCompatibility = " + GpuInfo.GetAdapterCompatibility());

        process = ProcessHelper.RunInBackground(
            "run.bat",
            string.Join(
                ' ',
                new[]
                {
                    "--disable-auto-launch",
                    "--listen " + uri.Host.ToLowerInvariant(),
                    "--port " + uri.Port,

                    //--gpu-only            Store and run everything (text encoders/CLIP models, etc... on the GPU).
                    //--highvram            By default models will be unloaded to CPU memory after being used. This option keeps them in GPU memory.
                    //--normalvram          Used to force normal vram use if lowvram gets automatically enabled.
                    //--lowvram             Split the unet in parts to use less vram.
                    //--novram              When lowvram isn't enough.
                    //--cpu                 To use the CPU for everything (slow).
                }.Where(x => !string.IsNullOrEmpty(x))
            ),
            directory: Path.Join(
                Application.StartupPath,
                GpuInfo.IsAmd ? @"external\ComfyUI-Zluda" : @"external\ComfyUI"
            ),
            logFunc: s =>
            {
                if (s != null)
                {
                    log.WriteLine("[process] " + s);
                    //var m = Regex.Match(s, @"^tiled upscale:\s*(\d+)%");
                    //if (m.Success) OnUpscaleProgress?.Invoke(int.Parse(m.Groups[1].Value));
                }
            },
            onExit: code =>
            {
                Running = false;
                log.WriteLine("[process] Exit " + code);
            }
        );
    }
}
