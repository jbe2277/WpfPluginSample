using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfPluginSample.Shell.Foundation
{
    public static class TaskHelper
    {
        public static Task Run(Action action, TaskScheduler scheduler)
        {
            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, scheduler);
        }
    }
}
