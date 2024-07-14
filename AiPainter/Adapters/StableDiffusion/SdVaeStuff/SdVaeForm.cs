using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion.SdVaeStuff;

public class SdVaeForm
{
    private readonly string vaeName;
    private readonly WaitingDialog dialog;

    public SdVaeForm(string vaeName)
    {
        this.vaeName = vaeName;
        
        dialog = new WaitingDialog("VAE downloading", "Downloading " + vaeName);
    }

    public DialogResult ShowDialog(IWin32Window parent)
    {
        var url = SdVaeHelper.GetConfig(vaeName).downloadUrl;
        if (url == null) return DialogResult.Abort;
        
        return dialog.ShowDialog(parent, async cancellationTokenSource =>
        {
            await DownloadTools.DownloadFileAsync(url, SdVaeHelper.GetDirPath(vaeName), new DownloadFileOptions
            {
                FileNameIfNotDetected = Path.GetFileName(new Uri(url).LocalPath),
                Progress = (size, total) =>
                {
                    dialog.ProgressStyle = total != null ? ProgressBarStyle.Blocks : ProgressBarStyle.Marquee;
                    if (total != null)
                    {
                        dialog.ProgressValue = (int)Math.Round(size / (double)total * 100);
                    }
                },
            }, cancellationTokenSource.Token);
        });
    }
}
