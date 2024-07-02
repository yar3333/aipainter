﻿using System.ComponentModel;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdCheckpointsForm : Form
    {
        private string[] checkpointNames = null;
        private string[] checkedNames = { };

        private bool ignoreCheckedChange = true;

        private ListViewItem.ListViewSubItem? mSelected = null;

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
                            try
                            {
                                downloadFile(name, url, fileName, text => updateStatus(name, text, null)).Wait();
                            }
                            catch (AggregateException)
                            {
                                updateStatus(name, null, null);
                                Invoke(() => btOk.Enabled = true);
                                break;
                            }
                        }
                    }
                    {
                        var url = SdCheckpointsHelper.GetConfig(name).inpaintCheckpointUrl;
                        if (!string.IsNullOrWhiteSpace(url) && SdCheckpointsHelper.GetPathToInpaintCheckpoint(name) == null)
                        {
                            var uri = new Uri(url);
                            var fileName = uri.LocalPath.EndsWith(".ckpt") || uri.LocalPath.EndsWith(".safetensors")
                                                ? Path.GetFileName(uri.LocalPath)
                                                : "inpaint.safetensors";
                            try
                            {
                                downloadFile(name, url, fileName, text => updateStatus(name, null, text)).Wait();
                            }
                            catch (AggregateException)
                            {
                                updateStatus(name, null, null);
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

            if (!ignoreCheckedChange) SdCheckpointsHelper.SetEnabled(e.Item.Name, e.Item.Checked);
        }

        private async Task downloadFile(string name, string url, string fileNameIfNotDetected, Action<string> progress)
        {
            Invoke(() => btOk.Enabled = false);

            var cancelationTokenSource = new CancellationTokenSource();
            await DownloadTools.DownloadFileAsync(url, fileNameIfNotDetected, SdCheckpointsHelper.GetDirPath(name), cancelationTokenSource.Token, (size, total) =>
            {
                progress((total != null ? Math.Round(size / (double)total * 100) + "%" : size + " bytes"));

                if (bwDownloading.CancellationPending || !checkedNames.Contains(name))
                {
                    cancelationTokenSource.Cancel();
                }
            });

            Invoke(() =>
            {
                updateStatus(name, null, null);
                btOk.Enabled = true;
            });
        }



        private void updateStatus(string name, string? textMain, string? textInapint)
        {
            Invoke(() =>
            {
                var item = lvModels.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Name == name);
                if (item != null)
                {
                    textMain ??= SdCheckpointsHelper.GetStatusMain(name);
                    if (item.SubItems[1].Text != textMain) item.SubItems[1].Text = textMain;

                    textInapint ??= SdCheckpointsHelper.GetStatusMain(name);
                    if (item.SubItems[2].Text != textInapint) item.SubItems[2].Text = textInapint;
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
    }
}
