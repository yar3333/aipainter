using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion;

static class StableDiffusionProcess
{
    public static bool Loading { get; private set; }

    public static Process? Start()
    {
        var log = StableDiffusionClient.Log;

        if (!Program.Config.UseEmbeddedStableDiffusion) return null;

        if (ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.StableDiffusionUrl);
            return null;
        }

        var stableDiffusionModelPath = Path.Combine(Application.StartupPath, @"external\StableDiffusion\models\Stable-diffusion\model.ckpt");
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

        var uri = new Uri(Program.Config.StableDiffusionUrl);

        Loading = true;
        return ProcessHelper.RunInBackground
        (
            "webui.bat",
            "--api"
                + (uri.Host != "127.0.0.1" && uri.Host.ToLowerInvariant() != "localhost" ? " --listen" : "")
                + " --port=" + uri.Port,
            directory: Path.Join(Application.StartupPath, @"external\StableDiffusion"),
            env: new Dictionary<string, string>
            {
                { "PYTHON", null },
                { "GIT", null },
                { "VENV_DIR", null },
                { "COMMANDLINE_ARGS", null },
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