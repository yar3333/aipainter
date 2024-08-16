using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.SdApiClientStuff;
using AiPainter.Helpers;

//using System.Web;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiApiClient
{
    public static readonly Log Log = new("StableDiffusion_ComfyUI");

    private readonly string client_id;
    private readonly ClientWebSocket ws;

    private ComfyUiApiClient(string client_id, ClientWebSocket ws)
    {
        this.client_id = client_id;
        this.ws = ws;
    }

    private static Uri getUri(string relativeUri)
    {
        return new Uri(Program.Config.StableDiffusionUrl.TrimEnd('/') + "/" + relativeUri);
    }

    public static async Task<ComfyUiApiClient> ConnectAsync()
    {
        var clientId = Guid.NewGuid().ToString();
        var ws = new ClientWebSocket();
        await ws.ConnectAsync(getUri("ws?clientId=" + clientId), CancellationToken.None);
        return new ComfyUiApiClient(clientId, ws);
    }

    private async Task<PromptResponse> queue_prompt(object prompt)
    {
        return await postAsync<PromptResponse>("prompt", new { prompt, client_id });
    }

    public static async Task interrupt()
    {
        await postAsync<JsonObject>("interrupt", new { });
    }

    /*public static async Task<string> get_image(string filename, string subfolder, string folder_type)
    {
        var uri = "view"
                + "?filename=" + HttpUtility.UrlEncode(filename)
                + "&subfolder=" + HttpUtility.UrlEncode(subfolder)
                + "&type=" + HttpUtility.UrlEncode(folder_type);

        return await getAsync<string>(uri, true);
    }*/

    /*public static async Task<string> get_history(string prompt_id)
    {
        return await getAsync<string>("history/" + prompt_id, true);
    }*/

    public async Task<Bitmap[]> RunPromptAsync(object prompt)
    {
        var prompt_id = (await queue_prompt(prompt)).prompt_id;
        var output_images = new List<Bitmap>();
        var current_node = "";

        var done = false;
        while (!done)
        {
            await ws.ReceiveMessageAsync
            (
                str =>
                {
                    var message = JsonSerializer.Deserialize<IncomingMessage>(str)!;
                    if (message.type == "executing")
                    {
                        var data = message.data;
                        if (data.prompt_id == prompt_id)
                        {
                            if (data.node == null) done = true;
                            else current_node = data.node;
                        }
                    }
                },
                bytes =>
                {
                    if (current_node == "save_image_websocket_node")
                    {
                        // 8 byte header:
                        //  * type of binary message (first 4 bytes);
                        //  * image format (next 4 bytes);
                        output_images.Add((Bitmap)Image.FromStream(new MemoryStream(bytes.Skip(8).ToArray())));
                    }
                }
            );
        }

        return output_images.ToArray();
    }

    private static async Task<T> postAsync<T>(string url, object request)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log))
        {
            BaseAddress = new Uri(Program.Config.StableDiffusionUrl),
            Timeout = TimeSpan.FromMinutes(10),
        };
        var raw = await httpClient.PostAsync(url, JsonContent.Create(request, null, new JsonSerializerOptions { PropertyNamingPolicy = null, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));
        return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
    }

    /*private static async Task<T> getAsync<T>(string url, bool isLog)
    {
        using var httpClient = isLog ? new HttpClient(new LoggerHttpClientHandler(Log)) : new HttpClient();
        httpClient.BaseAddress = new Uri(Program.Config.StableDiffusionUrl);
        httpClient.Timeout = TimeSpan.FromMinutes(10);

        var raw = await httpClient.GetAsync(url);
        return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
    }*/

    /*private static async Task<string> getStringAsync(string url)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log))
        {
            BaseAddress = new Uri(Program.Config.StableDiffusionUrl),
            Timeout = TimeSpan.FromMinutes(10)
        };
        var raw = await httpClient.GetAsync(url);
        return await raw.Content.ReadAsStringAsync();
    }*/
}
