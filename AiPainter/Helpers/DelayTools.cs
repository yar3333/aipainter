namespace AiPainter.Helpers;

static class DelayTools
{
    public static bool WaitForExit(int ms)
    {
        var n = ms / 200;
        for (var i = 0; i < n; i++)
        {
            if (Application.OpenForms.Count == 0) return true;
            Thread.Sleep(200);
        }
        return false;
    }    
    
    public static async Task<bool> WaitForExitAsync(int ms, CancellationToken? cancellationToken = null)
    {
        var n = ms / 200;
        for (var i = 0; i < n; i++)
        {
            if (Application.OpenForms.Count == 0) return true;
            if (cancellationToken != null) await Task.Delay(200, cancellationToken.Value);
            else                           await Task.Delay(200);
        }
        return false;
    }
}
