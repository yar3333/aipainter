using AiPainter.Helpers;
using AiPainter.Controls;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class StableDiffusionPanel : UserControl
    {
        public Action OnGenerate = null!;

        public StableDiffusionPanel()
        {
            InitializeComponent();
        }

        private void collapsablePanel_Load(object sender, EventArgs e)
        {
            var checkpoints = SdCheckpointsHelper.GetNames();
            if (!checkpoints.Contains(Program.Config.StableDiffusionCheckpoint))
            {
                checkpoints = new[] { Program.Config.StableDiffusionCheckpoint }.Concat(checkpoints).ToArray();
            }
            var checkpointItems = checkpoints.Select(x => new ListItem
            {
                Value = x, 
                Text = x + " (" + Math.Round(SdCheckpointsHelper.GetSize(x) / 1024.0 / 1024 / 1024, 1) + " GB)"
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
            ddInpaintingFill.SelectedIndex = 0;
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

        public void UpdateState(SmartPictureBox pb, bool isPortOpen)
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

            btGenerate.Text =         isPortOpen ? "Generate" 
                : StableDiffusionProcess.Loading ? "LOADING..." 
                                                 : "ERROR";

            ddCheckpoint.Enabled = StableDiffusionProcess.Loading;

            ddInpaintingFill.Enabled = pb.Image != null && pb.HasMask && cbUseInitImage.Checked;
        }

        private void ddCheckpoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)ddCheckpoint.SelectedItem).Value == Program.Config.StableDiffusionCheckpoint) return;

            Program.Config.StableDiffusionCheckpoint = ((ListItem)ddCheckpoint.SelectedItem).Value;
            Program.SaveConfig();

            Task.Run(async () =>
            {
                StableDiffusionProcess.Stop();
                while (ProcessHelper.IsPortOpen(Program.Config.StableDiffusionUrl)) await Task.Delay(1000);
                await Task.Delay(1000);
                StableDiffusionProcess.Start();
            });
        }
    }
}
