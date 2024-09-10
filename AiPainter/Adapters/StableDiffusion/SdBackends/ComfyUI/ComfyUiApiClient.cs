using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
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

    public static async Task<ComfyUiApiClient> ConnectAsync()
    {
        var clientId = Guid.NewGuid().ToString();
        var ws = new ClientWebSocket();
        var uriBuilder = new UriBuilder(Program.Config.StableDiffusionUrl.TrimEnd('/') + "/" + "ws?clientId=" + clientId);
        uriBuilder.Scheme = uriBuilder.Scheme == "http" ? "ws" : "https";
        await ws.ConnectAsync(uriBuilder.Uri, CancellationToken.None);
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

    public async Task<Bitmap[]> RunPromptAndGetImageAsync(object prompt, string imageNodeId, Action<int> progressStep)
    {
        var prompt_id = (await queue_prompt(prompt)).prompt_id;
        var output_images = new List<Bitmap>();
        var current_node = "";

        var done = false;
        while (ws.State == WebSocketState.Open && !done)
        {
            await ws.ReceiveMessageAsync
            (
                str =>
                {
                    var message = JsonSerializer.Deserialize<JsonObject>(str)!;
                    Log.WriteLine(JsonSerializer.Serialize(message, Program.DefaultJsonSerializerOptions));

                    switch (message["type"]?.ToString())
                    {
                        case "executing":
                        {
                            var data = message["data"]?.AsObject();
                            if (data?["prompt_id"]?.ToString() == prompt_id)
                            {
                                if (data["node"]?.AsValue() == null) done = true;
                                else current_node = data["node"]?.ToString();
                            }
                        }
                        break;

                        case "progress":
                        {
                            var data = message["data"]?.AsObject();
                            if (data?["prompt_id"]?.ToString() == prompt_id)
                            {
                                var step = data["value"]!.AsValue().GetValue<int>();
                                if (step > 0) progressStep(step - 1);
                            }
                        }
                        break;

                        case "execution_interrupted":
                        {
                            var data = message["data"]?.AsObject();
                            if (data?["prompt_id"]?.ToString() == prompt_id)
                            {
                                done = true;
                            }
                        }
                        break;
                    }
                },
                bytes =>
                {
                    if (current_node == imageNodeId)
                    {
                        var msgType = BitConverter.ToInt32(bytes.Take(4).Reverse().ToArray());
                        var imgFormat = BitConverter.ToInt32(bytes.Skip(4).Take(4).Reverse().ToArray());
                        output_images.Add((Bitmap)Image.FromStream(new MemoryStream(bytes.Skip(8).ToArray())));
                    }
                }
            );
        }

        return output_images.ToArray();
    }   
    
    public async Task<string?> RunPromptAndGetTextAsync(object prompt, string showTextNodeId)
    {
        var prompt_id = (await queue_prompt(prompt)).prompt_id;

        string? r = null;

        var done = false;
        while (ws.State == WebSocketState.Open && !done)
        {
            await ws.ReceiveMessageAsync
            (
                str =>
                {
                    var message = JsonSerializer.Deserialize<JsonObject>(str)!;
                    Log.WriteLine(JsonSerializer.Serialize(message, Program.DefaultJsonSerializerOptions));

                    switch (message["type"]?.ToString())
                    {
                        case "executed":
                        {
                            var data = message["data"]?.AsObject();
                            if (data?["prompt_id"]?.ToString() == prompt_id)
                            {
                                if (data["node"]?.ToString() == showTextNodeId)
                                {
                                    done = true;
                                    r = data["output"]?.AsObject()?["string"]?.AsArray().FirstOrDefault()?.ToString();
                                }
                            }
                        }
                        break;

                        case "execution_interrupted":
                        {
                            var data = message["data"]?.AsObject();
                            if (data?["prompt_id"]?.ToString() == prompt_id)
                            {
                                done = true;
                            }
                        }
                        break;
                    }
                },
                _ => {}
            );
        }

        return r;
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
