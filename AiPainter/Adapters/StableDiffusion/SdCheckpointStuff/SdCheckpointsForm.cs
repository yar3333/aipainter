using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdCheckpointStuff
{
    public partial class SdCheckpointsForm : Form
    {
        private static bool isNeedUpdateStatusLight;
        private static bool isNeedUpdateStatusDeep;
        private static bool isProvidedKeyInvalid;

        private readonly GenerationList generationList;

        private bool ignoreCheckedChange = true;

        public SdCheckpointsForm(GenerationList generationList)
        {
            InitializeComponent();

            this.generationList = generationList;
        }

        private void SdModelsForm_Load(object sender, EventArgs e)
        {
            tbCivitaiApiKey.Text = Program.Config.CivitaiApiKey;

            updateList();
        }

        private void updateList()
        {
            ignoreCheckedChange = true;

            lvModels.Items.Clear();

            foreach (var name in SdCheckpointsHelper.GetNames("").Where(x => x != ""))
            {
                if (SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null
                 || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl)
                 || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl))
                {
                    var item = new ListViewItem
                    {
                        Name = name,
                        UseItemStyleForSubItems = false,
                        Checked = SdCheckpointsHelper.IsEnabled(name) && SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null,
                    };
                    item.SubItems.Add(SdCheckpointsHelper.GetStatusMain(name));
                    item.SubItems.Add(SdCheckpointsHelper.GetStatusInpaint(name));
                    item.SubItems.Add(SdCheckpointsHelper.GetStatusVae(name));
                    item.SubItems.Add(name);
                    item.SubItems.Add(SdCheckpointsHelper.GetConfig(name).description);
                    item.SubItems.Add(SdCheckpointsHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

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
                SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_checkpoint_main_",    1, deep, SdCheckpointsHelper.GetStatusMain);
                SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_checkpoint_inpaint_", 2, deep, SdCheckpointsHelper.GetStatusInpaint);
                SdModelDownloadHelper.UpdateFileStatusInListView(generationList, item, "download_checkpoint_vae_",     3, deep, SdCheckpointsHelper.GetStatusVae);
            }
        }

        private static void addCheckpointMainToDownloadQueue(GenerationList generationList, string name)
        {
            if (SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null) return;

            var url = SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            generationList.AddGeneration(new SdDownloadingListItem
            (
                "download_checkpoint_main_" + name,
                "Download " + name + " / Main model",
                () => isReadyToDownload(name),
                async (progress, cancelationTokenSource) =>
                {
                    var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
                    (
                        url,
                        SdCheckpointsHelper.GetDirPath(name),
                        s => { progress(s); isNeedUpdateStatusLight = true; },
                        new DownloadFileOptions
                        {
                            FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, "main.safetensors"),
                            AuthorizationBearer = SdModelDownloadHelper.GetCheckpointAuthorizationBearer(name),
                        },
                        cancelationTokenSource
                    );
                    analyzeDownloadedModel(name, resultFilePath, progress);
                }
            ));
        }

        private static void addCheckpointInpaintToDownloadQueue(GenerationList generationList, string name)
        {
            if (SdCheckpointsHelper.GetPathToInpaintCheckpoint(name) != null) return;

            var url = SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            generationList.AddGeneration(new SdDownloadingListItem
            (
                "download_checkpoint_inpaint_" + name,
                "Download " + name + " / Inpaint model",
                () => isReadyToDownload(name),
                async (progress, cancelationTokenSource) =>
                {
                    var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
                    (
                         url,
                         SdCheckpointsHelper.GetDirPath(name),
                         s => { progress(s); isNeedUpdateStatusLight = true; },
                         new DownloadFileOptions
                         {
                             FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, "inpaint.safetensors"),
                             AuthorizationBearer = SdModelDownloadHelper.GetCheckpointAuthorizationBearer(name),
                         },
                         cancelationTokenSource
                    );
                    analyzeDownloadedModel(name, resultFilePath, progress);
                }
            ));
        }

        private static void addVaeToDownloadQueue(GenerationList generationList, string name)
        {
            if (SdCheckpointsHelper.GetPathToVae(name) != null) return;

            var url = SdCheckpointsHelper.GetConfig(name).vaeUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            generationList.AddGeneration(new SdDownloadingListItem
            (
                "download_checkpoint_vae_" + name,
                "Download " + name + " / VAE model",
                () => isReadyToDownload(name),
                async (progress, cancelationTokenSource) =>
                {
                    var resultFilePath = await SdModelDownloadHelper.DownloadFileAsync
                    (
                         url,
                         SdCheckpointsHelper.GetDirPath(name),
                         s => { progress(s); isNeedUpdateStatusLight = true; },
                         new DownloadFileOptions
                         {
                             FileNameIfNotDetected = prepareVaeFileName(SdModelDownloadHelper.GetModelFileNameFromUrl(url, "vae.pt")),
                             PreprocessFileName = prepareVaeFileName!,
                             AuthorizationBearer = SdModelDownloadHelper.GetCheckpointAuthorizationBearer(name),
                         },
                         cancelationTokenSource
                    );
                    analyzeDownloadedModel(name, resultFilePath, progress);
                }
            ));
        }

        private static void analyzeDownloadedModel(string name, string? resultFilePath, Action<string> progress)
        {
            var success = SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath, () =>
            {
                if (!SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload)
                {
                    SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload = true;
                    progress("need API key");
                }
                else
                {
                    progress("invalid API key");
                    isProvidedKeyInvalid = true;
                }
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
            
            SdCheckpointsHelper.SetEnabled(e.Item.Name, e.Item.Checked);
            
            if (e.Item.Checked)
            {
                addCheckpointMainToDownloadQueue(generationList, e.Item.Name);
                addCheckpointInpaintToDownloadQueue(generationList, e.Item.Name);
                addVaeToDownloadQueue(generationList, e.Item.Name);
            }
        }

        private void lvModels_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var hit = lvModels.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem == hit.Item.SubItems[6])
            {
                ProcessHelper.OpenUrlInBrowser(hit.SubItem.Text);
            }
        }

        private static string prepareVaeFileName(string fileName)
        {
            return !SdCheckpointsHelper.IsFilePathLikeVae(fileName)
                            ? Path.GetFileNameWithoutExtension(fileName) + "-vae" + Path.GetExtension(fileName)
                            : fileName;
        }

        private void tbCivitaiApiKey_TextChanged(object sender, EventArgs e)
        {
            if (Program.Config.CivitaiApiKey != tbCivitaiApiKey.Text)
            {
                isProvidedKeyInvalid = false;
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
            form.tabs.SelectedTab = form.tabCheckpoint;
            form.ShowDialog(this);
            updateList();
        }

        private static bool isReadyToDownload(string name)
        {
            return !SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload
                || !string.IsNullOrEmpty(Program.Config.CivitaiApiKey) && !isProvidedKeyInvalid;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            updateStatus();
        }
    }
}
