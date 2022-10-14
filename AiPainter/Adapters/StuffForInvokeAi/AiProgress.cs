namespace AiPainter.Adapters.StuffForInvokeAi;

#pragma warning disable CS8603

/// <summary>
/// {"event": "step", "step": 37, "url": null}
/// {"event": "result", "url": "outputs/img-samples\\000077.4116489622.png", "seed": 4116489622, "config": {...}
/// </summary>
class AiProgress
{
    public string @event { get; set; } = ""; // step, result
    public int? step { get; set; } // 1...
    public string? url { get; set; }
    public long? seed { get; set; }
    public AiImageInfo? config { get; set; }
}