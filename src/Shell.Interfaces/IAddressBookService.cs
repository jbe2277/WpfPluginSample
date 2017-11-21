namespace WpfPluginSample.Shell.Interfaces
{
    public interface IAddressBookService
    {
        void SelectContact(ContactDto contact);

        void ContactDeleted(ContactDto contact);
    }
}
