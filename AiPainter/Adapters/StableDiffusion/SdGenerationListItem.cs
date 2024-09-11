using AiPainter.Adapters.StableDiffusion.SdBackends;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdGenerationListItem : UserControl, IGenerationListItem
    {
        public GenerationParallelGroup ParallelGroup => GenerationParallelGroup.GENERATION;

        public bool HasWorkToRun => !isFatalError && numIterations.Value > 0;
        public bool InProcess { get; private set; }
        public bool WantToBeRemoved { get; private set; }

        private readonly StableDiffusionPanel sdPanel;
        private readonly SmartPictureBox pictureBox;
        private readonly MainForm mainForm;

        private int lastNumIterationsValue;
        private bool isFatalError = false;

        private readonly SdGenerationParameters sdGenerationParameters;
        private readonly ISdGenerator generator;

        private readonly string? savedFilePath;
        private readonly Primitive[] savedMask;
        private readonly Rectangle? savedActiveBox;
        private readonly Bitmap? savedOriginalImage;

        public SdGenerationListItem(StableDiffusionPanel sdPanel, SmartPictureBox pictureBox, MainForm mainForm)
        {
            InitializeComponent();
            
            this.sdPanel = sdPanel;
            this.pictureBox = pictureBox;
            this.mainForm = mainForm;

            sdPanel.GetImageSize(out var width, out var height);

            sdGenerationParameters = new SdGenerationParameters
            {
                checkpointName = sdPanel.selectedCheckpointName,
                vaeName = Program.Config.StableDiffusionVae,
                prompt = sdPanel.tbPrompt.Text.Trim(),
                negative = sdPanel.tbNegative.Text.Trim(),
                steps = (int)sdPanel.numSteps.Value,
                cfgScale = sdPanel.numCfgScale.Value,
                clipSkip = sdPanel.selectedClipSkip > 0 
                               ? sdPanel.selectedClipSkip
                               : (SdCheckpointsHelper.GetConfig(sdPanel.selectedCheckpointName).clipSkip ?? 1),
                
                seed = sdPanel.cbUseSeed.Checked && sdPanel.tbSeed.Text.Trim() != "" ? long.Parse(sdPanel.tbSeed.Text.Trim()) : -1,
                seedVariationStrength = sdPanel.trackBarSeedVariationStrength.Value / 100m,

                width = width,
                height = height,
                sampler = sdPanel.ddSampler.SelectedItem.ToString()!,
                changesLevel = sdPanel.cbUseInitImage.Checked ? sdPanel.trackBarChangesLevel.Value / 100.0m : -1,
                inpaintingFill = sdPanel.cbUseInitImage.Checked ? sdPanel.selectedInpaintingFill : null,
            };

            lastNumIterationsValue = (int)sdPanel.numIterations.Value;
            numIterations.Value = lastNumIterationsValue;

            pbIterations.Maximum = (int)numIterations.Value;
            pbIterations.Value = 0;
            pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
            pbIterations.Refresh();

            tbPrompt.Text = sdPanel.tbPrompt.Text;

            pbSteps.Value = 0;
            pbSteps.Maximum = sdGenerationParameters.steps;
            pbSteps.CustomText = "";
            pbSteps.Refresh();

            toolTip.SetToolTip(tbPrompt, getTooltip());

            savedFilePath = mainForm.FilePath;
            savedMask = pictureBox.SaveMask();
            savedActiveBox = sdPanel.cbUseInitImage.Checked ? savedActiveBox = pictureBox.ActiveBox : null;
            
            if (!sdPanel.cbUseInitImage.Checked)
            {
                generator = SdBackend.Instance.CreateGeneratorMain(this, mainForm.ImagesFolder);
            }
            else
            {
                savedActiveBox = pictureBox.ActiveBox;
                savedOriginalImage = BitmapTools.Clone(pictureBox.Image!);
                generator = SdBackend.Instance.CreateGeneratorInpaint
                (
                    this,
                    savedOriginalImage,
                    pictureBox.ActiveBox,
                    pictureBox.GetMaskCropped(Color.Black, Color.White),
                    mainForm.FilePath!
                );
            }
        }

        public void Run()
        {
            InProcess = true;

            Task.Run(async () =>
            {
                await runInnerAsync();
                InProcess = false;
            });
        }

        public void Cancel()
        {
            numIterations.Value = 0;
            btRemove.Enabled = false;
            numIterations.Enabled = false;
            WantToBeRemoved = true;
        }

        private async Task runInnerAsync()
        {
            NotifyStepsCustomText("Preparing...");

            try
            {
                if (await generator.RunAsync(sdGenerationParameters.CloneWithFixedSeed()))
                {
                    Invoke(() =>
                    {
                        lastNumIterationsValue = (int)numIterations.Value - 1;
                        numIterations.Value--;
                
                        pbIterations.Value++;
                        pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
                        pbIterations.Refresh();
                    });
                }
            }
            catch (SdGeneratorNeedRetryException)
            {
                await Task.Delay(1000);
            }
            catch (SdGeneratorFatalErrorException e)
            {
                if (!WantToBeRemoved)
                {
                    Invoke(() => pbIterations.CustomText = e.Message);
                }
            }            
            catch (System.Net.WebSockets.WebSocketException)
            {
                isFatalError = true;
                Invoke(() =>
                {
                    numIterations.Enabled = false;
                    pbIterations.CustomText = Program.Config.StableDiffusionBackend;
                });
                NotifyStepsCustomText("ERROR");
            }
            catch (Exception e)
            {
                Program.Log.WriteLine(e.ToString());
                await Task.Delay(1000);
            }
        }

        public bool IsWantToCancelProcessingResultOfCurrentGeneration => isFatalError || numIterations.Value == 0;

        public void NotifyProgress(int step)
        {
            Invoke(() =>
            {
                pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;

                pbSteps.Value = step;
                pbSteps.CustomText = pbSteps.Value + " / " + pbSteps.Maximum;
            });
        }

        public void NotifyStepsCustomText(string text)
        {
            Invoke(() =>
            {
                pbSteps.CustomText = text;
                pbSteps.Value = 0;
            });
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void btLoadParamsBackToPanel_Click(object sender, EventArgs e)
        {
            sdPanel.selectedCheckpointName = sdGenerationParameters.checkpointName;
            sdPanel.selectedVaeName = sdGenerationParameters.vaeName;
        
            sdPanel.numSteps.Value = sdGenerationParameters.steps;
            sdPanel.tbPrompt.Text = sdGenerationParameters.prompt;
            sdPanel.tbNegative.Text = sdGenerationParameters.negative;
            sdPanel.numCfgScale.Value = sdGenerationParameters.cfgScale;
            sdPanel.selectedClipSkip = sdGenerationParameters.clipSkip;

            sdPanel.cbUseSeed.Checked = sdGenerationParameters.seed > 0;
            sdPanel.tbSeed.Text = sdGenerationParameters.seed.ToString();
            sdPanel.trackBarSeedVariationStrength.Value = (int)Math.Round(sdGenerationParameters.seedVariationStrength * 100);
        
            sdPanel.ddSampler.SelectedItem = sdGenerationParameters.sampler;
            if (sdGenerationParameters.changesLevel >= 0) sdPanel.trackBarChangesLevel.Value = (int)Math.Round(sdGenerationParameters.changesLevel * 100);
            if (sdGenerationParameters.inpaintingFill != null) sdPanel.selectedInpaintingFill = sdGenerationParameters.inpaintingFill.Value;

            sdPanel.cbUseInitImage.Checked = savedOriginalImage != null;

            if (savedOriginalImage != null)
            {
                mainForm.FilePath = savedFilePath;

                pictureBox.HistoryAddCurrentState();
                if (savedActiveBox != null) pictureBox.ActiveBox = savedActiveBox.Value;
                pictureBox.Image = BitmapTools.Clone(savedOriginalImage);
                pictureBox.LoadMask(savedMask);
                pictureBox.Refresh();
            }

            sdPanel.SetImageSize(sdGenerationParameters.width, sdGenerationParameters.height);
        }

        private void numIterations_ValueChanged(object sender, EventArgs e)
        {
            if (lastNumIterationsValue != (int)numIterations.Value)
            {
                pbIterations.Maximum += (int)numIterations.Value - lastNumIterationsValue;
                pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
                
                lastNumIterationsValue = (int)numIterations.Value;
            
                if (numIterations.Value == 0 && InProcess)
                {
                    generator.Cancel();
                }
            }
        }

        private string getTooltip() => "Positive prompt:\n" + sdGenerationParameters.prompt + "\n\n"
                                     + "Negative prompt:\n" + sdGenerationParameters.negative;
    }
}
