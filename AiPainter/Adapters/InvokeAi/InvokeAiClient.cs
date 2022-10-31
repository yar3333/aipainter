using SocketIOClient;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

#pragma warning disable CS8603
#pragma warning disable CS8604

namespace AiPainter.Adapters.InvokeAi;

[Serializable]
class AiProgressUpdateEvent
{
    public int currentStep { get; set; }
    public int totalSteps { get; set; }
    public int currentIteration { get; set; }
    public int totalIterations { get; set; }
    public string? currentStatus { get; set; }
    public bool hasError { get; set; }
}

[Serializable]
class AiGenerationResultEvent
{
    public string url { get; set; }
}

static class InvokeAiClient
{
    public static readonly Log Log = new("InvokeAI");

    private static readonly Dictionary<string, Action<JsonObject>> actions = new();

    private static SocketIO? connection;
    private static void run(Action<SocketIO> action)
    {
        if (connection == null)
        {
            connection = new SocketIO(new Uri(Program.Config.InvokeAiUrl));
            
            connection.OnConnected += (a, b) =>
            {
                Log.WriteLine("OnConnected");
                action(connection);
            };

            connection.OnAny((name, response) =>
            {
                Log.WriteLine("ON: " + name);
                Log.WriteLine("response: " + response);

                if (actions.ContainsKey(name)) actions[name](response.GetValue<JsonObject>());
            });

            connection.ConnectAsync();
        }
        else
        {
            action(connection);
        }
    }

    public static void Generate(AiGenerationParameters generationParameters, AiGfpganParameters? gfpganParameters, Action<AiProgressUpdateEvent> onProgressUpdate, Action onProcessingCanceled, Action<AiGenerationResultEvent> onResult)
    {
        actions["progressUpdate"] = json => onProgressUpdate(json.Deserialize<AiProgressUpdateEvent>());
        actions["processingCanceled"] = _ => onProcessingCanceled();
        actions["generationResult"] = json => onResult(json.Deserialize<AiGenerationResultEvent>());

        run(ws =>
        {
            ws.EmitAsync
            (
                "generateImage",
                generationParameters, 
                false,//esrganParameters ?? (object)false, // superscale
                gfpganParameters ?? (object)false   // face fix
            );
        });
    }

    public static void Cancel()
    {
        run(ws =>
        {
            ws.EmitAsync("cancel");
        });
    }
}
