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
            if (InProcess) { InvokeAiClient.Cancel(); return; }

            if (tbPrompt.Text.Trim() == "") { tbPrompt.Focus(); return; }

            InProcess = true;

            var dx = Math.Min(0, pictureBox!.RedBoxDx);
            var dy = Math.Min(0, pictureBox!.RedBoxDy);

            var wasCropped = false;

            var activeImage = cbUseInitImage.Checked ? pictureBox.GetMaskedImage(0) : null;
            var croppedImage = activeImage != null
                                   ? pictureBox.GetMaskedImageCroppedToViewport(0, out wasCropped)
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

            pbIterations.Maximum = sdImage.iterations;
            pbIterations.Value = 0;
            pbIterations.CustomText = "0 / " + sdImage.iterations;
            pbIterations.Refresh();

            pbSteps.Value = 0;
            pbSteps.Maximum = sdImage.steps;
            pbSteps.CustomText = "0 / " + sdImage.steps;
            pbSteps.Refresh();

            InvokeAiClient.Generate(sdImage, progress =>
            {
                Invoke(() =>
                {
                    switch (progress.@event)
                    {
                        case "step":
                            pbSteps.Value = progress.step ?? 0;
                            pbSteps.CustomText = pbSteps.Value + " / " + sdImage.steps;
                            pbSteps.Refresh();
                            break;                    
                    
                        case "result":
                            pbSteps.Value = 0;
                            pbSteps.Refresh();
                            pbIterations.Value++;
                            pbIterations.CustomText = pbIterations.Value + " / " + sdImage.iterations;
                            pbIterations.Refresh();

                            fixResultImage(wasCropped, progress.url!, activeImage!, dx, dy, () =>
                            {
                                InProcess = pbIterations.Value < pbIterations.Maximum;
                            });
                            break;

                        case "canceled":
                            pbSteps.Value = 0;
                            pbSteps.CustomText = "";
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

        public void UpdateState(SmartPictureBox pb, bool isPortOpen)
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
                numImg2img.Enabled = cbUseInitImage.Checked;
            }

            btGenerate.Text = InProcess ? "CANCEL" : "Generate";
            tbPrompt.Enabled = !InProcess;

            btGenerate.Enabled = isPortOpen;
        }

        private void fixResultImage(bool wasCropped, string url, Bitmap activeImage, int dx, int dy, Action onFinish)
        {
            if (wasCropped)
            {
                var fName = url.Split('/', '\\').Last();
                var fPath = Path.Combine(Program.Config.InvokeAiOutputFolderPath, fName);

                _ = Task.Run(async () =>
                {
                    while (!File.Exists(fPath)) await Task.Delay(500);
                    await Task.Delay(1000);
                    using var bmp = BitmapTools.Load(fPath)!;

                    using var image = BitmapTools.Clone(activeImage)!;
                    using var g = Graphics.FromImage(image);
                    g.DrawImageUnscaled(bmp, -dx, -dy);
                    image.Save(fPath, ImageFormat.Png);

                    onFinish();
                });
            }
            else
            {
                onFinish();
            }
        }
    }
}
