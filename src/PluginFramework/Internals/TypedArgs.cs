using System;

namespace WpfPluginSample.PluginFramework.Internals
{
    public class TypedArgs
    {
        public TypedArgs(int parentProcessId, string assemblyFile, string instanceName)
        {
            ParentProcessId = parentProcessId;
            AssemblyFile = assemblyFile;
            InstanceName = instanceName;
        }


        public int ParentProcessId { get; }

        public string AssemblyFile { get; }

        public string InstanceName { get; }


        public static TypedArgs FromArgs(string[] args)
        {
            int parentProcessId = Convert.ToInt32(args[0]);
            var assemblyFile = args[1];
            var name = args[2];
            return new TypedArgs(parentProcessId, assemblyFile, name);
        }

        public string ToArgs()
        {
            // Enclose the AssemblyFile within " chars because the path might contain spaces.
            return ParentProcessId + " \"" + AssemblyFile + "\" " + InstanceName;
        }
    }
}
