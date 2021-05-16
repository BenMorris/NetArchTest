namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Mono.Cecil;

    /// <summary>
    /// Extensions for the <see cref="Type"/> class.
    /// </summary>
    static internal class TypeExtensions
    {
        /// <summary>
        /// Converts the value to a <see cref="TypeDefinition"/> instance.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The converted value.</returns>
        public static TypeDefinition ToTypeDefinition(this Type type)
        {
            // Get the assembly using reflection
            var assembly = Assembly.GetAssembly(type);

            // Load the assembly into the Mono.Cecil library
            var assemblyDef = AssemblyDefinition.ReadAssembly(assembly.Location);

            // Find the matching type
            var dependencies = (assemblyDef.Modules
                .SelectMany(t => t.Types)
                .Where(t => t.IsClass && t.Namespace != null && t.FullName.Equals(type.FullName, StringComparison.InvariantCultureIgnoreCase)));

            return dependencies.FirstOrDefault();
        }
    }
}
