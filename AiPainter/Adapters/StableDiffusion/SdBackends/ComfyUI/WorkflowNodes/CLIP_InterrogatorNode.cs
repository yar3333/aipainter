using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class CLIP_InterrogatorNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.CLIP_Interrogator;

    public object[]? image { get; set; }

    public string mode { get; set; } = ""; // fast, full
    public bool keep_model_alive { get; set; }
    public bool prepend_blip_caption { get; set; }
    public string save_prompt_to_txt_file { get; set; } = "";
    
    [JsonIgnore] public object[] Output_full_prompt => new object[] { Id, 0 };
    [JsonIgnore] public object[] Output_blip_caption => new object[] { Id, 1 };
}