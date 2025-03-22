using System.Management;

namespace AiPainter.Helpers;

static class GpuInfo
{
    public static bool IsNvidia => GetAdapterCompatibility() == "NVIDIA";
    public static bool IsAmd => GetName().StartsWith("AMD");

    public static string GetName()
    {
        using var searcher = new ManagementObjectSearcher("select * from Win32_VideoController");

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["Name"]?.ToString() ?? "";
        }

        return "";
    }

    public static string GetAdapterCompatibility()
    {
        using var searcher = new ManagementObjectSearcher("select * from Win32_VideoController");

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["AdapterCompatibility"]?.ToString() ?? "";
        }

        return "";
    }
}
