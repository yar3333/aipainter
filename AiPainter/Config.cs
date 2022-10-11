﻿using System.Text.Json.Serialization;

namespace AiPainter;

class Config
{
    public bool UseExternalInvokeAi { get; set; }
    public string ExternalInvokeAiUrl { get; set; }
    public string ExternalInvokeAiOutputFolderPath { get; set; }
    
    public bool UseExternalLamaCleaner { get; set; }
    public string ExternalLamaCleanerUrl { get; set; }

    [JsonIgnore]
    public string InvokeAiUrl => !UseExternalInvokeAi ? "http://127.0.0.1:9090/" : ExternalInvokeAiUrl;
    
    [JsonIgnore]
    public string InvokeAiOutputFolderPath => !UseExternalInvokeAi 
                                                  ? Path.Combine(Application.StartupPath, "external", "InvokeAi", "outputs", "img-samples")
                                                  : ExternalInvokeAiOutputFolderPath;
    [JsonIgnore]
    public string LamaCleanerUrl => !UseExternalLamaCleaner ? "http://127.0.0.1:9595/" : ExternalLamaCleanerUrl;

    public Config()
    {
        UseExternalInvokeAi = false;
        ExternalInvokeAiUrl = "http://127.0.0.1:9090/";
        ExternalInvokeAiOutputFolderPath = "";

        UseExternalLamaCleaner = false;
        ExternalLamaCleanerUrl = "http://127.0.0.1:9595/";
    }
}
