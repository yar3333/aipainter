using AiPainter.Adapters.StableDiffusion;

namespace AiPainter.Controls
{
    public partial class GenerationList : UserControl
    {
        private readonly List<GenerationListItem> items = new();

        public GenerationList()
        {
            InitializeComponent();
        }

        public void AddGeneration(StableDiffusionPanel sdPanel, SmartPictureBox pictureBox)
        {
            var item = new GenerationListItem();
            item.Init(sdPanel, pictureBox);
            item.Dock = DockStyle.Top;
            item.Parent = this;
            lock (items)
            {
                items.Add(item);
            }
        }

        private void stateManager_Tick(object sender, EventArgs e)
        {
            lock (items)
            {
                if (items.Any(x => x.State == GenerationState.IN_PROCESS)) return;
                var item = items.Find(x => x.State == GenerationState.PART_FINISHED || x.State == GenerationState.WAITING);
                item?.Run();
            }
        }
    }
}
