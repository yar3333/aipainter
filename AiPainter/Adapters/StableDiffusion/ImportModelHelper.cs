using System.Text.RegularExpressions;
using AiPainter.SiteClients.CivitaiClientStuff;

namespace AiPainter.Adapters.StableDiffusion;

static class ImportModelHelper
{
    public static string GetCheckpointName(string modelName, string versionName)
    {
        versionName = TrimEndString(versionName, "+ VAE");
        versionName = TrimEndString(versionName, "+VAE");
        versionName = Regex.Replace(versionName, @"^V(\d)", "v$1");

        return UnderscoresToCapitalisation(SanitizeText(modelName))
                              + "-" + SanitizeText(versionName);
    }

    public static string GetLoraName(string modelName, string versionName)
    {
        modelName = processLoraNameAndDetectForModels(modelName, out var forModelNames);

        return UnderscoresToCapitalisation(SanitizeText(modelName))
             + "_for_" + string.Join('_', forModelNames)
             + "-" + SanitizeText(versionName);
    }

    public static string GetEmbeddingName(string modelName, string versionName)
    {
        return modelName != versionName
                   ? SanitizeText(modelName) + "_" + SanitizeText(versionName)
                   : SanitizeText(versionName);
    }

    public static string GetInpaintDownloadUrl(CivitaiModel model, CivitaiVersion version)
    {
        var vv1 = model.modelVersions.Where(x => x.name.ToLowerInvariant().Contains("inpaint")).ToArray();
        if (vv1.Length == 1) return GetBestModelDownloadUrl(vv1[0].files, "Model");

        var vv2 = vv1.Where(x => x.name.ToLowerInvariant().StartsWith(version.name.ToLowerInvariant() + "-inpaint")).ToArray();
        if (vv2.Length == 1) return GetBestModelDownloadUrl(vv2[0].files, "Model");

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
        return files?.FirstOrDefault(x => x.type == type && x.metadata?.format == "SafeTensor")?.downloadUrl
            ?? files?.FirstOrDefault(x => x.type == type && x.metadata?.format == "PickleTensor")?.downloadUrl;
    }

    private static string SanitizeText(string? s)
    {
        if (s == null) return "";
        s = s.Replace("'", "");
        s = Regex.Replace(s, "[^-_a-zA-Z0-9.]+", "_");
        s = Regex.Replace(s, "_+", "_");
        s = s.Trim('_');
        return s;
    }

    private static string UnderscoresToCapitalisation(string s)
    {
        s = Regex.Replace(s, "_[a-zA-Z]", m => m.Value.Substring(1).ToUpperInvariant());
        return s;
    }

    private static string TrimEndString(string text, string end)
    {
        if (text.Length > end.Length && text.EndsWith(end))
        {
            text = text.Substring(0, text.Length - end.Length);
        }
        return text;
    }
}
