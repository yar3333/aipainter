namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class SaveImageWebsocketNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.SaveImageWebsocket;
    
    public object[]? images { get; set; } // [ "8", 0 ],
}
