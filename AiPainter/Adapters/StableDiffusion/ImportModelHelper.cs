using System.Text.RegularExpressions;
using AiPainter.SiteClients.CivitaiClientStuff;

namespace AiPainter.Adapters.StableDiffusion;

static class ImportModelHelper
{
    public static string GetCheckpointName(string modelName, string versionName)
    {
        return UnderscoresToCapitalisation(SanitizeText(modelName))
                              + "-" + SanitizeText(trimCheckpointVersionName(versionName));
    }

    private static string trimCheckpointVersionName(string versionName)
    {
        versionName = Regex.Replace(versionName, @"\s*[+]\s*VAE$", "");
        versionName = Regex.Replace(versionName, @"\s*[(]VAE[)]$", "");
        versionName = Regex.Replace(versionName, @"^v(\d)", "$1", RegexOptions.IgnoreCase);
        return versionName;
    }

    public static string GetLoraOrEmbeddingName(CivitaiModel model, CivitaiVersion version)
    {
        var modelName = processLoraNameAndDetectForModels(model.name, out var forModelNames);

        if (!string.IsNullOrEmpty(version.baseModel))
        {
            forModelNames = forModelNames.Concat(new []{ version.baseModel }).Distinct().ToArray();
        }

        forModelNames = forModelNames.Select(x => x.Replace(" ", "")).ToArray();

        return UnderscoresToCapitalisation(SanitizeText(modelName))
             + (forModelNames.Length > 0 ? "_for_" + string.Join('_', forModelNames) : "")
             + "-" + SanitizeText(version.name);
    }

    public static string GetInpaintDownloadUrl(CivitaiModel model, CivitaiVersion version)
    {
        var vv1 = model.modelVersions.Where(x => x.name.ToLowerInvariant().Contains("inpaint")).ToArray();
        if (vv1.Length == 1) return GetBestModelDownloadUrl(vv1[0].files, "Model");

        var vv2 = vv1.Where(x => x.name.ToLowerInvariant().StartsWith(version.name.ToLowerInvariant() + "-inpaint")).ToArray();
        if (vv2.Length == 1) return GetBestModelDownloadUrl(vv2[0].files, "Model");

        var trimmedVn = trimCheckpointVersionName(version.name).ToLowerInvariant();
        
        var vv3 = vv1.Where(x => x.name.ToLowerInvariant().StartsWith(trimmedVn + "-inpaint")).ToArray();
        if (vv3.Length == 1) return GetBestModelDownloadUrl(vv3[0].files, "Model");
        
        var vv4 = vv1.Where(x => x.name.ToLowerInvariant().StartsWith(trimmedVn + " inpaint")).ToArray();
        if (vv4.Length == 1) return GetBestModelDownloadUrl(vv4[0].files, "Model");

        return "";
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

    public static void ParsePhrases(string text, out string phrasesOutsideSquareBrackets, out string phrasesInsideSquareBrackets)
    {
        var phrasesInsideSquareBracketsList = new List<string>();
        text = Regex.Replace(text, @"\[([^]]+)\]", m =>
        {
            phrasesInsideSquareBracketsList.AddRange(m.Groups[1].Value.Trim().Split(','));
            return "";
        });

        phrasesInsideSquareBrackets  = string.Join(", ", phrasesInsideSquareBracketsList .Select(x => x.Trim()).Where(x => x != ""));
        phrasesOutsideSquareBrackets = string.Join(", ", text.Split(',').Select(x => x.Trim()).Where(x => x != ""));
    }

    public static string GetBestModelDownloadUrl(CivitaiFile[]? files, string type)
    {
        switch (type)
        {
            case "Model":
                return GetBestModelDownloadUrlInner(files, "Pruned Model") 
                    ?? GetBestModelDownloadUrlInner(files, "Model") 
                    ?? "";
            default:
                return GetBestModelDownloadUrlInner(files, type) ?? "";
        }
    }

    private static string? GetBestModelDownloadUrlInner(CivitaiFile[]? files, string type)
    {
        return files?.FirstOrDefault(x => x.type == type && x.metadata?.format == "SafeTensor"   && x.metadata?.size == "pruned")?.downloadUrl
            ?? files?.FirstOrDefault(x => x.type == type && x.metadata?.format == "SafeTensor")?.downloadUrl
            ?? files?.FirstOrDefault(x => x.type == type && x.metadata?.format == "PickleTensor" && x.metadata?.size == "pruned")?.downloadUrl
            ?? files?.FirstOrDefault(x => x.type == type && x.metadata?.format == "PickleTensor")?.downloadUrl;
    }

    private static string SanitizeText(string? s)
    {
        if (s == null) return "";
        s = s.Replace("'", "");
        s = Regex.Replace(s, "[^_a-zA-Z0-9.]+", "_");
        s = Regex.Replace(s, "_+", "_");
        s = s.Trim('_');
        return s;
    }

    private static string UnderscoresToCapitalisation(string s)
    {
        s = Regex.Replace(s, "_[a-zA-Z]", m => m.Value.Substring(1).ToUpperInvariant());
        return s;
    }
}
