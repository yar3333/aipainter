using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;

// ReSharper disable VirtualMemberCallInConstructor

namespace AiPainter.Adapters.StableDiffusion;

class SuggestedPromptContextMenu : ContextMenuStrip
{
    private readonly StableDiffusionPanel panStableDiffusion;

    public SuggestedPromptContextMenu(StableDiffusionPanel panStableDiffusion)
    {
        this.panStableDiffusion = panStableDiffusion;

        MinimumSize = new Size(180, MinimumSize.Height);

        {
            var checkpointName = panStableDiffusion.selectedCheckpointName;
            if (checkpointName != "")
            {
                var config = SdCheckpointsHelper.GetConfig(checkpointName);
                addPhrases("*** " + checkpointName + " ***", config.promptRequired, config.promptSuggested);
            }
        }

        var loras = panStableDiffusion.GetUsedLoras();
        foreach (var name in loras)
        {
            var config = SdLoraHelper.GetConfig(name);
            addPhrases("*** " + name + " ***", config.promptRequired, config.promptSuggested);
        }

        if (Items.Count == 0)
        {
            Items.Add(new ToolStripLabel("No suggested phrases"));
        }
    }

    private void addPhrases(string? label, string? promptRequired, string? promptSuggested)
    {
        var reqPhrases = getSuggestedPhrases(promptRequired);
        var sugPhrases = getSuggestedPhrases(promptSuggested);
        if (reqPhrases.Length > 0 || sugPhrases.Length > 0)
        {
            if (Items.Count > 0) Items.Add(new ToolStripSeparator());
            if (label != null) Items.Add(new ToolStripLabel(label));
            foreach (var s in reqPhrases)
            {
                Items.Add(new ToolStripMenuItem(s, null, (_, _) => panStableDiffusion.AddTextToPrompt(s))
                {
                    ForeColor = Color.Blue,
                    Checked = Regex.IsMatch(panStableDiffusion.tbPrompt.Text, @"\b" + Regex.Escape(s) + @"\b"),
                });
            }
            foreach (var s in sugPhrases)
            {
                Items.Add(new ToolStripMenuItem(s, null, (_, _) => panStableDiffusion.AddTextToPrompt(s))
                {
                    Checked = Regex.IsMatch(panStableDiffusion.tbPrompt.Text, @"\b" + Regex.Escape(s) + @"\b"),
                });
            }
        }
    }

    private static string[] getSuggestedPhrases(string? text)
    {
        if (text == null) return new string[] { };
        var parts = text.Split(',').Select(x => x.Trim(',', ' ')).ToList();
        parts.Insert(0, text);
        return parts.Where(x => x != "").Distinct().ToArray();
    }
}
