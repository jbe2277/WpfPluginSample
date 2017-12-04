using WpfPluginSample.AddressBookPlugin.Applications;
using WpfPluginSample.AddressBookPlugin.Presentation;
using WpfPluginSample.PluginFramework;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.AddressBookPlugin
{
    public class PluginController : IPluginController
    {
        private ILogService logService;
        private IAddressBookService addressBookService;
        private ContactController contactController;
        
        public void Initialize()
        {
            logService = RemoteServiceLocator.GetService<ILogService>();
            addressBookService = RemoteServiceLocator.GetService<IAddressBookService>();
            logService.Message("Initialize", true);
        }

        public object CreateMainView()
        {
            var viewModel = new ContactListViewModel(new ContactListView());
            contactController = new ContactController(logService, addressBookService, viewModel);
            contactController.Initialize();
            return viewModel.View;
        }

        public void Shutdown()
        {
            logService.Message("Shutdown", true);
            contactController.Shutdown();
        }
    }
}
