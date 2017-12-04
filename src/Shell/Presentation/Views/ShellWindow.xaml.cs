using System.ComponentModel.Composition;
using System.Windows;
using WpfPluginSample.Shell.Applications.Views;

namespace WpfPluginSample.Shell.Presentation.Views
{
    [Export(typeof(IShellView))]
    public partial class ShellWindow : IShellView
    {
        public ShellWindow()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            pluginsBox.Focus();
        }
    }
}
