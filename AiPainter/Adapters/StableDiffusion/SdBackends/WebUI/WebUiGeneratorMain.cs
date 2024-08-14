using AiPainter.Helpers;
using AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI;

class WebUiGeneratorMain : ISdGenerator
{
    private readonly SdGenerationParameters sdGenerationParameters;
    private readonly SdGenerationListItem control;

    private readonly string destDir;

    public WebUiGeneratorMain(SdGenerationParameters sdGenerationParameters, SdGenerationListItem control, string destDir)
    {
        this.sdGenerationParameters = sdGenerationParameters;
        this.control = control;

        this.destDir = destDir;
    }

    public async Task<bool> RunAsync()
    {
        var wasProgressShown = false;
        var isCheckpointSuccess = await WebUiGeneratorHelper.PrepareCheckpointAsync
        (
            true,
            sdGenerationParameters.checkpointName,
            sdGenerationParameters.vaeName,
            () => control.IsDisposed,
            s => { wasProgressShown = true; control.NotifyStepsCustomText(s); }
        );
        if (!isCheckpointSuccess) return false;
        if (!wasProgressShown) control.NotifyProgress(0);

        var parameters = new SdTxt2ImgRequest
        {
            prompt = sdGenerationParameters.prompt,
            negative_prompt = sdGenerationParameters.negative,
            cfg_scale = sdGenerationParameters.cfgScale,
            steps = sdGenerationParameters.steps,

            seed = sdGenerationParameters.seed,
            subseed_strength = sdGenerationParameters.seedVariationStrength,

            width = sdGenerationParameters.width,
            height = sdGenerationParameters.height,

            sampler_index = sdGenerationParameters.sampler,

            override_settings = new SdSettings
            {
                CLIP_stop_at_last_layers = sdGenerationParameters.clipSkip
            }
        };
        var response = await SdApiClient.txt2imgAsync(parameters, onProgress: step => control.NotifyProgress(step));

        if (response == null)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            throw new SdGeneratorFatalErrorException("ERROR");
        }

        // SD not ready, need retry later
        if (response.images == null) throw new SdGeneratorNeedRetryException();

        if (control.IsWantToCancelProcessingResultOfCurrentGeneration)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            return false;
        }

        processGenerationResult(BitmapTools.FromBase64(response.images[0]), response.infoParsed.seed);

        control.NotifyProgress(sdGenerationParameters.steps);

        return true;
    }

    public void Cancel()
    {
        Task.Run(SdApiClient.Cancel);
    }

    private void processGenerationResult(Bitmap resultImage, long seed)
    {
        try
        {
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            var destImageFilePath = Path.Combine(destDir, DateTime.UtcNow.Ticks / 10000 + ".png");
            SdPngHelper.Save(resultImage, sdGenerationParameters, seed, destImageFilePath);
            resultImage.Dispose();
        }
        catch (Exception ee)
        {
            SdApiClient.Log.WriteLine(ee.ToString());
        }
    }
}
