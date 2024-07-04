using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiPainter.SiteClients.CivitaiClientStuff;

static class CivitaiClient
{
    private static readonly Log Log = new("civitai");

    public static async Task<CivitaiModel> GetModelAsync(string modelId)
    {
        try
        {
            return await getAsync<CivitaiModel>("models/" + modelId);
        }
        catch (Exception e)
        {
            throw new SiteClientException(e.Message);
        }
    }

    public static async Task<CivitaiVersion> GetVersionAsync(string versionId)
    {
        try
        {
            return await getAsync<CivitaiVersion>("model-versions/" + versionId);
        }
        catch (Exception e)
        {
            throw new SiteClientException(e.Message);
        }
    }

    private static async Task<T> postAsync<T>(string url, object request)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log));
        httpClient.BaseAddress = new Uri("https://civitai.com/api/v1/");
        httpClient.Timeout = TimeSpan.FromMinutes(1);
        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.Config.CivitaiApiKey);

        var raw = await httpClient.PostAsync(url, JsonContent.Create(request, null, new JsonSerializerOptions { PropertyNamingPolicy = null, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));
        return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
    }

    private static async Task<T> getAsync<T>(string url)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log));
        httpClient.BaseAddress = new Uri("https://civitai.com/api/v1/");
        httpClient.Timeout = TimeSpan.FromMinutes(1);
        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.Config.CivitaiApiKey);

        var raw = await httpClient.GetAsync(url);
        var text = await raw.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(text)!;
    }

    private static async Task<string> getStringAsync(string url)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log));
        httpClient.BaseAddress = new Uri("https://civitai.com/api/v1/");
        httpClient.Timeout = TimeSpan.FromMinutes(1);
        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.Config.CivitaiApiKey);

        var raw = await httpClient.GetAsync(url);
        return await raw.Content.ReadAsStringAsync();
    }
}
