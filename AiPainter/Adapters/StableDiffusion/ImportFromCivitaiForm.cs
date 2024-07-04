using AiPainter.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using AiPainter.Adapters.StableDiffusion.SdApiClientStuff;
using AiPainter.Adapters.StableDiffusion.SdCheckpointStuff;


namespace AiPainter.Adapters.StableDiffusion
{
    public partial class ImportFromCivitaiForm : Form
    {
        private static readonly Log Log = new("civitai");

        private string? modelId;
        private string? versionId;

        public ImportFromCivitaiForm()
        {
            InitializeComponent();
        }

        private void ImportFromCivitaiForm_Load(object sender, EventArgs e)
        {
            tbCivitaiApiKey.Text = Program.Config.CivitaiApiKey;

            labCheckpointNameError.Text = "";

            tabCheckpoint.Enabled = false;
            tabLora.Enabled = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrlInBrowser("https://civitai.com/user/account");
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            var url = tbUrl.Text;
            if (string.IsNullOrWhiteSpace(url)) { tbUrl.Focus(); return; }

            if (string.IsNullOrWhiteSpace(tbCivitaiApiKey.Text)) { tbCivitaiApiKey.Focus(); return; }

            if (Program.Config.CivitaiApiKey != tbCivitaiApiKey.Text)
            {
                Program.Config.CivitaiApiKey = tbCivitaiApiKey.Text;
                Program.SaveConfig();
            }

            var uri = new Uri(url);

            var m1 = Regex.Match(uri.PathAndQuery, @"models/(\d+)\?modelVersionId=(\d+)");
            if (m1.Success)
            {
                btImport.Enabled = false;
                modelId = m1.Groups[1].Value;
                versionId = m1.Groups[2].Value;
                importModel();
                return;
            }

            var m2 = Regex.Match(uri.LocalPath, @"models/(\d+)");
            if (m2.Success)
            {
                btImport.Enabled = false;
                modelId = m1.Groups[1].Value;
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
                var model = await getAsync<JsonObject>("models/" + modelId);

                if (versionId == null)
                {
                    versionId = model["modelVersions"]?.AsArray()[0]?.AsObject()["id"]?.ToString();
                    if (versionId == null) return;
                }

                var version = await getAsync<JsonObject>("model-versions/" + versionId);

                Invoke(() =>
                {
                    if (model != null)
                    {
                        switch (model["type"]?.ToString())
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
                    }
                    else
                    {
                        tbUrl.Focus();
                    }

                    btImport.Enabled = true;
                });
            });
        }

        private void importCheckpoint(JsonObject model, JsonObject version)
        {
            tabs.SelectedTab = tabCheckpoint;
            tabCheckpoint.Enabled = true;
            tabLora.Enabled = false;

            tbCheckpointName.Text = DataTools.UnderscoresToCapitalisation(DataTools.SanitizeText(model["name"]?.ToString())) + "-" + DataTools.SanitizeText(version["name"]?.ToString());

            tbCheckpointPrompt.Text = "";
            var keywords = version["trainedWords"]?.AsArray();
            if (keywords != null)
            {
                tbCheckpointPrompt.Text = string.Join(", ", keywords.Select(x => x?.ToString() ?? "").Where(x => x != ""));
            }

            tbCheckpointDescription.Text = "";
            var tags = model["tags"]?.AsArray();
            if (tags != null)
            {
                tbCheckpointDescription.Text = string.Join(", ", tags.Select(x => x?.ToString() ?? "").Where(x => x != ""));
            }

            var files = version["files"]?.AsArray();
            if (files != null)
            {
                foreach (var file in files)
                {
                    switch (file?.AsObject()["type"]?.ToString())
                    {
                        case "Model":
                            tbCheckpointMainUrl.Text = file.AsObject()["downloadUrl"]?.ToString() ?? "";
                            break;

                        case "VAE":
                            tbCheckpointVaeUrl.Text = file.AsObject()["downloadUrl"]?.ToString() ?? "";
                            break;
                    }
                }
            }
        }

        private void importLora(JsonObject model, JsonObject version)
        {
            tabs.SelectedTab = tabLora;
            tabCheckpoint.Enabled = false;
            tabLora.Enabled = true;
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
            }
            else
            {
                btCheckpointOk.Text = "ERROR";
            }

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                Invoke(() =>
                {
                    btCheckpointOk.Enabled = true;
                    btCheckpointOk.Text = saveBtOkName;
                });
            });

        }

        private static async Task<T> postAsync<T>(string url, object request)
        {
            using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log));
            httpClient.BaseAddress = new Uri("https://civitai.com/api/v1/");
            httpClient.Timeout = TimeSpan.FromMinutes(1);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.Config.CivitaiApiKey);

            var raw = await httpClient.PostAsync(url, JsonContent.Create(request, null, new JsonSerializerOptions { PropertyNamingPolicy = null, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));
            return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
        }

        private static async Task<T> getAsync<T>(string url)
        {
            using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log));
            httpClient.BaseAddress = new Uri("https://civitai.com/api/v1/");
            httpClient.Timeout = TimeSpan.FromMinutes(1);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.Config.CivitaiApiKey);

            var raw = await httpClient.GetAsync(url);
            return JsonSerializer.Deserialize<T>(await raw.Content.ReadAsStringAsync())!;
        }

        private static async Task<string> getStringAsync(string url)
        {
            using var httpClient = new HttpClient(new LoggerHttpClientHandler(Log));
            httpClient.BaseAddress = new Uri("https://civitai.com/api/v1/");
            httpClient.Timeout = TimeSpan.FromMinutes(1);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.Config.CivitaiApiKey);

            var raw = await httpClient.GetAsync(url);
            return await raw.Content.ReadAsStringAsync();
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
                if (Directory.Exists(SdCheckpointsHelper.GetDirPath(tbCheckpointName.Text.Trim())))
                {
                    labCheckpointNameError.Text = "already exists";
                }
                else
                {
                    labCheckpointNameError.Text = "";
                }
            }
            catch (Exception e)
            {
                labCheckpointNameError.Text = "bad name";
            }
        }
    }
}
