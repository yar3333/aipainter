using System.Text.Json;
using System.Text.Json.Nodes;
using AiPainter.Helpers;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.SdApiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

class ComfyUiGeneratorMain : ISdGenerator
{
    private static Random random = new();

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
        /*var wasProgressShown = false;
        var isCheckpointSuccess = await WebUiGeneratorHelper.PrepareCheckpointAsync
        (
            true,
            sdGenerationParameters.checkpointName,
            sdGenerationParameters.vaeName,
            () => control.IsDisposed,
            s => { wasProgressShown = true; control.NotifyStepsCustomText(s); }
        );
        if (!isCheckpointSuccess) return false;
        if (!wasProgressShown) control.NotifyProgress(0);*/

        /*var parameters = new SdTxt2ImgRequest
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
        var response = await SdApiClient.txt2imgAsync(parameters, onProgress: step => control.NotifyProgress(step));*/

        var seed = sdGenerationParameters.seed > 0 ? sdGenerationParameters.seed : random.NextInt64(1, uint.MaxValue);

        var client = await ComfyUiApiClient.ConnectAsync();

        var workflow = JsonSerializer.Deserialize<JsonObject>(File.ReadAllText(Path.Join(Application.StartupPath, "ComfyWorkflowTemplates\\txt2img.json")))!;

        // KSampler
        {
            var inputs = workflow["3"]!.AsObject()["inputs"]!.AsObject();
            inputs["seed"] = JsonValue.Create(seed);
            inputs["steps"] = JsonValue.Create(sdGenerationParameters.steps);
            inputs["cfg"] = JsonValue.Create(sdGenerationParameters.cfgScale);
            inputs["sampler_name"] = JsonValue.Create(ComfyNamesHelper.GetSamplerName(sdGenerationParameters.sampler));
            //inputs["scheduler"] = JsonValue.Create("normal"); // karras
            //inputs["denoise"] = JsonValue.Create(1.0);
        }
                
        // CheckpointLoaderSimple
        {
            var inputs = workflow["4"]!.AsObject()["inputs"]!.AsObject();
            inputs["ckpt_name"] = JsonValue.Create(SdCheckpointsHelper.GetPathToMainCheckpoint(sdGenerationParameters.checkpointName)!);
        }

        // EmptyLatentImage
        {
            var inputs = workflow["5"]!.AsObject()["inputs"]!.AsObject();
            inputs["width"]  = JsonValue.Create(sdGenerationParameters.width);
            inputs["height"] = JsonValue.Create(sdGenerationParameters.height);
        }

        // CLIPTextEncode (positive)
        {
            var inputs = workflow["6"]!.AsObject()["inputs"]!.AsObject();
            inputs["text"] = JsonValue.Create(sdGenerationParameters.prompt);
        }        
        
        // CLIPTextEncode (negative)
        {
            var inputs = workflow["7"]!.AsObject()["inputs"]!.AsObject();
            inputs["text"] = JsonValue.Create(sdGenerationParameters.negative);
        }        
        
        // VAEDecode
        {
            var inputs = workflow["8"]!.AsObject()["inputs"]!.AsObject();
            inputs["vae"] = ComfyNodeOutputHelper.CheckpointLoaderSimple_vae("4");
        }

        var images = await client.RunPromptAsync(workflow);

        if (images == null || images.Length == 0)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            throw new SdGeneratorFatalErrorException("ERROR");
        }

        // SD not ready, need retry later
        //if (response.images == null) throw new SdGeneratorNeedRetryException();

        if (control.IsWantToCancelProcessingResultOfCurrentGeneration)
        {
            control.NotifyProgress(sdGenerationParameters.steps);
            return false;
        }

        SdGeneratorHelper.SaveMain(ComfyUiApiClient.Log, sdGenerationParameters, seed, destDir, images[0]);

        control.NotifyProgress(sdGenerationParameters.steps);

        return true;
    }

    public void Cancel()
    {
        //Task.Run(SdApiClient.Cancel);
    }
}
