using System.Text.Json;
using AiPainter.Adapters.StableDiffusion;

namespace AiPainter.Helpers;

static class JsonIntoPngEmbedder
{
    class JsonInfo
    {
        public string checkpointName { get; set; } = "";
        public string vaeName { get; set; } = "";
        public string prompt { get; set; } = "";
        public string negative { get; set; } = "";
        public int steps { get; set; }
        public decimal cfgScale { get; set; }
        public long seed { get; set; } = -1;
        public decimal seedVariationStrength { get; set; }
        public int width { get; set; } = 512;
        public int height { get; set; } = 512;
        public string sampler { get; set; } = "";
        public decimal changesLevel { get; set; }
    }
    
    public static void ProcessDir(string dirPath)
    {
        foreach (var filePath in Directory.GetFiles(dirPath, "*.png", SearchOption.AllDirectories))
        {
            var jsonFilePath = Path.Combine(Path.GetDirectoryName(filePath)!, Path.GetFileNameWithoutExtension(filePath) + ".json");
            if (File.Exists(jsonFilePath))
            {
                var info = JsonSerializer.Deserialize<JsonInfo>(File.ReadAllText(jsonFilePath));
                if (info != null)
                {
                    SdPngHelper.Save
                    (
                        BitmapTools.Load(filePath),
                        new SdGenerationParameters
                        {
                            checkpointName = info.checkpointName,
                            vaeName = info.vaeName,
                            prompt = info.prompt,
                            negative = info.negative,
                            steps = info.steps,
                            cfgScale = info.cfgScale,
                            clipSkip = 1,
                            inpaintingFill = SdInpaintingFill.original,

                            seed = info.seed,
                            seedVariationStrength = info.seedVariationStrength,
                            width = info.width,
                            height = info.height,
                            sampler = info.sampler,
                            changesLevel = info.changesLevel,
                        },
                        filePath
                    );
                    File.Delete(jsonFilePath);
                }
            }
        }
    }
}
