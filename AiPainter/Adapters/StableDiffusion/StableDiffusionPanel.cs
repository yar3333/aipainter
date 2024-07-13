using System.Diagnostics;
using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
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

        private bool inProcess = false;

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

        private void showManageCheckpointDialog()
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
            var needSave = false;

            if (selectedCheckpointName != Program.Config.StableDiffusionCheckpoint)
            {
                needSave = true;
                Program.Config.StableDiffusionCheckpoint = selectedCheckpointName;
            }

            if (selectedVaeName != Program.Config.StableDiffusionVae)
            {
                needSave = true;
                Program.Config.StableDiffusionVae = selectedVaeName;
            }

            var negativeText = tbNegative.Text.Trim(' ', ',', ';', '\r', '\n');
            if (negativeText != "" && Program.Config.NegativePrompts.FirstOrDefault() != negativeText)
            {
                needSave = true;
                Program.Config.NegativePrompts.Remove(negativeText);
                Program.Config.NegativePrompts.Insert(0, negativeText);
                Program.Config.NegativePrompts = Program.Config.NegativePrompts.Take(10).ToList();
            }

            if (!Program.Config.ImageSizes.Contains(ddImageSize.Text))
            {
                needSave = true;
                Program.Config.ImageSizes.Insert(0, ddImageSize.Text);
                Program.Config.ImageSizes = Program.Config.ImageSizes.Take(20).ToList();
            }

            if (needSave) Program.SaveConfig();
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
            
            btInterrogate.Enabled = !inProcess && pb.Image != null;
            btGenerate.Enabled = !inProcess;
            btReset.Enabled = !inProcess;
            btSuggestedPrompt.Enabled = !inProcess;
            btStyles.Enabled = !inProcess;
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

        private void ddCheckpoint_DropDown(object sender, EventArgs e)
        {
            if (ddCheckpoint.Items.Count == 0 || ddCheckpoint.Items.Count == 1 && string.IsNullOrEmpty(((ListItem)ddCheckpoint.Items[0]).Value))
            {
                showManageCheckpointDialog();
            }
        }

        private void btCheckpoint_Click(object sender, EventArgs e)
        {
            cmCheckpointMenu.Items.Clear();

            cmCheckpointMenu.Items.Add("Manage checkpoints...", null, (_, _) =>
            {
                showManageCheckpointDialog();
            });

            cmCheckpointMenu.Items.Add(new ToolStripSeparator());

            cmCheckpointMenu.Items.Add("Show in Explorer", null, (_, _) =>
            {
                if (selectedCheckpointName != "")
                {
                    ProcessHelper.ShowFolderInExplorer(SdCheckpointsHelper.GetDirPath(selectedCheckpointName));
                }
            });

            cmCheckpointMenu.Items.Add("Visit home page", null, (_, _) =>
            {
                if (selectedCheckpointName != "")
                {
                    var config = SdCheckpointsHelper.GetConfig(selectedCheckpointName);
                    if (!string.IsNullOrEmpty(config.homeUrl)) ProcessHelper.OpenUrlInBrowser(config.homeUrl);
                }
            });

            cmCheckpointMenu.Show(Cursor.Position);
        }

        private void btLoras_Click(object sender, EventArgs e)
        {
            cmLorasMenu.Items.Clear();

            cmLorasMenu.Items.Add("Manage LoRAs...", null, (_, _) =>
            {
                var form = new SdModelsForm(mainForm.panGenerationList, new SdLorasFormAdapter());
                form.ShowDialog(this);
            });

            cmLorasMenu.Items.Add(new ToolStripSeparator());

            var usedModels = GetUsedLoras();
            var models = SdLoraHelper.GetNames().Where(x => SdLoraHelper.GetPathToModel(x) != null && SdLoraHelper.IsEnabled(x)).ToArray();
            foreach (var name in models)
            {
                cmLorasMenu.Items.Add(new ToolStripMenuItem(SdLoraHelper.GetHumanName(name), null, (_, _) =>
                {
                    AddTextToPrompt(SdLoraHelper.GetPrompt(name));
                })
                {
                    Checked = usedModels.Contains(name)
                });
            }

            if (models.Length == 0)
            {
                cmLorasMenu.Items.Add(new ToolStripLabel("No enabled LoRA found"));
            }

            if (inProcess) foreach (ToolStripItem item in cmLorasMenu.Items) item.Enabled = false;

            cmLorasMenu.Show(Cursor.Position);
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
            var models = SdEmbeddingHelper.GetNames()
                                          .Where(x => SdEmbeddingHelper.GetPathToModel(x) != null
                                                   && SdEmbeddingHelper.IsEnabled(x)
                                                   && !SdEmbeddingHelper.GetConfig(x).isNegative)
                                          .ToArray();
            fillEmbeddingContextMenu(cmEmbeddingsMenu, models, false);
        }

        private void btNegativeEmbeddings_Click(object sender, EventArgs e)
        {
            var models = SdEmbeddingHelper.GetNames()
                                          .Where(x => SdEmbeddingHelper.GetPathToModel(x) != null
                                                   && SdEmbeddingHelper.IsEnabled(x)
                                                   && SdEmbeddingHelper.GetConfig(x).isNegative)
                                          .ToArray();
            fillEmbeddingContextMenu(cmNegativeEmbeddingsMenu, models, true);
        }

        private void fillEmbeddingContextMenu(ContextMenuStrip menu, string[] models, bool isForNegative)
        {
            menu.Items.Clear();

            menu.Items.Add("Manage Embeddings...", null, (_, _) =>
            {
                var form = new SdModelsForm(mainForm.panGenerationList, new SdEmbeddingFormAdapter());
                form.ShowDialog(this);
            });

            menu.Items.Add(new ToolStripSeparator());

            foreach (var name in models)
            {
                menu.Items.Add(new ToolStripMenuItem(SdEmbeddingHelper.GetHumanName(name), null, (_, _) =>
                {
                    if (!isForNegative) AddTextToPrompt("(" + name + ":1.0)");
                    else AddTextToNegative("(" + name + ":1.0)");
                })
                {
                    Checked = Regex.IsMatch(!isForNegative ? tbPrompt.Text : tbNegative.Text, @"\b" + Regex.Escape(name) + @"\b")
                });
            }

            if (models.Length == 0)
            {
                menu.Items.Add(new ToolStripLabel("No enabled Embeddings found"));
            }

            if (inProcess) foreach (ToolStripItem item in menu.Items) item.Enabled = false;

            menu.Show(Cursor.Position);
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
            tbPrompt.Text = (tbPrompt.Text.TrimEnd(',', ' ') + ", " + s).TrimStart(',', ' ');
        }

        public void AddTextToNegative(string s)
        {
            tbNegative.Text = (tbNegative.Text.TrimEnd(',', ' ') + ", " + s).TrimStart(',', ' ');
        }

        private void ddVae_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vaeName = selectedVaeName;
            if (vaeName != "" && SdVaeHelper.GetPathToVae(vaeName) == null)
            {
                var form = new SdVaeForm(vaeName);
                form.ShowDialog(this);
            }

            toolTip.SetToolTip(ddVae, baseVaeTooltip + (vaeName != "" ? "\n\n*** " + vaeName + "\n" + SdVaeHelper.GetConfig(vaeName).description : ""));
        }

        private void ddCheckpoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            var config = SdCheckpointsHelper.GetConfig(selectedCheckpointName);
            if (config.clipSkip != null) selectedClipSkip = config.clipSkip.Value;
            if (!string.IsNullOrEmpty(config.promptRequired)) AddTextToPrompt(config.promptRequired);
        }

        public string[] GetUsedLoras()
        {
            return Regex.Matches(tbPrompt.Text, @"<lora:([^:>]+)[:>]")
                        .Select(x => x.Groups[1].Value)
                        .ToArray();
        }

        private void btInterrogate_Click(object sender, EventArgs e)
        {
            inProcess = true;
            tbPrompt.ReadOnly = true;
            
            var croppedImage = BitmapTools.GetCropped(mainForm.pictureBox.Image!, mainForm.pictureBox.ActiveBox, Color.Black);
            
            Task.Run(async () =>
            {
                var result = await SdApiClient.interrogate(new SdInterrogateRequest
                {
                    image = BitmapTools.GetBase64String(croppedImage),
                    //model = "model_base_caption_capfilt_large",
                });

                Invoke(() =>
                {
                    if (result != null) tbPrompt.Text = result;

                    inProcess = false;
                    tbPrompt.ReadOnly = false;
                });
            });

        }
    }
}
