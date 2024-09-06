namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class SaveImageWebsocketNode : BaseNode
{
    public object[]? images { get; set; } // [ "8", 0 ],
}
