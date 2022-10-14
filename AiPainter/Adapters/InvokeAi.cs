using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AiPainter.Adapters.StuffForInvokeAi;

#pragma warning disable CS8603
#pragma warning disable CS8604

namespace AiPainter.Adapters;

static class InvokeAi
{
    public static async Task<AiImageInfo[]> GetImageList()
    {
        return await requestGet<AiImageInfo[]>("run_log.json");
    }

    public static void Generate(AiImageInfo imageInfo, Action<AiProgress> onEvent)
    {
        var thread = new Thread(() =>
        {
            var stream = requestPost("", imageInfo).ReadAsStream();

            var buf = new List<byte>();
            int b;
            while ((b = stream.ReadByte()) >= 0)
            {
                if (b == '\n')
                {
                    onEvent(JsonSerializer.Deserialize<AiProgress>(Encoding.UTF8.GetString(buf.ToArray())));
                    buf.Clear();
                }
                else
                {
                    buf.Add((byte)b);
                }
            }
            if (buf.Any()) onEvent(JsonSerializer.Deserialize<AiProgress>(Encoding.UTF8.GetString(buf.ToArray())));
        });
        thread.Start();
    }

    public static void Cancel()
    {
        requestGet("cancel");
    }

    private static async Task<T> requestGet<T>(string subUrl) where T : class
    {
        var httpClient = new HttpClient { BaseAddress = new Uri(Program.Config.InvokeAiUrl) };
        return await httpClient.GetFromJsonAsync<T>(subUrl, new JsonSerializerOptions { PropertyNamingPolicy = null });
    }

    private static void requestGet(string subUrl)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri(Program.Config.InvokeAiUrl) };
        httpClient.GetAsync(subUrl);
    }

    private static HttpContent requestPost(string subUrl, object dataObj)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri(Program.Config.InvokeAiUrl), MaxResponseContentBufferSize = 1 };

        var data = JsonSerializer.Serialize(dataObj, new JsonSerializerOptions { PropertyNamingPolicy = null });
        var content = new StringContent(data, Encoding.UTF8, "application/json");

        //var r = httpClient.PostAsync(subUrl, content).Result;
        var reqMes = new HttpRequestMessage(HttpMethod.Post, subUrl);
        reqMes.Content = content;
        var r = httpClient.Send(reqMes, HttpCompletionOption.ResponseHeadersRead);
        return r.Content;
    }
}
