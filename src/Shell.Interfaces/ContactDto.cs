using System;

namespace WpfPluginSample.Shell.Interfaces
{
    [Serializable]
    public class ContactDto
    {
        public ContactDto(string firstname, string lastname, string email)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
        }
        
        public string Firstname { get; }

        public string Lastname { get; }

        public string Email { get; }
        
        public override string ToString()
        {
            return Firstname + " " + Lastname;
        }
    }
}
