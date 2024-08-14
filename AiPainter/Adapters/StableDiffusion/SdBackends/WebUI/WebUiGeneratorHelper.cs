using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.WebUI;

static class WebUiGeneratorHelper
{
    public static async Task<bool> PrepareCheckpointAsync(bool isMainCheckpoint, string checkpointName, string vaeName, Func<bool> wantToCancel, Action<string> progress)
    {
        var checkpointFilePath = isMainCheckpoint
                                     ? SdCheckpointsHelper.GetPathToMainCheckpoint(checkpointName)
                                     : SdCheckpointsHelper.GetPathToInpaintCheckpoint(checkpointName)
                                     ?? SdCheckpointsHelper.GetPathToMainCheckpoint(checkpointName);

        if (checkpointFilePath == null) throw new SdGeneratorFatalErrorException("NOT FOUND");

        var vaeFilePath = SdVaeHelper.GetPathToVae(vaeName)
                       ?? SdCheckpointsHelper.GetPathToVae(checkpointName)
                       ?? "";

        if (Program.Config.UseEmbeddedStableDiffusion && WebUiProcess.Running)
        {
            if (WebUiProcess.ActiveCheckpointFilePath != checkpointFilePath || WebUiProcess.ActiveVaeFilePath != vaeFilePath)
            {
                progress("Stopping...");
                WebUiProcess.Stop();
                while (WebUiProcess.IsReady())
                {
                    if (await DelayTools.WaitForExitAsync(500) || wantToCancel()) return false;
                }
            }
        }

        if (Program.Config.UseEmbeddedStableDiffusion && !WebUiProcess.Running)
        {
            progress("Starting...");
            WebUiProcess.Start(checkpointFilePath, vaeFilePath);
            while (!WebUiProcess.IsReady())
            {
                if (await DelayTools.WaitForExitAsync(500) || wantToCancel()) return false;
            }
        }

        if (!WebUiProcess.IsReady())
        {
            progress("Waiting ready...");
            while (!WebUiProcess.IsReady())
            {
                if (await DelayTools.WaitForExitAsync(500) || wantToCancel()) return false;
            }
        }

        return true;
    }
}
