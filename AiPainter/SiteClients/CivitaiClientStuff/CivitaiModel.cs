namespace AiPainter.SiteClients.CivitaiClientStuff;

class CivitaiModel
{
    public CivitaiVersion[] modelVersions { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public string[]? tags { get; set; }
}
