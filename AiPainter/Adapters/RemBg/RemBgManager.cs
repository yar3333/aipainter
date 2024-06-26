using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.RemBg;

class RemBgManager
{
    private bool isPortOpen;

    public bool InProcess;

    public RemBgManager()
    {
        Task.Run(() =>
        {
            while (Application.OpenForms.Count == 0) Thread.Sleep(200);
            
            while (!DelayTools.WaitForExit(1000))
            {
                isPortOpen = RemBgProcess.IsReady();
            }
        });
    }

    public void Run(SmartPictureBox pictureBox, Action<Bitmap?> finished)
    {
        pictureBox.HistoryAddCurrentState();
            
        InProcess = true;

        var activeBox = pictureBox.ActiveBox;

        var fullImage = BitmapTools.Clone(pictureBox.Image!);
        var croppedImage = BitmapTools.GetCropped(fullImage, activeBox, Color.Black);

        Task.Run(() =>
        {
            try
            {
                var resultImage = StableDiffusionClient.RunAsync(croppedImage).Result;
                croppedImage.Dispose();
                    
                if (resultImage != null)
                {
                    BitmapTools.DrawBitmapAtPos(resultImage, fullImage, activeBox.X, activeBox.Y);
                    resultImage.Dispose();
                    finished(fullImage);
                }
            }
            catch (Exception ee)
            {
                StableDiffusionClient.Log.WriteLine(ee.ToString());
            }
            finally
            {
                InProcess = false;
            }
        });
    }

    public void UpdateState(SmartPictureBox pb, ToolStripButton btRemoveBackground)
    {
        btRemoveBackground.Enabled = !InProcess && pb.Image != null && !pb.HasMask && isPortOpen;

        btRemoveBackground.Text = InProcess ? "PROCESSING" 
                                  : isPortOpen ? "Remove background" 
                                  : RemBgProcess.Loading ? "LOADING..." 
                                  : "ERROR";
    }
}
