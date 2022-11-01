using AiPainter.Helpers;
using System.Drawing.Imaging;
using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class StableDiffusionPanel : UserControl
    {
        private SmartPictureBox pictureBox = null!;
        private readonly Random random = new();

        public bool InProcess;

        public StableDiffusionPanel()
        {
            InitializeComponent();
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            if (InProcess)
            {
                StableDiffusionClient.Cancel();
                InProcess = false;
                return;
            }

            if (tbPrompt.Text.Trim() == "") { tbPrompt.Focus(); return; }

            InProcess = true;

            pbIterations.Maximum = (int)numIterations.Value;
            pbIterations.Value = 0;
            pbIterations.CustomText = "0 / " + (int)numIterations.Value;
            pbIterations.Refresh();

            pbSteps.Value = 0;
            pbSteps.Maximum = (int)numSteps.Value;
            pbSteps.CustomText = "0 / " + (int)numSteps.Value;
            pbSteps.Refresh();

            if (!cbUseInitImage.Checked)
            {
                generate(null, _ => InProcess = pbIterations.Value < pbIterations.Maximum);
            }
            else
            {
                var activeBox = pictureBox.ActiveBox;
                var originalImage = BitmapTools.Clone(pictureBox.Image!);
                using var croppedMaskedImage = pictureBox.GetMaskedImageCropped(Color.Black, 0);
                using var image512 = BitmapTools.GetResized(croppedMaskedImage, 512, 512);
                generate(image512, resultFilePath =>
                {
                    try
                    {
                        using var resultImage = BitmapTools.Load(resultFilePath);
                        using var resultImageResized = BitmapTools.GetResized(resultImage, activeBox.Width, activeBox.Height)!;
                        using var tempOriginalImage = BitmapTools.Clone(originalImage);
                        BitmapTools.DrawBitmapAtPos(resultImageResized, tempOriginalImage, activeBox.X, activeBox.Y);
                        tempOriginalImage.Save(resultFilePath, ImageFormat.Png);
                    }
                    catch (Exception ee)
                    {
                        StableDiffusionClient.Log.WriteLine(ee.ToString());
                    }

                    InProcess = pbIterations.Value < pbIterations.Maximum;
                    if (!InProcess) originalImage.Dispose();
                });
            }
        }

        private void generate(Bitmap? image, Action<string> onGenerated)
        {
            var parameters = new SdGenerationRequest
            {
                prompt = tbPrompt.Text.Trim() != "" ? tbPrompt.Text.Trim() : "",
                cfg_scale = numCfgScale.Value,
                n_iter = (int)numIterations.Value,
                seed = tbSeed.Text.Trim() == "" 
                           ? -1
                           : long.Parse(tbSeed.Text.Trim()),
                steps = (int)numSteps.Value,
                //strength = numImg2img.Value,
                //init_img = image != null ? BitmapTools.GetBase64String(image) : null,
            };

            StableDiffusionClient.txt2img(parameters,
                onProgress: ev =>
                {
                    Invoke(() =>
                    {
                        pbSteps.Value = ev.state.sampling_step;
                        pbSteps.CustomText = pbSteps.Value + " / " + parameters.steps;
                        pbSteps.Refresh();
                    });
                },

                onFinish: _ =>
                {
                    Invoke(() =>
                    {
                        pbSteps.Value = 0;
                        pbSteps.CustomText = "";
                        InProcess = false;
                    });
                },

                onSuccess: ev =>
                {
                    Invoke(() => {
                        pbSteps.Value = 0;
                        pbSteps.Refresh();
                        pbIterations.Value++;
                        pbIterations.CustomText = pbIterations.Value + " / " + parameters.n_iter;
                        pbIterations.Refresh();

                        var resultFilePath = Path.Combine(Program.Config.OutputFolder, ev.infoParsed.seed + ".png");
                        File.WriteAllBytes(resultFilePath, Convert.FromBase64String(ev.images[0].Split(",").Last()));
                        onGenerated(resultFilePath);
                    });
                }
            );
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            var parameters = new SdGenerationRequest();
            //numImg2img.Value = generationParameters.strength; // TODO
            numCfgScale.Value = parameters.cfg_scale;
            numGfpGan.Value = parameters.restore_faces ? 1 : 0; // TODO
            numSteps.Value = parameters.steps;
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

            btGenerate.Text =        InProcess ? "CANCEL" 
                                  : isPortOpen ? "Generate" 
              : StableDiffusionProcess.Loading ? "LOADING..." 
                                               : "ERROR";

            tbPrompt.Enabled = !InProcess;

            btGenerate.Enabled = isPortOpen;
        }
    }
}
