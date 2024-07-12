using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion;

public interface ISdModelsFormAdapter
{
    public int[] SubItemIndexesForSearch { get; }
    public int? SubItemIndexWithLink { get; }

    public ColumnHeader[] GetColumnHeaders();
    
    public ListViewItem[] GetItems();
    public void UpdateItemStatus(GenerationList generationList, ListViewItem item, bool deep);
    public void SetEnabled(string name, bool enabled);
    public TabPage? GetDefaultTab(AddImportModelForm form);
    
    // void callAfterDownload(string? resultFilePath, Action<string> progress)
    public void StartDownloading(string name, GenerationList generationList, Action callOnProgress, Action<string?, Action<string>> callAfterDownload);
}
