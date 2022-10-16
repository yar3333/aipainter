using AiPainter.Controls;

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

            Task.Run(() =>
            {
                try
                {
                    var result = RemBgClient.RunAsync(pictureBox.GetImageWithMaskToTransparent()).Result;
                    Invoke(() =>
                    {
                        pictureBox.Image = result;
                        pictureBox.ResetMask();
                    });
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

            btRemoveBackground.Enabled = pb.Image != null && !pb.HasMask;
        }
    }
}
