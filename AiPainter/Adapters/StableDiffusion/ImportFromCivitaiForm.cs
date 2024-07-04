﻿using AiPainter.Helpers;
using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;
using AiPainter.Adapters.StableDiffusion.SdLoraStuff;
using AiPainter.SiteClients.CivitaiClientStuff;

namespace AiPainter.Adapters.StableDiffusion
{
    public partial class ImportFromCivitaiForm : Form
    {
        private string? modelId;
        private string? versionId;

        public ImportFromCivitaiForm()
        {
            InitializeComponent();
        }

        private void ImportFromCivitaiForm_Load(object sender, EventArgs e)
        {
            labUrlError.Text = "";
            labCheckpointNameError.Text = "";
            labLoraNameError.Text = "";
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
                    case "Checkpoint":
                        importCheckpoint(model, version);
                        break;

                    case "LORA":
                        importLora(model, version);
                        break;

                    default:
                        tbUrl.Focus();
                        break;
                }
            });
        }

        private void importCheckpoint(CivitaiModel model, CivitaiVersion version)
        {
            tabs.SelectedTab = tabCheckpoint;

            version.name = DataTools.TrimEndString(version.name, "+ VAE");
            version.name = DataTools.TrimEndString(version.name, "+VAE");

            tbCheckpointName.Text = DataTools.UnderscoresToCapitalisation(DataTools.SanitizeText(model.name)) 
                                  + "-" + DataTools.SanitizeText(version.name);

            tbCheckpointPrompt.Text = "";
            if (version.trainedWords != null)
            {
                tbCheckpointPrompt.Text = string.Join(", ", version.trainedWords);
            }

            tbCheckpointDescription.Text = "";
            if (model.tags != null)
            {
                tbCheckpointDescription.Text = string.Join(", ", model.tags);
            }

            tbCheckpointMainUrl.Text = "";
            tbCheckpointVaeUrl.Text = "";
            if (version.files != null)
            {
                foreach (var file in version.files)
                {
                    switch (file.type)
                    {
                        case "Model":
                            tbCheckpointMainUrl.Text = file.downloadUrl;
                            break;

                        case "VAE":
                            tbCheckpointVaeUrl.Text = file.downloadUrl;
                            break;
                    }
                }
            }

            tbCheckpointInpaintUrl.Text = "";
            var possibleInpaintVersions = model.modelVersions.Where(x => x.name.ToLowerInvariant().Contains("inpaint")).ToArray();
            if (possibleInpaintVersions.Length == 1)
            {
                tbCheckpointInpaintUrl.Text = possibleInpaintVersions[0].downloadUrl;
            }

            numCheckpointClipSkip.Value = 0;
        }

        private void importLora(CivitaiModel model, CivitaiVersion version)
        {
            tabs.SelectedTab = tabLora;

            model.name = processLoraNameAndDetectForModels(model.name, out var forModelNames);

            tbLoraName.Text = DataTools.UnderscoresToCapitalisation(DataTools.SanitizeText(model.name)) 
                            + "_for_" + string.Join('_', forModelNames)
                            + "-" + DataTools.SanitizeText(version.name);

            tbLoraPrompt.Text = "";
            if (version.trainedWords != null)
            {
                tbLoraPrompt.Text = string.Join(", ", version.trainedWords);
            }

            tbLoraDescription.Text = "";
            if (model.tags != null)
            {
                tbLoraDescription.Text = string.Join(", ", model.tags);
            }

            tbLoraDownloadUrl.Text = "";
            if (version.files != null)
            {
                foreach (var file in version.files)
                {
                    switch (file.type)
                    {
                        case "Model":
                            tbLoraDownloadUrl.Text = file.downloadUrl;
                            break;
                    }
                }
            }
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
                prompt = tbCheckpointPrompt.Text.Trim(),
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
                labCheckpointNameError.Text = Directory.Exists(SdCheckpointsHelper.GetDirPath(tbCheckpointName.Text.Trim())) 
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
                prompt = tbLoraPrompt.Text.Trim(),
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
                labLoraNameError.Text = SdLoraHelper.IsConfigExist(tbLoraName.Text.Trim()) 
                                            ? "already exists" 
                                            : "";
            }
            catch
            {
                labLoraNameError.Text = "bad name";
            }
        }

        private static string processLoraNameAndDetectForModels(string name, out string[] forModelNames)
        {
            var r = new List<string>();

            if (name.StartsWith("[Pony]"))
            {
                name = name.Substring("[Pony]".Length).Trim();
                r.Add("PonyXL");
            }

            if (name.StartsWith("[PonyXL]"))
            {
                name = name.Substring("[PonyXL]".Length).Trim();
                r.Add("PonyXL");
            }

            if (name.Contains("Pony")) r.Add("PonyXL");

            forModelNames = r.Distinct().OrderBy(x => x).ToArray();

            return name;
        }
    }
}