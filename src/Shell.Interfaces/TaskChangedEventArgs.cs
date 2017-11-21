using System;

namespace WpfPluginSample.Shell.Interfaces
{
    [Serializable]
    public class TaskChangedEventArgs : EventArgs
    {
        public TaskChangedEventArgs(string subject, ContactDto assignedTo)
        {
            Subject = subject;
            AssignedTo = assignedTo;
        }

        public string Subject { get; }

        public ContactDto AssignedTo { get; }
    }
}
