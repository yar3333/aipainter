using AiPainter.Controls;

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

            var checkpoints = SdCheckpointsHelper.GetNames();
            if (!checkpoints.Contains(Program.Config.StableDiffusionCheckpoint))
            {
                checkpoints = new[] { Program.Config.StableDiffusionCheckpoint }.Concat(checkpoints).ToArray();
            }
            var checkpointItems = checkpoints.Select(x => new ListItem
            {
                Value = x,
                Text = SdCheckpointsHelper.GetHumanName(x)
            }).ToArray();
            ddCheckpoint.ValueMember = "Value";
            ddCheckpoint.DisplayMember = "Text";
            ddCheckpoint.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            ddCheckpoint.Items.AddRange(checkpointItems);
            ddCheckpoint.SelectedItem = checkpointItems.Single(x => x.Value == Program.Config.StableDiffusionCheckpoint);

            ddInpaintingFill.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            ddInpaintingFill.Items.AddRange(Enum.GetNames<SdInpaintingFill>());
            ddInpaintingFill.SelectedIndex = 1;

            if (ddlSize.SelectedIndex == -1) ddlSize.SelectedIndex = 0;

            tbNegative.Text = Program.Config.NegativePrompt;

            foreach (var lora in SdLoraHelper.GetNames())
            {
                contextMenuCheckpoint.Items.Add("LoRA: " + SdLoraHelper.GetHumanName(lora), null, (_, args) =>
                {
                    tbPrompt.Text = SdLoraHelper.GetPrompt(lora) + ", " + tbPrompt.Text;
                });
            }

            ddlSampler.Items.Clear();
            ddlSampler.Items.Add("Euler a");
            ddlSampler.Items.Add("DPM++ 2M");
            ddlSampler.Items.Add("Heun");
            ddlSampler.SelectedItem = "DPM++ 2M";
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            if (tbPrompt.Text.Trim() == "") { tbPrompt.Focus(); return; }
            OnGenerate();
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            var parameters = new SdGenerationRequest();
            numCfgScale.Value = parameters.cfg_scale;
            numSteps.Value = parameters.steps;
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

            ddInpaintingFill.Enabled = pb.Image != null && pb.HasMask && cbUseInitImage.Checked;

            ddlSize.Enabled = !(pb.Image != null && cbUseInitImage.Checked);

            trackBarChangesLevel.Enabled = pb.Image != null;

            tbSeed.Enabled = cbUseSeed.Checked;
            tbVariationSeed.Enabled = cbUseSeed.Checked;
            trackBarSeedVariationStrength.Enabled = cbUseSeed.Checked;
        }

        private void ddCheckpoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)ddCheckpoint.SelectedItem).Value == Program.Config.StableDiffusionCheckpoint) return;

            Program.Config.StableDiffusionCheckpoint = ((ListItem)ddCheckpoint.SelectedItem).Value;
            Program.SaveConfig();
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

        private void btContextMenuCheckpoint_Click(object sender, EventArgs e)
        {
            contextMenuCheckpoint.Show(Cursor.Position);
        }
    }
}
