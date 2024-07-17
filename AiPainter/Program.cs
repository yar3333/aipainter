using AiPainter.Adapters.LamaCleaner;
using AiPainter.Adapters.StableDiffusion;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Helpers;
using AiPainter.SiteClients.CivitaiClientStuff;

namespace AiPainter;

static class Program
{
    public static readonly GlobalConfig Config = GlobalConfig.Load();
    public static readonly Log Log = new("_general");
    public static readonly Job Job = new();

    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            StableDiffusionProcess.Start(SdCheckpointsHelper.GetPathToMainCheckpoint(Config.StableDiffusionCheckpoint), SdVaeHelper.GetPathToVae(Config.StableDiffusionVae));
            LamaCleanerProcess.Start();

            ApplicationConfiguration.Initialize();

            var form = new MainForm();
            Config.MainWindowPosition?.ApplyToForm(form);
            Application.Run(form);
            Config.Save();

            LamaCleanerProcess.Stop();
            StableDiffusionProcess.Stop();
        }
        else if (DataTools.IsSequencesEqual(args, new []{ "--update-metadata-from-civitai" }))
        {
            CivitaiHelper.UpdateAsync(Log).Wait();
        }
    }
}
