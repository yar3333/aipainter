using System.Linq;
using System.Text.RegularExpressions;

namespace AiPainter.Helpers;

static class DataTools
{
    public static bool IsSequencesEqual<T>(IEnumerable<T> a, IEnumerable<T> b)
    {
        return !a.Except(b).Any() && !b.Except(a).Any();
    }

    public static string SanitizeText(string? s)
    {
        if (s == null) return "";
        s = s.Replace("'", "");
        s = Regex.Replace(s, "[^-_a-zA-Z0-9.]+", "_");
        s = Regex.Replace(s, "_+", "_");
        s = s.Trim('_');
        return s;
    }

    public static string UnderscoresToCapitalisation(string s)
    {
        s = Regex.Replace(s, "_[a-zA-Z]", m => m.Value.Substring(1).ToUpperInvariant());
        return s;
    }

    public static string TrimEndString(string text, string end)
    {
        if (text.Length > end.Length && text.EndsWith(end))
        {
            text = text.Substring(0, text.Length - end.Length);
        }
        return text;
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
}
