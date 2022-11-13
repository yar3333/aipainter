using System.Text.Json;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdModifiersForm : Form
    {
        private SdModCategory[] categories = null!;

        public SdModifiersForm()
        {
            InitializeComponent();
        }

        private void SdModifiersForm_Load(object sender, EventArgs e)
        {
            var baseDir = Path.Join(Application.StartupPath, "stable_diffusion_modifiers");
            var jsonFile = Path.Join(baseDir, "modifiers.json");
            if (File.Exists(jsonFile))
            {
                categories = JsonSerializer.Deserialize<SdModCategory[]>(File.ReadAllText(jsonFile))!;
                lbCategory.Items.Clear();
                // ReSharper disable once CoVariantArrayConversion
                lbCategory.Items.AddRange(categories.Select(x => x.category).Distinct().OrderBy(x => x).ToArray());
            }

            lvSelected.LargeImageList = new ImageList();
            lvSelected.LargeImageList.ImageSize = new Size(128, 128);  
            lvSelected.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
        }
        
        private void lbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var baseDir = Path.Join(Application.StartupPath, "stable_diffusion_modifiers");

            var category = (string)lbCategory.SelectedItem!;
            var modifiers = categories.Where(x => x.category == category).SelectMany(x => x.modifiers).ToArray();
            var images = modifiers.SelectMany(x => x.previews).Select(x => x.path).Distinct().ToArray();

            lvModifiers.LargeImageList = new ImageList();
            lvModifiers.LargeImageList.ImageSize = new Size(128, 128);  
            lvModifiers.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
            lvModifiers.LargeImageList.Images.AddRange(images.Select(x => Image.FromFile(Path.Join(baseDir, x))).ToArray());
            
            lvModifiers.Items.Clear();
            lvModifiers.Items.AddRange(modifiers.Select(x => new ListViewItem
            {
                Text = x.modifier,
                ImageIndex = Array.IndexOf(images, x.previews[0].path)
            }).ToArray());
        }

        private void lvModifiers_DoubleClick(object sender, EventArgs e)
        {
            if (lvModifiers.SelectedItems.Count == 0) return;

            var item = lvModifiers.SelectedItems[0];

            if (!isListViewItemExist(lvSelected.Items, item.Text))
            {
                lvSelected.LargeImageList.Images.Add(item.ImageList.Images[item.ImageIndex]);
                lvSelected.Items.Add(new ListViewItem
                {
                    Text = item.Text,
                    ImageIndex = lvSelected.LargeImageList.Images.Count - 1
                });
            }
        }
 
        private void lvSelected_DoubleClick(object sender, EventArgs e)
        {
            if (lvSelected.SelectedItems.Count == 0) return;

            var item = lvSelected.SelectedItems[0];
            lvSelected.Items.Remove(item);
        }
        
        private void btOk_Click(object sender, EventArgs e)
        {
            int b = 6;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {

        }

        private bool isListViewItemExist(ListView.ListViewItemCollection items, string text)
        {
            foreach (ListViewItem item in items)
            {
                if (item.Text == text) return true;
            }
            return false;
        }

    }
}
