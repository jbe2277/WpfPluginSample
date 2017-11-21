using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using WpfPluginSample.PluginFramework.Internals;
using WpfPluginSample.Shell.Foundation;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.Shell.Applications.RemoteServices
{
    [Export]
    public class AddressBookService : RemoteService, IAddressBookService
    {
        private readonly TaskScheduler taskScheduler;
        
        public AddressBookService()
        {
            taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }
        
        public Action<ContactDto> SelectContactCallback { get; set; }

        public Action<ContactDto> ContactDeletedCallback { get; set; }
        
        public void SelectContact(ContactDto contact)
        {
            TaskHelper.Run(() => SelectContactCallback?.Invoke(contact), taskScheduler);
        }

        public void ContactDeleted(ContactDto contact)
        {
            TaskHelper.Run(() => ContactDeletedCallback?.Invoke(contact), taskScheduler);
        }
    }
}
