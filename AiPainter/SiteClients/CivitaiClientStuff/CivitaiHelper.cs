using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using System.Text.RegularExpressions;

namespace AiPainter.SiteClients.CivitaiClientStuff;

static class CivitaiHelper
{
    public static string? GetCheckpointAuthorizationBearer(string name)
    {
        return new Uri(SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl!).Host.ToLowerInvariant() == "civitai.com" 
                   ? Program.Config.CivitaiApiKey 
                   : null;
    }

    public static async Task UpdateAsync(Log log)
    {
        log.WriteLine("Start updating metadata from civitai.com");

        await updateCheckpointsAsync(log);
    }

    public static bool ParseUrl(string? url, out string? modelId, out string? versionId)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            modelId = null;
            versionId = null;
            return false;
        }

        var uri = new Uri(url);

        var m1 = Regex.Match(uri.PathAndQuery, @"models/(\d+)\?modelVersionId=(\d+)");
        if (m1.Success)
        {
            modelId = m1.Groups[1].Value;
            versionId = m1.Groups[2].Value;
            return true;
        }

        var m2 = Regex.Match(uri.LocalPath, @"models/(\d+)");
        if (m2.Success)
        {
            modelId = m2.Groups[1].Value;
            versionId = null;
            return true;
        }

        modelId = null;
        versionId = null;
        return false;
    }

    public static async Task<Tuple<CivitaiModel, CivitaiVersion>?> LoadModelDataAsync(string? modelId, string? versionId)
    {
        if (string.IsNullOrEmpty(modelId)) return null;

        var model = await CivitaiClient.GetModelAsync(modelId);

        if (versionId == null)
        {
            versionId = model.modelVersions.FirstOrDefault()?.id.ToString();
            if (versionId == null)
            {
                return null;
            }
        }

        var version = model.modelVersions.Single(x => x.id.ToString() == versionId);

        return Tuple.Create(model, version);
    }

    public static Tuple<string, SdCheckpointConfig> DataToCheckpointConfig(CivitaiModel model, CivitaiVersion version)
    {
        var name = CivitaiParserHelper.GetCheckpointName(model.name, version.name);
        
        var config = new SdCheckpointConfig();

        //config.homeUrl = "https://civitai.com/models/" + version. + "?modelVersionId=" + versionId;
        
        config.promptRequired = "";
        config.promptSuggested = "";
        if (version.trainedWords != null)
        {
            CivitaiParserHelper.ParsePhrases(string.Join(", ", version.trainedWords), out var reqWords, out var sugWords);
            config.promptRequired = reqWords;
            config.promptSuggested = sugWords;
        }

        config.description = "";
        if (model.tags != null)
        {
            config.description = string.Join(", ", model.tags);
        }

        config.mainCheckpointUrl = CivitaiParserHelper.GetBestModelDownloadUrl(version.files, "Model");
        config.inpaintCheckpointUrl = CivitaiParserHelper.GetInpaintDownloadUrl(model, version);
        config.vaeUrl = CivitaiParserHelper.GetBestModelDownloadUrl(version.files, "VAE");

        //config.clipSkip = 1;
        
        config.baseModel = version.baseModel;

        return Tuple.Create(name, config);
    }

    private static async Task updateCheckpointsAsync(Log log)
    {
        foreach (var name in SdCheckpointsHelper.GetNames("").Where(x => x != "").ToArray())
        {
            log.WriteLine("Process checkpoint: " + name);
            
            var config = SdCheckpointsHelper.GetConfig(name);
            
            if (!ParseUrl(config.homeUrl, out var modelId, out var versionId))
            {
                log.WriteLine("  homeUrl not specified");
                continue;
            }
            
            var modelAndVersion = await LoadModelDataAsync(modelId, versionId);
            if (modelAndVersion == null)
            {
                log.WriteLine("  bad homeUrl or load error");
                continue;
            }

            var newConfig = DataToCheckpointConfig(modelAndVersion.Item1, modelAndVersion.Item2).Item2;

            config.description = newConfig.description;
            config.promptRequired = newConfig.promptRequired;
            config.promptSuggested = newConfig.promptSuggested;
            config.mainCheckpointUrl = newConfig.mainCheckpointUrl;
            config.inpaintCheckpointUrl = newConfig.inpaintCheckpointUrl;
            config.vaeUrl = newConfig.vaeUrl;
            config.baseModel = normalizeBaseModelName(newConfig.baseModel);

            SdCheckpointsHelper.SaveConfig(name, config);
        }
    }

    private static string? normalizeBaseModelName(string? name)
    {
        switch (name)
        {
            case "SDXL 1.0": return "SDXL-1.0";
            case "SD 1.5": return "SD-1.5";
            case "Pony": return "PonyXL";
        }
        return name;
    }
}
