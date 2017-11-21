using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace WpfPluginSample.Shell.Applications.Services
{
    internal static class PluginMetadataReader
    {
        public static PluginInfo Read(string dllFile)
        {
            using (var stream = File.OpenRead(dllFile))
            using (var reader = new PEReader(stream))
            {
                var metadata = reader.GetMetadataReader();
                return ReadCore(dllFile, metadata);
            }
        }

        private static PluginInfo ReadCore(string dllFile, MetadataReader reader)
        {
            var pluginController = reader.TypeDefinitions.Select(x => (TypeDefinition?)reader.GetTypeDefinition(x))
                .FirstOrDefault(x => GetInterfaceImplementations(reader, (TypeDefinition)x).Any(y => y.Name == "IPluginController" && y.Namespace == "WpfPluginSample.PluginFramework"));
            if (pluginController == null)
            {
                return null;
            }

            var assembly = reader.GetAssemblyDefinition();
            var customAttributes = assembly.GetCustomAttributes().Select(reader.GetCustomAttribute).Select(x =>
            {
                var ctor = reader.GetMemberReference((MemberReferenceHandle)x.Constructor);
                var attrType = reader.GetTypeReference((TypeReferenceHandle)ctor.Parent);
                return new
                {
                    Name = attrType.Name.ToString(reader),
                    Value = string.Join(";", x.GetParameterValues(reader))
                };
            }).ToArray();

            var pluginControllerName = string.Join(".", new[]
            {
                pluginController.Value.Namespace.ToString(reader),
                pluginController.Value.Name.ToString(reader)
            }.Where(x => !string.IsNullOrEmpty(x)));

            return new PluginInfo(dllFile, pluginControllerName, 
                customAttributes.FirstOrDefault(x => x.Name == "AssemblyProductAttribute")?.Value,
                assembly.Version.ToString(), 
                customAttributes.FirstOrDefault(x => x.Name == "AssemblyCompanyAttribute")?.Value,
                customAttributes.FirstOrDefault(x => x.Name == "AssemblyCopyrightAttribute")?.Value);
        }

        private static IReadOnlyList<TypeIdentifier> GetInterfaceImplementations(MetadataReader reader, TypeDefinition typeDefinition)
        {
            return typeDefinition.GetInterfaceImplementations().Select(reader.GetInterfaceImplementation).Select(x =>
            {
                if (x.Interface.Kind == HandleKind.TypeReference)
                {
                    var interfaceType = reader.GetTypeReference((TypeReferenceHandle)x.Interface);
                    return new TypeIdentifier(interfaceType.Name.ToString(reader), interfaceType.Namespace.ToString(reader));
                }
                else if (x.Interface.Kind == HandleKind.TypeDefinition)
                {
                    var interfaceType = reader.GetTypeDefinition((TypeDefinitionHandle) x.Interface);
                    return new TypeIdentifier(interfaceType.Name.ToString(reader), interfaceType.Namespace.ToString(reader));
                }
                return new TypeIdentifier();
            }).Where(x => !string.IsNullOrEmpty(x.Name)).ToArray();
        }
    
        public static string ToString(this StringHandle handle, MetadataReader reader)
        {
            return handle.IsNil ? null : reader.GetString(handle);
        }

        public static ImmutableArray<string> GetParameterValues(this CustomAttribute customAttribute, MetadataReader reader)
        {
            if (customAttribute.Constructor.Kind != HandleKind.MemberReference) throw new InvalidOperationException();

            var ctor = reader.GetMemberReference((MemberReferenceHandle)customAttribute.Constructor);
            var provider = new StringParameterValueTypeProvider(reader, customAttribute.Value);
            var signature = ctor.DecodeMethodSignature(provider, null);
            return signature.ParameterTypes;
        }


        private sealed class StringParameterValueTypeProvider : ISignatureTypeProvider<string, object>
        {
            private readonly BlobReader valueReader;

            public StringParameterValueTypeProvider(MetadataReader reader, BlobHandle value)
            {
                Reader = reader;
                valueReader = reader.GetBlobReader(value);

                var prolog = valueReader.ReadUInt16();
                if (prolog != 1) throw new BadImageFormatException("Invalid custom attribute prolog.");
            }

            public MetadataReader Reader { get; }

            public string GetArrayType(string elementType, ArrayShape shape) => "";
            public string GetByReferenceType(string elementType) => "";
            public string GetFunctionPointerType(MethodSignature<string> signature) => "";
            public string GetGenericInstance(string genericType, ImmutableArray<string> typestrings) => "";
            public string GetGenericInstantiation(string genericType, ImmutableArray<string> typeArguments) { throw new NotImplementedException(); }
            public string GetGenericMethodParameter(int index) => "";
            public string GetGenericMethodParameter(object genericContext, int index) { throw new NotImplementedException(); }
            public string GetGenericTypeParameter(int index) => "";
            public string GetGenericTypeParameter(object genericContext, int index) { throw new NotImplementedException(); }
            public string GetModifiedType(string modifier, string unmodifiedType, bool isRequired) => "";
            public string GetPinnedType(string elementType) => "";
            public string GetPointerType(string elementType) => "";
            public string GetPrimitiveType(PrimitiveTypeCode typeCode)
            {
                if (typeCode == PrimitiveTypeCode.String) return valueReader.ReadSerializedString();
                return "";
            }
            public string GetSZArrayType(string elementType) => "";
            public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind) => "";
            public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind) => "";
            public string GetTypeFromSpecification(MetadataReader reader, object genericContext, TypeSpecificationHandle handle, byte rawTypeKind) => "";
        }

        private struct TypeIdentifier
        {
            public TypeIdentifier(string name, string nameSpace)
            {
                Name = name;
                Namespace = nameSpace;
            }

            public string Name { get; }
            public string Namespace { get; }
        }
    }
}
