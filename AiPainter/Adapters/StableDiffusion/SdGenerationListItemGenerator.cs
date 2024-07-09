using AiPainter.Helpers;
using System.Text.RegularExpressions;
using AiPainter.Controls;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;

namespace AiPainter.Adapters.StableDiffusion;

public class SdGenerationListItemGenerator
{
    private readonly SdGenerationListItem control;
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

    public SdGenerationListItemGenerator(SdGenerationListItem control, StableDiffusionPanel sdPanel, SmartPictureBox pictureBox, MainForm mainForm)
    {
        this.control = control;
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
        if (!await prepareCheckpointAsync()) return;

        SdGenerationResponse? response;

        if (originalImage == null)
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
            response = await SdApiClient.txt2imgAsync(parameters, onProgress: step => control.NotifyProgress(step));
        }
        else
        {
            using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);

            var parameters = new SdImg2ImgRequest
            {
                prompt = getFullPromptText(),
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

                inpainting_fill = SdInpaintingFill.original, // looks like webui use 'fill' as default if mask specified, so force to use 'original'
                denoising_strength = sdGenerationParameters.changesLevel,

                override_settings = SdCheckpointsHelper.GetConfig(sdGenerationParameters.checkpointName).overrideSettings,
            };
            response = await SdApiClient.img2imgAsync(parameters, onProgress: step => control.NotifyProgress(step));
        }

        if (response == null)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            control.NotifyGenerateFail("ERROR");
            return;
        }

        // SD not ready, need retry later
        if (response.images == null) { control.NotifyNeedRetry(); return; }

        if (control.IsWantToCancelProcessingResultOfCurrentGeneration) return;

        processGenerationResult(BitmapTools.FromBase64(response.images[0]), response.infoParsed.seed);
        
        control.NotifyProgress(sdGenerationParameters.steps);
        control.NotifyGenerateSuccess();
    }

    private async Task<bool> prepareCheckpointAsync()
    {
        var checkpointFilePath = originalImage == null || croppedMask == null
                                     ? SdCheckpointsHelper.GetPathToMainCheckpoint(sdGenerationParameters.checkpointName)
                                     : (SdCheckpointsHelper.GetPathToInpaintCheckpoint(sdGenerationParameters.checkpointName) 
                                     ?? SdCheckpointsHelper.GetPathToMainCheckpoint(sdGenerationParameters.checkpointName));
        
        if (checkpointFilePath == null)
        {
            control.NotifyGenerateFail("NOT FOUND");
            return false;
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
                    if (await DelayTools.WaitForExitAsync(500) || control.IsDisposed) return false;
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
                if (await DelayTools.WaitForExitAsync(500) || control.IsDisposed) return false;
            }
        }

        if (!StableDiffusionProcess.IsReady())
        {
            waitTextShown = true;
            control.NotifyStepsCustomText("Waiting ready...");
            while (!StableDiffusionProcess.IsReady())
            {
                if (await DelayTools.WaitForExitAsync(500) || control.IsDisposed) return false;
            }
        }

        if (!waitTextShown) control.NotifyProgress(0);

        return true;
    }

    private void processGenerationResult(Bitmap resultImage, long seed)
    {
        if (originalImage == null)
        {
            try
            {
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                var destImageFilePath = Path.Combine(destDir, (DateTime.UtcNow.Ticks / 10000) + ".png");
                SdPngHelper.Save(resultImage, sdGenerationParameters, seed, destImageFilePath);
                resultImage.Dispose();
            }
            catch (Exception ee)
            {
                SdApiClient.Log.WriteLine(ee.ToString());
            }
        }
        else
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
            resultFilePath = basePath + "-aip" + n.ToString("D3") + ".png";
            if (!File.Exists(resultFilePath)) break;
            n++;
        }

        SdPngHelper.Save(image, sdGenerationParameters, seed, resultFilePath);
    }

    private string getFullPromptText()
    {
        var checkpointPrompt = SdCheckpointsHelper.GetConfig(sdGenerationParameters.checkpointName).promptRequired;

        var r = (!string.IsNullOrWhiteSpace(checkpointPrompt) ? checkpointPrompt + "; " : "")
              + (!string.IsNullOrWhiteSpace(sdGenerationParameters.prompt) ? sdGenerationParameters.prompt + "; " : "");

        return r.Trim(' ', ',', ';');
    }
}
