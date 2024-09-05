using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using AiPainter.Helpers;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiGeneratorInpaint : ISdGenerator
{
    private readonly SdGenerationListItem control;

    private readonly Bitmap originalImage;
    private readonly Rectangle activeBox;
    private readonly Bitmap? croppedMask;
    private readonly string originalFilePath;

    public ComfyUiGeneratorInpaint(SdGenerationListItem control, Bitmap originalImage, Rectangle activeBox, Bitmap? croppedMask, string originalFilePath)
    {
        this.control = control;
        this.originalImage = originalImage;
        this.activeBox = activeBox;
        this.croppedMask = croppedMask;
        this.originalFilePath = originalFilePath;
    }

    public async Task<bool> RunAsync(SdGenerationParameters sdGenerationParameters)
    {
        Debug.Assert(sdGenerationParameters.seed > 0);

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

        using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);

        var workflow = ComfyUiGeneratorHelper.CreateWorkflow("img2img.json", sdGenerationParameters);
        
        var nodeVAELoader = (VAELoaderNode?)workflow.SingleOrDefault(x => x.Id == "nodeVAELoader");
        if (nodeVAELoader != null)
        {
            var nodeVAEEncodeForInpaint = (VAEEncodeForInpaintNode)workflow.Single(x => x.Id == "nodeVAEEncodeForInpaint");
            var nodeInpaintModelConditioning = (InpaintModelConditioningNode)workflow.Single(x => x.Id == "nodeInpaintModelConditioning");
            nodeVAEEncodeForInpaint.vae = nodeVAELoader.Output_vae;
            nodeInpaintModelConditioning.vae = nodeVAELoader.Output_vae;;
        }

        var nodeETN_LoadImageBase64 = (ETN_LoadImageBase64Node)workflow.Single(x => x.Id == "nodeETN_LoadImageBase64");
        nodeETN_LoadImageBase64.image = BitmapTools.ToBase64(croppedImage);

        var nodeETN_LoadMaskBase64 = (ETN_LoadMaskBase64Node)workflow.Single(x => x.Id == "nodeETN_LoadMaskBase64");
        nodeETN_LoadMaskBase64.mask = BitmapTools.ToBase64(croppedMask ?? BitmapTools.CreateBitmap(croppedImage.Width, croppedImage.Height, Color.White));

        var nodeKSampler = (KSamplerNode)workflow.Single(x => x.Id == "nodeKSampler");
        nodeKSampler.denoise = sdGenerationParameters.changesLevel;

        var client = await ComfyUiApiClient.ConnectAsync();
        var images = await client.RunPromptAsync
                     (
                         JsonSerializer.Deserialize<JsonObject>(WorkflowHelper.SerializeWorkflow(workflow))!,
                         "nodeSaveImageWebsocket", 
                         step => control.NotifyProgress(step)
                     );

        if (control.IsWantToCancelProcessingResultOfCurrentGeneration)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            return false;
        }
        
        if (images == null || images.Length == 0)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            throw new SdGeneratorFatalErrorException("ERROR");
        }

        if (control.IsWantToCancelProcessingResultOfCurrentGeneration)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            return false;
        }

        processGenerationResult(sdGenerationParameters, images[0]);

        control.NotifyProgress(sdGenerationParameters.steps);

        return true;
    }

    public void Cancel()
    {
        Task.Run(async () => await ComfyUiApiClient.interrupt());
    }
    
    private void processGenerationResult(SdGenerationParameters sdGenerationParameters, Bitmap resultImage)
    {
        try
        {
            using var tempOriginalImage = BitmapTools.Clone(originalImage);
            BitmapTools.DrawBitmapAtPos(resultImage, tempOriginalImage, activeBox.X, activeBox.Y);
            resultImage.Dispose();
            SdGeneratorHelper.SaveInpaint(sdGenerationParameters, originalFilePath, tempOriginalImage);
        }
        catch (Exception e)
        {
            ComfyUiApiClient.Log.WriteLine(e.ToString());
        }
    }
}
