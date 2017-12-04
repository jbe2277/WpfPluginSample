using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.SamplePlugin.Applications
{
    internal class SampleController
    {
        private readonly ILogService logService;
        private readonly IEventAggregator eventAggregator;
        private readonly SampleViewModel sampleViewModel;
        private readonly Func<DialogViewModel> dialogViewModelFactory;
        private readonly DelegateCommand showDialogCommand;
        private readonly DelegateCommand blockUIThreadCommand;
        private readonly DelegateCommand exceptionUIThreadCommand;
        private readonly DelegateCommand exceptionBackgroundThreadCommand;
        private readonly DelegateCommand exceptionTaskCommand;
        private readonly DelegateCommand runGarbageCollectorCommand;

        public SampleController(ILogService logService, IEventAggregator eventAggregator, SampleViewModel sampleViewModel, Func<DialogViewModel> dialogViewModelFactory)
        {
            this.logService = logService;
            this.eventAggregator = eventAggregator;
            this.sampleViewModel = sampleViewModel;
            this.dialogViewModelFactory = dialogViewModelFactory;
            showDialogCommand = new DelegateCommand(ShowDialog);
            blockUIThreadCommand = new DelegateCommand(BlockUIThread);
            exceptionUIThreadCommand = new DelegateCommand(ExceptionUIThread);
            exceptionBackgroundThreadCommand = new DelegateCommand(ExceptionBackgroundThread);
            exceptionTaskCommand = new DelegateCommand(ExceptionTask);
            runGarbageCollectorCommand = new DelegateCommand(RunGarbageCollector);
        }
        
        public void Initialize()
        {
            eventAggregator.GetEvent<TaskChangedEventArgs>().ObserveOnDispatcher().Subscribe(TaskChangedHandler);
            sampleViewModel.ShowDialogCommand = showDialogCommand;
            sampleViewModel.BlockUIThreadCommand = blockUIThreadCommand;
            sampleViewModel.ExceptionUIThreadCommand = exceptionUIThreadCommand;
            sampleViewModel.ExceptionBackgroundThreadCommand = exceptionBackgroundThreadCommand;
            sampleViewModel.ExceptionTaskCommand = exceptionTaskCommand;
            sampleViewModel.RunGarbageCollectorCommand = runGarbageCollectorCommand;
        }

        public void Shutdown()
        {
        }

        private void TaskChangedHandler(TaskChangedEventArgs e)
        {
            sampleViewModel.TaskSubject = e.Subject;
        }

        private void ShowDialog()
        {
            logService.Message("Show dialog", true);
            // Note: This does not behave as modal dialog because it cannot attach to the parent window (Shell) which comes from another process.
            dialogViewModelFactory().ShowDialog();
        }

        private void BlockUIThread()
        {
            logService.Message("Block UI thread", true);
            Thread.Sleep(5000);
        }

        private void ExceptionUIThread()
        {
            logService.Message("Exception on UI thread", true);
            throw new InvalidOperationException("Exception on UI thread.");
        }

        private void ExceptionBackgroundThread()
        {
            logService.Message("Exception on background thread", true);
            ThreadPool.QueueUserWorkItem(state => { throw new InvalidOperationException("Exception on background thread."); });
        }

        private void ExceptionTask()
        {
            logService.Message("Exception on task", true);
            Task.Factory.StartNew(() => { throw new InvalidOperationException("Exception on task."); });
        }

        private void RunGarbageCollector()
        {
            logService.Message("Run garbage collector", true);
            GC.Collect();
        }
    }
}
