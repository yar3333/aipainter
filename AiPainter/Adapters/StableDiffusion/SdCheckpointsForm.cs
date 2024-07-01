using System.ComponentModel;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdCheckpointsForm : Form
    {
        private string[] checkpointNames = null;
        private string[] checkedNames = {};

        public SdCheckpointsForm()
        {
            InitializeComponent();
        }

        private void SdModelsForm_Load(object sender, EventArgs e)
        {
            checkpointNames = SdCheckpointsHelper.GetNames("").Where(x => x != "").ToArray();

            foreach (var name in checkpointNames)
            {
                var filePath = SdCheckpointsHelper.GetPathToMainCheckpoint(name);
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)
                 || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).downloadUrl)
                 || !string.IsNullOrEmpty(SdCheckpointsHelper.GetConfig(name).downloadInpaintUrl))
                {
                    var item = new ListViewItem(new[]
                    {
                        SdCheckpointsHelper.GetStatus(name),
                        name,
                        SdCheckpointsHelper.GetConfig(name).description,
                    });
                    item.Name = name;
                    item.Checked = SdCheckpointsHelper.IsEnabled(name) && SdCheckpointsHelper.GetPathToMainCheckpoint(name) != null;
                    lvModels.Items.Add(item);

                    if (item.Checked) checkedNames = checkedNames.Concat(new[] { name }).ToArray();
                }
            }

            bwDownloading.RunWorkerAsync();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            bwDownloading.CancelAsync();
        }

        private void bwDownloading_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bwDownloading.CancellationPending)
            {
                foreach (var name in checkedNames)
                {
                    {
                        var url = SdCheckpointsHelper.GetConfig(name).downloadUrl;
                        if (!string.IsNullOrWhiteSpace(url) && SdCheckpointsHelper.GetPathToMainCheckpoint(name) == null)
                        {
                            var uri = new Uri(url);
                            var fileName = uri.LocalPath.EndsWith(".ckpt") || uri.LocalPath.EndsWith(".safetensors")
                                                ? Path.GetFileName(uri.LocalPath)
                                                : "main.safetensors";
                            try
                            {
                                downloadFile(name, url, fileName, "main").Wait();
                            }
                            catch (AggregateException)
                            {
                                updateStatus(name);
                                Invoke(() => btOk.Enabled = true);
                                break;
                            }
                        }
                    }                   
                    {
                        var url = SdCheckpointsHelper.GetConfig(name).downloadInpaintUrl;
                        if (!string.IsNullOrWhiteSpace(url) && SdCheckpointsHelper.GetPathToInpaintCheckpoint(name) == null)
                        {
                            var uri = new Uri(url);
                            var fileName = uri.LocalPath.EndsWith(".ckpt") || uri.LocalPath.EndsWith(".safetensors")
                                                ? Path.GetFileName(uri.LocalPath)
                                                : "inpaint.safetensors";
                            try
                            {
                                downloadFile(name, url, fileName, "inpaint").Wait();
                            }
                            catch (AggregateException)
                            {
                                updateStatus(name);
                                Invoke(() => btOk.Enabled = true);
                                break;
                            }
                        }
                    }
                }

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

            SdCheckpointsHelper.SetEnabled(e.Item.Name, e.Item.Checked);
        }

        private async Task downloadFile(string name, string url, string fileNameIfNotDetected, string statusPrefix)
        {
            Invoke(() => btOk.Enabled = false);

            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromHours(10);

            var tempDestFile = Path.GetTempFileName();
            await using var file = new FileStream(tempDestFile, FileMode.Create, FileAccess.Write, FileShare.None);
            var cancelationTokenSource = new CancellationTokenSource();
            var fileName = await client.DownloadAsync(url, file, (size, total) =>
            {
                updateStatus(name, statusPrefix + ": " + (total != null ? Math.Round(size / (double)total * 100) + "%" : size + " bytes"));

                if (bwDownloading.CancellationPending || !checkedNames.Contains(name))
                {
                    cancelationTokenSource.Cancel();
                }
            }, cancelationTokenSource.Token);

            if (string.IsNullOrWhiteSpace(fileName)) fileName = fileNameIfNotDetected;

            File.Move(tempDestFile, Path.Combine(SdCheckpointsHelper.GetDirPath(name), fileName));

            Invoke(() =>
            {
                updateStatus(name);
                btOk.Enabled = true;
            });
        }

        private void updateStatus(string name, string? text = null)
        {
            Invoke(() =>
            {
                var item = lvModels.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Name == name);
                if (item != null)
                {
                    text ??= SdCheckpointsHelper.GetStatus(name);
                    if (item.SubItems[0].Text != text) item.SubItems[0].Text = text;
                }
            });
        }
    }
}
