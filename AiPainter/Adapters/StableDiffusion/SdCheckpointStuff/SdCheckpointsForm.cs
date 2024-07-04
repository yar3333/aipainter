using System;
using System.ComponentModel;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdCheckpointStuff
{
    public partial class SdCheckpointsForm : Form
    {
        private string[] checkedNames = { };

        private bool ignoreCheckedChange = true;

        private bool isProvidedKeyInvalid = false;

        public SdCheckpointsForm()
        {
            InitializeComponent();
        }

        private void SdModelsForm_Load(object sender, EventArgs e)
        {
            tbCivitaiApiKey.Text = Program.Config.CivitaiApiKey;

            updateList();

            bwDownloading.RunWorkerAsync();
        }

        private void updateList()
        {
            ignoreCheckedChange = true;

            lvModels.Items.Clear();

            foreach (var name in SdCheckpointsHelper.GetNames("").Where(x => x != ""))
            {
                var filePath = SdCheckpointsHelper.GetPathToMainCheckpoint(name);
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)
                 || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl)
                 || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl))
                {
                    var item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.SubItems.Add(SdCheckpointsHelper.GetStatusMain(name));
                    item.SubItems.Add(SdCheckpointsHelper.GetStatusInpaint(name));
                    item.SubItems.Add(SdCheckpointsHelper.GetStatusVae(name));
                    item.SubItems.Add(name);
                    item.SubItems.Add(SdCheckpointsHelper.GetConfig(name).description);
                    item.SubItems.Add(SdCheckpointsHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                    item.Name = name;
                    item.Checked = SdCheckpointsHelper.IsEnabled(name) && SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null;

                    lvModels.Items.Add(item);

                    if (item.Checked) checkedNames = checkedNames.Concat(new[] { name }).ToArray();
                }
            }

            ignoreCheckedChange = false;
        }

        private void bwDownloading_DoWork(object sender, DoWorkEventArgs _)
        {
            while (!bwDownloading.CancellationPending)
            {
                foreach (var name in checkedNames)
                {
                    downloadModelMain(name);
                    if (bwDownloading.CancellationPending) break;

                    downloadModelInpaint(name);
                    if (bwDownloading.CancellationPending) break;

                    downloadModelVae(name);
                    if (bwDownloading.CancellationPending) break;
                }

                if (bwDownloading.CancellationPending) break;
                Thread.Sleep(250);
            }
        }

        private void downloadModelMain(string name)
        {
            if (SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null) return;

            var url = SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            if (SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload && (string.IsNullOrEmpty(Program.Config.CivitaiApiKey) || isProvidedKeyInvalid)) return;

            var resultFilePath = downloadFile(name, url, status => updateStatus(name, status, null, null), new DownloadFileOptions
            {
                FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, "main.safetensors"),
                AuthorizationBearer = SdModelDownloadHelper.GetCheckpointAuthorizationBearer(name),
            });

            analyzeDownloadedModel(name, resultFilePath, status => updateStatus(name, status, null, null));
        }

        private void downloadModelInpaint(string name)
        {
            if (SdCheckpointsHelper.GetPathToInpaintCheckpoint(name) != null) return;

            var url = SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            if (SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload && (string.IsNullOrEmpty(Program.Config.CivitaiApiKey) || isProvidedKeyInvalid)) return;

            var resultFilePath = downloadFile(name, url, status => updateStatus(name, null, status, null), new DownloadFileOptions
            {
                FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, "inpaint.safetensors"),
                AuthorizationBearer = SdModelDownloadHelper.GetCheckpointAuthorizationBearer(name),
            });

            analyzeDownloadedModel(name, resultFilePath, status => updateStatus(name, null, status, null));
        }

        private void downloadModelVae(string name)
        {
            if (SdCheckpointsHelper.GetPathToVae(name) != null) return;

            var url = SdCheckpointsHelper.GetConfig(name).vaeUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            if (SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload && (string.IsNullOrEmpty(Program.Config.CivitaiApiKey) || isProvidedKeyInvalid)) return;

            var resultFilePath = downloadFile(name, url, status => updateStatus(name, null, null, status), new DownloadFileOptions
            {
                FileNameIfNotDetected = prepareVaeFileName(SdModelDownloadHelper.GetModelFileNameFromUrl(url, "vae.pt")),
                PreprocessFileName = prepareVaeFileName!,
                AuthorizationBearer = SdModelDownloadHelper.GetCheckpointAuthorizationBearer(name),
            });

            analyzeDownloadedModel(name, resultFilePath, status => updateStatus(name, null, null, status));
        }

        private void analyzeDownloadedModel(string name, string? resultFilePath, Action<string> onUpdateStatus)
        {
            SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath, () =>
            {
                if (!SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload)
                {
                    SdCheckpointsHelper.GetConfig(name).isNeedAuthToDownload = true;
                    onUpdateStatus("need API key");
                }
                else
                {
                    onUpdateStatus("invalid API key");
                    isProvidedKeyInvalid = true;
                }
            });
        }

        private void lvModels_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                if (!checkedNames.Contains(e.Item.Name)) checkedNames = checkedNames.Concat(new[] { e.Item.Name }).OrderBy(x => x).ToArray();
            }
            else
            {
                if (checkedNames.Contains(e.Item.Name)) checkedNames = checkedNames.Where(x => x != e.Item.Name).ToArray();
            }

            if (!ignoreCheckedChange) SdCheckpointsHelper.SetEnabled(e.Item.Name, e.Item.Checked);
        }

        private string? downloadFile(string name, string url, Action<string> progress, DownloadFileOptions options)
        {
            Invoke(() => btOk.Enabled = false);

            var cancelationTokenSource = new CancellationTokenSource();

            string? resultFilePath = null;
            try
            {
                var newOptions = options.Clone();
                newOptions.Progress = (size, total) =>
                {
                    progress(total != null ? Math.Round(size / (double)total * 100) + "%" : size + " bytes");

                    if (bwDownloading.CancellationPending || !checkedNames.Contains(name))
                    {
                        cancelationTokenSource.Cancel();
                    }
                };
                resultFilePath = DownloadTools.DownloadFileAsync(url, SdCheckpointsHelper.GetDirPath(name), newOptions, cancelationTokenSource.Token).Result;
            }
            catch (AggregateException e)
            {
                Program.Log.WriteLine("Downloading " + url + " ERROR: " + e.Message);
            }

            Invoke(() =>
            {
                updateStatus(name, null, null, null);
                btOk.Enabled = true;
            });

            return resultFilePath;
        }

        private void updateStatus(string name, string? textMain, string? textInapint, string? textVae)
        {
            Invoke(() =>
            {
                var item = lvModels.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Name == name);
                if (item != null)
                {
                    textMain ??= SdCheckpointsHelper.GetStatusMain(name);
                    if (item.SubItems[1].Text != textMain) item.SubItems[1].Text = textMain;

                    textInapint ??= SdCheckpointsHelper.GetStatusInpaint(name);
                    if (item.SubItems[2].Text != textInapint) item.SubItems[2].Text = textInapint;

                    textVae ??= SdCheckpointsHelper.GetStatusVae(name);
                    if (item.SubItems[3].Text != textVae) item.SubItems[3].Text = textVae;
                }
            });
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

        private string prepareVaeFileName(string fileName)
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

        private void SdCheckpointsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bwDownloading.CancelAsync();
        }
    }
}
