﻿using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.RemBg
{
    public partial class RemBgPanel : UserControl
    {
        private SmartPictureBox pictureBox = null!;
        private bool isPortOpen;

        public bool InProcess;

        public RemBgPanel()
        {
            InitializeComponent();

            portCheckWorker.RunWorkerAsync();
        }

        private void btRemBgRemoveBackground_Click(object sender, EventArgs e)
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
                        Invoke(() =>
                        {
                            BitmapTools.DrawBitmapAtPos(resultImage, fullImage, activeBox.X, activeBox.Y);
                            resultImage.Dispose();
                            pictureBox.Image = fullImage;
                            pictureBox.ResetMask();
                            pictureBox.Refresh();
                        });
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

        public void UpdateState(SmartPictureBox pb)
        {
            pictureBox = pb;

            btRemoveBackground.Enabled = !InProcess && pb.Image != null && !pb.HasMask && isPortOpen;

            btRemoveBackground.Text = InProcess ? "PROCESSING" 
                                   : isPortOpen ? "Remove background" 
                         : RemBgProcess.Loading ? "LOADING..." 
                                                : "ERROR";
        }

        private void portCheckWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (Application.OpenForms.Count == 0) Thread.Sleep(200);
            
            while (!DelayTools.WaitForExit(1000))
            {
                isPortOpen = RemBgProcess.IsReady();
            }
        }
    }
}
