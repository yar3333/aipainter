﻿using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;

public class SdEmbeddingFormAdapter : ISdModelsFormAdapter
{
    public int[] SubItemIndexesForSearch => new[] { 2, 3 };
    public int? SubItemIndexWithLink => 4;

    public ColumnHeader[] GetColumnHeaders()
    {
        return new ColumnHeader[]
        {
            new() { Text = "Enabled", Width = 35 }, // 0
            new() { Text = "File", TextAlign = HorizontalAlignment.Center }, // 1
            new() { Text = "Name", TextAlign = HorizontalAlignment.Left, Width = 180 }, // 2
            new() { Text = "Description", TextAlign = HorizontalAlignment.Left, Width = 270 }, // 3
            new() { Text = "Link", TextAlign = HorizontalAlignment.Left, Width = 350 }, // 4
        };
    }

    public ListViewItem[] GetItems()
    {
        var items = new List<ListViewItem>();
        
        foreach (var name in SdEmbeddingHelper.GetNames())
        {
            var filePath = SdEmbeddingHelper.GetPathToModel(name);
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)
             || !string.IsNullOrEmpty(SdEmbeddingHelper.GetConfig(name).downloadUrl))
            {
                var item = new ListViewItem();
                item.UseItemStyleForSubItems = false;
                item.SubItems.Add(SdEmbeddingHelper.GetStatus(name));
                item.SubItems.Add(SdEmbeddingHelper.GetHumanName(name));
                item.SubItems.Add(SdEmbeddingHelper.GetConfig(name).description);
                item.SubItems.Add(SdEmbeddingHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                item.Name = name;
                item.Checked = SdEmbeddingHelper.IsEnabled(name) && SdEmbeddingHelper.GetPathToModel(name) != null;

                items.Add(item);
            }
        }

        return items.ToArray();
    }

    public void UpdateItemStatus(GenerationList generationList, ListViewItem item, bool deep)
    {
        if (deep)
        {
            var text = SdEmbeddingHelper.GetStatus(item.Name);
            if (item.SubItems[1].Text != text) item.SubItems[1].Text = text;
        }
    }

    public void SetEnabled(string name, bool enabled)
    {
        SdEmbeddingHelper.SetEnabled(name, enabled);
    }

    public TabPage? GetDefaultTab(AddImportModelForm form)
    {
        return form.tabEmbedding;
    }

    public void StartDownloading(string name, GenerationList generationList, Action callOnProgress, Action<string?, Action<string>> callAfterDownload)
    {
        if (SdEmbeddingHelper.GetPathToModel(name) != null) return;

        var url = SdEmbeddingHelper.GetConfig(name).downloadUrl;
        if (string.IsNullOrWhiteSpace(url)) return;

        Task.Run(async () =>
        {
            var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
            (
                url,
                SdEmbeddingHelper.GetDir(),
                s => {},
                new DownloadFileOptions
                {
                    FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, name + ".safetensors"),
                    PreprocessFileName = x => name + Path.GetExtension(x),
                    AuthorizationBearer = Program.Config.CivitaiApiKey,
                },
                new CancellationTokenSource()
            );

            callAfterDownload(resultFilePath, null);
        });
    }
}
