using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.InvokeAi;

static class InvokeAiProcess
{
    public static Process? Start()
    {
        var log = InvokeAiClient.Log;

        if (Program.Config.UseExternalInvokeAi) return null;

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

            if (MessageBox.Show
                (
                    "Please, install `CUDA Toolkit 11.4`. Open browser for NVIDIA site?",
                    "Error",
                    MessageBoxButtons.YesNo
                ) == DialogResult.Yes)
            {
                ProcessHelper.OpenUrlInBrowser("https://developer.nvidia.com/cuda-11-4-0-download-archive?target_os=Windows&target_arch=x86_64&target_version=10&target_type=exe_network");
            }

            return null;
        }

        var stableDiffusionModelPath = Path.Combine(Application.StartupPath, @"external\InvokeAI\models\ldm\stable-diffusion-v1\model.ckpt");
        if (!File.Exists(stableDiffusionModelPath))
        {
            log.WriteLine($"Can't find {stableDiffusionModelPath}.");
            log.WriteLine("Please, download StableDiffusion model `sd-v1-4.ckpt` from HuggingFace site and save to that path.");
            log.WriteLine("https://huggingface.co/CompVis/stable-diffusion-v-1-4-original");

            if (MessageBox.Show
                (
                    $"Please, download StableDiffusion model `sd-v1-4.ckpt` from HuggingFace site and save to {stableDiffusionModelPath}. Open browser for HuggingFace site?",
                    "Error",
                    MessageBoxButtons.YesNo
                ) == DialogResult.Yes)
            {
                ProcessHelper.OpenUrlInBrowser("https://huggingface.co/CompVis/stable-diffusion-v-1-4-original");
            }

            return null;
        }

        /*
        --web_develop - Starts the web server in development mode.
        --web_verbose - Enables verbose logging
        --cors [CORS ...] - Additional allowed origins, comma-separated
        --host HOST - Web server: Host or IP to listen on. Set to 0.0.0.0 to accept traffic from other devices on your network.
        --port PORT - Web server: Port to listen on
        --gui - Start InvokeAI GUI - This is the "desktop mode" version of the web app. It uses Flask to create a desktop app experience of the webserver.            
        */

        var baseDir = Path.Join(Application.StartupPath, "external", "InvokeAI");

        return ProcessHelper.RunInBackground
        (
            Path.Join("legacy_api", "aipainter_invokeai.exe"),
            "--web"
                + " --host=" + new Uri(Program.Config.InvokeAiUrl).Host
                + " --port=" + new Uri(Program.Config.InvokeAiUrl).Port,
            directory: baseDir,
            env: new Dictionary<string, string>
            {
                {
                    "PATH",
                    Path.Join(baseDir, "stuff", "NVIDIA GPU Computing Toolkit_CUDA_v11.4", "bin") + ";" 
                        + Environment.GetEnvironmentVariable("PATH")
                }
            },
            logFunc: s => log.WriteLine("[process] " + s),
            onExit: code => log.WriteLine("[process] Exit " + code)
        );
    }
}