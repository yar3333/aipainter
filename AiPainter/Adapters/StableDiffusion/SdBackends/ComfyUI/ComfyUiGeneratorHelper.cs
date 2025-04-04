﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI;

static class ComfyUiGeneratorHelper
{
    public static List<BaseNode> LoadWorkflow(string templateJsonFileName)
    {
        return WorkflowHelper.DeserializeWorkflow(File.ReadAllText(Path.Join(Application.StartupPath, "ComfyWorkflowTemplates\\" + templateJsonFileName)));
    }

    public static List<BaseNode> CreateWorkflow(string templateJsonFileName, SdGenerationParameters sdGenerationParameters)
    {
        Debug.Assert(sdGenerationParameters.clipSkip > 0);

        // subseed_strength = sdGenerationParameters.seedVariationStrength

        var vaeName = getVaeNameForUse(sdGenerationParameters);
        var loras = SdPromptNormalizer.GetUsedLoras(sdGenerationParameters.prompt, out var prompt);
        prompt = fixEmbeddingsInPrompt(prompt);

        var workflow = LoadWorkflow(templateJsonFileName);

        var isFlux = SdCheckpointsHelper.GetConfig(sdGenerationParameters.checkpointName).baseModel?.StartsWith("Flux.") ?? false;

        // KSampler
        var nodeKSampler = (KSamplerNode)workflow.Single(x => x.Id == "nodeKSampler");
        nodeKSampler.seed = sdGenerationParameters.seed;
        nodeKSampler.steps = sdGenerationParameters.steps;
        nodeKSampler.cfg = sdGenerationParameters.cfgScale;
        nodeKSampler.sampler_name = !isFlux
                                ? ComfyUiNamesHelper.GetSamplerName(sdGenerationParameters.sampler)
                                : "euler";
        //nodeKSampler.scheduler = "normal"; // karras
        //nodeKSampler.denoise = 1.0;
                
        // CheckpointLoaderSimple
        var nodeCheckpointLoaderSimple = (CheckpointLoaderSimpleNode)workflow.Single(x => x.Id == "nodeCheckpointLoaderSimple");
        nodeCheckpointLoaderSimple.ckpt_name = Path.GetRelativePath(SdCheckpointsHelper.BaseDir, SdCheckpointsHelper.GetPathToMainCheckpoint(sdGenerationParameters.checkpointName)!);

        // CLIPTextEncode (positive)
        var nodeCLIPTextEncode_positive = (CLIPTextEncodeNode)workflow.Single(x => x.Id == "nodeCLIPTextEncode_positive");
        nodeCLIPTextEncode_positive.text = prompt;
       
        // CLIPTextEncode (negative)
        var nodeCLIPTextEncode_negative = (CLIPTextEncodeNode)workflow.Single(x => x.Id == "nodeCLIPTextEncode_negative");
        nodeCLIPTextEncode_negative.text = sdGenerationParameters.negative;

        // CLIPSetLastLayer
        var nodeCLIPSetLastLayer = (CLIPSetLastLayerNode)workflow.Single(x => x.Id == "nodeCLIPSetLastLayer");
        nodeCLIPSetLastLayer.stop_at_clip_layer = -sdGenerationParameters.clipSkip;
        
        // VAELoader
        var nodeVAELoader = string.IsNullOrEmpty(vaeName)
                                ? null
                                : new VAELoaderNode
                                {
                                    Id = "nodeVAELoader",
                                    vae_name = vaeName,
                                };
        if (nodeVAELoader != null) workflow.Add(nodeVAELoader);

        // VAEDecode
        var nodeVAEDecode = (VAEDecodeNode)workflow.Single(x => x.Id == "nodeVAEDecode");
        nodeVAEDecode.vae = nodeVAELoader?.Output_vae ?? nodeCheckpointLoaderSimple.Output_vae;

        var nodeLoraLoader_last = createLoraNodes
        (
            nodeCheckpointLoaderSimple.Output_model, 
            !isFlux ? nodeCLIPSetLastLayer.Output_clip : nodeCheckpointLoaderSimple.Output_clip, 
            loras, 
            workflow
        );
        if (nodeLoraLoader_last != null)
        {
            nodeCLIPTextEncode_positive.clip = nodeLoraLoader_last.Output_clip;
            nodeCLIPTextEncode_negative.clip = nodeLoraLoader_last.Output_clip;
            nodeKSampler.model = nodeLoraLoader_last.Output_model;
        }
        else
        {
            if (isFlux)
            {
                // exclude `nodeCLIPSetLastLayer` from workflow
                nodeCLIPTextEncode_positive.clip = nodeCheckpointLoaderSimple.Output_clip;
                nodeCLIPTextEncode_negative.clip = nodeCheckpointLoaderSimple.Output_clip;
            }
        }

        if (isFlux)
        {
            var nodeFluxGuidance = new FluxGuidanceNode
            {
                Id = "nodeFluxGuidance",
                conditioning = nodeCLIPTextEncode_positive.Output_conditioning,
                guidance = sdGenerationParameters.cfgScale,
            };
            workflow.Add(nodeFluxGuidance);
            nodeKSampler.positive = nodeFluxGuidance.Output_conditioning;
            nodeKSampler.cfg = 1.0m;
        }

        return workflow;
    }

    private static LoraLoaderNode? createLoraNodes(object[] parentModel, object[] parentClip, Dictionary<string, decimal> loras, List<BaseNode> workflow)
    {
        LoraLoaderNode? loraNode = null;
        var n = 0;
        foreach (var lora in loras)
        {
            loraNode = new LoraLoaderNode
            {
                Id = "nodeLoraLoader_" + n,
                model = parentModel,
                clip = parentClip,
                lora_name = Path.GetFileName(SdLoraHelper.GetPathToModel(lora.Key))!,
                strength_model = lora.Value,
                strength_clip = lora.Value,
            };
            workflow.Add(loraNode);
            
            parentModel = loraNode.Output_model;
            parentClip = loraNode.Output_clip;

            n++;
        }
        return loraNode;
    }

    private static string fixEmbeddingsInPrompt(string prompt)
    {
        foreach (var embedding in getActiveEmbeddingNames())
        {
            prompt = Regex.Replace(prompt, @"\b" + Regex.Escape(embedding) + @"\b", "embedding:$0");
        }
        return prompt;
    }

    private static string[] getActiveEmbeddingNames()
    {
        return SdEmbeddingHelper.GetNames()
                                .Where(x => SdEmbeddingHelper.GetPathToModel(x) != null
                                         && SdEmbeddingHelper.IsEnabled(x))
                                .ToArray();
    }

    private static string getVaeNameForUse(SdGenerationParameters sdGenerationParameters)
    {
        var r = SdVaeHelper.GetPathToVae(sdGenerationParameters.vaeName);
        if (r != null) return Path.GetRelativePath(SdVaeHelper.BaseDir, r);

        r = SdCheckpointsHelper.GetPathToVae(sdGenerationParameters.checkpointName);
        if (r != null) return Path.GetRelativePath(SdCheckpointsHelper.BaseDir, r);

        return "";
    }
}