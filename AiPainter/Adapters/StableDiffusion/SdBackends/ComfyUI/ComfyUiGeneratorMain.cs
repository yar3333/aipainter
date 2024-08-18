using System.Text.Json;
using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiGeneratorMain : ISdGenerator
{
    private static readonly Random random = new();

    private readonly SdGenerationParameters sdGenerationParameters;
    private readonly SdGenerationListItem control;

    private readonly string destDir;

    public ComfyUiGeneratorMain(SdGenerationParameters sdGenerationParameters, SdGenerationListItem control, string destDir)
    {
        this.sdGenerationParameters = sdGenerationParameters;
        this.control = control;

        this.destDir = destDir;
    }

    public async Task<bool> RunAsync()
    {
        // subseed_strength = sdGenerationParameters.seedVariationStrength
        
        var seed = sdGenerationParameters.seed > 0 
                       ? sdGenerationParameters.seed 
                       : random.NextInt64(1, uint.MaxValue);

        var workflow = ComfyUiGeneratorHelper.CreateWorkflow(sdGenerationParameters, seed);
        
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

        SdGeneratorHelper.SaveMain(sdGenerationParameters, seed, destDir, images[0]);

        control.NotifyProgress(sdGenerationParameters.steps);

        return true;
    }

    public void Cancel()
    {
        Task.Run(async () => await ComfyUiApiClient.interrupt());
    }
}