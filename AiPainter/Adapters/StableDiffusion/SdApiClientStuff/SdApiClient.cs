﻿using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

static class SdApiClient
{
    public static readonly Log Log = new("StableDiffusion");

    private static bool inProcess;

    // ReSharper disable once InconsistentNaming
    public static void txt2img(SdTxt2ImgRequest request, Action<int> onProgress, Action<SdGenerationResponse?> onSuccess)
    {
        request = request.Clone();
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
    public static void img2img(SdImg2ImgRequest request, Action<int> onProgress, Action<SdGenerationResponse?> onSuccess)
    {
        request = request.Clone();
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
        try { postAsync<object>("sdapi/v1/interrupt", new object()).Wait(); }
        catch { }
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
                    var r = await getAsync<SdGenerationProgess>("sdapi/v1/progress", false);
                    if (r.state?.sampling_step > 0) onProgress(r.state.sampling_step);
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
            Timeout = TimeSpan.FromMinutes(10),
        };
        var raw = await httpClient.PostAsync(url, JsonContent.Create(request, null, new JsonSerializerOptions { PropertyNamingPolicy = null, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));
        return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
    }

    private static async Task<T> getAsync<T>(string url, bool isLog)
    {
        using var httpClient = isLog ? new HttpClient(new LoggerHttpClientHandler(Log)) : new HttpClient();
        httpClient.BaseAddress = new Uri(Program.Config.StableDiffusionUrl);
        httpClient.Timeout = TimeSpan.FromMinutes(10);

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