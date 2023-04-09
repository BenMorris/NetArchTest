namespace NetArchTest.Rules.Extensions
{
    using Mono.Cecil;

    /// <summary>
    /// Extensions for the <see cref="TypeReference"/> class.
    /// </summary>
    internal static class TypeReferenceExtensions
    {
        /// <summary>
        /// Tests whether a Type is a non-nullable value type
        /// </summary>
        /// <param name="typeReference">The class to test.</param>
        /// <returns>An indication of whether the type has any members that are non-nullable value types</returns>
        public static bool IsNullable(this TypeReference typeReference)
            => !typeReference.IsValueType 
               || typeReference.Resolve().ToType() == typeof(System.Nullable<>);

        /// <summary>
        /// Returns namespace of the given type, if the type is nested, namespace of containing type is returned instead
        /// </summary>        
        public static string GetNamespace(this TypeReference typeReference)
            => typeReference.IsNested
                ? typeReference.DeclaringType.FullName
                : typeReference.Namespace;
    }
}