﻿using System.Text.RegularExpressions;
using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion;

public partial class SdDownloadingListItem : UserControl, IGenerationListItem
{
    public GenerationParallelGroup ParallelGroup => GenerationParallelGroup.DOWNLOAD;
        
    public bool HasWorkToRun => !IsDone && isReadyToStartWork();
    public bool InProcess { get; private set; }
    public bool WantToBeRemoved { get; private set; }
    
    public string DownloadStatus => pbProgress.CustomText ?? "";

    private readonly Func<bool> isReadyToStartWork;
    private readonly Func<Action<string>, CancellationTokenSource, Task> workAsync;
        
    public bool IsDone { get; private set; }

    private readonly CancellationTokenSource cancellationTokenSource = new();

    public SdDownloadingListItem(string name, string text, Func<bool> isReadyToStartWork, Func<Action<string>, CancellationTokenSource, Task> workAsync)
    {
        InitializeComponent();
            
        this.isReadyToStartWork = isReadyToStartWork;
        this.workAsync = workAsync;

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

    private async Task runInnerAsync()
    {
        try
        {
            await workAsync(percent =>
            {
                Invoke(() =>
                {
                    pbProgress.CustomText = percent;
                    pbProgress.Value = Regex.IsMatch(percent, @"^\d+%$") ? int.Parse(percent.TrimEnd('%')) : 0;
                    pbProgress.Refresh();
                });
            }, cancellationTokenSource);
                
            pbProgress.Value = 100;
            pbProgress.CustomText = "100%";
            pbProgress.Refresh();
        }
        catch (Exception e)
        {
            Program.Log.WriteLine(e.ToString());
            await Task.Delay(1000);
        }

        IsDone = true;
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