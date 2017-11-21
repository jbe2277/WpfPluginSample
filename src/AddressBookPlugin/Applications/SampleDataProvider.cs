using System.Collections.Generic;
using WpfPluginSample.AddressBookPlugin.Domain;

namespace WpfPluginSample.AddressBookPlugin.Applications
{
    public static class SampleDataProvider
    {
        public static IReadOnlyList<Contact> CreateContacts()
        {
            var contacts = new List<Contact>()
            {
                CreateContact("Jesper", "Aaberg", "jesper.aaberg@example.com", "(111) 555-0100", "A. Datum Corporation"),
                CreateContact("Lori", "Penor", "lori.penor@fabrikam.com", "(111) 555-0104", "Baldwin Museum of Science"),
                CreateContact("Michael", "Pfeiffer", "michael.pfeiffer@fabrikam.com", "(222) 555-0105", "Blue Yonder Airlines"),
                CreateContact("Terry", "Adams", "terry.adams@adventure-works.com", "(333) 555-0102", "Adventure Works"),
                CreateContact("Miles", "Reid", "miles.reid@adventure-works.com", "(444) 555-0123", "Adventure Works")
            };
            return contacts;
        }

        private static Contact CreateContact(string firstname, string lastname, string email, string phone, string company)
        {
            return new Contact { Firstname = firstname, Lastname = lastname, Email = email, Phone = phone, Company = company };
        }
    }
}
