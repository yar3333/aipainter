using AiPainter.Helpers;
using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;
using AiPainter.SiteClients.CivitaiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdEmbeddingStuff;

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
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            labUrlError.Text = "";

            var url = tbUrl.Text;
            if (string.IsNullOrWhiteSpace(url)) { tbUrl.Focus(); return; }

            var uri = new Uri(url);

            var m1 = Regex.Match(uri.PathAndQuery, @"models/(\d+)\?modelVersionId=(\d+)");
            if (m1.Success)
            {
                btImport.Enabled = false;
                tabs.Enabled = false;
                modelId = m1.Groups[1].Value;
                versionId = m1.Groups[2].Value;
                importModel();
                return;
            }

            var m2 = Regex.Match(uri.LocalPath, @"models/(\d+)");
            if (m2.Success)
            {
                btImport.Enabled = false;
                tabs.Enabled = false;
                modelId = m2.Groups[1].Value;
                versionId = null;
                importModel();
                return;
            }

            tbUrl.Focus();
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
            var model = await CivitaiClient.GetModelAsync(modelId!);

            if (versionId == null)
            {
                versionId = model.modelVersions.FirstOrDefault()?.id.ToString();
                if (versionId == null) return;
            }

            var version = model.modelVersions.Single(x => x.id.ToString() == versionId);

            Invoke(() =>
            {
                switch (model.type)
                {
                    case "Checkpoint":       importCheckpoint(model, version); break;
                    case "LORA":             importLora(model, version); break;
                    case "LoCon":            importLora(model, version); break;
                    case "TextualInversion": importEmbedding(model, version); break;
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

            tbCheckpointName.Text = ImportModelHelper.GetCheckpointName(model.name, version.name);

            tbCheckpointRequiredPrompt.Text = "";
            tbCheckpointRequiredPrompt.Text = "";
            if (version.trainedWords != null)
            {
                ImportModelHelper.ParsePhrases(string.Join(", ", version.trainedWords), out var reqWords, out var sugWords);
                tbCheckpointRequiredPrompt.Text = reqWords;
                tbCheckpointSuggestedPrompt.Text = sugWords;
            }

            tbCheckpointDescription.Text = "";
            if (model.tags != null)
            {
                tbCheckpointDescription.Text = string.Join(", ", model.tags);
            }

            tbCheckpointMainUrl.Text = ImportModelHelper.GetBestModelDownloadUrl(version.files, "Model");
            tbCheckpointInpaintUrl.Text = ImportModelHelper.GetInpaintDownloadUrl(model, version);
            tbCheckpointVaeUrl.Text = ImportModelHelper.GetBestModelDownloadUrl(version.files, "VAE");

            numCheckpointClipSkip.Value = 0;
        }

        private void importLora(CivitaiModel model, CivitaiVersion version)
        {
            tabs.SelectedTab = tabLora;

            tbLoraName.Text = ImportModelHelper.GetLoraName(model.name, version.name);

            tbLoraRequiredPrompt.Text = "";
            tbLoraSuggestedPrompt.Text = "";
            if (version.trainedWords != null)
            {
                ImportModelHelper.ParsePhrases(string.Join(", ", version.trainedWords), out var reqWords, out var sugWords);
                tbLoraRequiredPrompt.Text = reqWords;
                tbLoraSuggestedPrompt.Text = sugWords;
            }

            tbLoraDescription.Text = "";
            if (model.tags != null)
            {
                tbLoraDescription.Text = string.Join(", ", model.tags);
            }

            tbLoraDownloadUrl.Text = ImportModelHelper.GetBestModelDownloadUrl(version.files, "Model");
        }

        private void importEmbedding(CivitaiModel model, CivitaiVersion version)
        {
            tabs.SelectedTab = tabEmbedding;

            tbEmbeddingName.Text = ImportModelHelper.GetEmbeddingName(model.name, version.name);

            tbEmbeddingDescription.Text = "";
            if (model.tags != null)
            {
                tbEmbeddingDescription.Text = string.Join(", ", model.tags);
            }

            tbEmbeddingDownloadUrl.Text = ImportModelHelper.GetBestModelDownloadUrl(version.files, "Model");

            cbEmbeddingIsNegative.Checked = model.name.ToLowerInvariant().Contains("negative")
                || (model.tags?.Contains("negative") ?? false)
                || (model.tags?.Contains("negative embedding") ?? false);
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
                overrideSettings = numCheckpointClipSkip.Value == 0 ? null : new SdSettings
                {
                    CLIP_stop_at_last_layers = (int)numCheckpointClipSkip.Value
                },
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
                labCheckpointNameError.Text = Directory.Exists(SdCheckpointsHelper.GetDirPath(tbCheckpointName.Text.Trim())) ? "already exists" : "";
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
                labLoraNameError.Text = SdLoraHelper.IsConfigExist(tbLoraName.Text.Trim()) ? "already exists" : "";
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
                labEmbeddingNameError.Text = SdEmbeddingHelper.IsConfigExist(tbEmbeddingName.Text.Trim()) ? "already exists" : "";
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
