using AiPainter.Helpers;
using System.Globalization;

namespace AiPainter.Adapters.StableDiffusion;

static class SdPngHelper
{
    public static Bitmap Load(string filePath, out SdGenerationParameters? generationParameters)
    {
        var data = File.ReadAllBytes(filePath);
        var chunks = PngHelper.GetTextChunks(data);
        generationParameters = chunks.ContainsKey("parameters") ? loadParameters(chunks["parameters"]) : null;
        return (Bitmap)Image.FromStream(new MemoryStream(data));
    }    
    
    public static SdGenerationParameters? LoadGenerationParameters(string filePath)
    {
        var data = File.ReadAllBytes(filePath);
        var chunks = PngHelper.GetTextChunks(data);
        return chunks.ContainsKey("parameters") ? loadParameters(chunks["parameters"]) : null;
    }

    public static void Save(Bitmap image, SdGenerationParameters src, long seed, string filePath)
    {
        var pngData = PngHelper.EncodeImage(image, new Dictionary<string, string>
        {
            { "parameters", saveParameters(src, seed) }
        });
        File.WriteAllBytes(filePath, pngData);
    }

    private static SdGenerationParameters? loadParameters(string text)
    {
        // wooden house\n
        // Steps: 20, Sampler: DPM++ 2M, Schedule type: Karras, CFG scale: 7, Seed: 3654104940, Size: 512x512, Model hash: 6ce0161689, Model: v1-5-pruned-emaonly, Version: v1.9.4

        var lines = text.Split('\n');

        if (lines.Length == 0) return null;

        var generationParameters = new SdGenerationParameters();

        generationParameters.prompt = lines[0];

        foreach (var line in lines.Skip(1))
        {
            if (line.ToLowerInvariant().StartsWith("negative prompt:"))
            {
                generationParameters.negative = line.Substring("negative prompt:".Length).TrimStart(' ');
            }
            else
            {
                var pp = line.Split(',').Select(x => x.Trim()).ToArray();
                foreach (var p in pp)
                {
                    var m = p.IndexOf(':');
                    if (m <= 0) continue; 
            
                    var k = p.Substring(0, m).Trim();
                    var v = p.Substring(m + 1).Trim();
                    switch (k.ToLowerInvariant())
                    {
                        case "steps":
                            generationParameters.steps = parseInt(v) ?? generationParameters.steps;
                            break;

                        case "sampler":
                            // result must be one of: "Euler a", "DPM++ 2M", "Heun"
                            if      (v.ToLowerInvariant() == "dpm++ 2m")     generationParameters.sampler = "DPM++ 2M";
                            else if (v.ToLowerInvariant().Contains("euler")) generationParameters.sampler = "Euler a";
                            break;

                        case "cfg scale":
                            generationParameters.cfgScale = parseDecimal(v) ?? generationParameters.cfgScale;
                            break;

                        case "seed":
                            generationParameters.seed = parseLong(v) ?? generationParameters.seed;
                            break;

                        case "size":
                            generationParameters.width  = parseInt(v.ToLowerInvariant().Split("x").FirstOrDefault()) ?? generationParameters.width;
                            generationParameters.height = parseInt(v.ToLowerInvariant().Split("x").LastOrDefault())  ?? generationParameters.height;
                            break;

                        case "model":
                            break;

                        case "aip_model":
                            generationParameters.checkpointName = v;
                            break;
                    }
                }
            }
        }

        return generationParameters;
    }

    private static string saveParameters(SdGenerationParameters generationParameters, long seed)
    {
        return generationParameters.prompt.Replace('\n', ' ') + "\n"
               + (!string.IsNullOrEmpty(generationParameters.negative) ? "Negative prompt: " + generationParameters.negative + "\n" : "")
               + string.Join(", ",
                    "Steps: " + generationParameters.steps,
                    "Sampler: " + generationParameters.sampler,
                    "CFG scale: " + generationParameters.cfgScale.ToString(CultureInfo.InvariantCulture),
                    "Seed: " + seed,
                    "Size: " + generationParameters.width + "x" + generationParameters.height,
                    "aip_Model: " + generationParameters.checkpointName
                );
    }

    private static decimal? parseDecimal(string? s)
    {
        return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var r) ? r : null;
    }

    private static int? parseInt(string? s)
    {
        return int.TryParse(s, out var r) ? r : null;
    }

    private static long? parseLong(string? s)
    {
        return long.TryParse(s, out var r) ? r : null;
    }
}
