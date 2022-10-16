using System.Text.Json;
using AiPainter.Adapters.InvokeAi;
using AiPainter.Adapters.LamaCleaner;
using AiPainter.Adapters.RemBg;

namespace AiPainter;

static class Program
{
    public static Config Config = new();

    [STAThread]
    static void Main()
    {
        readConfig();

        var invokeAiProcess = InvokeAiProcess.Start();
        var lamaCleanerProcess = LamaCleanerProcess.Start();
        var remBgProcess = RemBgProcess.Start();

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());

        try { remBgProcess?.Kill(true); } catch {}
        try { lamaCleanerProcess?.Kill(true); } catch {}
        try { invokeAiProcess?.Kill(true); } catch {}
    }
    
    static void readConfig()
    {
        var pathToConfig = Path.Join(Application.StartupPath, "Config.json");
        
        if (File.Exists(pathToConfig)) Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(pathToConfig))!;
        else
        {
            File.WriteAllText
            (
                pathToConfig,
                JsonSerializer.Serialize
                    (Config, new JsonSerializerOptions { PropertyNamingPolicy = null, WriteIndented = true })
            );
        }
    }
}
