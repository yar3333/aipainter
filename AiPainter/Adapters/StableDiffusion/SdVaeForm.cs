using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdVaeForm : Form
    {
        private readonly string vaeName;
        private readonly CancellationTokenSource cancelationTokenSource;

        public SdVaeForm(string vaeName)
        {
            InitializeComponent();

            this.vaeName = vaeName;
            cancelationTokenSource = new CancellationTokenSource();
        }

        private void SdVaeForm_Load(object sender, EventArgs e)
        {
            var url = SdVaeHelper.GetConfig(vaeName).downloadUrl;
            if (url == null) return;

            Task.Run(() =>
            {
                try
                {
                    DownloadTools.DownloadFileAsync(url, Path.GetFileName(new Uri(url).LocalPath), SdVaeHelper.GetDirPath(vaeName), cancelationTokenSource.Token, (size, total) =>
                    {
                        Invoke(() =>
                        {
                            progressBar.Style = total != null ? ProgressBarStyle.Blocks : ProgressBarStyle.Marquee;
                            if (total != null)
                            {
                                progressBar.Value = (int)Math.Round(size / (double)total * 100);
                            }
                        });
                    }).Wait();
                }
                catch (AggregateException)
                {
                }

                Invoke(Close);
            });
        }

        private void SdVaeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancelationTokenSource.Cancel();
        }
    }
}
