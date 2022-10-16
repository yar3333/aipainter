using System.Diagnostics;

namespace AiPainter.Helpers;

static class ProcessHelper
{
    public static Process? RunInBackground(string exeFilePath, string arguments, Action<string?> logFunc, Action<int>? onExit = null, string? directory = null, IDictionary<string, string>? env = null)
    {
        var fullPathToExe = directory != null
                                ? Path.GetFullPath(exeFilePath, directory)
                                : Path.GetFullPath(exeFilePath);
        if (string.IsNullOrEmpty(fullPathToExe)) { logFunc("RUN FAIL: program '" + exeFilePath + "' not found."); return null; }

        logFunc("RUN: " + fullPathToExe + " " + arguments);

        var p = new Process();

        p.StartInfo.FileName = fullPathToExe;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.Arguments = arguments;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        if (!string.IsNullOrEmpty(directory)) p.StartInfo.WorkingDirectory = directory;
        if (env != null) foreach (var k in env.Keys) p.StartInfo.EnvironmentVariables[k] = env[k];

        p.OutputDataReceived += (_, x) => logFunc(x.Data);
        p.ErrorDataReceived += (_, x) => logFunc(x.Data);

        p.Start();

        _ = Task.Run(() =>
        {
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
            var r = p.ExitCode;
            p.Close();
            onExit?.Invoke(r);
        });

        return p;
    }

    public static void OpenUrlInBrowser(string url)
    {
        Process.Start
        (
            new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            }
        );
    }
}