using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using WpfPluginSample.PluginFramework;
using WpfPluginSample.Shell.Applications.RemoteServices;
using WpfPluginSample.Shell.Applications.Services;
using WpfPluginSample.Shell.Applications.ViewModels;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.Shell.Applications.Controllers
{
    [Export(typeof(IModuleController))]
    internal class ModuleController : IModuleController
    {
        private readonly Lazy<ShellViewModel> shellViewModel;
        private readonly Lazy<LogViewModel> logViewModel;
        private readonly Lazy<TaskViewModel> taskViewModel;
        private readonly Lazy<LogService> logService;
        private readonly Lazy<AddressBookService> addressBookService;
        private readonly Lazy<IEventAggregator> eventAggregator;
        private readonly PluginManager pluginManager;
        private readonly ObservableCollection<PluginInfo> plugins;
        private readonly DelegateCommand loadCommand;
        private readonly DelegateCommand unloadCommand;
        private readonly DelegateCommand updateTaskCommand;
            
        [ImportingConstructor]
        public ModuleController(Lazy<ShellViewModel> shellViewModel, Lazy<LogViewModel> logViewModel, Lazy<TaskViewModel> taskViewModel, 
            Lazy<LogService> logService, Lazy<AddressBookService> addressBookService, Lazy<IEventAggregator> eventAggregator)
        {
            this.shellViewModel = shellViewModel;
            this.logViewModel = logViewModel;
            this.taskViewModel = taskViewModel;
            this.logService = logService;
            this.addressBookService = addressBookService;
            this.eventAggregator = eventAggregator;
            pluginManager = new PluginManager();
            plugins = new ObservableCollection<PluginInfo>();
            loadCommand = new DelegateCommand(Load, CanLoad);
            unloadCommand = new DelegateCommand(Unload, CanUnload);
            updateTaskCommand = new DelegateCommand(UpdateTask);
        }

        private ShellViewModel ShellViewModel => shellViewModel.Value;

        private LogViewModel LogViewModel => logViewModel.Value;

        private TaskViewModel TaskViewModel => taskViewModel.Value;

        public void Initialize()
        {
            RemoteServiceLocator.InitializeIpcChannel(RemoteServiceLocator.GetIpcPortName<IAddressBookService>());
            RemoteServiceLocator.RegisterInstance<IAddressBookService>(addressBookService.Value);
            RemoteServiceLocator.RegisterInstance<ILogService>(logService.Value);
            RemoteServiceLocator.RegisterInstance<IEventAggregator>((MarshalByRefObject)eventAggregator.Value);
            
            ShellViewModel.Plugins = plugins;
            ShellViewModel.LoadCommand = loadCommand;
            ShellViewModel.UnloadCommand = unloadCommand;
            ShellViewModel.LogView = LogViewModel.View;
            ShellViewModel.TaskView = TaskViewModel.View;
            TaskViewModel.UpdateTaskCommand = updateTaskCommand;
            ShellViewModel.PropertyChanged += ShellViewModelPropertyChanged;
        }

        public void Run()
        {
            Discover();
            ShellViewModel.Show();
        }

        public void Shutdown()
        {
            pluginManager.Dispose();
        }

        private async void Discover()
        {
            var newPlugins = await pluginManager.DiscoverAsync();
            foreach (var plugin in newPlugins)
            {
                plugins.Add(plugin);
            }
            ShellViewModel.SelectedPlugin = plugins.FirstOrDefault();
        }

        private bool CanLoad()
        {
            return ShellViewModel.SelectedPlugin != null;
        }

        private void Load()
        {
            ShellViewModel.RemoteView = pluginManager.Load(ShellViewModel.SelectedPlugin);
        }

        private bool CanUnload()
        {
            return ShellViewModel.SelectedPlugin != null;
        }

        private void Unload()
        {
            pluginManager.Unload(ShellViewModel.SelectedPlugin);
        }

        private void UpdateTask()
        {
            eventAggregator.Value.Publish(new TaskChangedEventArgs(TaskViewModel.Subject, TaskViewModel.AssignedTo));
        }

        private void ShellViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShellViewModel.SelectedPlugin))
            {
                loadCommand.RaiseCanExecuteChanged();
                unloadCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
