namespace AiPainter.SiteClients.CivitaiClientStuff;

[Serializable]
class CivitaiVersion
{
    public long id { get; set; }
    public string[]? trainedWords { get; set; }
    public string name { get; set; }
    public CivitaiFile[]? files { get; set; }
    public string downloadUrl { get; set; }
    public string? baseModel { get; set; }
    //public string? baseModelType { get; set; } // "Standard"
}

[Serializable]
class CivitaiFile
{
    public string type { get; set; }
    public string downloadUrl { get; set; }
    public CivitaiFileMetadata? metadata { get; set; }
}

[Serializable]
class CivitaiFileMetadata
{
    public string format { get; set; } // PickleTensor, SafeTensor, Other
    public string size { get; set; } // full, pruned
    public string fp { get; set; } // fp16, fp32
}