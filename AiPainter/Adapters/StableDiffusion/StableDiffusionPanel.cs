using AiPainter.Controls;
using AiPainter.Helpers;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class StableDiffusionPanel : UserControl
    {
        private SdModifiersForm modifiersForm = null!;

        public Action OnGenerate = null!;

        public string[] Modifiers
        {
            get => lbModifiers.Items.OfType<string>().Where(x => x != "Modifiers. Click to select.").ToArray();
            set
            {
                lbModifiers.Items.Clear();
                // ReSharper disable once CoVariantArrayConversion
                lbModifiers.Items.AddRange(value);

                if (!value.Any())
                {
                    lbModifiers.Items.Add("Modifiers. Click to select.");
                }
            }
        }

        public StableDiffusionPanel()
        {
            InitializeComponent();
            Modifiers = new string[] { };
        }

        public bool IsTextboxInFocus => collapsablePanel.ActiveControl is TextBox;

        private void collapsablePanel_Load(object sender, EventArgs e)
        {
            modifiersForm = new SdModifiersForm();

            ddCheckpoint.DataSource = SdCheckpointsHelper.GetListItems(Program.Config.StableDiffusionCheckpoint);
            ddCheckpoint.ValueMember = "Value";
            ddCheckpoint.DisplayMember = "Text";
            ddCheckpoint.SelectedValue = Program.Config.StableDiffusionCheckpoint;

            tbNegative.Text = Program.Config.NegativePrompt;

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

            ddCheckpoint.DataSource = SdCheckpointsHelper.GetListItems(Program.Config.StableDiffusionCheckpoint);
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            ddImageSize.Text = getNormalizedImageSize();

            saveSelectedValuesToMainConfig();

            if (tbPrompt.Text.Trim() == "") { tbPrompt.Focus(); return; }

            if (string.IsNullOrEmpty(ddCheckpoint.SelectedValue?.ToString()))
            {
                ddCheckpoint.DroppedDown = true;
                return;
            }

            OnGenerate();
        }

        private void saveSelectedValuesToMainConfig()
        {
            var needSave = false;

            if ((ddCheckpoint.SelectedValue?.ToString() ?? "") != Program.Config.StableDiffusionCheckpoint)
            {
                needSave = true;
                Program.Config.StableDiffusionCheckpoint = ddCheckpoint.SelectedValue?.ToString() ?? "";
            }
            
            if (tbNegative.Text != "" && Program.Config.NegativePrompts.FirstOrDefault() != tbNegative.Text)
            {
                needSave = true;
                Program.Config.NegativePrompts.Remove(tbNegative.Text);
                Program.Config.NegativePrompts.Insert(0, tbNegative.Text);
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
            trackBarChangesLevel.Value = 0;
            numCfgScale.Value = 7.0m;
            ddImageSize.Text = "512x512";
            ddSampler.SelectedItem = 0;
            numSteps.Value = 35;
            cbUseSeed.Checked = false;
            trackBarSeedVariationStrength.Value = 0;
            Modifiers = new string[] {};
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

        private void lbModifiers_Click(object sender, EventArgs e)
        {
            modifiersForm.Modifiers = Modifiers;
            if (modifiersForm.ShowDialog(this) == DialogResult.OK)
            {
                Modifiers = modifiersForm.Modifiers;
            }
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

        private void btContextMenuCheckpoint_Click(object sender, EventArgs e)
        {
            contextMenuCheckpoint.Items.Clear();

            contextMenuCheckpoint.Items.Add("Manage checkpoints...", null, (_, _) =>
            {
                showManageCheckpointDialog();
            });
            
            contextMenuCheckpoint.Items.Add("Show in Explorer", null, (_, _) =>
            {
                if (!string.IsNullOrEmpty(ddCheckpoint.SelectedValue?.ToString()))
                {
                    ProcessHelper.ShowFolderInExplorer(SdCheckpointsHelper.GetDirPath(ddCheckpoint.SelectedValue.ToString()!));
                }
            });

            contextMenuCheckpoint.Items.Add(new ToolStripSeparator());

            contextMenuCheckpoint.Items.AddRange(SdVaeHelper.GetMenuItems(Program.Config.StableDiffusionVae, vaeName =>
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

            contextMenuCheckpoint.Show(Cursor.Position);
        }

        private void btContextMenuPrompt_Click(object sender, EventArgs e)
        {
            contextMenuPrompt.Items.Clear();

            contextMenuPrompt.Items.Add("Manage LoRAs...", null, (_, _) =>
            {
                var form = new SdLorasForm();
                form.ShowDialog(this);
            });
            
            contextMenuPrompt.Items.Add(new ToolStripSeparator());
            
            var loras = SdLoraHelper.GetNames().Where(x => SdLoraHelper.GetPathToModel(x) != null && SdLoraHelper.IsEnabled(x)).ToArray();
            foreach (var lora in loras)
            {
                contextMenuPrompt.Items.Add("Use LoRA: " + SdLoraHelper.GetHumanName(lora), null, (_, _) =>
                {
                    tbPrompt.Text = SdLoraHelper.GetPrompt(lora) + ", " + tbPrompt.Text;
                });
            }

            if (loras.Length == 0)
            {
                contextMenuPrompt.Items.Add(new ToolStripLabel("No enabled LoRA found"));
            }
            
            contextMenuPrompt.Show(Cursor.Position);
        }

        private void btContextMenuNegativePrompt_Click(object sender, EventArgs e)
        {
            contextMenuNegativePrompt.Items.Clear();

            foreach (var negativePrompt in Program.Config.NegativePrompts)
            {
                contextMenuNegativePrompt.Items.Add(negativePrompt, null, (_, _) =>
                {
                    tbNegative.Text = negativePrompt;
                });
            }

            if (Program.Config.NegativePrompts.Count == 0)
            {
                contextMenuNegativePrompt.Items.Add(new ToolStripLabel("No saved prompts"));
            }
            
            contextMenuNegativePrompt.Show(Cursor.Position);
        }
    }
}
