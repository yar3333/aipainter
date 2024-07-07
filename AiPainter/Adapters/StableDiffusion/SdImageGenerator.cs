using System.Drawing.Imaging;
using System.Text.Json;
using AiPainter.Helpers;
using System.Text.RegularExpressions;
using AiPainter.Controls;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;

namespace AiPainter.Adapters.StableDiffusion;

class SdImageGenerator : IImageGenerator
{
    private readonly StableDiffusionPanel sdPanel;
    private readonly SmartPictureBox pictureBox;
    private readonly MainForm mainForm;

    private readonly int originalCount;

    private readonly SdGenerationParameters sdGenerationParameters;

    private readonly Rectangle activeBox;
    private readonly Bitmap? originalImage;
    private readonly Bitmap? croppedMask;

    private readonly string? savedFilePath;
    private readonly Primitive[] savedMask;
    private readonly string destDir;

    private GenerationListItem control = null;

    public SdImageGenerator(StableDiffusionPanel sdPanel, SmartPictureBox pictureBox, MainForm mainForm)
    {
        this.sdPanel = sdPanel;
        this.pictureBox = pictureBox;
        this.mainForm = mainForm;

        originalCount = (int)sdPanel.numIterations.Value;

        sdGenerationParameters = new SdGenerationParameters
        {
            checkpointName = sdPanel.selectedCheckpointName,
            vaeName = Program.Config.StableDiffusionVae,
            prompt = sdPanel.tbPrompt.Text.Trim(),
            negative = sdPanel.tbNegative.Text.Trim(),
            steps = (int)sdPanel.numSteps.Value,
            cfgScale = sdPanel.numCfgScale.Value,
            
            seed = sdPanel.cbUseSeed.Checked && sdPanel.tbSeed.Text.Trim() != "" ? long.Parse(sdPanel.tbSeed.Text.Trim()) : -1,
            seedVariationStrength = sdPanel.trackBarSeedVariationStrength.Value / 100m,

            width = int.Parse(sdPanel.ddImageSize.SelectedItem.ToString()!.Split("x")[0]),
            height = int.Parse(sdPanel.ddImageSize.SelectedItem.ToString()!.Split("x")[1]),
            sampler = sdPanel.ddSampler.SelectedItem.ToString()!,
            changesLevel = sdPanel.cbUseInitImage.Checked ? sdPanel.trackBarChangesLevel.Value / 100.0m : -1,
        };

        if (sdPanel.cbUseInitImage.Checked)
        {
            activeBox = pictureBox.ActiveBox;
            originalImage = BitmapTools.Clone(pictureBox.Image!);
            croppedMask = pictureBox.GetMaskCropped(Color.Black, Color.White);
        }

        savedFilePath = mainForm.FilePath;
        savedMask = pictureBox.SaveMask();
        destDir = mainForm.ImagesFolder!;
    }

    public int GetOriginalCount() => originalCount;

    public string GetBasePromptText() => sdGenerationParameters.prompt;

    public int GetStepsMax() => sdGenerationParameters.steps;

    public string GetTooltip() => "Positive prompt:\n" + sdGenerationParameters.prompt + "\n\n"
                                + "Negative prompt:\n" + sdGenerationParameters.negative;

    // ReSharper disable once ParameterHidesMember
    public void SetControl(GenerationListItem control)
    {
        this.control = control;
    }

    public void LoadParamsBackToPanel()
    {
        sdPanel.selectedCheckpointName = sdGenerationParameters.checkpointName;
        sdPanel.selectedVaeName = sdGenerationParameters.vaeName;
        
        sdPanel.numSteps.Value = sdGenerationParameters.steps;
        sdPanel.tbPrompt.Text = sdGenerationParameters.prompt;
        sdPanel.tbNegative.Text = sdGenerationParameters.negative;
        sdPanel.numCfgScale.Value = sdGenerationParameters.cfgScale;

        sdPanel.cbUseSeed.Checked = sdGenerationParameters.seed > 0;
        sdPanel.tbSeed.Text = sdGenerationParameters.seed.ToString();
        sdPanel.trackBarSeedVariationStrength.Value = (int)Math.Round(sdGenerationParameters.seedVariationStrength * 100);
        
        sdPanel.ddSampler.SelectedItem = sdGenerationParameters.sampler;
        if (sdGenerationParameters.changesLevel >= 0) sdPanel.trackBarChangesLevel.Value = (int)Math.Round(sdGenerationParameters.changesLevel * 100);

        sdPanel.cbUseInitImage.Checked = originalImage != null;

        if (originalImage != null)
        {
            mainForm.FilePath = savedFilePath;

            pictureBox.HistoryAddCurrentState();
            pictureBox.ActiveBox = activeBox;
            pictureBox.Image = BitmapTools.Clone(originalImage);
            pictureBox.LoadMask(savedMask);
            pictureBox.Refresh();
        }

        sdPanel.SetImageSize(sdGenerationParameters.width, sdGenerationParameters.height);
    }

