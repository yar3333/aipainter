using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion.SdLoraStuff;

class SdLorasContextMenu : ContextMenuStrip
{
    public SdLorasContextMenu(StableDiffusionPanel panStableDiffusion, GenerationList panGenerationList)
    {
        Items.Add("Manage LoRAs...", null, (_, _) =>
        {
            var form = new SdModelsForm(panGenerationList, new SdLorasFormAdapter());
            form.ShowDialog(this);
        });

        Items.Add(new ToolStripSeparator());

        var usedModels = panStableDiffusion.GetUsedLoras();
        var models = SdLoraHelper.GetNames().Where(x => SdLoraHelper.GetPathToModel(x) != null && SdLoraHelper.IsEnabled(x)).ToArray();
        foreach (var name in models)
        {
            Items.Add(new ToolStripMenuItem(SdLoraHelper.GetHumanName(name), null, (_, _) =>
            {
                panStableDiffusion.AddTextToPrompt(SdLoraHelper.GetPrompt(name));
            })
            {
                Checked = usedModels.Contains(name),
                ForeColor = isCompatibleToCheckpoint(panStableDiffusion.selectedCheckpointName, name) ? Color.Black : Color.Gray,
            });
        }

        if (models.Length == 0)
        {
            Items.Add(new ToolStripLabel("No enabled LoRA found"));
        }

        if (panStableDiffusion.InProcess) foreach (ToolStripItem item in Items) item.Enabled = false;
    }

    private static bool isCompatibleToCheckpoint(string checkpoint, string lora)
    {
        if (string.IsNullOrEmpty(checkpoint)) return true;
        var checkpointBaseModel = SdCheckpointsHelper.GetConfig(checkpoint).baseModel;
        if (string.IsNullOrEmpty(checkpointBaseModel)) return true;
        var loraBaseModel = SdLoraHelper.GetConfig(lora).baseModel;
        if (string.IsNullOrEmpty(loraBaseModel)) return true;
        return loraBaseModel == checkpointBaseModel;
    }
}
