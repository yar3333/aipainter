namespace AiPainter.Adapters.StableDiffusion;

static class SdModelDownloadHelper
{
    public static string GetModelFileNameFromUrl(string url, string defaultFileName)
    {
        var uriLocalPath = new Uri(url).LocalPath;

        return uriLocalPath.EndsWith(".ckpt") 
            || uriLocalPath.EndsWith(".safetensors") 
            || uriLocalPath.EndsWith(".pt")
                   ? Path.GetFileName(uriLocalPath)
                   : defaultFileName;
    }

    public static string? GetCheckpointAuthorizationBearer(string name)
    {
        return SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload ? Program.Config.CivitaiApiKey : null;
    }

    public static void AnalyzeDownloadedModel(string? resultFilePath, Action onAuthErrorFound)
    {
        if (resultFilePath == null) return;
        if (new FileInfo(resultFilePath).Length > 1024 * 1024) return;
            
        var text = File.ReadAllText(resultFilePath);
        File.Delete(resultFilePath);

        if (text.Contains("\"error\":\"Unauthorized\""))
        {
            onAuthErrorFound();
        }
    }
}

