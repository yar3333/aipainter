using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;

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
        var loras = SdPromptNormalizer.GetUsedLoras(sdGenerationParameters.prompt, out var prompt);
        foreach (var embedding in getActiveEmbeddingNames())
        {
            prompt = Regex.Replace(prompt, @"\b" + Regex.Escape(embedding) + @"\b", "embedding:$0");
        }
        
        var client = await ComfyUiApiClient.ConnectAsync();

        var workflow = JsonSerializer.Deserialize<JsonObject>(File.ReadAllText(Path.Join(Application.StartupPath, "ComfyWorkflowTemplates\\txt2img.json")))!;

        // KSampler
        {
            var inputs = workflow["3"]!.AsObject()["inputs"]!.AsObject();
            inputs["seed"] = JsonValue.Create(seed);
            inputs["steps"] = JsonValue.Create(sdGenerationParameters.steps);
            inputs["cfg"] = JsonValue.Create(sdGenerationParameters.cfgScale);
            inputs["sampler_name"] = JsonValue.Create(ComfyUiNamesHelper.GetSamplerName(sdGenerationParameters.sampler));
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
            inputs["text"] = JsonValue.Create(prompt);
        }        
        
        // CLIPTextEncode (negative)
        {
            var inputs = workflow["7"]!.AsObject()["inputs"]!.AsObject();
            inputs["text"] = JsonValue.Create(sdGenerationParameters.negative);
        }

        if (sdGenerationParameters.clipSkip > 0)
        {
            // CLIPSetLastLayer
            {
                var inputs = workflow["13"]!.AsObject()["inputs"]!.AsObject();
                inputs["stop_at_clip_layer"] = JsonValue.Create(-sdGenerationParameters.clipSkip);
            }
        }
        
        // VAEDecode
        {
            var inputs = workflow["8"]!.AsObject()["inputs"]!.AsObject();
            inputs["vae"] = string.IsNullOrEmpty(sdGenerationParameters.vaeName)
                ? JsonSerializer.SerializeToNode(ComfyUiNodeOutputHelper.CheckpointLoaderSimple_vae("4"))
                : JsonSerializer.SerializeToNode(ComfyUiNodeOutputHelper.VAELoader_vae("11"));
        }

        if (!string.IsNullOrEmpty(sdGenerationParameters.vaeName))
        {
            // VAELoader
            {
                var inputs = workflow["11"]!.AsObject()["inputs"]!.AsObject();
                inputs["vae_name"] = SdVaeHelper.GetPathToVae(sdGenerationParameters.vaeName);
            }
        }

        createLoraNodes(loras, 1000, workflow);

        var images = await client.RunPromptAsync(workflow);

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

        SdGeneratorHelper.SaveMain
        (
            ComfyUiApiClient.Log, 
            sdGenerationParameters, 
            seed, 
            destDir, 
            images[0]
        );

        control.NotifyProgress(sdGenerationParameters.steps);

        return true;
    }

    public void Cancel()
    {
        Task.Run(async () => await ComfyUiApiClient.interrupt());
    }

    private static void createLoraNodes(Dictionary<string, decimal> loras, int startNodeId, JsonObject workflow)
    {
        var parentModel = ComfyUiNodeOutputHelper.CheckpointLoaderSimple_model("4");
        var parentClip = ComfyUiNodeOutputHelper.CLIPSetLastLayer_clip("13");

        foreach (var lora in loras)
        {
            var loraNode = createLoraNode(parentModel, parentClip, lora.Key, lora.Value);
            workflow.Add(startNodeId.ToString(), loraNode);

            parentModel = ComfyUiNodeOutputHelper.LoraLoader_model(startNodeId.ToString());
            parentClip = ComfyUiNodeOutputHelper.LoraLoader_clip(startNodeId.ToString());

            startNodeId++;
        }

        // CLIPTextEncode (positive)
        {
            var inputs = workflow["6"]!.AsObject()["inputs"]!.AsObject();
            inputs["clip"] = JsonSerializer.SerializeToNode(parentClip);
        }        
        
        // CLIPTextEncode (negative)
        {
            var inputs = workflow["7"]!.AsObject()["inputs"]!.AsObject();
            inputs["clip"] = JsonSerializer.SerializeToNode(parentClip);
        }
        
        // KSampler
        {
            var inputs = workflow["3"]!.AsObject()["inputs"]!.AsObject();
            inputs["model"] = JsonSerializer.SerializeToNode(parentModel);
        }
    }

    private static JsonObject createLoraNode(object[] parentModel, object[] parentClip, string lora_name, decimal weight)
    {
        return (JsonObject)JsonSerializer.SerializeToNode(ComfyUiNode.Create(new LoraLoaderInputs
        {
            model = parentModel,
            clip = parentClip,
            lora_name = lora_name,
            strength_model = weight,
            strength_clip = weight,
        }))!;
    }

    private static string[] getActiveEmbeddingNames()
    {
        return SdEmbeddingHelper.GetNames()
                                .Where(x => SdEmbeddingHelper.GetPathToModel(x) != null
                                         && SdEmbeddingHelper.IsEnabled(x))
                                .ToArray();
    }
}
