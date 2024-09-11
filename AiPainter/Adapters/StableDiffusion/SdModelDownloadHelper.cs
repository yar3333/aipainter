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

    public static async Task<string?> DownloadFileAsync(string url, string destDir, Action<string> progress, DownloadFileOptions options, CancellationTokenSource cancellationTokenSource)
    {
        var newOptions = options.Clone();
        newOptions.Progress = (size, total) =>
        {
            progress(total != null ? Math.Round(size / (double)total * 100) + "%" : size + " bytes");
        };
        
        return await DownloadTools.DownloadFileAsync(url, destDir, newOptions, cancellationTokenSource.Token);
    }

    /// <summary>
    /// Throws exception with message on errors. Can throw `SdListItemDownloadingInvalidApiKeyException`.
    /// </summary>
    public static void AnalyzeDownloadedModel(string? resultFilePath)
    {
        if (resultFilePath == null) throw new Exception("Unknown error");

        if (new FileInfo(resultFilePath).Length < 2048)
        {
            var text = File.ReadAllText(resultFilePath);
            File.Delete(resultFilePath);

            if (text.Contains("\"error\":\"Unauthorized\""))
            {
                throw new SdListItemDownloadingInvalidApiKeyException();
            }

            throw new Exception("Site error");
        }
    }

    public static void UpdateFileStatusInListView(GenerationList generationList, ListViewItem item, string genListItemNamePrefix, int subItemIndex, bool deep, Func<string, string?> getStatusFunc)
    {
        var genListItem = generationList.FindItem(genListItemNamePrefix + item.Name) as SdListItemDownloading;
        var text = genListItem != null && !genListItem.IsDone ? genListItem.DownloadStatus : null;
        if (deep) text ??= getStatusFunc(item.Name);
        if (text != null && item.SubItems[subItemIndex].Text != text) item.SubItems[subItemIndex].Text = text;
    }
}

