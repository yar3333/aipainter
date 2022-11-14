namespace AiPainter.Adapters.StableDiffusion;

class SdModCategory
{
    public string category { get; set; }
    public SdModModifier[] modifiers { get; set; }
}

class SdModModifier
{
    public string modifier { get; set; }
    public SdModPreview[] previews { get; set; }
}

class SdModPreview
{
    public string name { get; set; }
    public string path { get; set; }
}

/*[
{
    "category": "Drawing Style",
    "modifiers": [
    {
        "modifier": "Cel Shading",
        "previews": [
        {
            "name": "portrait",
            "path": "drawing_style/cel_shading/portrait-0.jpg"
        },
        {
            "name": "landscape",
            "path": "drawing_style/cel_shading/landscape-0.jpg"
        }
        ]
    },*/
