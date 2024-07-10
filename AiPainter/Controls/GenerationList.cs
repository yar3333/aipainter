namespace AiPainter.Controls
{
    public partial class GenerationList : UserControl
    {
        private readonly List<IGenerationListItem> items = new();

        public GenerationList()
        {
            InitializeComponent();            
        }

        public void AddGeneration(IGenerationListItem item)
        {
            lock (items)
            {
                item.Width = ClientSize.Width;
                item.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                
                items.Add(item);
                item.Parent = this;
                
                arrangeItems();

                ScrollControlIntoView((Control)item);
            }
        }
        
        public IGenerationListItem? FindItem(string name)
        {
            lock (items)
            {
                return items.FirstOrDefault(x => ((Control)x).Name == name);
            }
        }

        private void arrangeItems()
        {
            lock (items)
            {
                for (var i = 0; i < items.Count; i++)
                {
                    items[i].Top = i * items[i].ClientSize.Height - VerticalScroll.Value;
                }
            }
        }

        private void stateManager_Tick(object sender, EventArgs e)
        {
            lock (items)
            {
                var wasRemoved = false;
                foreach (var itemToRemove in items.Where(x => !x.InProcess && x.WantToBeRemoved).ToArray())
                {
                    wasRemoved = true;
                    Controls.Remove((Control)itemToRemove);
                    items.Remove(itemToRemove);
                    itemToRemove.Dispose();
                }
                if (wasRemoved) arrangeItems();

                foreach (var group in items.GroupBy(x => x.ParallelGroup))
                {
                    if (group.All(x => !x.InProcess))
                    {
                        var item = items.Find(x => x.HasWorkToRun);
                        item?.Run();
                    }
                }
            }
        }
    }
}
