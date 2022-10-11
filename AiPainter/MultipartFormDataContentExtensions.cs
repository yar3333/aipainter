namespace AiPainter;

static class MultipartFormDataContentExtensions
{
    public static MultipartFormDataContent Add(this MultipartFormDataContent content, string name, string value)
    {
        content.Add(new StringContent(value), name);
        return content;
    }
}