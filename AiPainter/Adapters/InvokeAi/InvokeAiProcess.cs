using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.InvokeAi;

static class InvokeAiProcess
{
    public static bool Loading { get; private set; }

    public static Process? Start()
    {
        var log = InvokeAiClient.Log;

        if (!Program.Config.UseEmbeddedInvokeAi) return null;

        if (ProcessHelper.IsPortOpen(Program.Config.InvokeAiUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.InvokeAiUrl);
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

        --outdir dir
        --prompt_as_dir
        */

        var baseDir = Path.Join(Application.StartupPath, "external", "InvokeAI");

        Loading = true;
        return ProcessHelper.RunInBackground
        (
            Path.Join("legacy_api", "aipainter_invokeai.exe"),
            "--web"
                + " --host=" + new Uri(Program.Config.InvokeAiUrl).Host
                + " --port=" + new Uri(Program.Config.InvokeAiUrl).Port
                + " --outdir=" + Path.Combine(Application.StartupPath, Program.Config.InvokeAiOutputFolderPath),
            directory: baseDir,
            env: new Dictionary<string, string>
            {
                {
                    "PATH", Path.Join(baseDir, "stuff", "NVIDIA GPU Computing Toolkit_CUDA_v11.4", "bin") + ";" 
                                + Environment.GetEnvironmentVariable("PATH")
                },
                { "TORCH_HOME", Path.Join(baseDir, "stuff", "models") },
                { "XDG_CACHE_HOME", Path.Join(baseDir, "stuff", "models") },

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