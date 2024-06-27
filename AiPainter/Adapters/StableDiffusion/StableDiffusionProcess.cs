using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion;

static class StableDiffusionProcess
{
    private static Process? process;
    
    public static bool Loading { get; private set; }
    public static string ActiveCheckpoint { get; private set; }

    public static bool IsReady()
    {
        return ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl);
    }

    public static void Start(string checkpoint)
    {
        var log = StableDiffusionClient.Log;

        if (!Program.Config.UseEmbeddedStableDiffusion) return;

        if (ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.StableDiffusionUrl);
            return;
        }

        var pathToCheckpoint = SdCheckpointsHelper.GetPath(checkpoint);
        if (!File.Exists(pathToCheckpoint))
        {
            log.WriteLine($"Can't find {pathToCheckpoint}.");
            log.WriteLine("Please, download StableDiffusion model `sd-v1-4.ckpt` from HuggingFace site and save to that path.");
            log.WriteLine("https://huggingface.co/CompVis/stable-diffusion-v-1-4-original");

            if (MessageBox.Show
                (
                    $"Please, download StableDiffusion model `sd-v1-4.ckpt` from HuggingFace site and save to {pathToCheckpoint}. Open browser for HuggingFace site?",
                    "Error",
                    MessageBoxButtons.YesNo
                ) == DialogResult.Yes)
            {
                ProcessHelper.OpenUrlInBrowser("https://huggingface.co/CompVis/stable-diffusion-v-1-4-original");
            }
            return;
        }

        var pyvenvCfgFilePath = Path.Join(Application.StartupPath, @"external\StableDiffusion\venv\pyvenv.cfg");
        if (File.Exists(pyvenvCfgFilePath))
        {
            File.WriteAllLines(pyvenvCfgFilePath, File.ReadAllLines(pyvenvCfgFilePath).Select(x =>
            {
                if (x.TrimStart().StartsWith("home = "))
                {
                    return "home = " + Path.Join(Application.StartupPath, @"external\_stuff\python-3.10.6");
                }
                return x;
            }));
        }

        var uri = new Uri(Program.Config.StableDiffusionUrl);

        Loading = true;

        ActiveCheckpoint = checkpoint;

        var vaeFileName = SdCheckpointsHelper.GetConfig(checkpoint).vae;
        var pathToVaeFile = !string.IsNullOrEmpty(vaeFileName) ? Path.Join(Application.StartupPath, @"stable_diffusion_vae", vaeFileName) : null;
        var pathToLoraDir = Path.Join(Application.StartupPath, "stable_diffusion_lora");
        
        process = ProcessHelper.RunInBackground
        (
            "run.bat",
            "--api"
                + (uri.Host != "127.0.0.1" && uri.Host.ToLowerInvariant() != "localhost" ? " --listen" : "")
                + " --port=" + uri.Port
                + " --ckpt=\"" + pathToCheckpoint + "\""
                + (!string.IsNullOrEmpty(pathToVaeFile) ? " --vae-path=\"" + pathToVaeFile + "\"" : "")
                + " --lora-dir=\"" + pathToLoraDir + "\"",
            
            directory: Path.Join(Application.StartupPath, @"external\StableDiffusion"),
            
            env: new Dictionary<string, string?>
            {
                //{
                //    "PATH", 
                //    Path.Join(Application.StartupPath, @"external\_stuff\python-3.10.6") + ";" 
                //  + Path.Join(Application.StartupPath, @"external\_stuff\python-3.10.6\Scripts") + ";" 
                //  + Environment.GetEnvironmentVariable("PATH")
                //},
                //{ "PYTHON", null },
                //{ "GIT", null },
                //{ "VENV_DIR", null },
                //{ "COMMANDLINE_ARGS", null },
            },
            
            logFunc: s => log.WriteLine("[process] " + s),
            
            onExit: code =>
            {
                Loading = false;
                log.WriteLine("[process] Exit " + code);
            }
        );

        if (process != null) Program.Job.AddProcess(process.Id);
    }

    public static void Stop()
    {
        Loading = false;

        try { process?.Kill(true); } catch {}
        try { process?.WaitForExit(); } catch {}
    }
}