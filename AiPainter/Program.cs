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
        LoadConfig();
        SaveConfig();

        StableDiffusionProcess.Start();
        LamaCleanerProcess.Start();
        RemBgProcess.Start();

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());

        RemBgProcess.Stop();
        LamaCleanerProcess.Stop();
        StableDiffusionProcess.Stop();
    }
    
    public static void LoadConfig()
    {
        var pathToConfig = Path.Join(Application.StartupPath, "Config.json");
        
        if (File.Exists(pathToConfig))
        {
            Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(pathToConfig)) ?? new Config();
        }
    }
    
    public static void SaveConfig()
    {
        var pathToConfig = Path.Join(Application.StartupPath, "Config.json");

        var oldText = File.Exists(pathToConfig) ? File.ReadAllText(pathToConfig) : "";
        var newText = JsonSerializer.Serialize(Config, new JsonSerializerOptions { PropertyNamingPolicy = null, WriteIndented = true });

        if (newText != oldText)
        {
            File.WriteAllText(pathToConfig, newText);
        }
    }
}
