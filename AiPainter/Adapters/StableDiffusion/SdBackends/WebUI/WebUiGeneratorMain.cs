using AiPainter.Helpers;
using AiPainter.Adapters.StableDiffusion.SdBackends.WebUI.SdApiClientStuff;
using System.Diagnostics;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI;

class WebUiGeneratorMain : ISdGenerator
{
    private readonly SdListItemGeneration control;

    private readonly string destDir;

    public WebUiGeneratorMain(SdListItemGeneration control, string destDir)
    {
        this.control = control;

        this.destDir = destDir;
    }

    public async Task<bool> RunAsync(SdGenerationParameters sdGenerationParameters)
    {
        Debug.Assert(sdGenerationParameters.seed > 0);

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
        var response = await WebUiApiClient.txt2imgAsync(parameters, onProgress: step => control.NotifyProgress(step));

        if (response == null)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            throw new SdGeneratorFatalErrorException("ERROR");
        }

        // SD not ready, need retry later
        if (response.images == null) throw new SdListItemGenerationNeedRetryException();

        if (control.IsWantToCancelProcessingResultOfCurrentGeneration)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            return false;
        }

        SdGeneratorHelper.SaveMain(sdGenerationParameters, destDir, BitmapTools.FromDataUri(response.images[0]));

        control.NotifyProgress(sdGenerationParameters.steps);

        return true;
    }

    public void Cancel()
    {
        Task.Run(WebUiApiClient.Cancel);
    }
}
