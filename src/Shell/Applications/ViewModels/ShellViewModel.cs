using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using WpfPluginSample.Shell.Applications.Services;
using WpfPluginSample.Shell.Applications.Views;

namespace WpfPluginSample.Shell.Applications.ViewModels
{
    [Export]
    public class ShellViewModel : ViewModel<IShellView>
    {
        private PluginInfo selectedPlugin;
        private object selectedPluginView;

        [ImportingConstructor]
        public ShellViewModel(IShellView view) : base(view)
        {
        }

        public ICommand LoadCommand { get; set; }

        public ICommand UnloadCommand { get; set; }

        public IReadOnlyList<PluginInfo> Plugins { get; set; }
        
        public PluginInfo SelectedPlugin 
        {
            get { return selectedPlugin; }
            set { SetProperty(ref selectedPlugin, value); }
        }

        public IReadOnlyList<object> PluginViews { get; set; }

        public object SelectedPluginView
        {
            get { return selectedPluginView; }
            set { SetProperty(ref selectedPluginView, value); }
        }
        
        public object LogView { get; set; }

        public object TaskView { get; set; }
        
        public void Show()
        {
            ViewCore.Show();
        }
    }
}
