using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace WpfPluginSample.PluginHost
{
    internal class RuntimeConfig
    {
        private static readonly XNamespace bindingNamespace = "urn:schemas-microsoft-com:asm.v1";

        private readonly XDocument document;
        private readonly XElement bindingElement;

        public RuntimeConfig()
        {
            bindingElement = new XElement(bindingNamespace + "assemblyBinding");

            document = new XDocument(
                new XElement("configuration",
                    new XElement("runtime",
                        bindingElement)));
        }

        public void AddCodeBaseFor(AssemblyName name)
        {
            bindingElement.Add(
                new XElement(bindingNamespace + "dependentAssembly",
                    new XElement(bindingNamespace + "assemblyIdentity",
                        new XAttribute("name", name.Name),
                        new XAttribute("publicKeyToken", FormatPublicKeyToken(name)),
                        new XAttribute("culture", FormatCulture(name))),
                    new XElement(bindingNamespace + "codeBase",
                        new XAttribute("version", name.Version.ToString()),
                        new XAttribute("href", name.CodeBase))));
        }

        public byte[] GetBytes()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                document.Save(writer);
                return stream.ToArray();
            }
        }

        private static string FormatCulture(AssemblyName assemblyName)
        {
            var culture = assemblyName.CultureInfo.Name;
            return string.IsNullOrEmpty(culture) ? "neutral" : culture;
        }

        private static string FormatPublicKeyToken(AssemblyName assemblyName)
        {
            var bytes = assemblyName.GetPublicKeyToken();
            if (bytes == null || bytes.Length == 0) return "null";
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
    }
}
