using System.ComponentModel;
using System.Text.RegularExpressions;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdCheckpointsForm : Form
    {
        private string[] checkpointNames = null;
        private string[] checkedNames = {};

        private bool ignoreCheckedChange = true;

        public SdCheckpointsForm()
        {
            InitializeComponent();
        }

        private void SdModelsForm_Load(object sender, EventArgs e)
        {
            checkpointNames = SdCheckpointsHelper.GetNames("").Where(x => x != "").ToArray();

            ignoreCheckedChange = true;
            foreach (var name in checkpointNames)
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
                    item.SubItems.Add(SdCheckpointsHelper.GetConfig(name).homeUrl, Color.Blue, Color.White, item.Font);

                    item.Name = name;
                    item.Checked = SdCheckpointsHelper.IsEnabled(name) && SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null;

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
                    {
                        var url = SdCheckpointsHelper.GetConfig(name).mainCheckpointUrl;
                        if (!string.IsNullOrWhiteSpace(url) && SdCheckpointsHelper.GetPathToMainCheckpoint(name) == null)
                        {
                            var uri = new Uri(url);
                            var fileName = uri.LocalPath.EndsWith(".ckpt") || uri.LocalPath.EndsWith(".safetensors")
                                                ? Path.GetFileName(uri.LocalPath)
                                                : "main.safetensors";
                            downloadFile(name, url, fileName, null, text => updateStatus(name, text, null, null));
                        }
                    }
                    
                    if (bwDownloading.CancellationPending) break;
                    
                    {
                        var url = SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl;
                        if (!string.IsNullOrWhiteSpace(url) && SdCheckpointsHelper.GetPathToInpaintCheckpoint(name) == null)
                        {
                            var uri = new Uri(url);
                            var fileName = uri.LocalPath.EndsWith(".ckpt") || uri.LocalPath.EndsWith(".safetensors")
                                                ? Path.GetFileName(uri.LocalPath)
                                                : "inpaint.safetensors";
                            downloadFile(name, url, fileName, null, text => updateStatus(name, null, text, null));
                        }
                    }
                    
                    if (bwDownloading.CancellationPending) break;
                    
                    {
                        var url = SdCheckpointsHelper.GetConfig(name).vaeUrl;
                        if (!string.IsNullOrWhiteSpace(url) && SdCheckpointsHelper.GetPathToVae(name) == null)
                        {
                            var uri = new Uri(url);
                            
                            var fileName = uri.LocalPath.EndsWith(".ckpt") || uri.LocalPath.EndsWith(".safetensors") || uri.LocalPath.EndsWith(".pt")
                                                ? prepareVaeFileName(Path.GetFileName(uri.LocalPath))
                                                : "vae.pt";
                            
                            downloadFile(name, url, fileName, prepareVaeFileName, text => updateStatus(name, null, null, text));
                        }
                    }

                    if (bwDownloading.CancellationPending) break;
                }

                if (bwDownloading.CancellationPending) break;
                Thread.Sleep(500);
            }
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

        private void downloadFile(string name, string url, string fileNameIfNotDetected, Func<string, string>? preprocessFileName, Action<string> progress)
        {
            Invoke(() => btOk.Enabled = false);

            var cancelationTokenSource = new CancellationTokenSource();

            try
            {
                DownloadTools.DownloadFileAsync(url, fileNameIfNotDetected, preprocessFileName, SdCheckpointsHelper.GetDirPath(name), cancelationTokenSource.Token, (size, total) =>
                {
                    progress(total != null ? Math.Round(size / (double)total * 100) + "%" : size + " bytes");

                    if (bwDownloading.CancellationPending || !checkedNames.Contains(name))
                    {
                        cancelationTokenSource.Cancel();
                    }
                }).Wait();
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
            if (hit.Item != null && hit.SubItem == hit.Item.SubItems[5])
            {
                ProcessHelper.OpenUrlInBrowser(hit.SubItem.Text);
            }
        }

        private string prepareVaeFileName(string fileName)
        {
            return !Regex.IsMatch(fileName, @"\bvae\b")
                            ? Path.GetFileNameWithoutExtension(fileName) + "-vae" + Path.GetExtension(fileName)
                            : fileName;
        }
    }
}
