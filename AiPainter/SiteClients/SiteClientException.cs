namespace AiPainter.SiteClients;

class SiteClientException : Exception
{
    public SiteClientException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}