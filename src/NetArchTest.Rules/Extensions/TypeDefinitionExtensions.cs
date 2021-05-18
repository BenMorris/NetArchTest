namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Mono.Cecil;
    using System.Runtime.CompilerServices;
    using Mono.Collections.Generic;

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
                return !child.IsSameTypeAs(parent)
                       && child.EnumerateBaseClasses().Any(b => b.IsSameTypeAs(parent));
            }

            return false;
        }

        /// <summary>
        /// Tests whether two type definitions are from the same assembly.
        /// The comparison is based on the full assembly names.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>An indication of whether the both types are from the same assembly.</returns>
        public static bool IsFromSameAssemblyAs(this TypeDefinition a, TypeDefinition b)
        {
            return a.Module.Assembly.ToString() == b.Module.Assembly.ToString();
        }

        /// <summary>
        /// Tests whether the provided types are the same type.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>An indication of whether the types are the same.</returns>
        public static bool IsSameTypeAs(this TypeDefinition a, TypeDefinition b)
        {
            return a.IsFromSameAssemblyAs(b) && a.MetadataToken == b.MetadataToken;
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
            // C++ template instantiations contain comma separator for template arguments,
            // getting address operators and pointer type designations which should be prefixed by backslash
            var fullName = typeDefinition.FullName.Replace("/", "+")
                .Replace(",", "\\,")
                .Replace("&", "\\&")
                .Replace("*", "\\*");
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

        /// <summary>
        /// Tests whether a Type has any memebers that are non-nullable value types
        /// </summary>
        /// <param name="typeDefinition">The class to test.</param>
        /// <returns>An indication of whether the type has any memebers that are non-nullable value types</returns>
        public static bool HasNullableMembers(this TypeDefinition typeDefinition)
        {
            var propertiesAreNullable = typeDefinition.Properties.All(p => p.IsNullable());
            var fieldsAreNullable = typeDefinition.Fields.All(f => f.IsNullable());
            return propertiesAreNullable && fieldsAreNullable;
        }

        public static bool IsCompilerGenerated(this TypeDefinition typeDefinition)
        {
            return typeDefinition.CustomAttributes.Any(x => x?.AttributeType?.FullName == typeof(CompilerGeneratedAttribute).FullName);
        }

        /// <summary>
        /// Returns namespace of the given type, if the type is nested, namespace of containing type is returned instead
        /// </summary>        
        /// <remarks>
        /// For nested classes this will take the name of the declaring class. See https://github.com/BenMorris/NetArchTest/issues/73
        /// </remarks>
        public static string GetNamespace(this TypeDefinition typeDefinition)
        {
            if ((typeDefinition.IsNestedPrivate) || (typeDefinition.IsNestedPublic))
            {
                return typeDefinition.DeclaringType.FullName;
            }
            return typeDefinition.Namespace;
        }
    }
}
