using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.LamaCleaner;

class LamaCleanerManager
{
    private bool isPortOpen;

    public bool InProcess;

    public LamaCleanerManager()
    {
        Task.Run(() =>
        {
            while (Application.OpenForms.Count == 0) Thread.Sleep(200);
            while (!DelayTools.WaitForExit(1000))
            {
                isPortOpen = LamaCleanerProcess.IsReady();
            }
        });
    }

    public void Run(SmartPictureBox pictureBox, Action<Bitmap?> finished)
    {
        pictureBox.HistoryAddCurrentState();

        InProcess = true;

        var activeBox = pictureBox.ActiveBox;

        var fullImage = BitmapTools.Clone(pictureBox.Image!);

        var croppedImage = pictureBox.GetMaskedImageCropped(Color.Black, 255);
        var croppedMask = pictureBox.GetMaskCropped(Color.Black, Color.White);
            
        Task.Run(() =>
        {
            try
            {
                var resultImage = LamaCleanerClient.RunAsync(croppedImage, croppedMask!).Result;
                croppedMask!.Dispose();
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
                LamaCleanerClient.Log.WriteLine(ee.ToString());
            }
            finally
            {
                InProcess = false;
            }
        });
    }

    public void UpdateState(SmartPictureBox pictureBox, ToolStripButton btInpaint)
    {

        btInpaint.Enabled = !InProcess && pictureBox.Image != null && pictureBox.HasMask && isPortOpen;

        btInpaint.Text = InProcess ? "PROCESSING" 
                               : isPortOpen ? "Clean masked area" 
                               : LamaCleanerProcess.Loading ? "LOADING..." 
                               : "ERROR";
    }
}
