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
            var node = (KSamplerInputs)WorkflowHelper.DeserializeNode(workflow, "3");
            
            node.seed = seed;
            node.steps = sdGenerationParameters.steps;
            node.cfg = sdGenerationParameters.cfgScale;
            node.sampler_name = ComfyUiNamesHelper.GetSamplerName(sdGenerationParameters.sampler);
            //node.scheduler = "normal"; // karras
            //node.denoise = 1.0;

            WorkflowHelper.SerializeNode(node, workflow);
        }
                
        // CheckpointLoaderSimple
        {
            var node = (CheckpointLoaderSimpleInputs)WorkflowHelper.DeserializeNode(workflow, "4");
            
            node.ckpt_name = Path.GetRelativePath(SdCheckpointsHelper.BaseDir, SdCheckpointsHelper.GetPathToMainCheckpoint(sdGenerationParameters.checkpointName)!);

            WorkflowHelper.SerializeNode(node, workflow);
        }

        // EmptyLatentImage
        {
            var node = (EmptyLatentImageInputs)WorkflowHelper.DeserializeNode(workflow, "5");

            node.width  = sdGenerationParameters.width;
            node.height = sdGenerationParameters.height;

            WorkflowHelper.SerializeNode(node, workflow);
        }

        // CLIPTextEncode (positive)
        {
            var node = (CLIPTextEncodeInputs)WorkflowHelper.DeserializeNode(workflow, "6");

            node.text = prompt;

            WorkflowHelper.SerializeNode(node, workflow);
        }        
        
        // CLIPTextEncode (negative)
        {
            var node = (CLIPTextEncodeInputs)WorkflowHelper.DeserializeNode(workflow, "7");

            node.text = sdGenerationParameters.negative;

            WorkflowHelper.SerializeNode(node, workflow);
        }

        if (sdGenerationParameters.clipSkip > 0)
        {
            // CLIPSetLastLayer
            {
                var node = (CLIPSetLastLayerInputs)WorkflowHelper.DeserializeNode(workflow, "13");

                node.stop_at_clip_layer = -sdGenerationParameters.clipSkip;

                WorkflowHelper.SerializeNode(node, workflow);
            }
        }
        
        // VAEDecode
        {
            var node = (VAEDecodeInputs)WorkflowHelper.DeserializeNode(workflow, "8");

            node.vae = string.IsNullOrEmpty(sdGenerationParameters.vaeName)
                                ? ComfyUiNodeOutputHelper.CheckpointLoaderSimple_vae("4")
                                : ComfyUiNodeOutputHelper.VAELoader_vae("11");

            WorkflowHelper.SerializeNode(node, workflow);
        }

        if (!string.IsNullOrEmpty(sdGenerationParameters.vaeName))
        {
            // VAELoader
            {
                var node = (VAELoaderInputs)WorkflowHelper.DeserializeNode(workflow, "11");

                node.vae_name = sdGenerationParameters.vaeName;

                WorkflowHelper.SerializeNode(node, workflow);
            }
        }

        createLoraNodes(loras, 1000, workflow);

        var images = await client.RunPromptAsync(workflow, "10", step => control.NotifyProgress(step));
        
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

    private static void createLoraNodes(Dictionary<string, decimal> loras, int startNodeId, JsonObject workflow)
    {
        var parentModel = ComfyUiNodeOutputHelper.CheckpointLoaderSimple_model("4");
        var parentClip = ComfyUiNodeOutputHelper.CLIPSetLastLayer_clip("13");

        foreach (var lora in loras)
        {
            var loraNode = new LoraLoaderInputs
            {
                Id = startNodeId.ToString(),
                model = parentModel,
                clip = parentClip,
                lora_name = lora.Key,
                strength_model = lora.Value,
                strength_clip = lora.Value,
            };
            WorkflowHelper.SerializeNode(loraNode, workflow);

            parentModel = ComfyUiNodeOutputHelper.LoraLoader_model(loraNode.Id);
            parentClip = ComfyUiNodeOutputHelper.LoraLoader_clip(loraNode.Id);

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

    private static string[] getActiveEmbeddingNames()
    {
        return SdEmbeddingHelper.GetNames()
                                .Where(x => SdEmbeddingHelper.GetPathToModel(x) != null
                                         && SdEmbeddingHelper.IsEnabled(x))
                                .ToArray();
    }
}
