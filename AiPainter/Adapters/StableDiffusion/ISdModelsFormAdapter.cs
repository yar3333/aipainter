using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion;

public interface ISdModelsFormAdapter
{
    public string Title { get; }

    public int[] SubItemIndexesForSearch { get; }
    public int? SubItemIndexWithLink { get; }

    public ColumnHeader[] GetColumnHeaders();
    
    public ListViewItem[] GetItems();
    public void UpdateItemStatus(GenerationList generationList, ListViewItem item, bool deep);
    public void SetEnabled(string name, bool enabled);
    public TabPage? GetDefaultTab(AddImportModelForm form);
    
    public void StartDownloading(string name, GenerationList generationList);

    public void AddUpdateListEventHandler(Action handler);
    public void RemoveUpdateListEventHandler(Action handler);
    
    public void AddDownloadProgressEventHandler(Action<string> handler);
    public void RemoveDownloadProgressEventHandler(Action<string> handler);
}
