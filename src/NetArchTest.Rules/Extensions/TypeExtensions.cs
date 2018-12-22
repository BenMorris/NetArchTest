namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Mono.Cecil;

    internal static class TypeExtensions
    {
        public static TypeDefinition ToTypeDefinition(this Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            var uri = new UriBuilder(assembly.CodeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var assemblyDef = AssemblyDefinition.ReadAssembly(path);

            var dependencies = (assemblyDef.Modules
                .SelectMany(t => t.Types)
                .Where(t => t.IsClass && t.Namespace != null && t.FullName.Equals(type.FullName)));

            return dependencies.FirstOrDefault();
        }
    }
}
