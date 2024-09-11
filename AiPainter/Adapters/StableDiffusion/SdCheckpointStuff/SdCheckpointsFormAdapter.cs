using AiPainter.Controls;
using AiPainter.Helpers;
using AiPainter.SiteClients.CivitaiClientStuff;

namespace AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;

public class SdCheckpointsFormAdapter : ISdModelsFormAdapter
{
    public string Title => "Stable Diffusion Checkpoints (image generation models)";

    public int[] SubItemIndexesForSearch => new []{ 4, 5 };
    public int? SubItemIndexWithLink => 6;

    public ColumnHeader[] GetColumnHeaders()
    {
        return new ColumnHeader[]
        {
            new() { Text = "Enabled", Width = 35 }, // 0
            new() { Text = "Main file", TextAlign = HorizontalAlignment.Center }, // 1
            new() { Text = "Inpaint file", TextAlign = HorizontalAlignment.Center }, // 2
            new() { Text = "VAE file", TextAlign = HorizontalAlignment.Center }, // 3
            new() { Text = "Name", TextAlign = HorizontalAlignment.Left, Width = 180 }, // 4
            new() { Text = "Description", TextAlign = HorizontalAlignment.Left, Width = 270 }, // 5
            new() { Text = "Link", TextAlign = HorizontalAlignment.Left, Width = 350 }, // 6
        };
    }
        
    public ListViewItem[] GetItems()
    {
        var items = new List<ListViewItem>();

        foreach (var name in SdCheckpointsHelper.GetNames("").Where(x => x != ""))
        {
            if (SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null
             || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl)
             || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl))
            {
                var item = new ListViewItem
                {
                    Name = name,
                    UseItemStyleForSubItems = false,
                    Checked = SdCheckpointsHelper.IsEnabled(name) && SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null,
                };
                item.SubItems.Add(SdCheckpointsHelper.GetStatusMain(name));
                item.SubItems.Add(SdCheckpointsHelper.GetStatusInpaint(name));
                item.SubItems.Add(SdCheckpointsHelper.GetStatusVae(name));
                item.SubItems.Add(name);
                item.SubItems.Add(SdCheckpointsHelper.GetConfig(name).description);
                item.SubItems.Add(SdCheckpointsHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                items.Add(item);
            }
        }

