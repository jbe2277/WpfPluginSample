using System.Waf.Applications;

namespace WpfPluginSample.Shell.Applications.Views
{
    public interface ILogView : IView
    {
        void AppendOutputText(string text);

        void AppendErrorText(string text);
    }
}
