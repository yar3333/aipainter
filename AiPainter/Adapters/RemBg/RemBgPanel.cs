using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.RemBg
{
    public partial class RemBgPanel : UserControl
    {
        private SmartPictureBox pictureBox = null!;

        public bool InProcess;

        public RemBgPanel()
        {
            InitializeComponent();
        }

        private void btRemBgRemoveBackground_Click(object sender, EventArgs e)
        {
            InProcess = true;

            var activeBox = pictureBox.ActiveBox;

            var fullImage = pictureBox.Image!;
            var croppedImage = BitmapTools.GetCropped(fullImage, activeBox, Color.Black);

            Task.Run(() =>
            {
                try
                {
                    var resultImage = RemBgClient.RunAsync(croppedImage).Result;
                    croppedImage.Dispose();
                    
                    if (resultImage != null)
                    {
                        Invoke(() =>
                        {
                            BitmapTools.DrawBitmapAtPos(resultImage, fullImage, activeBox.X, activeBox.Y);
                            pictureBox.ResetMask();
                            pictureBox.Refresh();
                        });
                    }
                }
                catch (Exception ee)
                {
                    RemBgClient.Log.WriteLine(ee.ToString());
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

            btRemoveBackground.Enabled = !InProcess && pb.Image != null && !pb.HasMask && isPortOpen;
            btRemoveBackground.Text = !InProcess ? "Remove background" : "PROCESSING";
        }
    }
}
