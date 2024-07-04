namespace AiPainter.SiteClients.CivitaiClientStuff;

class CivitaiVersion
{
    public long id { get; set; }
    public string[]? trainedWords { get; set; }
    public string name { get; set; }
    public CivitaiFile[]? files { get; set; }
    public string downloadUrl { get; set; }
}

class CivitaiFile
{
    public string type { get; set; }
    public string downloadUrl { get; set; }
}