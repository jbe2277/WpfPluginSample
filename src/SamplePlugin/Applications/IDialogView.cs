using System.Waf.Applications;

namespace WpfPluginSample.SamplePlugin.Applications
{
    public interface IDialogView : IView
    {
        bool? ShowDialog();

        void Close();
    }
}
