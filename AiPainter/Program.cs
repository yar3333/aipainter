using System.Text.Json;
using AiPainter.Adapters.LamaCleaner;
using AiPainter.Adapters.StableDiffusion;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Helpers;
using AiPainter.SiteClients.CivitaiClientStuff;

namespace AiPainter;

static class Program
{
    public static Config Config = new();
    public static readonly Log Log = new("_general");
    public static readonly Job Job = new();

    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            LoadConfig();
            SaveConfig();

            StableDiffusionProcess.Start(SdCheckpointsHelper.GetPathToMainCheckpoint(Config.StableDiffusionCheckpoint), SdVaeHelper.GetPathToVae(Config.StableDiffusionVae));
            LamaCleanerProcess.Start();

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            LamaCleanerProcess.Stop();
            StableDiffusionProcess.Stop();
        }
        else if (DataTools.IsSequencesEqual(args, new []{ "--update-metadata-from-civitai" }))
        {
            LoadConfig();
            CivitaiHelper.UpdateAsync(Log).Wait();
        }
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
