using AiPainter.Adapters.StableDiffusion;
using AiPainter.Helpers;
using System.Drawing.Imaging;
using System.Text.Json;

namespace AiPainter.Controls
{
    public partial class GenerationListItem : UserControl
    {
        private MainForm mainForm = null!;
        private StableDiffusionPanel sdPanel = null!;
        private SmartPictureBox pictureBox = null!;
        private SdGenerationParameters sdGenerationParameters = null!;

        public bool InProcess;
        public int ImagesdInQueue => (int)numIterations.Value;

        private Rectangle activeBox;
        private Bitmap? originalImage;
        private Bitmap? croppedMask;
        private SdInpaintingFill inpaintingFill;

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

            sdGenerationParameters = new SdGenerationParameters
            {
                checkpoint = ((ListItem)sdPanel.ddCheckpoint.SelectedItem).Value,
                prompt = sdPanel.tbPrompt.Text.Trim(),
                negative = sdPanel.tbNegative.Text.Trim(),
                steps = (int)sdPanel.numSteps.Value,
                cfgScale = sdPanel.numCfgScale.Value,
                seed = sdPanel.tbSeed.Text.Trim() != "" ? long.Parse(sdPanel.tbSeed.Text.Trim()) : -1,
                modifiers = sdPanel.Modifiers,
            };

            tbPrompt.Text = sdGenerationParameters.prompt;

            ignoreNumIterationsChange = true;
            numIterations.Value = sdPanel.numIterations.Value;
            ignoreNumIterationsChange = false;

            pbIterations.Maximum = (int)sdPanel.numIterations.Value;
            pbIterations.Value = 0;
            pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
            pbIterations.Refresh();
            
            pbSteps.Value = 0;
            pbSteps.Maximum = sdGenerationParameters.steps;
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
                "Positive prompt:\n" + sdGenerationParameters.prompt + "\n\n"
              + "Negative prompt:\n" + sdGenerationParameters.negative
            );

            savedFilePath = mainForm.FilePath;
            savedMask = pictureBox.SaveMask();
        }

        public void Run()
        {
            InProcess = true;

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
                if (StableDiffusionProcess.ActiveCheckpoint != sdGenerationParameters.checkpoint)
                {
                    Invoke(() => pbSteps.CustomText = "Stopping...");
                    StableDiffusionProcess.Stop();
                    while (StableDiffusionProcess.IsReady())
                    {
                        if (await DelayTools.WaitForExitAsync(500) || IsDisposed) return;
                    }
                    
                    Invoke(() => pbSteps.CustomText = "Starting...");
                    StableDiffusionProcess.Start(sdGenerationParameters.checkpoint);
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
                generate(null, null, (resultImage, resultFilePath, seed) => 
                {
                    resultImage.Save(resultFilePath, ImageFormat.Png);
                    resultImage.Dispose();

                    var t = sdGenerationParameters.ShallowCopy();
                    t.seed = seed;
                    File.WriteAllText(Path.Join(Path.GetDirectoryName(resultFilePath), Path.GetFileNameWithoutExtension(resultFilePath)) + ".json", JsonSerializer.Serialize(t, new JsonSerializerOptions { WriteIndented = true }));
                });
            }
            else
            {
                using var croppedImage = BitmapTools.GetCropped(originalImage, activeBox, Color.Black);
                using var image512 = BitmapTools.GetResized(croppedImage, 512, 512);
                using var mask512 = croppedMask != null ? BitmapTools.GetResized(croppedMask, 512, 512) : null;
                
                generate(image512, mask512, (resultImage, resultFilePath, _) =>
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

        private void generate(Bitmap? initImage, Bitmap? maskImage, Action<Bitmap, string, long> processGeneratedImage)
        {
            if (initImage == null)
            {
                var parameters = new SdGenerationRequest
                {
                    prompt = tbPrompt.Text + (sdGenerationParameters.modifiers.Any() ? "; " + string.Join(", ", sdGenerationParameters.modifiers) : ""),
                    negative_prompt = sdGenerationParameters.negative,
                    cfg_scale = sdGenerationParameters.cfgScale,
                    seed = sdGenerationParameters.seed,
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
                    prompt = tbPrompt.Text + (sdGenerationParameters.modifiers.Any() ? "; " + string.Join(", ", sdGenerationParameters.modifiers) : ""),
                    negative_prompt = sdGenerationParameters.negative,
                    cfg_scale = sdGenerationParameters.cfgScale,
                    seed = sdGenerationParameters.seed,
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

        private void onImageGenerated(SdGenerationResponse? ev, Action<Bitmap, string, long> processGeneratedImage)
        {
            if (ev != null)
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
                
                    processGeneratedImage(BitmapTools.FromBase64(ev.images[0]), getDestImageFilePath(ev), ev.infoParsed.seed);
                }
            }
            else
            {
                Invoke(() =>
                {
                    pbIterations.CustomText = "ERROR";
                });
            }
            
            InProcess = false;
        }

        private void onProgress(int step)
        {
            Invoke(() =>
            {
                pbSteps.Value = step;
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
            sdPanel.tbNegative.Text = sdGenerationParameters.negative;
            sdPanel.numCfgScale.Value = sdGenerationParameters.cfgScale;
            sdPanel.tbSeed.Text = sdGenerationParameters.seed.ToString();

            sdPanel.numIterations.Value = pbIterations.Maximum;

            sdPanel.cbUseInitImage.Checked = originalImage != null;

            if (originalImage != null)
            {
                pictureBox.ActiveBox = activeBox;
                sdPanel.ddInpaintingFill.SelectedItem = inpaintingFill.ToString();
                pictureBox.Image = BitmapTools.Clone(originalImage);
                pictureBox.LoadMask(savedMask);
            }

            sdPanel.Modifiers = sdGenerationParameters.modifiers;

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
            var destDir = Path.Combine(Application.StartupPath, Program.Config.OutputFolder);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            return Path.Combine(destDir, ev.infoParsed.seed + ".png");
        }
    }
}
