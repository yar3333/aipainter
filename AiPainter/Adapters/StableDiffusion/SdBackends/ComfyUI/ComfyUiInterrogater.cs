using System.Text.Json;
using System.Text.Json.Nodes;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyUiInterrogater
{
    public static async Task<string?> RunAsync(Bitmap image)
    {
        var workflow = ComfyUiGeneratorHelper.LoadWorkflow("interrogate.json");

        var nodeETN_LoadImageBase64 = (ETN_LoadImageBase64Node)workflow.Single(x => x.Id == "nodeETN_LoadImageBase64");
        nodeETN_LoadImageBase64.image = BitmapTools.ToBase64(image);
        
        var client = await ComfyUiApiClient.ConnectAsync();
        return await client.RunPromptAndGetTextAsync
        (
            JsonSerializer.Deserialize<JsonObject>(WorkflowHelper.SerializeWorkflow(workflow))!,
            "nodeShowText"
        );
    }
}
