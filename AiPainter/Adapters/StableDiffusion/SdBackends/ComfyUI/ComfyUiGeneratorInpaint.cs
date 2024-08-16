using AiPainter.Helpers;
using System.Text.RegularExpressions;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiGeneratorInpaint : ISdGenerator
{
    private readonly SdGenerationParameters sdGenerationParameters;
    private readonly SdGenerationListItem control;

    private readonly Bitmap originalImage;
    private readonly Rectangle activeBox;
    private readonly Bitmap? croppedMask;
    private readonly string originalFilePath;

    public ComfyUiGeneratorInpaint(SdGenerationParameters sdGenerationParameters, SdGenerationListItem control, Bitmap originalImage, Rectangle activeBox, Bitmap? croppedMask, string originalFilePath)
    {
        this.sdGenerationParameters = sdGenerationParameters;
        this.control = control;

        this.activeBox = activeBox;
        this.originalImage = originalImage;
        this.croppedMask = croppedMask;
        this.originalFilePath = originalFilePath;
    }

    public async Task<bool> RunAsync()
    {
        /*var wasProgressShown = false;
        var isCheckpointSuccess = await WebUiGeneratorHelper.PrepareCheckpointAsync
        (
            false,
            sdGenerationParameters.checkpointName,
            sdGenerationParameters.vaeName,
            () => control.IsDisposed,
            s => { wasProgressShown = true; control.NotifyStepsCustomText(s); }
        );
        if (!isCheckpointSuccess) return false;
        if (!wasProgressShown) control.NotifyProgress(0);

        using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);

        var parameters = new SdImg2ImgRequest
        {
            prompt = sdGenerationParameters.prompt,
            negative_prompt = sdGenerationParameters.negative,
            cfg_scale = sdGenerationParameters.cfgScale,
            steps = sdGenerationParameters.steps,

            seed = sdGenerationParameters.seed,
            subseed_strength = sdGenerationParameters.seedVariationStrength,

            init_images = new[] { BitmapTools.GetBase64String(croppedImage) },
            mask = croppedMask != null ? BitmapTools.GetBase64String(croppedMask) : null,

            width = croppedImage.Width,
            height = croppedImage.Height,

            sampler_index = sdGenerationParameters.sampler,

            // looks like webui use 'fill' as default if mask specified, so force to use 'original'
            inpainting_fill = sdGenerationParameters.inpaintingFill ?? SdInpaintingFill.original,
            denoising_strength = sdGenerationParameters.changesLevel,

            override_settings = new SdSettings
            {
                CLIP_stop_at_last_layers = sdGenerationParameters.clipSkip
            }
        };
        var response = await SdApiClient.img2imgAsync(parameters, onProgress: step => control.NotifyProgress(step));*/

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
        Task.Run(async () => await ComfyUiApiClient.interrupt());
    }
    
    private void processGenerationResult(Bitmap resultImage, long seed)
    {
        try
        {
            using var tempOriginalImage = BitmapTools.Clone(originalImage);
            BitmapTools.DrawBitmapAtPos(resultImage, tempOriginalImage, activeBox.X, activeBox.Y);
            resultImage.Dispose();
            saveImageBasedOnOriginalImage(tempOriginalImage, seed);
        }
        catch (Exception ee)
        {
            SdApiClient.Log.WriteLine(ee.ToString());
        }
    }

    private void saveImageBasedOnOriginalImage(Bitmap image, long seed)
    {
        var basePath = Path.Join(Path.GetDirectoryName(originalFilePath), Path.GetFileNameWithoutExtension(originalFilePath));
        var matches = Regex.Matches(basePath, @"-aip(\d+)$");

        var n = 1;
        if (matches.Count == 1)
        {
            n = int.Parse(matches[0].Groups[1].Value) + 1;
            basePath = basePath.Substring(0, basePath.Length - matches[0].Groups[0].Value.Length);
        }

        string resultFilePath;
        for (; ; )
        {
            resultFilePath = basePath + "-aip" + n.ToString("D3") + ".png";
            if (!File.Exists(resultFilePath)) break;
            n++;
        }

        SdPngHelper.Save(image, sdGenerationParameters, seed, resultFilePath);
    }
}
