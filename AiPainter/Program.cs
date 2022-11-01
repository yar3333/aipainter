using System.Text.Json;
using AiPainter.Adapters.LamaCleaner;
using AiPainter.Adapters.RemBg;
using AiPainter.Adapters.StableDiffusion;

namespace AiPainter;

static class Program
{
    public static Config Config = new();
    public static readonly Log Log = new("_general");

    [STAThread]
    static void Main()
    {
        readConfig();

        var stableDiffusionProcess = StableDiffusionProcess.Start();
        var lamaCleanerProcess = LamaCleanerProcess.Start();
        var remBgProcess = RemBgProcess.Start();

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());

        try { remBgProcess?.Kill(true); } catch {}
        try { lamaCleanerProcess?.Kill(true); } catch {}
        try { stableDiffusionProcess?.Kill(true); } catch {}
    }
    
    static void readConfig()
    {
        var pathToConfig = Path.Join(Application.StartupPath, "Config.json");

        var oldText = "";
        
        if (File.Exists(pathToConfig))
        {
            oldText = File.ReadAllText(pathToConfig);
            Config = JsonSerializer.Deserialize<Config>(oldText) ?? new Config();
        }

        var newText = JsonSerializer.Serialize(Config, new JsonSerializerOptions { PropertyNamingPolicy = null, WriteIndented = true });

        if (newText != oldText)
        {
            File.WriteAllText(pathToConfig, newText);
        }
    }
}
