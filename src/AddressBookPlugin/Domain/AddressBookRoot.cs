using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Foundation;

namespace WpfPluginSample.AddressBookPlugin.Domain
{
    public class AddressBookRoot : Model
    {
        private readonly ObservableCollection<Contact> contacts;
        
        public AddressBookRoot()
        {
            contacts = new ObservableCollection<Contact>();
        }
        
        public IReadOnlyList<Contact> Contacts => contacts;
        
        public void AddContact(Contact contact)
        {
            contacts.Add(contact);
        }

        public void RemoveContact(Contact contact)
        {
            contacts.Remove(contact);
        }
    }
}
