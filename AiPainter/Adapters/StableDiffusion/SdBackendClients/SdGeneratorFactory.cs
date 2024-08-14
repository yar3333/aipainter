namespace AiPainter.Adapters.StableDiffusion.SdBackendClients;

static class SdGeneratorFactory
{
    public static ISdGenerator CreateGeneratorMain(SdGenerationParameters sdGenerationParameters, SdGenerationListItem control, string destDir)
    {
        return new WebUI.SdGeneratorMain(sdGenerationParameters, control, destDir);
    }

    public static ISdGenerator CreateGeneratorInpaint(SdGenerationParameters sdGenerationParameters, SdGenerationListItem control, Bitmap originalImage, Rectangle activeBox, Bitmap? croppedMask, string originalFilePath)
    {
        return new WebUI.SdGeneratorInpaint
        (
            sdGenerationParameters,
            control,
            originalImage,
            activeBox,
            croppedMask,
            originalFilePath
        );
    }
}