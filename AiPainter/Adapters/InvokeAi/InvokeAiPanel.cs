using AiPainter.Helpers;
using System.Drawing.Imaging;
using AiPainter.Controls;

namespace AiPainter.Adapters.InvokeAi
{
    public partial class InvokeAiPanel : UserControl
    {
        private SmartPictureBox pictureBox = null!;
        private readonly Random random = new();

        public bool InProcess;

        public InvokeAiPanel()
        {
            InitializeComponent();
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            if (InProcess)
            {
                InvokeAiClient.Cancel();
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
                        InvokeAiClient.Log.WriteLine(ee.ToString());
                    }

                    InProcess = pbIterations.Value < pbIterations.Maximum;
                    if (!InProcess) originalImage.Dispose();
                });
            }
        }

        private void generate(Bitmap? image, Action<string> onGenerated)
        {
            var generationParameters = new AiGenerationParameters
            {
                prompt = tbPrompt.Text.Trim() != "" ? tbPrompt.Text.Trim() : "",
                cfg_scale = numCfgScale.Value,
                iterations = (int)numIterations.Value,
                seed = tbSeed.Text.Trim() == "" 
                           ? (uint)random.NextInt64(4294967295) 
                           : uint.Parse(tbSeed.Text.Trim()),
                steps = (int)numSteps.Value,
                strength = numImg2img.Value,
                init_img = image != null ? BitmapTools.GetBase64String(image) : null,
            };

            var gfpganParameters = new AiGfpganParameters
            {
                strength = numGfpGan.Value
            };

            InvokeAiClient.Generate(generationParameters, gfpganParameters, 
                onProgressUpdate: ev =>
                {
                    Invoke(() =>
                    {
                        pbSteps.Value = ev.currentStep;
                        pbSteps.CustomText = pbSteps.Value + " / " + generationParameters.steps;
                        pbSteps.Refresh();
                    });
                },

                onProcessingCanceled: () =>
                {
                    pbSteps.Value = 0;
                    pbSteps.CustomText = "";
                    InProcess = false;
                },

                onResult: ev =>
                {
                    pbSteps.Value = 0;
                    pbSteps.Refresh();
                    pbIterations.Value++;
                    pbIterations.CustomText = pbIterations.Value + " / " + generationParameters.iterations;
                    pbIterations.Refresh();

                    var resultFilePath = Path.Combine(Program.Config.InvokeAiOutputFolderPath, ev.url.Split('/', '\\').Last());
                    Task.Run(async () =>
                    {
                        while (!File.Exists(resultFilePath))
                        {
                            if (!InProcess) return;
                            await Task.Delay(500);
                        }
                        await Task.Delay(1000);
                        onGenerated(resultFilePath);
                    });
                }
            );
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            var generationParameters = new AiGenerationParameters();
            var gfpganParameters = new AiGfpganParameters();
            numImg2img.Value = generationParameters.strength;
            numCfgScale.Value = generationParameters.cfg_scale;
            numGfpGan.Value = gfpganParameters.strength;
            numSteps.Value = generationParameters.steps;
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

            btGenerate.Text = InProcess ? "CANCEL" 
                           : isPortOpen ? "Generate" 
              : InvokeAiProcess.Loading ? "LOADING..." 
                                        : "ERROR";

            tbPrompt.Enabled = !InProcess;

            btGenerate.Enabled = isPortOpen;
        }
    }
}
