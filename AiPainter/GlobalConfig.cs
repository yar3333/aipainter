using System.Text.Json;

namespace AiPainter;

class GlobalConfig
{
    public class WindowPosition
    {
        public FormWindowState state { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public static WindowPosition Create(Form form)
        {
            return new WindowPosition
            {
                state = form.WindowState,
                x = form.Left,
                y = form.Top,
                width = form.Width,
                height = form.Height,
            };
        }

        public void ApplyToForm(Form form)
        {
            form.Left = x;
            form.Top = y;
            form.Width = width;
            form.Height = height;
            form.WindowState = state;
        }
    }

    public string ImagesFolder { get; set; } = "images";

    public string StableDiffusionBackend { get; set; } = "ComfyUI"; // "WebUI" or "ComfyUI"
    public bool UseEmbeddedStableDiffusion { get; set; } = true;
    public string StableDiffusionUrl { get; set; } = "http://127.0.0.1:7860/";
    public string StableDiffusionCheckpoint { get; set; } = "";
    public string StableDiffusionVae { get; set; } = "";
    
    public bool UseEmbeddedLamaCleaner { get; set; } = true;
    public string LamaCleanerUrl { get; set; } = "http://127.0.0.1:9595/";

    public List<string> NegativePrompts { get; set; } = new()
    {
        "[deformed | disfigured], poorly drawn, [bad : wrong] anatomy, [extra | missing | floating | disconnected] limb, (mutated hands and fingers), blurry"
    };

    public List<string> ImageSizes { get; set; } = new()
    {
        "512x512",
        "512x768",
        "512x1024",
        "768x512",
        "768x768",
        "768x1024",
        "1024x512",
        "1024x768",
        "1024x1024",
    };

    public string CivitaiApiKey { get; set; } = "";

    public WindowPosition? MainWindowPosition { get; set; } = null;

    public static GlobalConfig Load()
    {
        var pathToConfig = Path.Join(Application.StartupPath, "Config.json");
        
        return File.Exists(pathToConfig) 
                   ? (JsonSerializer.Deserialize<GlobalConfig>(File.ReadAllText(pathToConfig)) ?? new GlobalConfig())
                   : new GlobalConfig();
    }
    
    public void Save()
    {
        var pathToConfig = Path.Join(Application.StartupPath, "Config.json");

        var oldText = File.Exists(pathToConfig) ? File.ReadAllText(pathToConfig) : "";
        var newText = JsonSerializer.Serialize(this, Program.DefaultJsonSerializerOptions);

        if (newText != oldText)
        {
            File.WriteAllText(pathToConfig, newText);
        }
    }
}
