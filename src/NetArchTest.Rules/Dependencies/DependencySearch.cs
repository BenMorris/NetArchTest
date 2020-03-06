namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// Finds dependencies within a given set of types.
    /// </summary>
    internal class DependencySearch
    {
        /// <summary>
        /// Finds types that have a dependency on any item in a given list of dependencies.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of dependencies found in the input classes.</returns>
        internal IReadOnlyList<TypeDefinition> FindTypesWithAnyDependencies(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {
            // Set up the search definition
            var results = new SearchDefinition(dependencies);

            // Check each type in turn
            foreach (var type in input)
            {
                CheckType(type, ref results);
            }

            var output = new List<TypeDefinition>();

            foreach (var found in results.TypesFound)
            {
                // NB: Nested classes won't be picked up here
                var match = input.FirstOrDefault(d => d.FullName.Equals(found, StringComparison.InvariantCultureIgnoreCase));
                if (match != null)
                {
                    output.Add(match);
                }
            }

            return output;
        }

        /// <summary>
        /// Finds types that have a dependency on every items in a given list of dependencies.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of dependencies found in the input classes.</returns>
        internal IReadOnlyList<TypeDefinition> FindTypesWithAllDependencies(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {
            // Set up the search definition
            var results = new SearchDefinition(dependencies);

            // Check each type in turn
            foreach (var type in input)
            {
                CheckType(type, ref results);
            }

            var output = new List<TypeDefinition>();

            foreach (var typeFound in results.TypesFound)
            {
                // NB: Nested classes won't be picked up here
                var match = input.FirstOrDefault(d => d.FullName.Equals(typeFound, StringComparison.InvariantCultureIgnoreCase));
                if (match != null &&
                    results.GetAllDependenciesMatchingAnyOf(results.GetDependenciesFoundForType(typeFound)).Count() == results.UniqueDependenciesCount)
                {
                    // Check found 
                    output.Add(match);
                }
            }

            return output;
        }

        /// <summary>
        /// Finds matching dependencies for a given type by walking through the type contents.
        /// </summary>
        private void CheckType(TypeDefinition type, ref SearchDefinition results)
        {
            // Have we already checked this type?
            if (results.IsChecked(type))
            {
                return;
            }

            // Add the current type to the checked list - this prevents any circular checks
            results.AddToChecked(type);

            // Does this directly inherit from a dependency?
            var baseClass = type.BaseType?.Resolve();
            if (baseClass != null)
            {
                foreach (var dependency in results.GetAllMatchingDependencies(baseClass.FullName))
                {

                    results.AddToFound(type, dependency);
                }
            }

            // Check the properties
            CheckProperties(type, ref results);

            // Check the generic parameters for the type
            if (type.HasGenericParameters)
            {
                CheckGenericParameters(type, type.GenericParameters, ref results);
            }

            // Check the fields
            CheckFields(type, ref results);

            // Check the events
            CheckEvents(type, ref results);

            // Check the nested types
            foreach (var nested in type.NestedTypes)
            {
                this.CheckType(nested, ref results);
            }

            // Check each method
            foreach (var method in type.Methods)
            {
                this.CheckMethod(type, method, ref results);
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the IL instructions.
        /// </summary>
        private void CheckMethod(TypeDefinition type, MethodDefinition method, ref SearchDefinition results)
        {
            // Check the return type
            if (method.ReturnType.ContainsGenericParameter)
            {
                CheckGenericParameters(type, method.ReturnType.GenericParameters, ref results);
            }

            if (results.GetAllMatchingDependencies(method.ReturnType.FullName).Any())
            {
                results.AddToFound(type, method.ReturnType.FullName);
            }

            // Check for any generic parameters
            if (method.ContainsGenericParameter)
            {
                CheckGenericParameters(type, method.GenericParameters, ref results);
            }

            if (method.HasParameters)
            {
                CheckParameters(type, method.Parameters, ref results);
            }

            // Check the contents of the method body
            CheckMethodBody(type, method, ref results);
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the properties.
        /// </summary>
        private void CheckProperties(TypeDefinition type, ref SearchDefinition results)
        {
            if (type.HasProperties)
            {
                foreach (var property in type.Properties)
                {
                    // The property could be a generic property
                    if (property.ContainsGenericParameter)
                    {
                        CheckGenericParameters(type, property.PropertyType.GenericParameters, ref results);
                    }

                    // Check the property type
                    if (results.GetAllMatchingDependencies(property.PropertyType.FullName).Any())
                    {
                        results.AddToFound(type, property.PropertyType.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the fields.
        /// </summary>
        private void CheckFields(TypeDefinition type, ref SearchDefinition results)
        {
            if (type.HasFields)
            {
                foreach (var field in type.Fields)
                {
                    // The field could be a generic property
                    if (field.ContainsGenericParameter)
                    {
                        CheckGenericParameters(type, field.FieldType.GenericParameters, ref results);
                    }

                    if (results.GetAllMatchingDependencies(field.FieldType.FullName).Any())
                    {
                        results.AddToFound(type, field.FieldType.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the events.
        /// </summary>
        private void CheckEvents(TypeDefinition type, ref SearchDefinition results)
        {
            if (type.HasEvents)
            {
                foreach (var eventDef in type.Events)
                {
                    if (eventDef.HasOtherMethods)
                    {
                        foreach (var method in eventDef.OtherMethods)
                        {
                            CheckMethod(type, method, ref results);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by scanning the code.
        /// </summary>
        private void CheckMethodBody(TypeDefinition type, MethodDefinition method, ref SearchDefinition results)
        {
            if (method.HasBody)
            {
                foreach (var variable in method.Body.Variables)
                {
                    // Check any nested types in methods - the compiler will create one for every asynchronous method or iterator.
                    if (variable.VariableType.IsNested)
                    {
                        CheckType(variable.VariableType.Resolve(), ref results);
                    }
                    else
                    {
                        if (variable.VariableType.ContainsGenericParameter)
                        {
                            CheckGenericParameters(type, variable.VariableType.GenericParameters, ref results);
                        }

                        if (results.GetAllMatchingDependencies(variable.VariableType.FullName).Any())
                        {
                            results.AddToFound(type, variable.VariableType.FullName);
                        }
                    }
                }

                // Check each instruction for references to our types
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand != null)
                    {
                        var operands = instruction.Operand.ToString().Split(new char[] { ' ', '<', ',', '>' });
                        var matches = results.GetAllDependenciesMatchingAnyOf(operands);
                        foreach (var item in matches)
                        {
                            results.AddToFound(type, item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a set of generic parameters
        /// </summary>
        private static void CheckGenericParameters(TypeDefinition type, IEnumerable<GenericParameter> parameters, ref SearchDefinition results)
        {
            foreach (var generic in parameters)
            {
                if (results.GetAllMatchingDependencies(generic.FullName).Any())
                {
                    results.AddToFound(type, generic.FullName);
                }
            }
        }

        private void CheckParameters(TypeDefinition type, IEnumerable<ParameterDefinition> parameters, ref SearchDefinition results)
        {
            foreach (var parameter in parameters)
            {
                string fullName = parameter.ParameterType?.FullName ?? String.Empty;
                if (results.GetAllMatchingDependencies(fullName).Any())
                {
                    results.AddToFound(type, fullName);
                }
            }
        }
    }
}
