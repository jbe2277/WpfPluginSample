using System;
using System.Globalization;
using System.Threading;
using WpfPluginSample.PluginFramework;
using WpfPluginSample.SamplePlugin.Applications;
using WpfPluginSample.SamplePlugin.Presentation;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.SamplePlugin
{
    public class PluginController : IPluginController
    {
        private ILogService logService;
        private IEventAggregator eventAggregator;
        private SampleController sampleController;
        
        public void Initialize()
        {
            logService = RemoteServiceLocator.GetService<ILogService>();
            eventAggregator = RemoteServiceLocator.GetService<IEventAggregator>();
            logService.Message("Initialize", true);

            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");
            logService.Message("UICulture " + Thread.CurrentThread.CurrentUICulture, true);
        }

        public object CreateMainView()
        {
            var viewModel = new SampleViewModel(new SampleView());
            sampleController = new SampleController(logService, eventAggregator, viewModel, () => new DialogViewModel(new DialogWindow()));
            sampleController.Initialize();
            return viewModel.View;
        }

        public void Shutdown()
        {
            logService.Message("Shutdown", true);
            sampleController.Shutdown();
        }
    }
}
