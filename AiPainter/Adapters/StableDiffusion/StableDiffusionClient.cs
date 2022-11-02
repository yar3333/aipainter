﻿using System.Net.Http.Json;
using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion;

static class StableDiffusionClient
{
    public static readonly Log Log = new("StableDiffusion");

    private static bool inProcess;
    private static bool wantToCancel;

    // ReSharper disable once InconsistentNaming
    public static void txt2img(SdGenerationRequest request, Action<SdGenerationProgess> onProgress, Action<SdGenerationResponse> onSuccess, Action<int> onFinish)
    {
        request = JsonSerializer.Deserialize<SdGenerationRequest>(JsonSerializer.Serialize(request))!;

        Task.Run(async () =>
        {
            var count = request.n_iter;
            request.n_iter = 1;

            inProcess = true;
            var i = 0; for (; i < count && !wantToCancel; i++)
            {
                try
                {
                    var result = await postAsync<SdGenerationResponse>("sdapi/v1/txt2img", request);
                    if (!wantToCancel) onSuccess(result);
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.ToString());
                }
            }
            inProcess = false;
            wantToCancel = false;

            onFinish(i);
        });
        
        runProgressUpdateTask(onProgress);
    }

    // ReSharper disable once InconsistentNaming
    public static void img2img(SdInpaintRequest request, Action<SdGenerationProgess> onProgress, Action<SdGenerationResponse> onSuccess, Action<int> onFinish)
    {
        request = JsonSerializer.Deserialize<SdInpaintRequest>(JsonSerializer.Serialize(request))!;

        Task.Run(async () =>
        {
            var count = request.n_iter;
            request.n_iter = 1;

            inProcess = true;
            var i = 0; for (; i < count && !wantToCancel; i++)
            {
                try
                {
                    var result = await postAsync<SdGenerationResponse>("sdapi/v1/img2img", request);
                    if (!wantToCancel) onSuccess(result);
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.ToString());
                }
            }
            inProcess = false;
            wantToCancel = false;

            onFinish(i);
        });

        runProgressUpdateTask(onProgress);
    }

    public static async Task Cancel()
    {
        if (!inProcess) return;
        wantToCancel = true;
        await postAsync<object>("/sdapi/v1/interrupt", new object());
    }

    private static void runProgressUpdateTask(Action<SdGenerationProgess> onProgress)
    {
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            while (true)
            {
                for (var i = 0; i < 3; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                    if (!inProcess) return;
                }
                
                try
                {
                    onProgress(await getAsync<SdGenerationProgess>("sdapi/v1/progress"));
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.ToString());
                }
            }
        });
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