        return items.ToArray();
    }

    public void UpdateItemStatus(GenerationList generationList, ListViewItem item, bool deep)
    {
        SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_checkpoint_main_",    1, deep, SdCheckpointsHelper.GetStatusMain);
        SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_checkpoint_inpaint_", 2, deep, SdCheckpointsHelper.GetStatusInpaint);
        SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_checkpoint_vae_",     3, deep, SdCheckpointsHelper.GetStatusVae);
    }

    public void SetEnabled(string name, bool enabled)
    {
        SdCheckpointsHelper.SetEnabled(name, enabled);
    }

    public TabPage? GetDefaultTab(AddImportModelForm form)
    {
        return form.tabCheckpoint;
    }

    public void StartDownloading(string name, GenerationList generationList)
    {
        startDownloadingCheckpointMainDown(name, generationList);
        startDownloadingCheckpointInpaint (name, generationList);
        startDownloadingVae               (name, generationList);
    }

    public void AddUpdateListEventHandler(Action handler)
    {
        GlobalEvents.CheckpointFileDownloaded += handler;
    }

    public void RemoveUpdateListEventHandler(Action handler)
    {
        GlobalEvents.CheckpointFileDownloaded -= handler;
    }

    public void AddDownloadProgressEventHandler(Action<string> handler)
    {
        GlobalEvents.CheckpointDownloadProgress += handler;
    }

    public void RemoveDownloadProgressEventHandler(Action<string> handler)
    {
        GlobalEvents.CheckpointDownloadProgress -= handler;
    }

    private static void startDownloadingCheckpointMainDown(string name, GenerationList generationList)
    {
        if (SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null) return;

        var url = SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl;
        if (string.IsNullOrWhiteSpace(url)) return;

        var genItemName = "download_checkpoint_main_" + name;
        if (generationList.FindItem(genItemName) != null) return;

        var downItem = new SdListItemDownloading(genItemName, "Download " + name + " / Main model", () => true);
        downItem.WorkAsync = async cancellationTokenSource =>
        {
            var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
            (
                url,
                SdCheckpointsHelper.GetDirPath(name),
                s =>
                {
                    downItem.NotifyProgress(s);
                    GlobalEvents.CheckpointDownloadProgress?.Invoke(name);
                },
                new DownloadFileOptions
                {
                    FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, "main.safetensors"),
                    AuthorizationBearer = CivitaiHelper.GetCheckpointAuthorizationBearer(name),
                },
                cancellationTokenSource
            );

            try
            {
                SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath);
                GlobalEvents.CheckpointFileDownloaded?.Invoke();
            }
            catch (Exception e)
            {
                downItem.NotifyProgress(e.Message);
                throw;
            }
        };
        generationList.AddGeneration(downItem);
    }

    private static void startDownloadingCheckpointInpaint(string name, GenerationList generationList)
    {
        if (SdCheckpointsHelper.GetPathToInpaintCheckpoint(name) != null) return;

        var url = SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl;
        if (string.IsNullOrWhiteSpace(url)) return;

        var genItemName = "download_checkpoint_inpaint_" + name;
        if (generationList.FindItem(genItemName) != null) return;

        var downItem = new SdListItemDownloading(genItemName, "Download " + name + " / Inpaint model", () => true);
        downItem.WorkAsync = async cancellationTokenSource =>
        {
            var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
                                 (
                                     url,
                                     SdCheckpointsHelper.GetDirPath(name),
                                     s =>
                                     {
                                         downItem.NotifyProgress(s);
                                         GlobalEvents.CheckpointDownloadProgress?.Invoke(name);
                                     },
                                     new DownloadFileOptions
                                     {
                                         FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, "inpaint.safetensors"),
                                         AuthorizationBearer = CivitaiHelper.GetCheckpointAuthorizationBearer(name),
                                     },
                                     cancellationTokenSource
                                 );
            
            try
            {
                SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath);
                GlobalEvents.CheckpointFileDownloaded?.Invoke();
            }
            catch (Exception e)
            {
                downItem.NotifyProgress(e.Message);
                throw;
            }
        };
        generationList.AddGeneration(downItem);
    }

    private static void startDownloadingVae(string name, GenerationList generationList)
    {
        if (SdCheckpointsHelper.GetPathToVae(name) != null) return;

        var url = SdCheckpointsHelper.GetConfig(name).vaeUrl;
        if (string.IsNullOrWhiteSpace(url)) return;

        var genItemName = "download_checkpoint_vae_" + name;
        if (generationList.FindItem(genItemName) != null) return;

        var downItem = new SdListItemDownloading(genItemName, "Download " + name + " / VAE model", () => true);
        downItem.WorkAsync = async cancellationTokenSource =>
        {
            var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
                                 (
                                     url,
                                     SdCheckpointsHelper.GetDirPath(name),
                                     s =>
                                     {
                                         downItem.NotifyProgress(s);
                                         GlobalEvents.CheckpointDownloadProgress?.Invoke(name);
                                     },
                                     new DownloadFileOptions
                                     {
                                         FileNameIfNotDetected = prepareVaeFileName(SdModelDownloadHelper.GetModelFileNameFromUrl(url, "vae.pt")),
                                         PreprocessFileName = prepareVaeFileName!,
                                         AuthorizationBearer = CivitaiHelper.GetCheckpointAuthorizationBearer(name),
                                     },
                                     cancellationTokenSource
                                 );

            try
            {
                SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath);
                GlobalEvents.CheckpointFileDownloaded?.Invoke();
            }
            catch (Exception e)
            {
                downItem.NotifyProgress(e.Message);
                throw;
            }
        };
        generationList.AddGeneration(downItem);
    }

    private static string prepareVaeFileName(string fileName)
    {
        return !SdCheckpointsHelper.IsFilePathLikeVae(fileName)
                   ? Path.GetFileNameWithoutExtension(fileName) + "-vae" + Path.GetExtension(fileName)
                   : fileName;
    }
}