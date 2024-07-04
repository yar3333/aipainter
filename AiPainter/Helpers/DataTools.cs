namespace AiPainter.Helpers;

static class DataTools
{
    public static bool IsSequencesEqual<T>(IEnumerable<T> a, IEnumerable<T> b)
    {
        return !a.Except(b).Any() && !b.Except(a).Any();
    }
}
