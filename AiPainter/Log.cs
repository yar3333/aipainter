using System.Text.Json;
using AiPainter.Helpers;

namespace AiPainter;

public class Log : IDisposable
{
    private readonly StreamWriter streamWriter;

    public Log(string name)
    {
        var baseDir = Path.Join(Application.StartupPath, "logs");
        if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);

        var logFile = File.Open(Path.Join(baseDir, name) + ".log", FileMode.Create, FileAccess.Write, FileShare.Read);
        streamWriter = new StreamWriter(logFile);
    }

    public void WriteLine(string s)
    {
        lock (streamWriter)
        {
            streamWriter.WriteLine(s);
            streamWriter.Flush();
        }
    }

    public void Dispose()
    {
        streamWriter.Dispose();
    }
}

class LoggerHttpClientHandler : HttpClientHandler
{
    private readonly Log log;

    public LoggerHttpClientHandler(Log log)
    {
        this.log = log;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        log.WriteLine("[request] " + request.RequestUri);
        if (request.Content != null) logBigString(await request.Content.ReadAsStringAsync());

        var r = await base.SendAsync(request, cancellationToken);

        log.WriteLine("[response] " + r.StatusCode + " " + r.ReasonPhrase);
        logBigString(await r.Content.ReadAsStringAsync());
        
        return r;
    }

    void logBigString(string? text)
    {
        if (text == null) return;

        try
        {
            var json = JsonSerializer.Deserialize<JsonElement>(text);
            log.WriteLine(JsonForLogConverter.Serialize(json));
        }
        catch
        {
            log.WriteLine(text.Length <= 1024 ? text : text[..1024]);
        }
    }
}
