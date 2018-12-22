namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Mono.Cecil;

    internal static class TypeDefinitionExtensions
    {
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

        private static IEnumerable<TypeDefinition> EnumerateBaseClasses(this TypeDefinition classType)
        {
            for (var typeDefinition = classType; typeDefinition != null; typeDefinition = typeDefinition.BaseType?.Resolve())
            {
                yield return typeDefinition;
            }
        }

        public static Type ToType(this TypeDefinition typeDefinition)
        {
            // Nested types have a forward slash that should be replaced with "+"
            var fullName = typeDefinition.FullName.Replace("/", "+");
            return Type.GetType(string.Concat(fullName, ", ", typeDefinition.Module.Assembly.FullName), true);
        }
    }
}
