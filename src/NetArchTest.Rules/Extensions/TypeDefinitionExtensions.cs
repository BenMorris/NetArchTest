namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Mono.Cecil;

    /// <summary>
    /// Extensions for the <see cref="TypeDefinition"/> class.
    /// </summary>
    static internal class TypeDefinitionExtensions
    {
        /// <summary>
        /// Tests whether one class inherits from another.
        /// </summary>
        /// <param name="child">The class that is inheriting from the parent.</param>
        /// <param name="parent">The parent that is inherited.</param>
        /// <returns>An indication of whether the child inherits from the parent.</returns>
        public static bool IsSubclassOf(this TypeDefinition child, TypeDefinition parent)
        {
            if (parent != null)
            {
                return child.MetadataToken
                    != parent.MetadataToken
                    && child.EnumerateBaseClasses().Any(b => b.MetadataToken == parent.MetadataToken);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Enumerate the base classes throughout the chain of inheritence.
        /// </summary>
        /// <param name="classType">The class to enumerate.</param>
        /// <returns>The enumeration of base classes.</returns>
        private static IEnumerable<TypeDefinition> EnumerateBaseClasses(this TypeDefinition classType)
        {
            for (var typeDefinition = classType; typeDefinition != null; typeDefinition = typeDefinition.BaseType?.Resolve())
            {
                yield return typeDefinition;
            }
        }

        /// <summary>
        /// Convert the definition to a <see cref="Type"/> object instance.
        /// </summary>
        /// <param name="typeDefinition">The type definition to convert.</param>
        /// <returns>The equivalent <see cref="Type"/> object instance.</returns>
        public static Type ToType(this TypeDefinition typeDefinition)
        {
            // Nested types have a forward slash that should be replaced with "+"
            var fullName = typeDefinition.FullName.Replace("/", "+");
            return Type.GetType(string.Concat(fullName, ", ", typeDefinition.Module.Assembly.FullName), true);
        }

        /// <summary>
        /// Tests whether a class is immutable, i.e. all public fields are readonly and properties have no set method
        /// </summary>
        /// <param name="typeDefinition">The class to test.</param>
        /// <returns>An indication of whether the type is immutable</returns>
        public static bool IsImmutable(this TypeDefinition typeDefinition)
        {
            var propertiesAreReadonly = typeDefinition.Properties.All(p => p.IsReadonly());
            var fieldsAreReadonly = typeDefinition.Fields.All(f => f.IsReadonly());
            return propertiesAreReadonly && fieldsAreReadonly;
        }
    }
}
