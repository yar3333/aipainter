using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using AiPainter.Helpers;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiGeneratorInpaint : ISdGenerator
{
    private readonly SdListItemGeneration control;

    private readonly Bitmap originalImage;
    private readonly Rectangle activeBox;
    private readonly Bitmap? croppedMask;
    private readonly string originalFilePath;

    public ComfyUiGeneratorInpaint(SdListItemGeneration control, Bitmap originalImage, Rectangle activeBox, Bitmap? croppedMask, string originalFilePath)
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

        List<BaseNode> workflow;
        try
        {
            workflow = prepareWorkFlow(sdGenerationParameters);
        }
        catch (Exception e)
        {
            ComfyUiApiClient.Log.WriteLine(e.ToString());
            throw;
        }

        var client = await ComfyUiApiClient.ConnectAsync();
        var images = await client.RunPromptAndGetImageAsync
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

    private List<BaseNode> prepareWorkFlow(SdGenerationParameters sdGenerationParameters)
    {
        using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);

        var workflow = ComfyUiGeneratorHelper.CreateWorkflow("img2img.json", sdGenerationParameters);
        
        var nodeVAELoader = (VAELoaderNode?)workflow.SingleOrDefault(x => x.Id == "nodeVAELoader");
        if (nodeVAELoader != null)
        {
            //var nodeVAEEncodeForInpaint = (VAEEncodeForInpaintNode)workflow.Single(x => x.Id == "nodeVAEEncodeForInpaint");
            //nodeVAEEncodeForInpaint.vae = nodeVAELoader.Output_vae;
            var nodeInpaintModelConditioning = (InpaintModelConditioningNode)workflow.Single(x => x.Id == "nodeInpaintModelConditioning");
            nodeInpaintModelConditioning.vae = nodeVAELoader.Output_vae;;
        }

        var nodeETN_LoadImageBase64 = (ETN_LoadImageBase64Node)workflow.Single(x => x.Id == "nodeETN_LoadImageBase64");
        nodeETN_LoadImageBase64.image = BitmapTools.ToBase64(croppedImage);

        var nodeETN_LoadMaskBase64 = (ETN_LoadMaskBase64Node)workflow.Single(x => x.Id == "nodeETN_LoadMaskBase64");
        nodeETN_LoadMaskBase64.mask = BitmapTools.ToBase64(croppedMask ?? BitmapTools.CreateBitmap(croppedImage.Width, croppedImage.Height, Color.White));

        var nodeKSampler = (KSamplerNode)workflow.Single(x => x.Id == "nodeKSampler");
        nodeKSampler.denoise = sdGenerationParameters.changesLevel;

        return workflow;
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
