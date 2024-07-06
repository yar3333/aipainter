using System.ComponentModel;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff
{
    public partial class SdEmbeddingForm : Form
    {
        //private string[] modelNames = null;
        private string[] checkedNames = { };

        private bool ignoreCheckedChange = true;

        private bool isProvidedKeyInvalid = false;

        public SdEmbeddingForm()
        {
            InitializeComponent();
        }

        private void SdLorasForm_Load(object sender, EventArgs e)
        {
            tbCivitaiApiKey.Text = Program.Config.CivitaiApiKey;

            updateList();

            bwDownloading.RunWorkerAsync();
        }

        private void updateList()
        {
            ignoreCheckedChange = true;

            lvModels.Items.Clear();

            var newChecked = new List<string>();
            
            foreach (var name in SdEmbeddingHelper.GetNames())
            {
                var filePath = SdEmbeddingHelper.GetPathToModel(name);
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)
                 || !string.IsNullOrEmpty(SdEmbeddingHelper.GetConfig(name).downloadUrl))
                {
                    var item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.SubItems.Add(SdEmbeddingHelper.GetStatus(name));
                    item.SubItems.Add(SdEmbeddingHelper.GetHumanName(name));
                    item.SubItems.Add(SdEmbeddingHelper.GetConfig(name).description);
                    item.SubItems.Add(SdEmbeddingHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                    item.Name = name;
                    item.Checked = SdEmbeddingHelper.IsEnabled(name) && SdEmbeddingHelper.GetPathToModel(name) != null;

                    lvModels.Items.Add(item);

                    if (item.Checked) newChecked.Add(name);
                }
            }

            checkedNames = newChecked.ToArray();

            ignoreCheckedChange = false;
        }

        private void bwDownloading_DoWork(object sender, DoWorkEventArgs _)
        {
            while (!bwDownloading.CancellationPending)
            {
                foreach (var name in checkedNames)
                {
                    downloadModel(name);
                    if (bwDownloading.CancellationPending) break;
                }

                if (bwDownloading.CancellationPending) break;
                Thread.Sleep(500);
            }
        }

        private void downloadModel(string name)
        {
            if (SdEmbeddingHelper.GetPathToModel(name) != null) return;

            var url = SdEmbeddingHelper.GetConfig(name).downloadUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            if (SdEmbeddingHelper.GetConfig(name).isNeedAuthToDownload && (string.IsNullOrEmpty(Program.Config.CivitaiApiKey) || isProvidedKeyInvalid)) return;

            var resultFilePath = downloadFile(name, url, text => updateStatus(name, text), new DownloadFileOptions
            {
                FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, name + ".safetensors"),
                PreprocessFileName = x => name + Path.GetExtension(x),
                AuthorizationBearer = Program.Config.CivitaiApiKey,
            });

            SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath, () =>
            {
                if (!SdEmbeddingHelper.GetConfig(name).isNeedAuthToDownload)
                {
                    SdEmbeddingHelper.GetConfig(name).isNeedAuthToDownload = true;
                    updateStatus(name, "need API key");
                }
                else
                {
                    updateStatus(name, "invalid API key");
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

            if (!ignoreCheckedChange) SdEmbeddingHelper.SetEnabled(e.Item.Name, e.Item.Checked);
        }

        private string? downloadFile(string name, string url, Action<string> progress, DownloadFileOptions options)
        {
            Invoke(() =>
            {
                btOk.Enabled = false;
                btImportFromCivitai.Enabled = false;
            });

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

                resultFilePath = DownloadTools.DownloadFileAsync(url, SdEmbeddingHelper.GetDir(), newOptions, cancelationTokenSource.Token).Result;
            }
            catch (AggregateException e)
            {
                Program.Log.WriteLine("Downloading " + url + " ERROR: " + e.Message);
            }

            Invoke(() =>
            {
                updateStatus(name, null);
                btOk.Enabled = true;
                btImportFromCivitai.Enabled = true;
            });

            return resultFilePath;
        }

        private void updateStatus(string name, string? text)
        {
            Invoke(() =>
            {
                var item = lvModels.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Name == name);
                if (item != null)
                {
                    text ??= SdEmbeddingHelper.GetStatus(name);
                    if (item.SubItems[1].Text != text) item.SubItems[1].Text = text;
                }
            });
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrlInBrowser("https://civitai.com/user/account");
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

        private void btImportFromCivitai_Click(object sender, EventArgs e)
        {
            var form = new AddImportModelForm();
            form.tabs.SelectedTab = form.tabEmbedding;
            form.ShowDialog(this);
            updateList();
        }

        private void SdEmbeddingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bwDownloading.CancelAsync();
        }
    }
}
