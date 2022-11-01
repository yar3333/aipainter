using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion;

[Serializable]
class SdBaseGenerationRequest
{
    public string prompt { get; set; } = "";
    public string negative_prompt { get; set; } = "";
    
    public int n_iter { get; set; } = 1;
    public int steps { get; set; } = 50;

    public int width { get; set; } =  512;
    public int height { get; set; } =  512;
    
    public string sampler_index {  get; set; } = "Euler";
    public decimal cfg_scale { get; set; } = 7;
    public decimal denoising_strength { get; set; } = 0.75m;
    
    public long seed { get; set; } = -1;
    public long subseed { get; set; } = -1;
    public decimal subseed_strength { get; set; } = 0;
    public int seed_resize_from_h { get; set; } = -1;
    public int seed_resize_from_w { get; set; } = -1;
    
    public string[] styles { get; set; } = {};
    
    public int batch_size { get; set; } =  1;
    
    public bool tiling { get; set; } = false;
    public bool restore_faces { get; set; } = false;
    
    // sampler parameters
    public decimal? eta { get; set; } = 0; // 0..1
    public decimal? s_churn { get; set; } = 0; // 0 or 1 are best
    public decimal? s_noise { get; set; } = 1; // best is 1
    public decimal? s_tmax { get; set; } = 0;
    public decimal? s_tmin { get; set; } = 0;
}

[Serializable]
class SdGenerationRequest : SdBaseGenerationRequest
{
    public bool enable_hr { get; set; } = false;
    public int firstphase_width { get; set; } = 0;
    public int firstphase_height { get; set; } = 0;
    //public JsonObject override_settings { get; set; } = new();
}

[Serializable]
class SdInpaintRequest : SdBaseGenerationRequest
{
    public string[] init_images { get; set; } = {};
    public int resize_mode { get; set; } = 0;
    public string? mask { get; set; }
    public int mask_blur { get; set; } = 4;
    public int inpainting_fill { get; set; } = 0;
    public bool inpaint_full_res { get; set; } = true;
    public int inpaint_full_res_padding { get; set; } = 0;
    public int inpainting_mask_invert { get; set; } = 0;
    //public string override_settings { get; set; } = {};
    public bool include_init_images { get; set; } = false;
}

[Serializable]
class SdGenerationResponse
{
    public string[] images { get; set; } = null!; // data:image/png;base64,iVBOR....
    public SdGenerationRequest parameters { get; set; } = null!;
    public string info { get; set; } = null!;
    public SdGenerationInfo infoParsed => JsonSerializer.Deserialize<SdGenerationInfo>(info)!;
}

[Serializable]
class SdGenerationInfo
{
    public string prompt { get; set; }
    public string[] all_prompts { get; set; }
    public string negative_prompt { get; set; }
    public long seed { get; set; }
    public long[] all_seeds { get; set; }
    public long subseed { get; set; }
    public long[] all_subseeds { get; set; }
    public decimal subseed_strength { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int sampler_index { get; set; }
    public string sampler { get; set; }
    public decimal cfg_scale { get; set; }
    public int steps { get; set; }
    public int batch_size { get; set; }
    public bool restore_faces { get; set; }
    public string? face_restoration_model { get; set; }
    public string? sd_model_hash { get; set; }
    public int seed_resize_from_w { get; set; }
    public int seed_resize_from_h { get; set; }
    public decimal denoising_strength { get; set; }
    public JsonObject? extra_generation_params { get; set; }
    public int index_of_first_image { get; set; }
}

[Serializable]
class SdGenerationProgess
{
    public decimal progress { get; set; }
    public decimal eta_relative { get; set; }
    public SdGenerationProgessState state { get; set; }
    string? current_image { get; set; }
}

[Serializable]
class SdGenerationProgessState
{
    public bool skipped { get; set; }
    public bool interrupted { get; set; }
    public string? job { get; set; }
    public int job_count { get; set; }
    public int job_no { get; set; }
    public int sampling_step { get; set; }
    public int sampling_steps { get; set; }
}

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
                    onSuccess(result);
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

        /*Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            try
            {
                var result = await getStringAsync("sdapi/v1/progress");
                //onProgress(result);
                int a = 5;
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }
        });*/
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
                    onSuccess(result);
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
    }

    public static void Cancel()
    {
        if (!inProcess) return;
        wantToCancel = true;
        //await getAsync<JsonObject>("sdapi/v1/progress?skip_current_image=true");
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