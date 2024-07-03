using System.ComponentModel;
using System.IO;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdLorasForm : Form
    {
        //private string[] modelNames = null;
        private string[] checkedNames = {};

        private bool ignoreCheckedChange = true;

        private bool isProvidedKeyInvalid = false;

        public SdLorasForm()
        {
            InitializeComponent();
        }

        private void SdLorasForm_Load(object sender, EventArgs e)
        {
            tbCivitaiApiKey.Text = Program.Config.CivitaiApiKey;

            var modelNames = SdLoraHelper.GetNames();

            ignoreCheckedChange = true;
            foreach (var name in modelNames)
            {
                var filePath = SdLoraHelper.GetPathToModel(name);
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)
                 || !string.IsNullOrEmpty(SdLoraHelper.GetConfig(name).downloadUrl))
                {
                    var item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.SubItems.Add(SdLoraHelper.GetStatus(name));
                    item.SubItems.Add(SdLoraHelper.GetHumanName(name));
                    item.SubItems.Add(SdLoraHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                    item.Name = name;
                    item.Checked = SdLoraHelper.IsEnabled(name) && SdLoraHelper.GetPathToModel(name) != null;

                    lvModels.Items.Add(item);

                    if (item.Checked) checkedNames = checkedNames.Concat(new[] { name }).ToArray();
                }
            }
            ignoreCheckedChange = false;

            bwDownloading.RunWorkerAsync();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            bwDownloading.CancelAsync();
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
            if (SdLoraHelper.GetPathToModel(name) != null) return;

            var url = SdLoraHelper.GetConfig(name).downloadUrl;
            if (string.IsNullOrWhiteSpace(url)) return;

            if (SdLoraHelper.GetConfig(name).isNeedAuthToDownload && (string.IsNullOrEmpty(Program.Config.CivitaiApiKey) || isProvidedKeyInvalid)) return;

            var resultFilePath = downloadFile(name, url, text => updateStatus(name, text), new DownloadFileOptions
            {
                FileNameIfNotDetected = SdModelDownloadHelper.GetModelFileNameFromUrl(url, name + ".safetensors"),
                PreprocessFileName = x => name + Path.GetExtension(x),
                AuthorizationBearer = SdLoraHelper.GetConfig(name).isNeedAuthToDownload ? Program.Config.CivitaiApiKey : null,
            });

            SdModelDownloadHelper.AnalyzeDownloadedModel(resultFilePath, () =>
            {
                if (!SdLoraHelper.GetConfig(name).isNeedAuthToDownload)
                {
                    SdLoraHelper.GetConfig(name).isNeedAuthToDownload = true;
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

            if (!ignoreCheckedChange) SdLoraHelper.SetEnabled(e.Item.Name, e.Item.Checked);
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

                resultFilePath = DownloadTools.DownloadFileAsync(url, SdLoraHelper.GetDir(), newOptions, cancelationTokenSource.Token).Result;
            }
            catch (AggregateException e)
            {
                Program.Log.WriteLine("Downloading " + url + " ERROR: " + e.Message);
            }

            Invoke(() =>
            {
                updateStatus(name, null);
                btOk.Enabled = true;
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
                    text ??= SdLoraHelper.GetStatus(name);
                    if (item.SubItems[1].Text != text) item.SubItems[1].Text = text;
                }
            });
        }

        private void lvModels_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var hit = lvModels.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem == hit.Item.SubItems[3])
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
    }
}
