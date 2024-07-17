namespace AiPainter.SiteClients.CivitaiClientStuff;

[Serializable]
class CivitaiModel
{
    public long id { get; set;}
    public CivitaiVersion[] modelVersions { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public string[]? tags { get; set; }
}
