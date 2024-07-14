namespace AiPainter.Controls
{
    public partial class WaitingDialog : Form
    {
        private Func<CancellationTokenSource, Task> workAsync = null!;
        private readonly CancellationTokenSource cancellationTokenSource;

        public WaitingDialog(string title, string label, int progressMin = 0, int progressMax = 100)
        {
            InitializeComponent();

            this.Text = title;
            this.label.Text = label;
            this.progressBar.Minimum = progressMin;
            this.progressBar.Maximum = progressMax;

            cancellationTokenSource = new CancellationTokenSource();
        }

        public string LabelText
        {
            get => label.Text;
            set { Invoke(() => label.Text = value); }
        }

        public int ProgressValue
        {
            get => progressBar.Value;
            set { Invoke(() => progressBar.Value = value); }
        }

        public ProgressBarStyle ProgressStyle
        {
            get => progressBar.Style;
            set { Invoke(() => progressBar.Style = value); }
        }

        // ReSharper disable once ParameterHidesMember
        public DialogResult ShowDialog(IWin32Window parent, Func<CancellationTokenSource, Task> workAsync)
        {
            this.workAsync = workAsync;
            return base.ShowDialog(parent);
        }

        private void WaitingDialog_Load(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                try
                {
                    await workAsync(cancellationTokenSource);

                    Invoke(() =>
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    });
                }
                catch (AggregateException)
                {
                    Invoke(() =>
                    {
                        DialogResult = DialogResult.Cancel;
                        Close();
                    });
                }
            });
        }

        private void WaitingDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
