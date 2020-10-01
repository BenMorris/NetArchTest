namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Mono.Cecil;

    static internal class TypeReferenceExtensions
    {
        /// <summary>
        /// Tests whether a Type is a non-nullable value type
        /// </summary>
        /// <param name="typeReference">The class to test.</param>
        /// <returns>An indication of whether the type has any memebers that are non-nullable value types</returns>
        public static bool IsNullable(this TypeReference typeReference)
        {
            return !typeReference.IsValueType || typeReference.Resolve().ToType() == typeof(System.Nullable<>);
        }

        /// <summary>
        /// Returns namespace of the given type, if the type is nested, namespace of containing type is returned instead
        /// </summary>        
        public static string GetNamespace(this TypeReference typeReference)
        {
            if (typeReference.IsNested)
            {
                return typeReference?.DeclaringType.FullName;
            }
            return typeReference.Namespace;
        }
    }
}