using AiPainter.Adapters.StableDiffusion;
using AiPainter.Helpers;
using System.Drawing.Imaging;

namespace AiPainter.Controls
{
    public partial class GenerationListItem : UserControl
    {
        private MainForm mainForm = null!;
        private StableDiffusionPanel sdPanel = null!;
        private SmartPictureBox pictureBox = null!;

        public bool InProcess;
        public int ImagesdInQueue => (int)numIterations.Value;

        private string checkpoint;
        private string negative;
        private decimal cfgScale;
        private long seed;
        
        private SdInpaintingFill inpaintingFill;
        private Rectangle activeBox;
        private Bitmap? originalImage;
        private Bitmap? croppedMask;

        private string? savedFilePath;
        private Primitive[] savedMask;
        
        private int lastIterations;

        private bool ignoreNumIterationsChange;

        public GenerationListItem()
        {
            InitializeComponent();
        }

        public void Init(StableDiffusionPanel sdPanel, SmartPictureBox pictureBox, MainForm mainForm)
        {
            this.mainForm = mainForm;
            this.sdPanel = sdPanel;
            this.pictureBox = pictureBox;

            checkpoint = ((ListItem)sdPanel.ddCheckpoint.SelectedItem).Value;
            tbPrompt.Text = sdPanel.tbPrompt.Text.Trim();
            negative = sdPanel.tbNegative.Text.Trim();
            cfgScale = sdPanel.numCfgScale.Value;
            seed = sdPanel.tbSeed.Text.Trim() != "" ? long.Parse(sdPanel.tbSeed.Text.Trim()) : -1;

            ignoreNumIterationsChange = true;
            numIterations.Value = sdPanel.numIterations.Value;
            ignoreNumIterationsChange = false;

            pbIterations.Maximum = (int)sdPanel.numIterations.Value;
            pbIterations.Value = 0;
            pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
            pbIterations.Refresh();
            
            pbSteps.Value = 0;
            pbSteps.Maximum = (int)sdPanel.numSteps.Value;
            pbSteps.CustomText = "";
            pbSteps.Refresh();

            lastIterations = (int)numIterations.Value;
            
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

            savedFilePath = mainForm.FilePath;
            savedMask = pictureBox.SaveMask();
        }

        public void Run()
        {
            InProcess = true;

            pbSteps.Value = 0;
            pbSteps.CustomText = pbSteps.Value + " / " + pbSteps.Maximum;
            pbSteps.Refresh();

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
            if (Program.Config.UseEmbeddedStableDiffusion && StableDiffusionProcess.Loading)
            {
                if (StableDiffusionProcess.ActiveCheckpoint != checkpoint)
                {
                    Invoke(() => pbSteps.CustomText = "Stopping...");
                    StableDiffusionProcess.Stop();
                    while (StableDiffusionProcess.IsReady())
                    {
                        if (await DelayTools.WaitForExitAsync(500) || IsDisposed) return;
                    }
                    
                    Invoke(() => pbSteps.CustomText = "Starting...");
                    StableDiffusionProcess.Start(checkpoint);
                }
            }

            if (!StableDiffusionProcess.IsReady())
            {
                Invoke(() => pbSteps.CustomText = "Waiting ready...");
                while (!StableDiffusionProcess.IsReady())
                {
                    if (await DelayTools.WaitForExitAsync(500) || IsDisposed) return;
                }
                if (await DelayTools.WaitForExitAsync(2000) || IsDisposed) return;
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
                    resultImage.Save(resultFilePath, ImageFormat.Png);
                    resultImage.Dispose();
                });
            }
            else
            {
                using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);
                using var image512 = BitmapTools.GetResized(croppedImage, 512, 512);
                using var mask512 = croppedMask != null ? BitmapTools.GetResized(croppedMask, 512, 512) : null;
                
                generate(image512, mask512, (resultImage, resultFilePath) =>
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
                });
            }
        }

        private void generate(Bitmap? initImage, Bitmap? maskImage, Action<Bitmap, string> processGeneratedImage)
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
                    onSuccess: ev => onImageGenerated(ev, processGeneratedImage)
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
                    onSuccess: ev => onImageGenerated(ev, processGeneratedImage)
                );
            }
        }

        private void onImageGenerated(SdGenerationResponse ev, Action<Bitmap, string> processGeneratedImage)
        {
            if (numIterations.Value > 0)
            {
                Invoke(() =>
                {
                    ignoreNumIterationsChange = true;
                    numIterations.Value--;
                    ignoreNumIterationsChange = false;
                    
                    lastIterations = (int)numIterations.Value;
                    
                    pbIterations.Value++;
                    pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
                    pbIterations.Refresh();
                });
                
                processGeneratedImage(BitmapTools.FromBase64(ev.images[0]), getDestImageFilePath(ev));
            }
            
            InProcess = false;
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

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (InProcess)
            {
                numIterations.Value = 0;
                Task.Run(StableDiffusionClient.Cancel);
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
                pictureBox.LoadMask(savedMask);
            }

            mainForm.FilePath = savedFilePath;
        }

        private void numIterations_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreNumIterationsChange) return;

            pbIterations.Maximum += (int)numIterations.Value - lastIterations;
            pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
            
            lastIterations = (int)numIterations.Value;
            
            if (numIterations.Value == 0 && InProcess)
            {
                Task.Run(StableDiffusionClient.Cancel);
                InProcess = false;
            }
        }

        private string getDestImageFilePath(SdGenerationResponse ev)
        {
            return Path.Combine(Program.Config.OutputFolder, ev.infoParsed.seed + ".png");
        }
    }
}
