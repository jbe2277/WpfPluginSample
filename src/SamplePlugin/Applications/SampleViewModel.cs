using System.Waf.Applications;
using System.Windows.Input;
using WpfPluginSample.SamplePlugin.Properties;

namespace WpfPluginSample.SamplePlugin.Applications
{
    public class SampleViewModel : ViewModel<ISampleView>
    {
        private string taskSubject;

        public SampleViewModel(ISampleView view) : base(view)
        {
        }

        public string TestApplicationSetting => Settings.Default.TestSetting;

        public ICommand ShowDialogCommand { get; set; }
        
        public string TaskSubject
        {
            get { return taskSubject; }
            set { SetProperty(ref taskSubject, value); }
        }

        public ICommand BlockUIThreadCommand { get; set; }

        public ICommand ExceptionUIThreadCommand { get; set; }

        public ICommand ExceptionBackgroundThreadCommand { get; set; }

        public ICommand ExceptionTaskCommand { get; set; }

        public ICommand RunGarbageCollectorCommand { get; set; }
    }
}
