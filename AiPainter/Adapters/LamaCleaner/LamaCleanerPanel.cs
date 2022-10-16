using AiPainter.Controls;

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
            
            Task.Run(() =>
            {
                try
                {
                    var result = LamaCleanerClient.RunAsync(pictureBox!.Image!, pictureBox!.GetMaskAsWhiteOnBlack()!).Result;
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

        public void UpdateState(SmartPictureBox pb)
        {
            pictureBox = pb;

            btInpaint.Enabled = !InProcess && pb.Image != null && pb.HasMask;
        }
    }
}
