using System.Waf.Foundation;

namespace WpfPluginSample.AddressBookPlugin.Domain
{
    public class Contact : Model
    {
        private string firstname;
        private string lastname;
        string company;
        private string email;
        private string phone;
        
        public string Firstname
        {
            get { return firstname; }
            set { SetProperty(ref firstname, value); }
        }

        public string Lastname
        {
            get { return lastname; }
            set { SetProperty(ref lastname, value); }
        }

        public string Company
        {
            get { return company; }
            set { SetProperty(ref company, value); }
        }

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }
    }
}
