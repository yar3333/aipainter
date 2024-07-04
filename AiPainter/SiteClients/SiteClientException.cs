namespace AiPainter.SiteClients;

class SiteClientException : Exception
{
    public SiteClientException(string message)
        : base(message)
    {
    }
}