using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdLoraStuff
{
    public partial class SdLorasForm : Form
    {
        private static bool isNeedUpdateStatusLight;
        private static bool isNeedUpdateStatusDeep;

        private readonly GenerationList generationList;

        private bool ignoreCheckedChange = true;

        public SdLorasForm(GenerationList generationList)
        {
            InitializeComponent();

            this.generationList = generationList;
        }

        private void SdLorasForm_Load(object sender, EventArgs e)
        {
            tbCivitaiApiKey.Text = Program.Config.CivitaiApiKey;

            updateList();
        }

        private void updateList()
        {
            ignoreCheckedChange = true;

            lvModels.Items.Clear();

            foreach (var name in SdLoraHelper.GetNames())
            {
                if (SdLoraHelper.GetPathToModel(name) != null
                 || !string.IsNullOrEmpty(SdLoraHelper.GetConfig(name).downloadUrl))
                {
                    var item = new ListViewItem
                    {
                        Name = name,
                        UseItemStyleForSubItems = false,
                        Checked = SdLoraHelper.IsEnabled(name) && SdLoraHelper.GetPathToModel(name) != null,
                    };
                    item.SubItems.Add(SdLoraHelper.GetStatus(name));
                    item.SubItems.Add(SdLoraHelper.GetHumanName(name));
                    item.SubItems.Add(SdLoraHelper.GetConfig(name).description);
                    item.SubItems.Add(SdLoraHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                    lvModels.Items.Add(item);
                }
            }

            ignoreCheckedChange = false;
        }

        private void updateStatus()
        {
            if (!isNeedUpdateStatusLight && !isNeedUpdateStatusDeep) return;

            var deep = isNeedUpdateStatusDeep;

            isNeedUpdateStatusLight = false;
            isNeedUpdateStatusDeep = false;

            foreach (ListViewItem item in lvModels.Items)
            {
                SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_lora_", 1, deep, SdLoraHelper.GetStatus);
            }
        }

        private static void addLoraToDownloadQueue(GenerationList generationList, string name)
        {
            if (SdLoraHelper.GetPathToModel(name) != null) return;

            var url = SdLoraHelper.GetConfig(name).downloadUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            var genItemName = "download_lora_" + name;
            if (generationList.FindItem(genItemName) != null) return;

            generationList.AddGeneration(new SdDownloadingListItem
            (
                genItemName,
                "Download " + name + " / LoRA model",
                () => true,
                async (progress, cancelationTokenSource) =>
                {
                    var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
                    (
                        url,
                        SdLoraHelper.GetDir(),
                        s => { progress(s); isNeedUpdateStatusLight = true; },
                        new DownloadFileOptions
                        {
                            FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, name + ".safetensors"),
                            PreprocessFileName = x => name + Path.GetExtension(x),
                            AuthorizationBearer = Program.Config.CivitaiApiKey,
                        },
                        cancelationTokenSource
                    );
                    analyzeDownloadedModel(resultFilePath, progress);
                }
            ));
        }

        private static void analyzeDownloadedModel(string? resultFilePath, Action<string> progress)
        {
            var success = SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath, () =>
            {
                progress("Invalid API key");
            });
            if (success)
            {
                progress("+");
                isNeedUpdateStatusDeep = true;
            }
        }

        private void lvModels_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ignoreCheckedChange) return;

            SdLoraHelper.SetEnabled(e.Item.Name, e.Item.Checked);

            if (e.Item.Checked)
            {
                addLoraToDownloadQueue(generationList, e.Item.Name);
            }
        }

        private void lvModels_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var hit = lvModels.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem == hit.Item.SubItems[4])
            {
                ProcessHelper.OpenUrlInBrowser(hit.SubItem.Text);
            }
        }

        private void tbCivitaiApiKey_TextChanged(object sender, EventArgs e)
        {
            if (Program.Config.CivitaiApiKey != tbCivitaiApiKey.Text)
            {
                Program.Config.CivitaiApiKey = tbCivitaiApiKey.Text;
                Program.SaveConfig();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrlInBrowser("https://civitai.com/user/account");
        }

        private void btImportFromCivitai_Click(object sender, EventArgs e)
        {
            var form = new AddImportModelForm();
            form.tabs.SelectedTab = form.tabLora;
            form.ShowDialog(this);
            updateList();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            updateStatus();
        }
    }
}
