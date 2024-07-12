using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdLoraStuff;

public class SdLorasFormAdapter : ISdModelsFormAdapter
{
    public int[] SubItemIndexesForSearch => new []{ 2, 3 };
    public int? SubItemIndexWithLink => 4;

    public ColumnHeader[] GetColumnHeaders()
    {
        return new ColumnHeader[]
        {
            new() { Text = "Enabled", Width = 35 }, // 0
            new() { Text = "File", TextAlign = HorizontalAlignment.Center, Width = 100 }, // 1
            new() { Text = "Name", TextAlign = HorizontalAlignment.Left, Width = 330 }, // 2
            new() { Text = "Description", TextAlign = HorizontalAlignment.Left, Width = 430 }, // 3
            new() { Text = "Link", TextAlign = HorizontalAlignment.Left, Width = 350 }, // 4
        };
    }

    public ListViewItem[] GetItems()
    {
        var items = new List<ListViewItem>();

        foreach (var name in SdLoraHelper.GetNames())
        {
            if (SdLoraHelper.GetPathToModel(name) != null
             || !string.IsNullOrEmpty(SdLoraHelper.GetConfig(name).downloadUrl))
            {
                var item = new ListViewItem
                {
                    Name = name,
                    UseItemStyleForSubItems = false,
                    Checked = SdLoraHelper.IsEnabled(name) && SdLoraHelper.GetPathToModel(name) != null,
                };
                item.SubItems.Add(SdLoraHelper.GetStatus(name));
                item.SubItems.Add(SdLoraHelper.GetHumanName(name));
                item.SubItems.Add(SdLoraHelper.GetConfig(name).description);
                item.SubItems.Add(SdLoraHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                items.Add(item);
            }
        }

        return items.ToArray();
    }

    public void UpdateItemStatus(GenerationList generationList, ListViewItem item, bool deep)
    {
        SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_lora_", 1, deep, SdLoraHelper.GetStatus);
    }

    public void SetEnabled(string name, bool enabled)
    {
        SdLoraHelper.SetEnabled(name, enabled);
    }

    public TabPage? GetDefaultTab(AddImportModelForm form)
    {
        return form.tabLora;
    }

    public void StartDownloading(string name, GenerationList generationList, Action callOnProgress, Action<string?, Action<string>> callAfterDownload)
    {
        if (SdLoraHelper.GetPathToModel(name) != null) return;

        var url = SdLoraHelper.GetConfig(name).downloadUrl;
        if (string.IsNullOrWhiteSpace(url)) return;

        var genItemName = "download_lora_" + name;
        if (generationList.FindItem(genItemName) != null) return;

        generationList.AddGeneration(new SdDownloadingListItem
        (
            genItemName,
            "Download " + name + " / LoRA model",
            () => true,
            async (progress, cancelationTokenSource) =>
            {
                var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
                                     (
                                         url,
                                         SdLoraHelper.GetDir(),
                                         s => { progress(s); callOnProgress(); },
                                         new DownloadFileOptions
                                         {
                                             FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, name + ".safetensors"),
                                             PreprocessFileName = x => name + Path.GetExtension(x),
                                             AuthorizationBearer = Program.Config.CivitaiApiKey,
                                         },
                                         cancelationTokenSource
                                     );
                callAfterDownload(resultFilePath, progress);
            }
        ));
    }
}