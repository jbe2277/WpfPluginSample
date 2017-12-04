using System.ComponentModel;
using System.Waf.Applications;
using WpfPluginSample.AddressBookPlugin.Domain;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.AddressBookPlugin.Applications
{
    internal class ContactController
    {
        private readonly ILogService logService;
        private readonly IAddressBookService addressBookService;
        private readonly ContactListViewModel viewModel;
        private readonly DelegateCommand selectContactCommand;
        private readonly DelegateCommand deleteContactCommand;
        private readonly AddressBookRoot root;
        
        public ContactController(ILogService logService, IAddressBookService addressBookService, ContactListViewModel contactListViewModel)
        {
            this.logService = logService;
            this.addressBookService = addressBookService;
            viewModel = contactListViewModel;
            selectContactCommand = new DelegateCommand(SelectContact, CanSelectContact);
            deleteContactCommand = new DelegateCommand(DeleteContact, CanDeleteContact);
            root = new AddressBookRoot();
            contactListViewModel.PropertyChanged += ContactListViewModelPropertyChanged;
        }
        
        public void Initialize()
        {
            viewModel.SelectContactCommand = selectContactCommand;
            viewModel.DeleteContactCommand = deleteContactCommand;
            foreach (var contact in SampleDataProvider.CreateContacts()) { root.AddContact(contact); }
            viewModel.Contacts = root.Contacts;
        }

        public void Shutdown()
        {
        }

        private bool CanSelectContact()
        {
            return viewModel.SelectedContact != null;
        }

        private void SelectContact()
        {
            logService.Message("Select contact", true);
            addressBookService.SelectContact(GetDto(viewModel.SelectedContact));
        }

        private bool CanDeleteContact()
        {
            return viewModel.SelectedContact != null;
        }

        private void DeleteContact()
        {
            var contactToDelete = viewModel.SelectedContact;
            root.RemoveContact(contactToDelete);
            logService.Message("Delete contact", true);
            addressBookService.ContactDeleted(GetDto(contactToDelete));
        }

        private static ContactDto GetDto(Contact contact)
        {
            return new ContactDto(contact.Firstname, contact.Lastname, contact.Email);
        }

        private void ContactListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContactListViewModel.SelectedContact))
            {
                selectContactCommand.RaiseCanExecuteChanged();
                deleteContactCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
