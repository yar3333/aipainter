using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.LamaCleaner
{
    public partial class LamaCleanerPanel : UserControl
    {
        private SmartPictureBox? pictureBox;

        public bool InProcess;
        
        public LamaCleanerPanel()
        {
            InitializeComponent();
        }

        private void btInpaint_Click(object sender, EventArgs e)
        {
            InProcess = true;

            var activeBox = pictureBox!.ActiveBox;

            var fullImage = pictureBox!.Image!;
            var croppedImage = BitmapTools.GetCropped(fullImage, -activeBox.X, -activeBox.Y, activeBox.Width, activeBox.Height, Color.Black)!;
            var croppedMask = pictureBox!.GetMaskCropped(-activeBox.X, -activeBox.Y, activeBox.Width, activeBox.Height, Color.Black, Color.White)!;
            
            Task.Run(() =>
            {
                try
                {
                    var resultImage = LamaCleanerClient.RunAsync(croppedImage, croppedMask).Result;
                    if (resultImage != null)
                    {
                        Invoke(() =>
                        {
                            BitmapTools.DrawBitmapAtPos(resultImage, fullImage, -activeBox.X, -activeBox.Y);
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

        public void UpdateState(SmartPictureBox pb, bool isPortOpen)
        {
            pictureBox = pb;

            btInpaint.Enabled = !InProcess && pb.Image != null && pb.HasMask && isPortOpen;
            btInpaint.Text = !InProcess ? "Clean masked area" : "PROCESSING";
        }
    }
}
