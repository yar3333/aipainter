using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdVaeStuff
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
                    DownloadTools.DownloadFileAsync(url, SdVaeHelper.GetDirPath(vaeName), new DownloadFileOptions
                    {
                        FileNameIfNotDetected = Path.GetFileName(new Uri(url).LocalPath),
                        Progress = (size, total) =>
                        {
                            Invoke(() =>
                            {
                                progressBar.Style = total != null ? ProgressBarStyle.Blocks : ProgressBarStyle.Marquee;
                                if (total != null)
                                {
                                    progressBar.Value = (int)Math.Round(size / (double)total * 100);
                                }
                            });
                        }
                    }, cancelationTokenSource.Token).Wait();
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
