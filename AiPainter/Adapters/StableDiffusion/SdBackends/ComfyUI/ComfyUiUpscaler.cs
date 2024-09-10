using System.Text.Json;
using System.Text.Json.Nodes;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyUiUpscaler
{
    public static async Task<Bitmap?> RunAsync(UpscalerType upscaler, int resizeFactor, string imageBase64, Action<int> progressPercent, CancellationTokenSource cancellationTokenSource)
    {
        var workflow = ComfyUiGeneratorHelper.LoadWorkflow("upscale.json");

        var nodeETN_LoadImageBase64 = (ETN_LoadImageBase64Node)workflow.Single(x => x.Id == "nodeETN_LoadImageBase64");
        nodeETN_LoadImageBase64.image = imageBase64.Split(',').Last();
        
        var nodeUpscaleModelLoader = (UpscaleModelLoaderNode)workflow.Single(x => x.Id == "nodeUpscaleModelLoader");
        nodeUpscaleModelLoader.model_name = getUpscalerName(upscaler);

        var cancelCalled = false;
        var client = await ComfyUiApiClient.ConnectAsync();
        var images = await client.RunPromptAndGetImageAsync
                     (
                         JsonSerializer.Deserialize<JsonObject>(WorkflowHelper.SerializeWorkflow(workflow))!,
                         "nodeSaveImageWebsocket", 
                         step =>
                         {
                             progressPercent(step * 25);
                             if (!cancelCalled && cancellationTokenSource.IsCancellationRequested)
                             {
                                 cancelCalled = true;
                                 #pragma warning disable CS4014
                                 ComfyUiApiClient.interrupt();
                                 #pragma warning restore CS4014
                             }
                         }
                     );

        if (cancellationTokenSource.IsCancellationRequested || images.Length == 0) return null;

        return resizeFactor == 4 
                   ? images[0] 
                   : BitmapTools.GetResized(images[0], images[0].Width / 4 * resizeFactor, images[0].Height / 4 * resizeFactor);
    }

    private static string getUpscalerName(UpscalerType upscaler)
    {
        return upscaler switch
        {
            UpscalerType.R_ESRGAN_4x         => "RealESRGAN_x4plus.pth",
            UpscalerType.R_ESRGAN_4x_Anime6B => "RealESRGAN_x4plus_anime_6B.pth",
            _ => throw new ArgumentException("upscaler = " + (int)upscaler)
        };
    }
}
