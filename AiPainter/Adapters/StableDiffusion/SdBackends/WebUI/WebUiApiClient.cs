﻿using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI;

static class WebUiApiClient
{
    public static readonly Log Log = new("StableDiffusion_WebUI");

    private static bool inProcess;

    // ReSharper disable once InconsistentNaming
    public static async Task<SdGenerationResponse?> txt2imgAsync(
        SdTxt2ImgRequest request,
        Action<int> onProgress
    )
    {
        request = request.Clone();
        request.n_iter = 1;

        return await runWithProgressAsync<SdGenerationResponse>(
            "sdapi/v1/txt2img",
            request,
            onProgress
        );
    }

    // ReSharper disable once InconsistentNaming
    public static async Task<SdGenerationResponse?> img2imgAsync(
        SdImg2ImgRequest request,
        Action<int> onProgress
    )
    {
        request = request.Clone();
        request.n_iter = 1;

        return await runWithProgressAsync<SdGenerationResponse?>(
            "sdapi/v1/img2img",
            request,
            onProgress
        );
    }

    public static async Task<string?> interrogateAsync(SdInterrogateRequest request)
    {
        try
        {
            return (
                await postAsync<SdInterrogateResponse?>("sdapi/v1/interrogate", request)
            )?.caption;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<SdExtraImageResponse?> extraImageAsync(
        SdExtraImageRequest request,
        Action<int> onProgress
    )
    {
        inProcess = true;
        WebUiProcess.OnUpscaleProgress += onProgress;
        try
        {
            return await postAsync<SdExtraImageResponse?>("sdapi/v1/extra-single-image", request);
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            WebUiProcess.OnUpscaleProgress -= onProgress;
            inProcess = false;
        }
    }

    private static async Task<T?> runWithProgressAsync<T>(
        string url,
        object request,
        Action<int> onProgress
    )
        where T : class?
    {
        inProcess = true;
        var cancelation = new CancellationTokenSource();

        try
        {
            var resultTask = postAsync<T?>(url, request);
            runProgressUpdateTask(onProgress, cancelation.Token);
            var result = await resultTask;
            cancelation.Cancel();
            inProcess = false;
            return result;
        }
        catch (Exception e)
        {
            Log.WriteLine(e.ToString());
            cancelation.Cancel();
            inProcess = false;
            return null;
        }
    }

    public static void Cancel()
    {
        if (!inProcess)
            return;
        try
        {
            postAsync<object>("sdapi/v1/interrupt", new object()).Wait();
        }
        catch { }
    }

    public static async Task ChangeSettingsAsync(SdSettings settings)
    {
        try
        {
            var result = await postAsync<JsonObject>("sdapi/v1/options", settings);
            if (result != null)
                throw new SdApiException("webui produce error", null);
        }
        catch (Exception e)
        {
            throw new SdApiException("request to webui error", e);
        }
    }

    private static void runProgressUpdateTask(
        Action<int> onProgress,
        CancellationToken cancellationToken
    )
    {
        Task.Run(
            async () =>
            {
                var startTime = DateTime.UtcNow;

                await DelayTools.WaitForExitAsync(1000, cancellationToken);

                while (inProcess && !cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var r = await getAsync<SdGenerationProgess?>("sdapi/v1/progress", true);
                        if (r != null)
                        {
                            switch (r.state?.job)
                            {
                                case "Upscale":
                                    //Log.WriteLine(JsonSerializer.Serialize(r, Program.DefaultJsonSerializerOptions));
                                    var secondsTotal = Math.Ceiling(
                                        Math.Abs(r.eta_relative) * 1000
                                    );
                                    var percent =
                                        (decimal)(DateTime.UtcNow - startTime).TotalSeconds
                                        / secondsTotal
                                        * 100;
                                    onProgress((int)Math.Round(percent));
                                    break;

                                default:
                                    if (r.state?.sampling_step > 0)
                                        onProgress(r.state.sampling_step);
                                    break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.WriteLine(e.ToString());
                    }
                    await DelayTools.WaitForExitAsync(250, cancellationToken);
                }
            },
            cancellationToken
        );
    }

    private static async Task<T> postAsync<T>(string url, object request)
    {
        using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log))
        {
            BaseAddress = new Uri(Program.Config.StableDiffusionUrl),
            Timeout = TimeSpan.FromMinutes(10),
        };
        var raw = await httpClient.PostAsync(
            url,
            JsonContent.Create(
                request,
                null,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                }
            )
        );
        return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
    }

    private static async Task<T> getAsync<T>(string url, bool isLog)
    {
        using var httpClient = isLog
            ? new HttpClient(new LoggerHttpClientHandler(Log))
            : new HttpClient();
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
            Timeout = TimeSpan.FromMinutes(10),
        };
        var raw = await httpClient.GetAsync(url);
        return await raw.Content.ReadAsStringAsync();
    }
}
