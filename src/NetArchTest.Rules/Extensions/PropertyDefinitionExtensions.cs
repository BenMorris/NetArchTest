namespace NetArchTest.Rules.Extensions
{
    using Mono.Cecil;

    /// <summary>
    /// Extensions for the <see cref="PropertyDefinition"/> class.
    /// </summary>
    internal static class PropertyDefinitionExtensions
    {
        /// <summary>
        /// Tests whether a property is readonly
        /// </summary>
        /// <param name="propertyDefinition">The property to test.</param>
        /// <returns>An indication of whether the property is readonly.</returns>
        public static bool IsReadonly(this PropertyDefinition propertyDefinition)
            => !propertyDefinition.SetMethod?.IsPublic ?? true;

        /// <summary>
        /// Tests whether a property is nullable
        /// </summary>
        /// <param name="propertyDefinition">The property to test.</param>
        /// <returns>An indication of whether the property is nullable.</returns>
        public static bool IsNullable(this PropertyDefinition propertyDefinition)
            => propertyDefinition.PropertyType.IsNullable();
    }
}