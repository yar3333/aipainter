using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class StableDiffusionPanel : UserControl
    {
        public Action OnGenerate = null!;

        public bool IsTextboxInFocus => collapsablePanel.ActiveControl is TextBox;

        private string selectedCheckpointName
        {
            get => ddCheckpoint.SelectedValue?.ToString() ?? "";
            set
            {
                if (ddCheckpoint.DataSource == null || ddCheckpoint.Items.Cast<ListItem>().All(x => x.Value != value))
                {
                    ddCheckpoint.DataSource = SdCheckpointsHelper.GetListItems(value);
                }
                ddCheckpoint.SelectedValue = value;
            }
        }

        public StableDiffusionPanel()
        {
            InitializeComponent();
        }


        private void collapsablePanel_Load(object sender, EventArgs e)
        {
            selectedCheckpointName = Program.Config.StableDiffusionCheckpoint ?? "";

            tbNegative.Text = Program.Config.NegativePrompts.FirstOrDefault() ?? "";

            ddSampler.DataSource = new[]
            {
                "Euler a",
                "DPM++ 2M",
                "Heun",
            };
            ddSampler.SelectedItem = "DPM++ 2M";

            ddImageSize.DataSource = Program.Config.ImageSizes;
            ddImageSize.Text = Program.Config.ImageSizes.FirstOrDefault() ?? "512x512";
        }

        private void showManageCheckpointDialog()
        {
            var form = new SdCheckpointsForm();
            form.ShowDialog(this);

            var saveName = selectedCheckpointName;
            ddCheckpoint.DataSource = SdCheckpointsHelper.GetListItems(saveName);
            selectedCheckpointName = saveName;

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

            OnGenerate();
        }

        private void saveSelectedValuesToMainConfig()
        {
            var needSave = false;

            if (selectedCheckpointName != Program.Config.StableDiffusionCheckpoint)
            {
                needSave = true;
                Program.Config.StableDiffusionCheckpoint = selectedCheckpointName;
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
        }

        public void UpdateState(SmartPictureBox pb)
        {
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
        }

        public void SetImageSize(int w, int h)
        {
            ddImageSize.Text = w + "x" + h;
        }

        public void SetVaeName(string name)
        {
            Program.Config.StableDiffusionVae = name;
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

            cmCheckpointMenu.Items.Add(new ToolStripSeparator());

            cmCheckpointMenu.Items.AddRange(SdVaeHelper.GetMenuItems(Program.Config.StableDiffusionVae, vaeName =>
            {
                if (vaeName != "" && SdVaeHelper.GetPathToVae(vaeName) == null)
                {
                    var form = new SdVaeForm(vaeName);
                    form.ShowDialog(this);
                }
                if (vaeName == "" || SdVaeHelper.GetPathToVae(vaeName) != null)
                {
                    Program.Config.StableDiffusionVae = vaeName;
                    Program.SaveConfig();
                }
            }));

            cmCheckpointMenu.Show(Cursor.Position);
        }

        private void btLoras_Click(object sender, EventArgs e)
        {
            cmLorasMenu.Items.Clear();

            cmLorasMenu.Items.Add("Manage LoRAs...", null, (_, _) =>
            {
                var form = new SdLorasForm();
                form.ShowDialog(this);
            });

            cmLorasMenu.Items.Add(new ToolStripSeparator());

            var models = SdLoraHelper.GetNames().Where(x => SdLoraHelper.GetPathToModel(x) != null && SdLoraHelper.IsEnabled(x)).ToArray();
            foreach (var name in models)
            {
                cmLorasMenu.Items.Add(SdLoraHelper.GetHumanName(name), null, (_, _) =>
                {
                    tbPrompt.Text = SdLoraHelper.GetPrompt(name) + ", " + tbPrompt.Text;
                });
            }

            if (models.Length == 0)
            {
                cmLorasMenu.Items.Add(new ToolStripLabel("No enabled LoRA found"));
            }

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
            fillEmbeddingContextMenu(cmEmbeddingsMenu, tbPrompt, models);
        }

        private void btNegativeEmbeddings_Click(object sender, EventArgs e)
        {
            var models = SdEmbeddingHelper.GetNames()
                                          .Where(x => SdEmbeddingHelper.GetPathToModel(x) != null
                                                   && SdEmbeddingHelper.IsEnabled(x)
                                                   && SdEmbeddingHelper.GetConfig(x).isNegative)
                                          .ToArray();
            fillEmbeddingContextMenu(cmNegativeEmbeddingsMenu, tbNegative, models);
        }

        private void fillEmbeddingContextMenu(ContextMenuStrip menu, TextBox tb, string[] models)
        {
            menu.Items.Clear();

            menu.Items.Add("Manage Embeddings...", null, (_, _) =>
            {
                var form = new SdEmbeddingForm();
                form.ShowDialog(this);
            });

            menu.Items.Add(new ToolStripSeparator());

            foreach (var name in models)
            {
                menu.Items.Add(SdEmbeddingHelper.GetHumanName(name), null, (_, _) =>
                {
                    tb.Text = (tb.Text + ", (" + name + ":1.0), ").TrimStart(' ', ',');
                });
            }

            if (models.Length == 0)
            {
                menu.Items.Add(new ToolStripLabel("No enabled Embeddings found"));
            }

            menu.Show(Cursor.Position);
        }

        private void btStyles_Click(object sender, EventArgs e)
        {
            var form = new SdStylesForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                addTextToPrompt(string.Join(", ", form.Modifiers));
            }
        }

        private void btSuggestedPrompt_Click(object sender, EventArgs e)
        {
            cmSuggestedPromptMenu.Items.Clear();

            var checkpointName = selectedCheckpointName;
            if (checkpointName != "")
            {
                var phrases = getSuggestedPhrases(SdCheckpointsHelper.GetConfig(checkpointName).promptSuggested);
                if (phrases.Length > 0)
                {
                    cmSuggestedPromptMenu.Items.Add(new ToolStripLabel("*** " + checkpointName + " ***"));
                    foreach (var s in phrases)
                    {
                        cmSuggestedPromptMenu.Items.Add(s, null, (_, _) => addTextToPrompt(s));
                    }
                }
            }

            var loras = Regex.Matches(tbPrompt.Text, @"<lora:([^:>]+)[:>]").Select(x => x.Groups[1].Value).ToArray();
            foreach (var name in loras)
            {
                var phrases = getSuggestedPhrases(SdLoraHelper.GetConfig(name).promptSuggested);
                if (phrases.Length > 0)
                {
                    cmSuggestedPromptMenu.Items.Add(new ToolStripSeparator());
                    cmSuggestedPromptMenu.Items.Add(new ToolStripLabel("*** " + name + " ***"));
                    foreach (var s in phrases)
                    {
                        cmSuggestedPromptMenu.Items.Add(s, null, (_, _) => addTextToPrompt(s));
                    }
                }
            }

            if (cmSuggestedPromptMenu.Items.Count == 0)
            {
                cmSuggestedPromptMenu.Items.Add(new ToolStripLabel("No suggested phrases"));
            }

            cmSuggestedPromptMenu.Show(Cursor.Position);
        }

        private void addTextToPrompt(string s)
        {
            tbPrompt.Text = (tbPrompt.Text.TrimEnd(',', ' ') + ", " + s).TrimStart(',', ' ');
        }

        private static string[] getSuggestedPhrases(string? text)
        {
            if (text == null) return new string[] {};
            var parts = text.Split(',').Select(x => x.Trim(',', ' ')).ToList();
            parts.Insert(0, text);
            return parts.Where(x => x != "").Distinct().ToArray();
        }
    }
}
