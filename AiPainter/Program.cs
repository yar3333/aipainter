using System.Diagnostics;
using System.Text.Json;

namespace AiPainter;

static class Program
{
    public static Config Config = new();

    [STAThread]
    static void Main()
    {
        var pathToConfig = Path.Join(Application.StartupPath, "Config.json");
        if (File.Exists(pathToConfig)) Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(pathToConfig))!;
        else
            File.WriteAllText
            (
                pathToConfig,
                JsonSerializer.Serialize
                    (Config, new JsonSerializerOptions { PropertyNamingPolicy = null, WriteIndented = true })
            );

        var logFile = File.Open
            (Path.Join(Application.StartupPath, "_trace.log"), FileMode.Create, FileAccess.Write, FileShare.Read);
        var log = new StreamWriter(logFile);

        Process? invokeAiProcess = null;
        if (!Config.UseExternalInvokeAi)
        {
            var nvidiaDir = @"c:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.4\bin";
            if (!Directory.Exists(nvidiaDir))
            {
                log.WriteLine
                (
                    @"Can't find `c:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.4\bin`. Install `CUDA Toolkit 11.4`."
                );
                log.WriteLine("Install `CUDA Toolkit 11.4`.");
                log.WriteLine
                (
                    "https://developer.nvidia.com/cuda-11-4-0-download-archive?target_os=Windows&target_arch=x86_64&target_version=10&target_type=exe_network"
                );
                log.Close();
                if (MessageBox.Show
                    (
                        "Please, install `CUDA Toolkit 11.4`. Open browser for NVIDIA site?",
                        "Error",
                        MessageBoxButtons.YesNo
                    ) == DialogResult.Yes)
                {
                    Process.Start
                    (
                        new ProcessStartInfo
                        {
                            FileName = "https://developer.nvidia.com/cuda-11-4-0-download-archive?target_os=Windows&target_arch=x86_64&target_version=10&target_type=exe_network",
                            UseShellExecute = true
                        }
                    );
                }

                return;
            }

            var stableDiffusionModelPath = Path.Combine(Application.StartupPath, @"external\InvokeAi\models\ldm\stable-diffusion-v1\model.ckpt");
            if (!File.Exists(stableDiffusionModelPath))
            {
                log.WriteLine($"Can't find {stableDiffusionModelPath}.");
                log.WriteLine("Please, download StableDiffusion model `sd-v1-4.ckpt` from HuggingFace site and save to that path.");
                log.WriteLine("https://huggingface.co/CompVis/stable-diffusion-v-1-4-original");
                log.Close();
                if (MessageBox.Show
                    (
                        $"Please, download StableDiffusion model `sd-v1-4.ckpt` from HuggingFace site and save to {stableDiffusionModelPath}. Open browser for HuggingFace site?",
                        "Error",
                        MessageBoxButtons.YesNo
                    ) == DialogResult.Yes)
                {
                    Process.Start
                    (
                        new ProcessStartInfo
                        {
                            FileName = "https://huggingface.co/CompVis/stable-diffusion-v-1-4-original",
                            UseShellExecute = true
                        }
                    );
                }

                return;
            }

            /*
            --web_develop - Starts the web server in development mode.
            --web_verbose - Enables verbose logging
            --cors [CORS ...] - Additional allowed origins, comma-separated
            --host HOST - Web server: Host or IP to listen on. Set to 0.0.0.0 to accept traffic from other devices on your network.
            --port PORT - Web server: Port to listen on
            --gui - Start InvokeAI GUI - This is the "desktop mode" version of the web app. It uses Flask to create a desktop app experience of the webserver.            
            */

            invokeAiProcess = ProcessRunner.RunInBackground
            (
                Path.Join("dream", "dream.exe"),
                "--web"
              + " --host=" + new Uri(Config.InvokeAiUrl).Host
              + " --port=" + new Uri(Config.InvokeAiUrl).Port,
                directory: Path.Join(Application.StartupPath, "external", "InvokeAi"),
                env: new Dictionary<string, string>
                {
                    {
                        "PATH",
                        @"c:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.4\bin;"
                            + Environment.GetEnvironmentVariable("PATH")
                    }
                },
                logFunc: s =>
                {
                    lock (log)
                    {
                        log.WriteLine("[InvokeAi] " + s);
                        log.Flush();
                    }
                },
                onExit: code =>
                {
                    lock (log)
                    {
                        log.WriteLine("[InvokeAi] Exit " + code);
                        log.Flush();
                    }
                }
            );
        }

        Process? lamaCleanerProcess = null;
        if (!Config.UseExternalLamaCleaner)
        {
            lamaCleanerProcess = ProcessRunner.RunInBackground
            (
                Path.Join("main", "main.exe"),
                "--model=lama"
              + " --device=cpu"
              + " --port=" + +new Uri(Config.LamaCleanerUrl).Port,
                directory: Path.Join(Application.StartupPath, "external", "lama-cleaner"),
                logFunc: s =>
                {
                    lock (log)
                    {
                        log.WriteLine("[lama-cleaner] " + s);
                        log.Flush();
                    }
                },
                onExit: code =>
                {
                    lock (log)
                    {
                        log.WriteLine("[lama-cleaner] Exit " + code);
                        log.Flush();
                    }
                }
            );
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());

        try { lamaCleanerProcess?.Kill(true); } catch {}
        try { invokeAiProcess?.Kill(true); } catch {}
    }
}
