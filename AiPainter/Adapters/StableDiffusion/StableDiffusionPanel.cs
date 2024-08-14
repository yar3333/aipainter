using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdBackends;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class StableDiffusionPanel : UserControl
    {
        private readonly string baseVaeTooltip;

        // ReSharper disable once InconsistentNaming
        public MainForm mainForm = null!;

        public bool IsTextboxInFocus => collapsablePanel.ActiveControl is TextBox;

        public bool InProcess = false;

        public string selectedCheckpointName
        {
            get => ddCheckpoint.SelectedValue?.ToString() ?? "";
            set
            {
                if (ddCheckpoint.DataSource == null || ddCheckpoint.Items.Cast<ListItem>().All(x => x.Value != value))
                {
                    ddCheckpoint.DataSource = SdCheckpointsHelper.GetListItems(value ?? "");
                }
                ddCheckpoint.SelectedValue = value ?? "";
            }
        }

        public string selectedVaeName
        {
            get => ddVae.SelectedValue?.ToString() ?? "";
            set
            {
                if (ddVae.DataSource == null || ddVae.Items.Cast<ListItem>().All(x => x.Value != value))
                {
                    ddVae.DataSource = SdVaeHelper.GetListItems();
                }
                ddVae.SelectedValue = ddVae.Items.Cast<ListItem>().Any(x => x.Value == value) ? (value ?? "") : "";
            }
        }

        public int selectedClipSkip
        {
            get => int.Parse(ddClipSkip.SelectedItem!.ToString()!);
            set
            {
                if (value == 1 || value == 2)
                {
                    ddClipSkip.SelectedItem = value.ToString();
                }
            }
        }

        public SdInpaintingFill selectedInpaintingFill
        {
            get => Enum.TryParse<SdInpaintingFill>(ddInpaintingFill.SelectedItem?.ToString() ?? "", true, out var r) ? r : SdInpaintingFill.original;
            set => ddInpaintingFill.SelectedItem = value.ToString();
        }

        public StableDiffusionPanel()
        {
            InitializeComponent();

            baseVaeTooltip = toolTip.GetToolTip(ddVae);
        }

        public void updateCheckpoints()
        {
            var saveName = selectedCheckpointName;
            ddCheckpoint.DataSource = SdCheckpointsHelper.GetListItems(saveName);
            ddCheckpoint.SelectedValue = saveName;
        }

        private void collapsablePanel_Load(object sender, EventArgs e)
        {
            selectedCheckpointName = Program.Config.StableDiffusionCheckpoint ?? "";
            selectedVaeName = Program.Config.StableDiffusionVae;

            ddSampler.DataSource = new[]
            {
                "Euler a",
                "DPM++ 2M",
                "Heun",
            };
            ddSampler.SelectedItem = "DPM++ 2M";

            ddClipSkip.SelectedItem = "1";

            selectedInpaintingFill = SdInpaintingFill.original;

            ddImageSize.DataSource = Program.Config.ImageSizes;
            ddImageSize.Text = Program.Config.ImageSizes.FirstOrDefault() ?? "512x512";

            tbNegative.Text = Program.Config.NegativePrompts.FirstOrDefault() ?? "";

            GlobalEvents.CheckpointFileDownloaded += () => Invoke(updateCheckpoints);
        }

        public void ShowManageCheckpointDialog()
        {
            var form = new SdModelsForm(mainForm.panGenerationList, new SdCheckpointsFormAdapter());
            form.ShowDialog(this);

            updateCheckpoints();
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            ddImageSize.Text = getNormalizedImageSize();

            saveSelectedValuesToMainConfig();

            if (tbPrompt.Text.Trim() == "") { tbPrompt.Focus(); return; }

            if (selectedCheckpointName == "")
            {
                ddCheckpoint.DroppedDown = true;
                return;
            }

            mainForm.panGenerationList.AddGeneration(new SdGenerationListItem(this, mainForm.pictureBox, mainForm));
        }

        private void saveSelectedValuesToMainConfig()
        {
            Program.Config.StableDiffusionCheckpoint = selectedCheckpointName;
            Program.Config.StableDiffusionVae = selectedVaeName;

            var negativeText = tbNegative.Text.Trim(' ', ',', ';', '\r', '\n');
            if (negativeText != "" && Program.Config.NegativePrompts.FirstOrDefault() != negativeText)
            {
                Program.Config.NegativePrompts.Remove(negativeText);
                Program.Config.NegativePrompts.Insert(0, negativeText);
                Program.Config.NegativePrompts = Program.Config.NegativePrompts.Take(10).ToList();
            }

            if (!Program.Config.ImageSizes.Contains(ddImageSize.Text))
            {
                Program.Config.ImageSizes.Insert(0, ddImageSize.Text);
                Program.Config.ImageSizes = Program.Config.ImageSizes.Take(20).ToList();
            }
        }

        private string getNormalizedImageSize()
        {
            var s = ddImageSize.Text
                               .Replace(" ", "")
                               .Replace("-", "")
                               .Replace("+", "")
                               .Replace("\t", "")
                               .Replace("X", "x")
                               .Replace("х", "x") // cyrillic
                               .Replace(",", "x")
                               .Replace(";", "x")
                               .Replace("*", "x")
                               .Trim('x');

            var parts = s.Split('x');
            if (parts.Length != 2) return "512x512";

            if (!int.TryParse(parts[0], out var w) || w <= 0) return "512x512";
            if (!int.TryParse(parts[1], out var h) || h <= 0) return "512x512";

            return w + "x" + h;
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            cbUseInitImage.Checked = false;
            trackBarChangesLevel.Value = 100;
            numCfgScale.Value = 7.0m;
            ddImageSize.Text = "512x512";
            ddSampler.SelectedItem = 0;
            numSteps.Value = 35;
            cbUseSeed.Checked = false;
            trackBarSeedVariationStrength.Value = 0;
            selectedInpaintingFill = SdInpaintingFill.original;
        }

        public void UpdateState()
        {
            var pb = mainForm.pictureBox;

            if (pb.Image != null && pb.HasMask)
            {
                cbUseInitImage.Enabled = false;
                cbUseInitImage.Checked = true;
            }
            else if (pb.Image == null)
            {
                cbUseInitImage.Enabled = false;
                cbUseInitImage.Checked = false;
            }
            else
            {
                cbUseInitImage.Enabled = true;
            }

            ddImageSize.Enabled = !(pb.Image != null && cbUseInitImage.Checked);

            trackBarChangesLevel.Enabled = pb.Image != null;

            tbSeed.Enabled = cbUseSeed.Checked;
            trackBarSeedVariationStrength.Enabled = cbUseSeed.Checked;

            trackBarChangesLevel.Enabled = cbUseInitImage.Checked;
            ddInpaintingFill.Enabled = cbUseInitImage.Checked;
            
            btInterrogate.Enabled = !InProcess && pb.Image != null;
            btGenerate.Enabled = !InProcess;
            btReset.Enabled = !InProcess;
            btSuggestedPrompt.Enabled = !InProcess;
            btStyles.Enabled = !InProcess;
        }

        public void SetImageSize(int w, int h)
        {
            ddImageSize.Text = w + "x" + h;
        }

        public void LoadParametersToSdGenerationPanel(SdGenerationParameters sdGenerationParameters)
        {
            selectedCheckpointName = sdGenerationParameters.checkpointName;
            selectedVaeName = sdGenerationParameters.vaeName;

            numSteps.Value = sdGenerationParameters.steps;

            tbPrompt.Text = sdGenerationParameters.prompt;
            tbNegative.Text = sdGenerationParameters.negative;

            if (sdGenerationParameters.cfgScale != 0)
            {
                numCfgScale.Value = Math.Max(numCfgScale.Minimum, Math.Min(numCfgScale.Maximum, sdGenerationParameters.cfgScale));
            }

            if (sdGenerationParameters.clipSkip != 0) selectedClipSkip = sdGenerationParameters.clipSkip;

            tbSeed.Text = sdGenerationParameters.seed.ToString();

            SetImageSize(sdGenerationParameters.width, sdGenerationParameters.height);
            ddSampler.SelectedItem = sdGenerationParameters.sampler;

            if (sdGenerationParameters.inpaintingFill != null) selectedInpaintingFill = sdGenerationParameters.inpaintingFill.Value;
        }

        private void collapsablePanel_Resize(object sender, EventArgs e)
        {
            Height = collapsablePanel.Height;
        }

        private void btCheckpoint_Click(object sender, EventArgs e)
        {
            new SdCheckpointsContextMenu(this).Show(Cursor.Position);
        }

        private void btLoras_Click(object sender, EventArgs e)
        {
            new SdLorasContextMenu(this, mainForm.panGenerationList).Show(Cursor.Position);
        }

        private void btNegativePromptHistory_Click(object sender, EventArgs e)
        {
            cmNegativePromptHistoryMenu.Items.Clear();

            foreach (var negativePrompt in Program.Config.NegativePrompts)
            {
                cmNegativePromptHistoryMenu.Items.Add(negativePrompt, null, (_, _) =>
                {
                    tbNegative.Text = negativePrompt;
                });
            }

            if (Program.Config.NegativePrompts.Count == 0)
            {
                cmNegativePromptHistoryMenu.Items.Add(new ToolStripLabel("No saved prompts"));
            }

            cmNegativePromptHistoryMenu.Show(Cursor.Position);
        }

        private void btEmbeddings_Click(object sender, EventArgs e)
        {
            new SdEmbeddingsContextMenu(this, mainForm.panGenerationList, false).Show(Cursor.Position);
        }

        private void btNegativeEmbeddings_Click(object sender, EventArgs e)
        {
            new SdEmbeddingsContextMenu(this, mainForm.panGenerationList, true).Show(Cursor.Position);
        }

        private void btStyles_Click(object sender, EventArgs e)
        {
            var form = new SdStylesForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                AddTextToPrompt(string.Join(", ", form.Modifiers));
            }
        }

        private void btSuggestedPrompt_Click(object sender, EventArgs e)
        {
            new SuggestedPromptContextMenu(this).Show(Cursor.Position);
        }

        public void AddTextToPrompt(string s)
        {
            if (!Regex.IsMatch(tbPrompt.Text, @"\b" + Regex.Escape(s) + @"\b"))
            {
                tbPrompt.Text = (tbPrompt.Text.TrimEnd(',', ' ') + ", " + s).TrimStart(',', ' ');
            }
        }

        public void AddTextToNegative(string s)
        {
            if (!Regex.IsMatch(tbNegative.Text, @"\b" + Regex.Escape(s) + @"\b"))
            {
                tbNegative.Text = (tbNegative.Text.TrimEnd(',', ' ') + ", " + s).TrimStart(',', ' ');
            }
        }

        public string[] GetUsedLoras()
        {
            return Regex.Matches(tbPrompt.Text, @"<lora:([^:>]+)[:>]")
                        .Select(x => x.Groups[1].Value)
                        .ToArray();
        }

        private void ddCheckpoint_DropDown(object sender, EventArgs e)
        {
            if (ddCheckpoint.Items.Count == 0 || ddCheckpoint.Items.Count == 1 && string.IsNullOrEmpty(((ListItem)ddCheckpoint.Items[0]).Value))
            {
                ShowManageCheckpointDialog();
            }
        }

        private void ddCheckpoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            var config = SdCheckpointsHelper.GetConfig(selectedCheckpointName);
            if (config.clipSkip != null) selectedClipSkip = config.clipSkip.Value;
            if (!string.IsNullOrEmpty(config.promptRequired)) AddTextToPrompt(config.promptRequired);
        }

        private void ddVae_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vaeName = selectedVaeName;
            if (vaeName != "" && SdVaeHelper.GetPathToVae(vaeName) == null)
            {
                var form = new SdVaeForm(vaeName);
                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    selectedVaeName = "";
                }
            }

            toolTip.SetToolTip(ddVae, baseVaeTooltip + (vaeName != "" ? "\n\n*** " + vaeName + "\n" + SdVaeHelper.GetConfig(vaeName).description : ""));
        }

        private void btInterrogate_Click(object sender, EventArgs e)
        {
            InProcess = true;
            tbPrompt.ReadOnly = true;
            
            var croppedImage = BitmapTools.GetCropped(mainForm.pictureBox.Image!, mainForm.pictureBox.ActiveBox, Color.Black);
            
            Task.Run(async () =>
            {
                var result = await SdBackend.Instance.InterrogateAsync(croppedImage);

                Invoke(() =>
                {
                    if (result != null) tbPrompt.Text = result;

                    InProcess = false;
                    tbPrompt.ReadOnly = false;
                });
            });

        }
    }
}
