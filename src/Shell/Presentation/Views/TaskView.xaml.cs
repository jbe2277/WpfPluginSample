using System.ComponentModel.Composition;
using WpfPluginSample.Shell.Applications.Views;

namespace WpfPluginSample.Shell.Presentation.Views
{
    [Export(typeof(ITaskView))]
    public partial class TaskView : ITaskView
    {
        public TaskView()
        {
            InitializeComponent();
        }
    }
}
