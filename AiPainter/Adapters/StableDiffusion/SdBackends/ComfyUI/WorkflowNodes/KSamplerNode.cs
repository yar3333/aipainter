﻿using System.Text.Json.Serialization;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

[Serializable]
class KSamplerNode : BaseNode
{
    public override ComfyUiNodeType NodeType => ComfyUiNodeType.KSampler;
    
    public object[]? model { get; set; } // [ "4", 0 ],
    public object[]? positive { get; set; } // [ "6", 0 ],
    public object[]? negative { get; set; } // [ "7", 0 ],
    public object[]? latent_image { get; set; } // [ "5", 0 ]

    public long seed { get; set; } // 278559787642457,
    public int steps { get; set; } // 20,
    public decimal cfg { get; set; } // 8,
    public string sampler_name { get; set; } = ""; // "euler",
    public string scheduler { get; set; } = ""; // "normal",
    public decimal denoise { get; set; } // 1,

    [JsonIgnore] public object[] Output_latent => new object[] { Id, 0 };
}
