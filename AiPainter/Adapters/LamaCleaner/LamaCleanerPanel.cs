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

            var activeBoxX = pictureBox!.ActiveBoxX;
            var activeBoxY = pictureBox!.ActiveBoxY;
            var activeBoxW = pictureBox!.ActiveBoxW;
            var activeBoxH = pictureBox!.ActiveBoxH;

            var fullImage = pictureBox!.Image!;
            var fullMask = pictureBox!.GetMaskAsWhiteOnBlack()!;

            var croppedImage = BitmapTools.GetCropped(fullImage, -activeBoxX, -activeBoxY, activeBoxW, activeBoxH, Color.Black)!;
            var croppedMask  = BitmapTools.GetCropped(fullMask,  -activeBoxX, -activeBoxY, activeBoxW, activeBoxH, Color.Black)!;
            
            Task.Run(() =>
            {
                try
                {
                    var resultImage = LamaCleanerClient.RunAsync(croppedImage, croppedMask).Result;
                    if (resultImage != null)
                    {
                        Invoke(() =>
                        {
                            BitmapTools.DrawBitmapAtPos(resultImage, fullImage, -activeBoxX, -activeBoxY);
                            //pictureBox.Image = resultImage;
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
