using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Controls;
using AiPainter.Helpers;

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

    public static async Task<string?> DownloadFileAsync(string url, string destDir, Action<string> progress, DownloadFileOptions options, CancellationTokenSource cancelationTokenSource)
    {
        string? resultFilePath = null;
        try
        {
            var newOptions = options.Clone();
            newOptions.Progress = (size, total) =>
            {
                progress(total != null ? Math.Round(size / (double)total * 100) + "%" : size + " bytes");
            };
            resultFilePath = await DownloadTools.DownloadFileAsync(url, destDir, newOptions, cancelationTokenSource.Token);
        }
        catch (AggregateException e)
        {
            Program.Log.WriteLine("Downloading " + url + " ERROR: " + e.Message);
        }

        return resultFilePath;
    }

    public static bool AnalyzeDownloadedModel(string? resultFilePath, Action onAuthErrorFound)
    {
        if (resultFilePath == null) return false;
        if (new FileInfo(resultFilePath).Length > 2048) return true;
            
        var text = File.ReadAllText(resultFilePath);
        File.Delete(resultFilePath);

        if (text.Contains("\"error\":\"Unauthorized\""))
        {
            onAuthErrorFound();
        }

        return false;
    }

    public static void UpdateFileStatusInListView(GenerationList generationList, ListViewItem item, string genListItemNamePrefix, int subItemIndex, bool deep, Func<string, string?> getStatusFunc)
    {
        var genListItem = generationList!.FindItem(genListItemNamePrefix + item.Name) as SdDownloadingListItem;
        var text = genListItem != null && !genListItem.IsDone ? genListItem.DownloadStatus : null;
        if (deep) text ??= getStatusFunc(item.Name);
        if (text != null && item.SubItems[subItemIndex].Text != text) item.SubItems[subItemIndex].Text = text;
    }
}

