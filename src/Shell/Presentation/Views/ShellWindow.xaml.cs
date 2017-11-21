using System.ComponentModel.Composition;
using WpfPluginSample.Shell.Applications.Views;

namespace WpfPluginSample.Shell.Presentation.Views
{
    [Export(typeof(IShellView))]
    public partial class ShellWindow : IShellView
    {
        public ShellWindow()
        {
            InitializeComponent();
        }
    }
}
