using System.Diagnostics;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion;

static class StableDiffusionProcess
{
    private static Process? process;
    
    public static bool Loading { get; private set; }
    public static string ActiveCheckpointFilePath { get; private set; }

    public static bool IsReady()
    {
        return ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl);
    }

    public static void Start(string checkpointFilePath, string? vaeFilePath)
    {
        var log = StableDiffusionClient.Log;

        if (!Program.Config.UseEmbeddedStableDiffusion) return;

        if (ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl))
        {
            log.WriteLine("Port are busy: " + Program.Config.StableDiffusionUrl);
            return;
        }

        if (!File.Exists(checkpointFilePath))
        {
            log.WriteLine($"Can't find {checkpointFilePath}.");
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

        ActiveCheckpointFilePath = checkpointFilePath;

        //var pathToVaeFile = !string.IsNullOrEmpty(vaeFilePath) ? Path.Join(Application.StartupPath, @"stable_diffusion_vae", vaeFilePath) : null;
        var pathToLoraDir = Path.Join(Application.StartupPath, "stable_diffusion_lora");
        
        process = ProcessHelper.RunInBackground
        (
            "run.bat",
            "--api"
                + (uri.Host != "127.0.0.1" && uri.Host.ToLowerInvariant() != "localhost" ? " --listen" : "")
                + " --port=" + uri.Port
                + " --ckpt=\"" + checkpointFilePath + "\""
                + (!string.IsNullOrEmpty(vaeFilePath) ? " --vae-path=\"" + vaeFilePath + "\"" : "")
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