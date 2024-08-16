namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class VAEDecodeInputs : IComfyNodeInputs
{
    public object[] samples { get; set; } // [ "3", 0 ],
    public object[] vae { get; set; } // [ "4", 2 ]
}
