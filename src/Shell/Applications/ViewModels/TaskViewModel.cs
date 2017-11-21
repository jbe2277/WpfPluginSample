using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using WpfPluginSample.Shell.Applications.RemoteServices;
using WpfPluginSample.Shell.Applications.Views;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.Shell.Applications.ViewModels
{
    [Export]
    public class TaskViewModel : ViewModel<ITaskView>
    {
        private string subject = "My first task";
        private ContactDto assignedTo;
        
        [ImportingConstructor]
        public TaskViewModel(ITaskView view, AddressBookService addressBookService) : base(view)
        {
            addressBookService.SelectContactCallback = SelectContact;
            addressBookService.ContactDeletedCallback = ContactDeleted;
        }
        
        public string Subject
        {
            get { return subject; }
            set { SetProperty(ref subject, value); }
        }
        
        public ContactDto AssignedTo
        {
            get { return assignedTo; }
            set { SetProperty(ref assignedTo, value); }
        }

        public ICommand UpdateTaskCommand { get; set; }
        
        private void SelectContact(ContactDto contact)
        {
            AssignedTo = contact;
        }

        private void ContactDeleted(ContactDto contactDto)
        {
            if (AssignedTo?.Firstname == contactDto.Firstname
                && AssignedTo?.Lastname == contactDto.Lastname
                && AssignedTo?.Email == contactDto.Email)
            {
                AssignedTo = null;
            }
        }
    }
}
