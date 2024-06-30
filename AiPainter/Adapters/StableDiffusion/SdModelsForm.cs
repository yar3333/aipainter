using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class SdModelsForm : Form
    {
        public SdModelsForm()
        {
            InitializeComponent();
        }

        private void SdModelsForm_Load(object sender, EventArgs e)
        {
            var checkpointNames = SdCheckpointsHelper.GetNames("").Where(x => x != "").ToArray();

            foreach (var name in checkpointNames)
            {
                lvModels.Items.Add(new ListViewItem(new[]
                {
                    "",
                    "Checkpoint",
                    name,
                    SdCheckpointsHelper.GetConfig(name).description,
                    SdCheckpointsHelper.GetConfig(name).homeUrl ?? ""
                }));
            }

        }

        private void btOk_Click(object sender, EventArgs e)
        {

        }
    }
}
