using System.Text.RegularExpressions;
using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion;

public partial class SdListItemDownloading : UserControl, IGenerationListItem
{
    public GenerationParallelGroup ParallelGroup => GenerationParallelGroup.DOWNLOAD;
        
    public bool HasWorkToRun => !IsDone 
                             && (lastBadCivitApiKey == null || lastBadCivitApiKey != Program.Config.CivitaiApiKey) 
                             && isReadyToStartWork();
    public bool InProcess { get; private set; }
    public bool WantToBeRemoved { get; private set; }
    
    public string DownloadStatus => pbProgress.CustomText ?? "";

    private readonly Func<bool> isReadyToStartWork;
    
    public Func<CancellationTokenSource, Task>? WorkAsync;
        
    public bool IsDone { get; private set; }
    private string? lastBadCivitApiKey;

    private readonly CancellationTokenSource cancellationTokenSource = new();

    public SdListItemDownloading(string name, string text, Func<bool> isReadyToStartWork)
    {
        InitializeComponent();
            
        this.isReadyToStartWork = isReadyToStartWork;

        pbProgress.Maximum = 100;
        pbProgress.Value = 0;
        pbProgress.CustomText = "0%";
        pbProgress.Refresh();

        this.Name = name;
        tbText.Text = text;

        toolTip.SetToolTip(tbText, text);
    }

    public void Run()
    {
        InProcess = true;

        Task.Run(async () =>
        {
            await runInnerAsync();
            InProcess = false;
        });
    }

    public void NotifyProgress(string s)
    {
        Invoke(() =>
        {
            pbProgress.CustomText = s;
            pbProgress.Value = Regex.IsMatch(s, @"^\d+%$") ? int.Parse(s.TrimEnd('%')) : 0;
            pbProgress.Refresh();
        });
    }

    private async Task runInnerAsync()
    {
        try
        {
            await WorkAsync!(cancellationTokenSource);
            NotifyProgress("100%");
            IsDone = true;
        }
        catch (SdListItemDownloadingInvalidApiKeyException)
        {
            lastBadCivitApiKey = Program.Config.CivitaiApiKey;
            await Task.Delay(1000);
        }
        catch (Exception e)
        {
            Program.Log.WriteLine(e.ToString());
            NotifyProgress(e.Message);
            IsDone = true;
        }
    }

    public void Cancel()
    {
        cancellationTokenSource.Cancel();
    }

    private void btRemove_Click(object sender, EventArgs e)
    {
        btRemove.Enabled = false;
        WantToBeRemoved = true;
        cancellationTokenSource.Cancel();
    }
}