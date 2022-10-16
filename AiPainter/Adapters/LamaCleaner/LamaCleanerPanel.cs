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
            if (pictureBox.Image == null) return;

            btInpaint.Enabled = false;
            Task.Run(() =>
            {
                var result = LamaCleanerClient.RunAsync(pictureBox.GetImageWithMaskToTransparent()).Result;
                Invoke(() =>
                {
                    btInpaint.Enabled = true;
                    pictureBox.Image = result;
                    pictureBox.ResetMask();
                });
            });
        }

        public void UpdateState(SmartPictureBox pb)
        {
            pictureBox = pb;

            btInpaint.Enabled = pb.Image != null && pb.HasMask;
        }
    }
}
