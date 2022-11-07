using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.LamaCleaner
{
    public partial class LamaCleanerPanel : UserControl
    {
        private SmartPictureBox pictureBox = null!;
        private bool isPortOpen;

        public bool InProcess;
        
        public LamaCleanerPanel()
        {
            InitializeComponent();
            
            portCheckWorker.RunWorkerAsync();
        }

        private void btInpaint_Click(object sender, EventArgs e)
        {
            InProcess = true;

            var activeBox = pictureBox.ActiveBox;

            var fullImage = pictureBox.Image!;

            var croppedImage = pictureBox.GetMaskedImageCropped(Color.Black, 255);
            var croppedMask = pictureBox.GetMaskCropped(Color.Black, Color.White);
            
            Task.Run(() =>
            {
                try
                {
                    var resultImage = LamaCleanerClient.RunAsync(croppedImage, croppedMask).Result;
                    croppedMask.Dispose();
                    croppedImage.Dispose();
                    
                    if (resultImage != null)
                    {
                        Invoke(() =>
                        {
                            BitmapTools.DrawBitmapAtPos(resultImage, fullImage, activeBox.X, activeBox.Y);
                            resultImage.Dispose();
                            pictureBox.ResetMask();
                            pictureBox.Refresh();
                        });
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

        public void UpdateState(SmartPictureBox pb)
        {
            pictureBox = pb;

            btInpaint.Enabled = !InProcess && pb.Image != null && pb.HasMask && isPortOpen;

            btInpaint.Text =       InProcess ? "PROCESSING" 
                                : isPortOpen ? "Clean masked area" 
                : LamaCleanerProcess.Loading ? "LOADING..." 
                                             : "ERROR";
        }

        private void portCheckWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (Application.OpenForms.Count == 0) Thread.Sleep(200);

            while (!DelayTools.WaitForExit(1000))
            {
                isPortOpen = LamaCleanerProcess.IsReady();
            }
        }
    }
}
