using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdStylesForm : Form
    {
        private static string BasePath => Path.Join(Application.StartupPath, "stable_diffusion_styles");
        
        private SdModCategory[] categories = null!;

        private string previewName = "portrait"; // "landscape"

        private readonly ImageList imageList = new() { ImageSize = new Size(128, 128), ColorDepth = ColorDepth.Depth32Bit };

        public string[] Modifiers
        {
            get => lvSelected.Items.OfType<ListViewItem>().Select(x => x.Text).ToArray();
            set 
            {
                lvSelected.Items.Clear();
                foreach (var modifier in value)
                {
                    ensureImageLoaded(modifier);
                    lvSelected.Items.Add(modifier, modifier, modifier);
                }
            }
        }

        public SdStylesForm()
        {
            InitializeComponent();            
            
            var jsonFile = Path.Join(BasePath, "styles.json");
            if (File.Exists(jsonFile))
            {
                categories = JsonSerializer.Deserialize<SdModCategory[]>(File.ReadAllText(jsonFile))!;
                lbCategory.Items.Clear();
                // ReSharper disable once CoVariantArrayConversion
                lbCategory.Items.AddRange(categories.Select(x => x.category).Distinct().OrderBy(x => x).ToArray());
            }

            lvModifiers.LargeImageList = imageList;
            lvSelected.LargeImageList = imageList;
        }
        
        private void lbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvModifiers.Items.Clear();
            foreach (var modifier in categories.Single(x => x.category == (string)lbCategory.SelectedItem).modifiers.Select(x => x.modifier))
            {
                ensureImageLoaded(modifier);
                lvModifiers.Items.Add(modifier, modifier, modifier);
            }
        }

        private void lvModifiers_DoubleClick(object sender, EventArgs e)
        {
            if (lvModifiers.SelectedItems.Count == 0) return;

            var item = lvModifiers.SelectedItems[0];

            if (!isListViewItemExist(lvSelected.Items, item.Text))
            {
                lvSelected.Items.Add(item.Text, item.Text, item.Text);
            }
        }
 
        private void lvSelected_DoubleClick(object sender, EventArgs e)
        {
            if (lvSelected.SelectedItems.Count == 0) return;

            lvSelected.Items.Remove(lvSelected.SelectedItems[0]);
        }
        
        private bool isListViewItemExist(ListView.ListViewItemCollection items, string text)
        {
            foreach (ListViewItem item in items)
            {
                if (item.Text == text) return true;
            }
            return false;
        }

        private void ensureImageLoaded(string modifier)
        {
            if (imageList.Images.ContainsKey(modifier)) return;

            var m = categories.SelectMany(x => x.modifiers).FirstOrDefault(x => x.modifier == modifier);
            if (m == null) return;

            var preview = m.previews.SingleOrDefault(x => x.name == previewName)
                       ?? m.previews.First();
            var path = Path.Join(BasePath, preview.path);
            lvModifiers.LargeImageList.Images.Add(modifier, Image.FromFile(path));
        }
    }
}
