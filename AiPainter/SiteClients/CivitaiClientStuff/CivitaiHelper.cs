using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;

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
        await updateLorasAsync(log);
        await updateEmbeddingsAsync(log);
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

    public static SdCheckpointConfig DataToCheckpointConfig(CivitaiModel model, CivitaiVersion version)
    {
        var config = new SdCheckpointConfig();

        config.homeUrl = "https://civitai.com/models/" + model.id + "?modelVersionId=" + version.id;
        
        config.promptRequired = "";
        config.promptSuggested = "";
        if (version.trainedWords != null)
        {
            CivitaiParserHelper.ParsePhrases(string.Join(", ", version.trainedWords), out var reqWords, out var sugWords);
            config.promptRequired = reqWords;
            config.promptSuggested = sugWords;
        }

        config.description = model.tags != null ? string.Join(", ", model.tags) : "";
        config.mainCheckpointUrl = CivitaiParserHelper.GetBestModelDownloadUrl(version.files, "Model");
        config.inpaintCheckpointUrl = CivitaiParserHelper.GetInpaintDownloadUrl(model, version);
        config.vaeUrl = CivitaiParserHelper.GetBestModelDownloadUrl(version.files, "VAE");
        //config.clipSkip = 1;
        config.baseModel = CivitaiParserHelper.NormalizeBaseModelName(version.baseModel);

        return config;
    }

    private static async Task updateCheckpointsAsync(Log log)
    {
        foreach (var name in SdCheckpointsHelper.GetNames("").Where(x => x != "").ToArray())
        {
            log.WriteLine("Process Checkpoint: " + name);
            
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

            var newConfig = DataToCheckpointConfig(modelAndVersion.Item1, modelAndVersion.Item2);

            config.description = newConfig.description;
            config.promptRequired = newConfig.promptRequired;
            config.promptSuggested = newConfig.promptSuggested;
            config.mainCheckpointUrl = newConfig.mainCheckpointUrl;
            config.inpaintCheckpointUrl = newConfig.inpaintCheckpointUrl;
            config.vaeUrl = newConfig.vaeUrl;
            config.baseModel = newConfig.baseModel;

            SdCheckpointsHelper.SaveConfig(name, config);
        }
    }

    public static SdLoraConfig DataToLoraConfig(CivitaiModel model, CivitaiVersion version)
    {
        var config = new SdLoraConfig();

        config.homeUrl = "https://civitai.com/models/" + model.id + "?modelVersionId=" + version.id;
        
        config.promptRequired = "";
        config.promptSuggested = "";
        if (version.trainedWords != null)
        {
            CivitaiParserHelper.ParsePhrases(string.Join(", ", version.trainedWords), out var reqWords, out var sugWords);
            config.promptRequired = reqWords;
            config.promptSuggested = sugWords;
        }

        config.description = model.tags != null ? string.Join(", ", model.tags) : "";
        config.downloadUrl = CivitaiParserHelper.GetBestModelDownloadUrl(version.files, "Model");
        config.baseModel = CivitaiParserHelper.NormalizeBaseModelName(version.baseModel);
        
        return config;
    }

    private static async Task updateLorasAsync(Log log)
    {
        foreach (var name in SdLoraHelper.GetNames())
        {
            log.WriteLine("Process LoRA: " + name);
            
            var config = SdLoraHelper.GetConfig(name);
            
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

            var newConfig = DataToLoraConfig(modelAndVersion.Item1, modelAndVersion.Item2);

            config.description = newConfig.description;
            config.promptRequired = newConfig.promptRequired;
            config.promptSuggested = newConfig.promptSuggested;
            config.downloadUrl = newConfig.downloadUrl;
            config.baseModel = newConfig.baseModel;

            SdLoraHelper.SaveConfig(name, config);
        }
    }

    public static SdEmbeddingConfig DataToEmbeddingConfig(CivitaiModel model, CivitaiVersion version)
    {
        var config = new SdEmbeddingConfig();

        config.homeUrl = "https://civitai.com/models/" + model.id + "?modelVersionId=" + version.id;
        
        config.description = model.tags != null ? string.Join(", ", model.tags) : "";
        config.downloadUrl = CivitaiParserHelper.GetBestModelDownloadUrl(version.files, "Model");
        config.baseModel = CivitaiParserHelper.NormalizeBaseModelName(version.baseModel);
        config.isNegative = model.name.ToLowerInvariant().Contains("negative")
                         || (model.tags?.Contains("negative") ?? false)
                         || (model.tags?.Contains("negative embedding") ?? false);

        return config;
    }

    private static async Task updateEmbeddingsAsync(Log log)
    {
        foreach (var name in SdEmbeddingHelper.GetNames())
        {
            log.WriteLine("Process Embedding: " + name);
            
            var config = SdEmbeddingHelper.GetConfig(name);
            
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

            var newConfig = DataToEmbeddingConfig(modelAndVersion.Item1, modelAndVersion.Item2);

            config.description = newConfig.description;
            config.downloadUrl = newConfig.downloadUrl;
            config.baseModel = newConfig.baseModel;
            config.isNegative = newConfig.isNegative;

            SdEmbeddingHelper.SaveConfig(name, config);
        }
    }
}
