namespace AiPainter.Controls
{
    public partial class GenerationListItem : UserControl
    {
        private IImageGenerator generator = null!;

        public int ImagesdInQueue => (int)numIterations.Value;
        public bool InProcess { get; private set; }
        public bool WantToBeRemoved;

        private bool ignoreNumIterationsChange;
        private int lastIterations;

        public GenerationListItem()
        {
            InitializeComponent();
        }

        public void Init(IImageGenerator imageGenerator)
        {
            generator = imageGenerator;

            generator.SetControl(this);
            
            ignoreNumIterationsChange = true;
            numIterations.Value = generator.GetOriginalCount();
            ignoreNumIterationsChange = false;

            pbIterations.Maximum = (int)numIterations.Value;
            pbIterations.Value = 0;
            pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
            pbIterations.Refresh();

            tbPrompt.Text = generator.GetBasePromptText();

            pbSteps.Value = 0;
            pbSteps.Maximum = generator.GetStepsMax();
            pbSteps.CustomText = "";
            pbSteps.Refresh();

            toolTip.SetToolTip(tbPrompt, generator.GetTooltip());

            lastIterations = (int)numIterations.Value;
        }

        public void Run()
        {
            InProcess = true;

            Task.Run(async () =>
            {
                try
                {
                    await generator.RunAsync();
                }
                catch (Exception e)
                {
                    Program.Log.WriteLine(e.ToString());
                }
            });
        }

        public bool IsWantToCancelProcessingResultOfCurrentGeneration => numIterations.Value == 0;

        public void NotifyGenerateSuccess()
        {
            InProcess = false;
            
            if (!IsWantToCancelProcessingResultOfCurrentGeneration)
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
            }
        }

        public void NotifyGenerateFail(string text)
        {
            InProcess = false;
            
            if (!WantToBeRemoved)
            {
                Invoke(() =>
                {
                    pbIterations.CustomText = text;
                });
            }
        }

        public void NotifyNeedRetry()
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                InProcess = false;
            });
        }

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
            numIterations.Value = 0;
            btRemove.Enabled = false;
            numIterations.Enabled = false;
            
            WantToBeRemoved = true;
        }

        private void btLoadParamsBackToPanel_Click(object sender, EventArgs e)
        {
            generator.LoadParamsBackToPanel();
        }

        private void numIterations_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreNumIterationsChange) return;

            pbIterations.Maximum += (int)numIterations.Value - lastIterations;
            pbIterations.CustomText = pbIterations.Value + " / " + pbIterations.Maximum;
            
            lastIterations = (int)numIterations.Value;
            
            if (numIterations.Value == 0 && InProcess)
            {
                generator.Cancel();
            }
        }
    }
}
