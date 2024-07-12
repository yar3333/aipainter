namespace AiPainter;

public static class GlobalEvents
{
    public static Action? CheckpointFileDownloaded = null;
    public static Action? LoraFileDownloaded = null;
    public static Action? EmbeddingFileDownloaded = null;

    public static Action<string>? CheckpointDownloadProgress = null;
    public static Action<string>? LoraDownloadProgress = null;
    public static Action<string>? EmbeddingDownloadProgress = null;
}
