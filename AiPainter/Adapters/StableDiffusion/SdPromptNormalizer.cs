using System.Globalization;
using System.Text.RegularExpressions;

namespace AiPainter.Adapters.StableDiffusion;

static class SdPromptNormalizer
{
    public class PhraseAndWeight
    {
        public string phrase;
        public decimal weight;
    }

    public static PhraseAndWeight[] Parse(string text)
    {
        var r = new List<PhraseAndWeight>();

        var matches = Regex.Matches(text, @"([-_a-zA-Z0-9/#*$%& \t\r\n'""]+)(:\s*[-+]?\s*\d+(?:[.]\d+)?)?");
        foreach (Match match in matches)
        {
            var weight = match.Groups[2].Success 
                             ? decimal.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture) 
                             : detectWeight(text, match.Index);
            if (match.Groups[1].Value.Trim() != "") r.Add(new PhraseAndWeight
            {
                phrase = match.Groups[1].Value.Trim(),
                weight = weight
            });
        }

        return r.ToArray();
    }

    public static PhraseAndWeight[] Parse(IEnumerable<string>? texts)
    {
        if (texts == null) return Array.Empty<PhraseAndWeight>();
        return Parse(string.Join(", ", texts));
    }

    public static string[] GetNormalizedPhrases(IEnumerable<PhraseAndWeight> items)
    {
        return items.Select(x =>
        {
            if (x.weight == 1.0m) return x.phrase;
            if (x.weight == 1.1m) return "(" + x.phrase + ")";
            if (x.weight == 1.1m*1.1m) return "((" + x.phrase + "))";
            if (x.weight == 0.9m) return "[" + x.phrase + "]";
            if (x.weight == 0.9m*0.9m) return "[[" + x.phrase + "]]";
            return "(" + x.phrase + ":" + x.weight.ToString(CultureInfo.InvariantCulture) + ")";

        }).ToArray();
    }

    public static string[] GetNormalizedPhrases(IEnumerable<string>? texts)
    {
        return GetNormalizedPhrases(Parse(texts));
    }
    
    public static string[] GetNormalizedPhrases(string text)
    {
        return GetNormalizedPhrases(Parse(text));
    }

    private static decimal detectWeight(string text, int index)
    {
        return 1.0m;
    }
}
