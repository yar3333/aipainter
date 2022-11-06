using AiPainter.Adapters.StableDiffusion;
using AiPainter.Helpers;
using System.Drawing.Imaging;

namespace AiPainter.Controls
{
    public partial class GenerationListItem : UserControl
    {
        private StableDiffusionPanel sdPanel = null!;
        private SmartPictureBox pictureBox = null!;

        public GenerationState State { get; private set; } = GenerationState.WAITING;

        private string checkpoint;
        private string negative;
        private decimal cfgScale;
        private long seed;
        
        private SdInpaintingFill inpaintingFill;
        private Rectangle activeBox;
        private Bitmap? originalImage;
        private Bitmap? croppedMask;

        private bool wantCancel;

        public GenerationListItem()
        {
            InitializeComponent();
        }

        public void Init(StableDiffusionPanel sdPanel, SmartPictureBox pictureBox)
        {
            this.sdPanel = sdPanel;
            this.pictureBox = pictureBox;

            checkpoint = ((ListItem)sdPanel.ddCheckpoint.SelectedItem).Value;
            tbPrompt.Text = sdPanel.tbPrompt.Text.Trim();
            negative = sdPanel.tbNegative.Text.Trim();
            cfgScale = sdPanel.numCfgScale.Value;
            seed = sdPanel.tbSeed.Text.Trim() != "" ? long.Parse(sdPanel.tbSeed.Text.Trim()) : -1;

            numIterations.Value = sdPanel.numIterations.Value;
            //numSteps.Value = sdPanel.numSteps.Value;

            pbIterations.Maximum = (int)sdPanel.numIterations.Value;
            pbIterations.Value = 0;
            pbIterations.CustomText = "0 / " + pbIterations.Maximum;
            pbIterations.Refresh();
            
            pbSteps.Value = 0;
            pbSteps.Maximum = (int)sdPanel.numSteps.Value;
            pbSteps.CustomText = "";
            pbSteps.Refresh();
            
            if (sdPanel.cbUseInitImage.Checked)
            {
                inpaintingFill = Enum.Parse<SdInpaintingFill>((string)sdPanel.ddInpaintingFill.SelectedItem);
                activeBox = pictureBox.ActiveBox;
                originalImage = BitmapTools.Clone(pictureBox.Image!);
                croppedMask = pictureBox.GetMaskCropped(Color.Black, Color.White);
            }

            toolTip.SetToolTip
            (
                tbPrompt, 
                "Positive:\n" + tbPrompt.Text
          + "\n\nNegative:\n" + negative
            );
        }

        public void Run()
        {
            State = pbIterations.Value < pbIterations.Maximum ? GenerationState.IN_PROCESS : GenerationState.FULLY_FINISHED;
            if (State == GenerationState.FULLY_FINISHED) return;

            Task.Run(() =>
            {
                try
                {
                    runInTask().Wait();
                }
                catch (Exception e)
                {
                    Program.Log.WriteLine(e.ToString());
                }
            });
        }

        private async Task runInTask()
        {
            if (Program.Config.UseEmbeddedStableDiffusion)
            {
                if (StableDiffusionProcess.ActiveCheckpoint != checkpoint)
                {
                    Invoke(() => pbSteps.CustomText = "Stopping...");
                    StableDiffusionProcess.Stop();
                    while (StableDiffusionProcess.IsReady()) if (await DelayTools.WaitForExitAsync(500)) return;
                    
                    Invoke(() => pbSteps.CustomText = "Starting...");
                    StableDiffusionProcess.Start(checkpoint);
                }
            }

            if (!StableDiffusionProcess.IsReady())
            {
                Invoke(() => pbSteps.CustomText = "Waiting ready...");
                while (!StableDiffusionProcess.IsReady()) if (await DelayTools.WaitForExitAsync(500)) return;
                if (await DelayTools.WaitForExitAsync(2000)) return;
            }

            Invoke(() =>
            {
                pbSteps.Value = 0;
                pbSteps.CustomText = "0 / " + pbSteps.Maximum;
                pbSteps.Refresh();
            });

            if (originalImage == null)
            {
                generate(null, null, (resultImage, resultFilePath) => 
                {
                    if (!wantCancel)
                    {
                        resultImage.Save(resultFilePath, ImageFormat.Png);
                        resultImage.Dispose();

                        State = GenerationState.PART_FINISHED;
                    }
                    else
                    {
                        wantCancel = false;
                        State = GenerationState.FULLY_FINISHED;
                    }
                });
            }
            else
            {
                using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);
                using var image512 = BitmapTools.GetResized(croppedImage, 512, 512);
                using var mask512 = croppedMask != null ? BitmapTools.GetResized(croppedMask, 512, 512) : null;
                
                generate(image512, mask512, (resultImage, resultFilePath) =>
                {
                    if (!wantCancel)
                    {
                        try
                        {
                            using var resultImageResized = BitmapTools.GetResized(resultImage, activeBox.Width, activeBox.Height)!;
                            resultImage.Dispose();
                        
                            using var tempOriginalImage = BitmapTools.Clone(originalImage);
                            BitmapTools.DrawBitmapAtPos(resultImageResized, tempOriginalImage, activeBox.X, activeBox.Y);
                            tempOriginalImage.Save(resultFilePath, ImageFormat.Png);
                        }
                        catch (Exception ee)
                        {
                            StableDiffusionClient.Log.WriteLine(ee.ToString());
                        }

                        State = GenerationState.PART_FINISHED;
                    }
                    else
                    {
                        wantCancel = false;
                        State = GenerationState.FULLY_FINISHED;
                    }
                });
            }
        }

