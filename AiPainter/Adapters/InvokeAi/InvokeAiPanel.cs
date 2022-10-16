using AiPainter.Helpers;
using System.Drawing.Imaging;
using AiPainter.Controls;

namespace AiPainter.Adapters.InvokeAi
{
    public partial class InvokeAiPanel : UserControl
    {
        private SmartPictureBox? pictureBox;

        public bool InProcess;

        public InvokeAiPanel()
        {
            InitializeComponent();
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            if (InProcess)
            {
                btGenerate.Enabled = false;
                InvokeAiClient.Cancel();
                return;
            }

            if (tbPrompt.Text.Trim() == "")
            {
                tbPrompt.Focus();
                return;
            }

            InProcess = true;

            var dx = pictureBox.ViewDeltaX;
            var dy = pictureBox.ViewDeltaY;

            var wasCropped = false;

            var activeImage = cbUseInitImage.Checked ? pictureBox.GetImageWithMaskToTransparent() : null;
            var croppedImage = activeImage != null
                                   ? pictureBox.GetImageWithMaskToTransparentCroppedToViewport(out wasCropped)
                                   : null;

            var sdImage = new AiImageInfo
            {
                prompt = tbPrompt.Text.Trim() != "" ? tbPrompt.Text.Trim() : null,
                cfg_scale = numCfgScale.Value,
                gfpgan_strength = numGfpGan.Value,
                iterations = (int)numIterations.Value,
                seed = tbSeed.Text.Trim() == "" ? -1 : long.Parse(tbSeed.Text.Trim()),
                steps = (int)numSteps.Value,
                strength = numImg2img.Value,
                initimg = BitmapTools.GetBase64String(croppedImage),
            };

            var oldGenerateText = btGenerate.Text;
            btGenerate.Text = "CANCEL";
            tbPrompt.Enabled = false;
            
            pbIterations.Maximum = sdImage.iterations;
            pbIterations.Value = 0;

            pbSteps.Value = 0;
            pbSteps.Maximum = sdImage.steps;

            InvokeAiClient.Generate(sdImage, progress =>
            {
                Invoke(() =>
                {
                    switch (progress.@event)
                    {
                        case "step":
                            pbSteps.Value = progress.step ?? 0;
                            break;                    
                    
                        case "result":
                            pbSteps.Value = 0;
                            pbIterations.Value++;
                            if (pbIterations.Value == pbIterations.Maximum)
                            {
                                btGenerate.Text = oldGenerateText;
                                btGenerate.Enabled = true;
                                tbPrompt.Enabled = true;

                                var fName = progress.url.Split('/', '\\').Last();
                                var fPath = Path.Combine(Program.Config.InvokeAiOutputFolderPath, fName);

                                _ = Task.Run(async () =>
                                {
                                    while (!File.Exists(fPath)) await Task.Delay(500);
                                    await Task.Delay(1000);
                                    var bmp = BitmapTools.Load(fPath)!;
                                    if (wasCropped)
                                    {
                                        using var g = Graphics.FromImage(activeImage);
                                        g.DrawImageUnscaled(bmp, -dx, -dy);
                                        activeImage.Save(fPath, ImageFormat.Png);
                                    }
                                    InProcess = false;
                                });
                            }
                            break;

                        case "canceled":
                            pbSteps.Value = 0;
                            btGenerate.Text = oldGenerateText;
                            btGenerate.Enabled = true;
                            tbPrompt.Enabled = true;
                            InProcess = false;
                            break;
                    }
                });
            });

        }

        private void btReset_Click(object sender, EventArgs e)
        {
            var sdImage = new AiImageInfo();
            numImg2img.Value = sdImage.strength;
            numCfgScale.Value = sdImage.cfg_scale;
            numGfpGan.Value = sdImage.gfpgan_strength;
            numSteps.Value = sdImage.steps;
        }

        public void UpdateState(SmartPictureBox pb)
        {
            pictureBox = pb;

            if (pb.Image != null && pb.HasMask)
            {
                cbUseInitImage.Enabled = false;
                cbUseInitImage.Checked = true;
                numImg2img.Enabled = true;
            }
            else if (pb.Image == null)
            {
                cbUseInitImage.Enabled = false;
                cbUseInitImage.Checked = false;
                numImg2img.Enabled = false;
            }
            else
            {
                cbUseInitImage.Enabled = true;
                numImg2img.Enabled = true;
            }
        }
    }
}
