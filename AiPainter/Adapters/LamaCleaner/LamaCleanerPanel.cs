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

            var image = pictureBox!.GetMaskedImage(255)!;
            var mask = pictureBox!.GetMaskAsWhiteOnBlack()!;
            
            Task.Run(() =>
            {
                try
                {
                    var result = LamaCleanerClient.RunAsync(image, mask).Result;
                    Invoke(() =>
                    {
                        pictureBox.Image = result;
                        pictureBox.ResetMask();
                    });
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
