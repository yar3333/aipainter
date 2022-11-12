using System.Net.Http.Json;
using System.Text.Json;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion;

static class StableDiffusionClient
{
    public static readonly Log Log = new("StableDiffusion");

    private static bool inProcess;

    // ReSharper disable once InconsistentNaming
    public static void txt2img(SdGenerationRequest request, Action<int> onProgress, Action<SdGenerationResponse?> onSuccess)
    {
        request = JsonSerializer.Deserialize<SdGenerationRequest>(JsonSerializer.Serialize(request))!;
        request.n_iter = 1;

        inProcess = true;
        var cancelation = new CancellationTokenSource();
        
        // ReSharper disable once MethodSupportsCancellation
        Task.Run(async () =>
        {
            try
            {
                var result = await postAsync<SdGenerationResponse>("sdapi/v1/txt2img", request);
                cancelation.Cancel();
                onProgress(request.steps);
                onSuccess(result);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
                cancelation.Cancel();
                onSuccess(null);
            }
            inProcess = false;
        });
        
        runProgressUpdateTask(onProgress, cancelation.Token);
    }

    // ReSharper disable once InconsistentNaming
    public static void img2img(SdInpaintRequest request, Action<int> onProgress, Action<SdGenerationResponse?> onSuccess)
    {
        request = JsonSerializer.Deserialize<SdInpaintRequest>(JsonSerializer.Serialize(request))!;
        request.n_iter = 1;

        inProcess = true;
        var cancelation = new CancellationTokenSource();
        
        // ReSharper disable once MethodSupportsCancellation
        Task.Run(async () =>
        {
            try
            {
                var result = await postAsync<SdGenerationResponse>("sdapi/v1/img2img", request);
                cancelation.Cancel();
                onProgress(request.steps);
                onSuccess(result);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
                cancelation.Cancel();
                onSuccess(null);
            }
            inProcess = false;
        });

        runProgressUpdateTask(onProgress, cancelation.Token);
    }

    public static void Cancel()
    {
        if (!inProcess) return;
        try { postAsync<object>("/sdapi/v1/interrupt", new object()).Wait(); }
        catch {}
    }

    private static void runProgressUpdateTask(Action<int> onProgress, CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            await DelayTools.WaitForExitAsync(1000, cancellationToken);

            while (inProcess && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var r = await getAsync<SdGenerationProgess>("sdapi/v1/progress");
                    if (r.state.sampling_step > 0) onProgress(r.state.sampling_step);
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.ToString());
                }
                await DelayTools.WaitForExitAsync(250, cancellationToken);
            }
        }, cancellationToken);
    }

    private static async Task<T> postAsync<T>(string url, object request)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log))
        {
            BaseAddress = new Uri(Program.Config.StableDiffusionUrl),
            Timeout = TimeSpan.FromMinutes(10)
        };
        var raw = await httpClient.PostAsync(url, JsonContent.Create(request));
        return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
    }

    private static async Task<T> getAsync<T>(string url)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log))
        {
            BaseAddress = new Uri(Program.Config.StableDiffusionUrl),
            Timeout = TimeSpan.FromMinutes(10)
        };
        var raw = await httpClient.GetAsync(url);
        return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
    }

    private static async Task<string> getStringAsync(string url)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log))
        {
            BaseAddress = new Uri(Program.Config.StableDiffusionUrl),
            Timeout = TimeSpan.FromMinutes(10)
        };
        var raw = await httpClient.GetAsync(url);
        return await raw.Content.ReadAsStringAsync();
    }
}