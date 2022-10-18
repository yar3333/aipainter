using System.Text.Json.Serialization;

namespace AiPainter;

class Config
{
    public bool UseEmbeddedInvokeAi { get; set; } = true;
    public string InvokeAiUrl { get; set; } = "http://127.0.0.1:9090/";
    public string ExternalInvokeAiOutputFolderPath { get; set; } = "";

    public bool UseEmbeddedLamaCleaner { get; set; } = true;
    public string LamaCleanerUrl { get; set; } = "http://127.0.0.1:9595/";

    public bool UseEmbeddedRemBg { get; set; } = true;
    public string RemBgUrl { get; set; } = "http://127.0.0.1:9696/";

    public int ShrinkImageOnOpenMaxWidth { get; set; } = 2048;
    public int ShrinkImageOnOpenMaxHeight { get; set; } = 2048;

    [JsonIgnore]
    public string InvokeAiOutputFolderPath => UseEmbeddedInvokeAi 
                                                  ? Path.Combine(Application.StartupPath, "external", "InvokeAI", "outputs", "img-samples")
                                                  : ExternalInvokeAiOutputFolderPath;
}