        private void generate(Bitmap? initImage, Bitmap? maskImage, Action<Bitmap, string> onGenerated)
        {
            if (initImage == null)
            {
                var parameters = new SdGenerationRequest
                {
                    prompt = tbPrompt.Text,
                    negative_prompt = negative,
                    cfg_scale = cfgScale,
                    seed = seed,
                    steps = pbSteps.Maximum,
                };

                StableDiffusionClient.txt2img
                (
                    parameters,
                    onProgress: onProgress,
                    onSuccess: ev => onSuccess(parameters, ev, onGenerated)
                );
            }
            else
            {
                var parameters = new SdInpaintRequest
                {
                    prompt = tbPrompt.Text,
                    negative_prompt = negative,
                    cfg_scale = cfgScale,
                    seed = seed,
                    steps = pbSteps.Maximum,
                    init_images = new[] { BitmapTools.GetBase64String(initImage) },
                    mask = maskImage != null ? BitmapTools.GetBase64String(maskImage) : null,

                    inpainting_fill = inpaintingFill,
                };

                StableDiffusionClient.img2img
                (
                    parameters,
                    onProgress: onProgress,
                    onSuccess: ev => onSuccess(parameters, ev, onGenerated)
                );
            }
        }

        private void onProgress(SdGenerationProgess ev)
        {
            Invoke(() =>
            {
                pbSteps.Value = ev.state.sampling_step;
                pbSteps.CustomText = pbSteps.Value + " / " + pbSteps.Maximum;
                pbSteps.Refresh();
            });
        }

        private void onSuccess(SdBaseGenerationRequest parameters, SdGenerationResponse ev, Action<Bitmap, string> onGenerated)
        {
            Invoke(() => {
                pbSteps.Value = 0;
                pbSteps.Refresh();
                pbIterations.Value++;
                pbIterations.CustomText = pbIterations.Value + " / " + parameters.n_iter;
                pbIterations.Refresh();

                var resultFilePath = Path.Combine(Program.Config.OutputFolder, ev.infoParsed.seed + ".png");
                var resultBitmap = BitmapTools.FromBase64(ev.images[0]);
                
                onGenerated(resultBitmap, resultFilePath);
            });
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (State == GenerationState.IN_PROCESS)
            {
                wantCancel = true;
                StableDiffusionClient.Cancel();
            }
            Parent = null;
            Dispose();
        }

        private void btLoadParamsBackToPanel_Click(object sender, EventArgs e)
        {
            sdPanel.numSteps.Value = pbSteps.Maximum;
            sdPanel.tbPrompt.Text = tbPrompt.Text;
            sdPanel.tbNegative.Text = negative;
            sdPanel.numCfgScale.Value = cfgScale;
            sdPanel.tbSeed.Text = seed.ToString();

            sdPanel.numIterations.Value = numIterations.Value;

            sdPanel.numIterations.Value = pbIterations.Maximum;

            sdPanel.cbUseInitImage.Checked = originalImage != null;

            if (originalImage != null)
            {
                pictureBox.ActiveBox = activeBox;
                sdPanel.ddInpaintingFill.SelectedItem = inpaintingFill.ToString();
                pictureBox.Image = BitmapTools.Clone(originalImage);
            }
        }

        private void numIterations_ValueChanged(object sender, EventArgs e)
        {
            pbIterations.Maximum = (int)numIterations.Value;
        }
    }
}
