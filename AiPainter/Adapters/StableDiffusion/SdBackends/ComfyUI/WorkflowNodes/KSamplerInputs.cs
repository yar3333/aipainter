﻿using System.Text.Json.Nodes;

namespace AiPainter.Adapters.StableDiffusion.SdBackends.ComfyUI.WorkflowNodes;

class KSamplerInputs : IComfyNodeInputs
{
    public long seed { get; set; } // 278559787642457,
    public int steps { get; set; } // 20,
    public decimal cfg { get; set; } // 8,
    public string sampler_name { get; set; } // "euler",
    public string scheduler { get; set; } // "normal",
    public decimal denoise { get; set; } // 1,
    public JsonValue[] model { get; set; } // [ "4", 0 ],
    public JsonValue[] positive { get; set; } // [ "6", 0 ],
    public JsonValue[] negative { get; set; } // [ "7", 0 ],
    public JsonValue[] latent_image { get; set; } // [ "5", 0 ]
}
