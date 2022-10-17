using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.RemBg
{
    public partial class RemBgPanel : UserControl
    {
        private SmartPictureBox? pictureBox;

        public bool InProcess;

        public RemBgPanel()
        {
            InitializeComponent();
        }

        private void btRemBgRemoveBackground_Click(object sender, EventArgs e)
        {
            InProcess = true;

            var image = BitmapTools.Clone(pictureBox!.Image)!;

            Task.Run(() =>
            {
                try
                {
                    var result = RemBgClient.RunAsync(image).Result;
                    Invoke(() =>
                    {
                        pictureBox.Image = result;
                        pictureBox.ResetMask();
                    });
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

        public void UpdateState(SmartPictureBox pb)
        {
            pictureBox = pb;

            btRemoveBackground.Enabled = !InProcess && pb.Image != null && !pb.HasMask;
        }
    }
}
