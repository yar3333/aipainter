using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;
using AiPainter.Adapters.StableDiffusion.SdVaeStuff;
using AiPainter.Helpers;
using AiPainter.SiteClients.CivitaiClientStuff;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class AddImportModelForm : Form
    {
        private string? modelId;
        private string? versionId;

        public AddImportModelForm()
        {
            InitializeComponent();
        }

        private void ImportFromCivitaiForm_Load(object sender, EventArgs e)
        {
            labUrlError.Text = "";
            labCheckpointNameError.Text = "";
            labLoraNameError.Text = "";
            labEmbeddingNameError.Text = "";

            ddCheckpointPredefinedVae.DataSource = SdVaeHelper.GetListItems();
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            labUrlError.Text = "";

            if (CivitaiHelper.ParseUrl(tbUrl.Text, out modelId, out versionId))
            {
                btImport.Enabled = false;
                tabs.Enabled = false;
                importModel();
            }
            else
            {
                tbUrl.Focus();
            }
        }

        private void importModel()
        {
            Task.Run(async () =>
            {
                try
                {
                    await importModelInner();
                }
                catch (Exception e)
                {
                    Invoke(() =>
                    {
                        labUrlError.Text = e.Message;
                    });
                }

                Invoke(() =>
                {
                    tabs.Enabled = true;
                    btImport.Enabled = true;
                });
            });
        }

        private async Task importModelInner()
        {
            var modelAndVersion = await CivitaiHelper.LoadModelDataAsync(modelId!, versionId);
            if (modelAndVersion == null)
                return;

            var model = modelAndVersion.Item1;
            var version = modelAndVersion.Item2;

            Invoke(() =>
            {
                switch (model.type)
                {
                    case "Checkpoint":
                        importCheckpoint(model, version);
                        break;
                    case "LORA":
                        importLora(model, version);
                        break;
                    case "LoCon":
                        importLora(model, version);
                        break;
                    case "TextualInversion":
                        importEmbedding(model, version);
                        break;
                    default:
                        labUrlError.Text = "unsupported model type: " + model.type;
                        tbUrl.Focus();
                        break;
                }
            });
        }

        private void importCheckpoint(CivitaiModel model, CivitaiVersion version)
        {
            tabs.SelectedTab = tabCheckpoint;

            ddCheckpointPredefinedVae.SelectedValue = "";

            tbCheckpointName.Text = CivitaiParserHelper.GetCheckpointName(model.name, version.name);

            var config = CivitaiHelper.DataToCheckpointConfig(model, version);
            tbCheckpointRequiredPrompt.Text = config.promptRequired;
            tbCheckpointSuggestedPrompt.Text = config.promptSuggested;
            tbCheckpointDescription.Text = config.description;
            tbCheckpointMainUrl.Text = config.mainCheckpointUrl;
            tbCheckpointInpaintUrl.Text = config.inpaintCheckpointUrl;
            tbCheckpointVaeUrl.Text = config.vaeUrl;
            numCheckpointClipSkip.Value = config.clipSkip ?? 1;
            tbCheckpointBaseModel.Text = config.baseModel;
        }

        private void importLora(CivitaiModel model, CivitaiVersion version)
        {
            tabs.SelectedTab = tabLora;

            tbLoraName.Text = CivitaiParserHelper.GetLoraOrEmbeddingName(model, version);

            var config = CivitaiHelper.DataToLoraConfig(model, version);

            tbLoraRequiredPrompt.Text = config.promptRequired;
            tbLoraSuggestedPrompt.Text = config.promptSuggested;
            tbLoraDescription.Text = config.description;
            tbLoraDownloadUrl.Text = config.downloadUrl;
            tbLoraBaseModel.Text = config.baseModel;
        }

        private void importEmbedding(CivitaiModel model, CivitaiVersion version)
        {
            tabs.SelectedTab = tabEmbedding;

            tbEmbeddingName.Text = CivitaiParserHelper.GetLoraOrEmbeddingName(model, version);

            var config = CivitaiHelper.DataToEmbeddingConfig(model, version);

            tbEmbeddingDescription.Text = config.description;
            tbEmbeddingDownloadUrl.Text = config.downloadUrl;
            tbEmbeddingBaseModel.Text = config.baseModel;
            cbEmbeddingIsNegative.Checked = config.isNegative;
        }

        private void btCheckpointOk_Click(object sender, EventArgs e)
        {
            if (tbCheckpointName.Text.Trim() == "")
            {
                tbCheckpointName.Focus();
                return;
            }

            if (tbCheckpointMainUrl.Text.Trim() == "")
            {
                tbCheckpointMainUrl.Focus();
                return;
            }

            var config = new SdCheckpointConfig
            {
                homeUrl = "https://civitai.com/models/" + modelId + "?modelVersionId=" + versionId,
                mainCheckpointUrl = tbCheckpointMainUrl.Text.Trim(),
                inpaintCheckpointUrl = tbCheckpointInpaintUrl.Text.Trim(),
                vaeUrl = tbCheckpointVaeUrl.Text.Trim(),
                promptRequired = tbCheckpointRequiredPrompt.Text.Trim(),
                promptSuggested = tbCheckpointSuggestedPrompt.Text.Trim(),
                description = tbCheckpointDescription.Text.Trim(),
                baseModel = tbCheckpointBaseModel.Text.Trim(),
                usePredefinedVae = (string)ddCheckpointPredefinedVae.SelectedValue!,
                clipSkip =
                    numCheckpointClipSkip.Value == 1 ? null : (int)numCheckpointClipSkip.Value,
            };

            var saveBtOkName = btCheckpointOk.Text;
            if (SdCheckpointsHelper.SaveConfig(tbCheckpointName.Text.Trim(), config))
            {
                tbCheckpointName_TextChanged(null, null);
                btCheckpointOk.Enabled = false;

                btCheckpointOk.Text = "SUCCESS";
                tbUrl.Text = "";
            }
            else
            {
                btCheckpointOk.Text = "ERROR";
            }

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                Invoke(() =>
                {
                    btCheckpointOk.Enabled = true;
                    btCheckpointOk.Text = saveBtOkName;
                });
            });
        }

        private void tbCheckpointName_TextChanged(object sender, EventArgs _)
        {
            if (tbCheckpointName.Text.Trim() == "")
            {
                labCheckpointNameError.Text = "";
                return;
            }

            try
            {
                labCheckpointNameError.Text = Directory.Exists(
                    SdCheckpointsHelper.GetDirPath(tbCheckpointName.Text.Trim())
                )
                    ? "already exists"
                    : "";
            }
            catch
            {
                labCheckpointNameError.Text = "bad name";
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrlInBrowser("https://civitai.com");
        }

        private void btLoraOk_Click(object sender, EventArgs e)
        {
            if (tbLoraName.Text.Trim() == "")
            {
                tbLoraName.Focus();
                return;
            }

            if (tbLoraDownloadUrl.Text.Trim() == "")
            {
                tbLoraDownloadUrl.Focus();
                return;
            }

            var config = new SdLoraConfig
            {
                homeUrl = "https://civitai.com/models/" + modelId + "?modelVersionId=" + versionId,
                downloadUrl = tbLoraDownloadUrl.Text.Trim(),
                promptRequired = tbLoraRequiredPrompt.Text.Trim(),
                promptSuggested = tbLoraSuggestedPrompt.Text.Trim(),
                description = tbLoraDescription.Text.Trim(),
                baseModel = tbLoraBaseModel.Text.Trim(),
            };

            var saveBtOkName = btLoraOk.Text;
            if (SdLoraHelper.SaveConfig(tbLoraName.Text.Trim(), config))
            {
                tbLoraName_TextChanged(null, null);
                btLoraOk.Enabled = false;

                btLoraOk.Text = "SUCCESS";
                tbUrl.Text = "";
            }
            else
            {
                btLoraOk.Text = "ERROR";
            }

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                Invoke(() =>
                {
                    btLoraOk.Enabled = true;
                    btLoraOk.Text = saveBtOkName;
                });
            });
        }

        private void tbLoraName_TextChanged(object sender, EventArgs e)
        {
            if (tbLoraName.Text.Trim() == "")
            {
                labLoraNameError.Text = "";
                return;
            }

            try
            {
                labLoraNameError.Text = SdLoraHelper.IsConfigExist(tbLoraName.Text.Trim())
                    ? "already exists"
                    : "";
            }
            catch
            {
                labLoraNameError.Text = "bad name";
            }
        }

        private void tbEmbeddingName_TextChanged(object sender, EventArgs e)
        {
            if (tbEmbeddingName.Text.Trim() == "")
            {
                labEmbeddingNameError.Text = "";
                return;
            }

            try
            {
                labEmbeddingNameError.Text = SdEmbeddingHelper.IsConfigExist(
                    tbEmbeddingName.Text.Trim()
                )
                    ? "already exists"
                    : "";
            }
            catch
            {
                labEmbeddingNameError.Text = "bad name";
            }
        }

        private void btEmbeddingOk_Click(object sender, EventArgs e)
        {
            if (tbEmbeddingName.Text.Trim() == "")
            {
                tbEmbeddingName.Focus();
                return;
            }

            if (tbEmbeddingDownloadUrl.Text.Trim() == "")
            {
                tbEmbeddingDownloadUrl.Focus();
                return;
            }

            var config = new SdEmbeddingConfig
            {
                homeUrl = "https://civitai.com/models/" + modelId + "?modelVersionId=" + versionId,
                downloadUrl = tbEmbeddingDownloadUrl.Text.Trim(),
                description = tbEmbeddingDescription.Text.Trim(),
                baseModel = tbEmbeddingBaseModel.Text.Trim(),
                isNegative = cbEmbeddingIsNegative.Checked,
            };

            var saveBtOkName = btEmbeddingOk.Text;
            if (SdEmbeddingHelper.SaveConfig(tbEmbeddingName.Text.Trim(), config))
            {
                tbEmbeddingName_TextChanged(null, null);
                btEmbeddingOk.Enabled = false;

                btEmbeddingOk.Text = "SUCCESS";
                tbUrl.Text = "";
            }
            else
            {
                btEmbeddingOk.Text = "ERROR";
            }

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                Invoke(() =>
                {
                    btEmbeddingOk.Enabled = true;
                    btEmbeddingOk.Text = saveBtOkName;
                });
            });
        }
    }
}
