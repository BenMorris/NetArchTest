namespace NetArchTest.Rules.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Mono.Cecil;

    /// <summary>
    /// Extensions for the <see cref="FieldDefinition"/> class.
    /// </summary>
    static internal class FieldDefinitionExtensions
    {
        /// <summary>
        /// Tests whether a field is readonly
        /// </summary>
        /// <param name="fieldDefinition">The field to test.</param>
        /// <returns>An indication of whether the field is readonly.</returns>
        public static bool IsReadonly(this FieldDefinition fieldDefinition)
        {
            return !fieldDefinition.IsPublic || fieldDefinition.IsInitOnly || fieldDefinition.HasConstant || fieldDefinition.IsCompilerControlled;
        }
    }
}