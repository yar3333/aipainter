using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiGeneratorMain : ISdGenerator
{
    private readonly SdListItemGeneration control;

    private readonly string destDir;

    public ComfyUiGeneratorMain(SdListItemGeneration control, string destDir)
    {
        this.control = control;
        this.destDir = destDir;
    }

    public async Task<bool> RunAsync(SdGenerationParameters sdGenerationParameters)
    {
        Debug.Assert(sdGenerationParameters.seed > 0);

        var workflow = ComfyUiGeneratorHelper.CreateWorkflow("txt2img.json", sdGenerationParameters);

        // EmptyLatentImage
        var nodeEmptyLatentImage = (EmptyLatentImageNode)workflow.Single(x => x.Id == "nodeEmptyLatentImage");
        nodeEmptyLatentImage.width = sdGenerationParameters.width;
        nodeEmptyLatentImage.height = sdGenerationParameters.height;
        
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

        SdGeneratorHelper.SaveMain(sdGenerationParameters, destDir, images[0]);

        control.NotifyProgress(sdGenerationParameters.steps);

        return true;
    }

    public void Cancel()
    {
        Task.Run(async () => await ComfyUiApiClient.interrupt());
    }
}