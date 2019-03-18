namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Mono.Cecil;

    /// <summary>
    /// Extensions for the <see cref="PropertyDefinition"/> class.
    /// </summary>
    static internal class PropertyDefinitionExtensions
    {
        /// <summary>
        /// Tests whether a property is readonly
        /// </summary>
        /// <param name="propertyDefinition">The property to test.</param>
        /// <returns>An indication of whether the property is readonly.</returns>
        public static bool IsReadonly(this PropertyDefinition propertyDefinition)
        {
            return propertyDefinition.SetMethod == null || !propertyDefinition.SetMethod.IsPublic;
        }
        
        /// <summary>
        /// Tests whether a property is nullable
        /// </summary>
        /// <param name="propertyDefinition">The property to test.</param>
        /// <returns>An indication of whether the property is nullable.</returns>
        public static bool IsNullable(this PropertyDefinition propertyDefinition)
        {
            return propertyDefinition.PropertyType.IsNullable();
        }
    }
}