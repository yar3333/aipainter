namespace AiPainter.Adapters.StableDiffusion.SdBackends;

static class SdBackend
{
    private static ISdBackend? instance;
    public static ISdBackend Instance
    {
        get
        {
            if (instance == null)
            {
                switch (Program.Config.StableDiffusionBackend)
                {
                    case "ComfyUI":
                        instance = new WebUI.WebUiBackend();
                        break;

                    default:
                        instance = new WebUI.WebUiBackend();
                        break;
                }
            }
            return instance!;
        }
    }
}
