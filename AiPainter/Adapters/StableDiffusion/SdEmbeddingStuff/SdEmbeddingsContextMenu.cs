using System.Text.RegularExpressions;
using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;

class SdEmbeddingsContextMenu : ContextMenuStrip
{
    public SdEmbeddingsContextMenu(StableDiffusionPanel panStableDiffusion, GenerationList panGenerationList, bool isNegative)
    {
        var models = SdEmbeddingHelper.GetNames()
                                      .Where(x => SdEmbeddingHelper.GetPathToModel(x) != null
                                               && SdEmbeddingHelper.IsEnabled(x)
                                               && SdEmbeddingHelper.GetConfig(x).isNegative == isNegative)
                                      .ToArray();

        Items.Add("Manage Embeddings...", null, (_, _) =>
        {
            var form = new SdModelsForm(panGenerationList, new SdEmbeddingFormAdapter());
            form.ShowDialog(this);
        });

        Items.Add(new ToolStripSeparator());

        foreach (var name in models)
        {
            Items.Add(new ToolStripMenuItem(SdEmbeddingHelper.GetHumanName(name), null, (_, _) =>
            {
                if (!isNegative) panStableDiffusion.AddTextToPrompt  ("(" + name + ":1.0)");
                else             panStableDiffusion.AddTextToNegative("(" + name + ":1.0)");
            })
            {
                Checked = Regex.IsMatch(!isNegative ? panStableDiffusion.tbPrompt.Text : panStableDiffusion.tbNegative.Text, @"\b" + Regex.Escape(name) + @"\b")
            });
        }

        if (models.Length == 0)
        {
            Items.Add(new ToolStripLabel("No enabled Embeddings found"));
        }

        if (panStableDiffusion.InProcess) foreach (ToolStripItem item in Items) item.Enabled = false;
    }
}
