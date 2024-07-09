using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdGeneratorStuff.ExceptionsAndHelpers;

static class SdGeneratorHelper
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

        if (Program.Config.UseEmbeddedStableDiffusion && StableDiffusionProcess.Running)
        {
            if (StableDiffusionProcess.ActiveCheckpointFilePath != checkpointFilePath || StableDiffusionProcess.ActiveVaeFilePath != vaeFilePath)
            {
                progress("Stopping...");
                StableDiffusionProcess.Stop();
                while (StableDiffusionProcess.IsReady())
                {
                    if (await DelayTools.WaitForExitAsync(500) || wantToCancel()) return false;
                }
            }
        }

        if (Program.Config.UseEmbeddedStableDiffusion && !StableDiffusionProcess.Running)
        {
            progress("Starting...");
            StableDiffusionProcess.Start(checkpointFilePath, vaeFilePath);
            while (!StableDiffusionProcess.IsReady())
            {
                if (await DelayTools.WaitForExitAsync(500) || wantToCancel()) return false;
            }
        }

        if (!StableDiffusionProcess.IsReady())
        {
            progress("Waiting ready...");
            while (!StableDiffusionProcess.IsReady())
            {
                if (await DelayTools.WaitForExitAsync(500) || wantToCancel()) return false;
            }
        }

        return true;
    }
}
