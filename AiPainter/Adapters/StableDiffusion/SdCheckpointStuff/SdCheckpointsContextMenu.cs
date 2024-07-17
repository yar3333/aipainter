using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;

class SdCheckpointsContextMenu : ContextMenuStrip
{
    public SdCheckpointsContextMenu(StableDiffusionPanel panStableDiffusion)
    {
        Items.Add("Manage checkpoints...", null, (_, _) =>
        {
            panStableDiffusion.ShowManageCheckpointDialog();
        });

        Items.Add(new ToolStripSeparator());

        Items.Add("Show in Explorer", null, (_, _) =>
        {
            if (panStableDiffusion.selectedCheckpointName != "")
            {
                ProcessHelper.ShowFolderInExplorer(SdCheckpointsHelper.GetDirPath(panStableDiffusion.selectedCheckpointName));
            }
        });

        Items.Add("Visit home page", null, (_, _) =>
        {
            if (panStableDiffusion.selectedCheckpointName != "")
            {
                var config = SdCheckpointsHelper.GetConfig(panStableDiffusion.selectedCheckpointName);
                if (!string.IsNullOrEmpty(config.homeUrl)) ProcessHelper.OpenUrlInBrowser(config.homeUrl);
            }
        });
    }
}
