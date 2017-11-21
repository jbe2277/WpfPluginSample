using System;

namespace WpfPluginSample.Shell.Applications.Services
{
    [Serializable]
    public class PluginInfo
    {
        public PluginInfo(string assemblyFile, string pluginControllerName, string name, string version, string company, string copyright)
        {
            AssemblyFile = assemblyFile;
            PluginControllerName = pluginControllerName;
            Name = name;
            Version = version;
            Company = company;
            Copyright = copyright;
        }
        
        public string AssemblyFile { get; }

        public string PluginControllerName { get; }

        public string Name { get; }

        public string Version { get; }

        public string Company { get; }

        public string Copyright { get; }
    }
}
