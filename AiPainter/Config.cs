namespace AiPainter;

class Config
{
    public bool UseEmbeddedInvokeAi { get; set; } = true;
    public string InvokeAiUrl { get; set; } = "http://127.0.0.1:9090/";
    public string InvokeAiOutputFolderPath { get; set; } = "images";

    public bool UseEmbeddedLamaCleaner { get; set; } = true;
    public string LamaCleanerUrl { get; set; } = "http://127.0.0.1:9595/";

    public bool UseEmbeddedRemBg { get; set; } = true;
    public string RemBgUrl { get; set; } = "http://127.0.0.1:9696/";

    public int ShrinkImageOnOpenMaxWidth { get; set; } = 2048;
    public int ShrinkImageOnOpenMaxHeight { get; set; } = 2048;
}