    public async Task RunAsync()
    {
        var checkpointFilePath = originalImage == null || croppedMask == null
                                     ? SdCheckpointsHelper.GetPathToMainCheckpoint(sdGenerationParameters.checkpointName)
                                     : (SdCheckpointsHelper.GetPathToInpaintCheckpoint(sdGenerationParameters.checkpointName) 
                                     ?? SdCheckpointsHelper.GetPathToMainCheckpoint(sdGenerationParameters.checkpointName));
        
        if (checkpointFilePath == null)
        {
            control.NotifyGenerateFail("NOT FOUND");
            return;
        }

        var vaeFilePath = SdVaeHelper.GetPathToVae(sdGenerationParameters.vaeName) 
                       ?? SdCheckpointsHelper.GetPathToVae(sdGenerationParameters.checkpointName) 
                       ?? "";

        if (Program.Config.UseEmbeddedStableDiffusion && StableDiffusionProcess.Running)
        {
            if (StableDiffusionProcess.ActiveCheckpointFilePath != checkpointFilePath || StableDiffusionProcess.ActiveVaeFilePath != vaeFilePath)
            {
                control.NotifyStepsCustomText("Stopping...");
                StableDiffusionProcess.Stop();
                while (StableDiffusionProcess.IsReady())
                {
                    if (await DelayTools.WaitForExitAsync(500) || control.IsDisposed) return;
                }
            }
        }

        var waitTextShown = false;

        if (Program.Config.UseEmbeddedStableDiffusion && !StableDiffusionProcess.Running)
        {
            waitTextShown = true;
            control.NotifyStepsCustomText("Starting...");
            StableDiffusionProcess.Start(checkpointFilePath, vaeFilePath);
            while (!StableDiffusionProcess.IsReady())
            {
                if (await DelayTools.WaitForExitAsync(500) || control.IsDisposed) return;
            }
        }

        if (!StableDiffusionProcess.IsReady())
        {
            waitTextShown = true;
            control.NotifyStepsCustomText("Waiting ready...");
            while (!StableDiffusionProcess.IsReady())
            {
                if (await DelayTools.WaitForExitAsync(500) || control.IsDisposed) return;
            }
        }

        if (!waitTextShown) control.NotifyProgress(0);

        if (originalImage == null)
        {
            generate
            (
                null,
                null,
                (resultImage, seed) =>
                {
                    try
                    {
                        var resultFilePath = getDestImageFilePath();
                        resultImage.Save(resultFilePath, ImageFormat.Png);
                        resultImage.Dispose();

                        saveSdGeneraqtionParameters(resultFilePath, seed);
                    }
                    catch (Exception ee)
                    {
                        SdApiClient.Log.WriteLine(ee.ToString());
                    }
                }
            );
        }
        else
        {
            using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);
            using var image512 = BitmapTools.GetResized(croppedImage, 512, 512);
            using var mask512 = croppedMask != null ? BitmapTools.GetResized(croppedMask, 512, 512) : null;

            generate
            (
                image512,
                mask512,
                (resultImage, seed) =>
                {
                    try
                    {
                        using var resultImageResized = BitmapTools.GetResized(resultImage, activeBox.Width, activeBox.Height);
                        resultImage.Dispose();

                        using var tempOriginalImage = BitmapTools.Clone(originalImage);
                        BitmapTools.DrawBitmapAtPos(resultImageResized, tempOriginalImage, activeBox.X, activeBox.Y);

                        saveImageBasedOnOriginalImage(tempOriginalImage, seed);
                    }
                    catch (Exception ee)
                    {
                        SdApiClient.Log.WriteLine(ee.ToString());
                    }
                }
            );
        }
    }

    public void Cancel()
    {
        Task.Run(SdApiClient.Cancel);
    }

    private void saveImageBasedOnOriginalImage(Bitmap image, long seed)
    {
        var basePath = Path.Join(Path.GetDirectoryName(savedFilePath), Path.GetFileNameWithoutExtension(savedFilePath));
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
            resultFilePath = basePath + "-aip" + n.ToString("D3") + Path.GetExtension(savedFilePath);
            if (!File.Exists(resultFilePath)) break;
            n++;
        }

        image.Save
        (
            resultFilePath,
            Path.GetExtension(resultFilePath).ToLowerInvariant() == ".png" ? ImageFormat.Png : ImageFormat.Jpeg
        );
        saveSdGeneraqtionParameters(resultFilePath, seed);
    }

    private void generate(Bitmap? initImage, Bitmap? maskImage, Action<Bitmap, long> processGeneratedImage)
    {
        if (initImage == null)
        {
            var parameters = new SdTxt2ImgRequest
            {
                prompt = getFullPromptText(),
                negative_prompt = sdGenerationParameters.negative,
                cfg_scale = sdGenerationParameters.cfgScale,
                steps = sdGenerationParameters.steps,

                seed = sdGenerationParameters.seed,
                subseed_strength = sdGenerationParameters.seedVariationStrength,

                width = sdGenerationParameters.width,
                height = sdGenerationParameters.height,
                
                sampler_index = sdGenerationParameters.sampler,

                override_settings = SdCheckpointsHelper.GetConfig(sdGenerationParameters.checkpointName).overrideSettings,
            };

            SdApiClient.txt2img
            (
                parameters,
                onProgress: step => control.NotifyProgress(step),
                onSuccess: ev => onImageGenerated(ev, processGeneratedImage)
            );
        }
        else
        {
            var parameters = new SdImg2ImgRequest
            {
                prompt = getFullPromptText(),
                negative_prompt = sdGenerationParameters.negative,
                cfg_scale = sdGenerationParameters.cfgScale,
                steps = sdGenerationParameters.steps,
                
                seed = sdGenerationParameters.seed,
                subseed_strength = sdGenerationParameters.seedVariationStrength,
                
                init_images = new[] { BitmapTools.GetBase64String(initImage) },
                mask = maskImage != null ? BitmapTools.GetBase64String(maskImage) : null,
                
                width = initImage.Width,
                height = initImage.Height,
                
                sampler_index = sdGenerationParameters.sampler,

                inpainting_fill = SdInpaintingFill.original, // looks like webui use 'fill' as default if mask specified, so force to use 'original'
                denoising_strength = sdGenerationParameters.changesLevel,

                override_settings = SdCheckpointsHelper.GetConfig(sdGenerationParameters.checkpointName).overrideSettings,
            };

            SdApiClient.img2img
            (
                parameters,
                onProgress: step => control.NotifyProgress(step),
                onSuccess: ev => onImageGenerated(ev, processGeneratedImage)
            );
        }
    }

    private void onImageGenerated(SdGenerationResponse? ev, Action<Bitmap, long> processGeneratedImage)
    {
        if (ev == null)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            control.NotifyGenerateFail("ERROR");
            return;
        }

        // SD not ready, need retry later
        if (ev.images == null) { control.NotifyNeedRetry(); return; }

        control.NotifyProgress(sdGenerationParameters.steps);

        if (!control.IsWantToCancelProcessingResultOfCurrentGeneration)
        {
            processGeneratedImage(BitmapTools.FromBase64(ev.images[0]), ev.infoParsed.seed);
        }

        control.NotifyGenerateSuccess();
    }

    private string getFullPromptText()
    {
        var checkpointPrompt = SdCheckpointsHelper.GetConfig(sdGenerationParameters.checkpointName).promptRequired;

        var r = (!string.IsNullOrWhiteSpace(checkpointPrompt) ? checkpointPrompt + "; " : "")
              + (!string.IsNullOrWhiteSpace(sdGenerationParameters.prompt) ? sdGenerationParameters.prompt + "; " : "");

        return r.Trim(' ', ',', ';');
    }

    private string getDestImageFilePath()
    {
        if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
        return Path.Combine(destDir, (DateTime.UtcNow.Ticks / 10000) + ".png");
    }

    private void saveSdGeneraqtionParameters(string resultImageFilePath, long seed)
    {
        var t = sdGenerationParameters.ShallowCopy();
        t.seed = seed;
        var resultJsonFilePath = Path.Join(Path.GetDirectoryName(resultImageFilePath), Path.GetFileNameWithoutExtension(resultImageFilePath)) + ".json";
        File.WriteAllText(resultJsonFilePath, JsonSerializer.Serialize(t, new JsonSerializerOptions { PropertyNamingPolicy = null, WriteIndented = true }));
    }
}
