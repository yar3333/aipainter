namespace AiPainter.Adapters.StableDiffusion.SdBackendClients;

static class SdProcess
{
    public static void Start(string? checkpointFilePath, string? vaeFilePath)
    {
        WebUI.WebUiProcess.Start(checkpointFilePath, vaeFilePath);
    }

    public static void Stop()
    {
        WebUI.WebUiProcess.Stop();
    }
}