using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdModelsForm : Form
    {
        private readonly GenerationList generationList;
        private readonly ISdModelsFormAdapter modelsAdapter;

        private bool ignoreCheckedChange = true;
        private ListViewItem[] allItems = { };

        public SdModelsForm(GenerationList generationList, ISdModelsFormAdapter modelsAdapter)
        {
            InitializeComponent();

            lvModels.Columns.AddRange(modelsAdapter.GetColumnHeaders());

            this.generationList = generationList;
            this.modelsAdapter = modelsAdapter;
        }

        private void SdModelsForm_Load(object sender, EventArgs e)
        {
            tbCivitaiApiKey.Text = Program.Config.CivitaiApiKey;

            updateList();

            modelsAdapter.AddUpdateListEventHandler(updateList);
            modelsAdapter.AddDownloadProgressEventHandler(updateItemDownloadProgress);
        }

        private void SdModelsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            modelsAdapter.RemoveUpdateListEventHandler(updateList);
            modelsAdapter.RemoveDownloadProgressEventHandler(updateItemDownloadProgress);
        }

        private void updateList()
        {
            Invoke(() =>
            {
                allItems = modelsAdapter.GetItems();
                fillModelListFiltered();
            });
        }

        private void fillModelListFiltered()
        {
            ignoreCheckedChange = true;
            lvModels.Items.Clear();
            lvModels.Items.AddRange(allItems.Where(isItemVisible).ToArray());
            ignoreCheckedChange = false;
        }

        private void updateItemDownloadProgress(string name)
        {
            Invoke(() =>
            {
                var item = allItems.FirstOrDefault(x => x.Name == name);
                if (item != null) modelsAdapter.UpdateItemStatus(generationList, item, false);
            });
        }

        private void lvModels_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ignoreCheckedChange) return;

            modelsAdapter.SetEnabled(e.Item.Name, e.Item.Checked);

            if (e.Item.Checked)
            {
                modelsAdapter.StartDownloading(e.Item.Name, generationList);
            }
        }

        private void lvModels_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || modelsAdapter.SubItemIndexWithLink == null) return;

            var hit = lvModels.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem == hit.Item.SubItems[modelsAdapter.SubItemIndexWithLink.Value])
            {
                ProcessHelper.OpenUrlInBrowser(hit.SubItem.Text);
            }
        }

        private void tbCivitaiApiKey_TextChanged(object sender, EventArgs e)
        {
            if (Program.Config.CivitaiApiKey != tbCivitaiApiKey.Text)
            {
                Program.Config.CivitaiApiKey = tbCivitaiApiKey.Text;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrlInBrowser("https://civitai.com/user/account");
        }

        private void btImportFromCivitai_Click(object sender, EventArgs e)
        {
            var form = new AddImportModelForm();
            form.tabs.SelectedTab = modelsAdapter.GetDefaultTab(form) ?? form.tabs.SelectedTab;
            form.ShowDialog(this);
            updateList();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            fillModelListFiltered();
        }

        private bool isItemVisible(ListViewItem item)
        {
            if (tbSearch.Text.Trim() == "") return true;

            var words = tbSearch.Text.Split(',', ';', ' ').Select(x => x.Trim().ToLowerInvariant()).ToArray();
            var text = string.Join(' ', modelsAdapter.SubItemIndexesForSearch.Select(x => item.SubItems[x].Text)).ToLowerInvariant();

            return words.All(word => text.Contains(word));
        }
    }
}
