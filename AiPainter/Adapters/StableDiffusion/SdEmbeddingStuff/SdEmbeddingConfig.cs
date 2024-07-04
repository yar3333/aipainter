﻿namespace AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;

class SdEmbeddingConfig
{
    public string homeUrl { get; set; } = "";
    public string downloadUrl { get; set; } = "";
    public string description { get; set; } = "";
    public bool isNegative { get; set; } = false;

    public bool isNeedAuthToDownload = false;
}
