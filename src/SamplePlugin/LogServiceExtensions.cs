using System.Diagnostics;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.SamplePlugin
{
    internal static class LogServiceExtensions
    {
        public static string Prefix => "SamplePlugin (" + Process.GetCurrentProcess().Id + "): ";

        public static void Message(this ILogService logService, string text, bool usePrefix)
        {
            logService.Message(usePrefix ? Prefix + text : text);
        }

        public static void Error(this ILogService logService, string text, bool usePrefix)
        {
            logService.Error(usePrefix ? Prefix + text : text);
        }
    }
}
