﻿namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class SaveImageInputs : IComfyUiNodeInputs
{
    public string filename_prefix { get; set; } // "ComfyUI",
    public object[] images { get; set; } // [ "8", 0 ]
}
