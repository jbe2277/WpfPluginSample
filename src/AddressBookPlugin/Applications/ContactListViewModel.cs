using System.Collections.Generic;
using System.Waf.Applications;
using System.Windows.Input;
using WpfPluginSample.AddressBookPlugin.Domain;

namespace WpfPluginSample.AddressBookPlugin.Applications
{
    public class ContactListViewModel : ViewModel<IContactListView>
    {
        private Contact selectedContact;
        
        public ContactListViewModel(IContactListView view) : base(view)
        {
        }
        
        public IReadOnlyList<Contact> Contacts { get; set; }

        public Contact SelectedContact
        {
            get { return selectedContact; }
            set { SetProperty(ref selectedContact, value); }
        }
        
        public ICommand SelectContactCommand { get; set; }
        
        public ICommand DeleteContactCommand { get; set; }
    }
}
