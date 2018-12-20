using Mono.Collections.Generic;

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
        /// Finds types that have dependencies on a given list of type definitions.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of dependencies found in the input classes.</returns>
        internal IReadOnlyList<TypeDefinition> FindTypesWithDependencies(IEnumerable<TypeDefinition> input, 
            IEnumerable<string> dependencies)
        {
            // Set up the search definition
            var results = new SearchDefinition();

            Action<TypeDefinition, string> matchAct = (type, target) =>
            {
                foreach (var dependency in dependencies)
                {
                    if (target.StartsWith(dependency, StringComparison.InvariantCultureIgnoreCase))
                    {
                        results.AddToFound(type, dependency);
                    }
                }
            };

            return FindTypesWithDependenciesMatch(input, matchAct, ref results);
        }
        
        /// <summary>
        /// Finds types that have dependencies on a given list of type definitions.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependenciesMatch">The match rule.</param>
        /// <returns>A list of dependencies found in the input classes.</returns>
        internal IReadOnlyList<TypeDefinition> FindTypesWithDependenciesMatch(IEnumerable<TypeDefinition> input, 
            Func<string, bool> dependenciesMatch)
        {
            // Set up the search definition
            var results = new SearchDefinition();
            Action<TypeDefinition, string> matchAct = (type, target) =>
            {
                if (dependenciesMatch(target))
                {
                    results.AddToFound(type, "");
                }
            };

            return FindTypesWithDependenciesMatch(input, matchAct, ref results);
        }

        private IReadOnlyList<TypeDefinition> FindTypesWithDependenciesMatch(IEnumerable<TypeDefinition> input, 
            Action<TypeDefinition, string> matchAct, ref SearchDefinition results)
        {
            // Check each type in turn
            foreach (var type in input)
            {
                CheckType(type, results, matchAct);
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
        /// Finds matching dependencies for a given type by walking through the type contents.
        /// </summary>
        private void CheckType(TypeDefinition type, SearchDefinition results, Action<TypeDefinition, string> matchAct)
        {
            // Have we already checked this type?
            if (results.IsChecked(type.FullName))
            {
                return;
            }

            // Add the current type to the checked list - this prevents any circular checks
            results.AddToChecked(type);

            // Does this directly inherit from a dependency?
            if (type.BaseType != null)
            {
                CheckTypeReference(type, matchAct, type.BaseType, results);
            }

            // Check the properties
            CheckProperties(type, matchAct);

            // Check the generic parameters for the type
            if (type.HasGenericParameters)
            {
                CheckGenericParameters(type, type.GenericParameters, matchAct);
            }

            // Check the fields
            CheckFields(type, matchAct);

            // Check the events
            CheckEvents(type, ref results, matchAct);

            // Check the nested types
            foreach (var nested in type.NestedTypes)
            {
                CheckType(nested, results, matchAct);
            }

            // Check each method 
            foreach (var method in type.Methods)
            {
                CheckMethod(type, method, results, matchAct);
            }
        }
        
        /// <summary>
        /// Finds matching dependencies for a given method by walking through the IL instructions.
        /// </summary>
        private void CheckMethod(TypeDefinition type, MethodDefinition method, SearchDefinition results, 
            Action<TypeDefinition, string> matchAct)
        {
            // Check the return type
            if (method.ReturnType.ContainsGenericParameter)
            {
                CheckGenericParameters(type, method.ReturnType.GenericParameters, matchAct);
            }

            matchAct(type, method.ReturnType.FullName);
            
            CheckMethodParameters(type, method.Parameters, matchAct, results);

            // Check for any generic parameters
            if (method.ContainsGenericParameter)
            {
                CheckGenericParameters(type, method.GenericParameters, matchAct);
            }

            // Check the contents of the method body
            CheckMethodBody(type, method, ref results, matchAct);
        }

        private void CheckMethodParameters(TypeDefinition type,
            Collection<ParameterDefinition> parameters,
            Action<TypeDefinition, string> matchAct, SearchDefinition results)
        {
            foreach (var parameter in parameters)
            {
                CheckTypeReference(type, matchAct, parameter.ParameterType, results);
            }
        }

        private static void CheckTypeReference(TypeDefinition type, Action<TypeDefinition, string> matchAct, 
            TypeReference typeReference, SearchDefinition results)
        {
            if (results.IsChecked(typeReference?.FullName))
            {
                return;
            }

            if (typeReference is GenericInstanceType genericInstanceType)
            {
                foreach (var genericArgument in genericInstanceType.GenericArguments)
                {
                    matchAct(type, genericArgument.FullName);
                }
                CheckGenericParameters(type, typeReference.GenericParameters, matchAct);
            }
            matchAct(type, typeReference.FullName);
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the properties.
        /// </summary>
        private void CheckProperties(TypeDefinition type, Action<TypeDefinition, string> matchAct)
        {
            if (type.HasProperties)
            {
                foreach (var property in type.Properties)
                {
                    // The property could be a generic property
                    if (property.ContainsGenericParameter)
                    {
                        CheckGenericParameters(type, property.PropertyType.GenericParameters, matchAct);
                    }
                    matchAct(type, property.PropertyType.FullName);
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the fields.
        /// </summary>
        private void CheckFields(TypeDefinition type,
            Action<TypeDefinition, string> matchAct)
        {
            if (type.HasFields)
            {
                foreach (var field in type.Fields)
                {
                    // The field could be a generic property
                    if (field.ContainsGenericParameter)
                    {
                        CheckGenericParameters(type, field.FieldType.GenericParameters, matchAct);
                    }
                    matchAct(type, field.FieldType.FullName); 
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the events.
        /// </summary>
        private void CheckEvents(TypeDefinition type, ref SearchDefinition results, Action<TypeDefinition, string> matchAct)
        {
            if (type.HasEvents)
            {
                foreach (var eventDef in type.Events)
                {
                    if (eventDef.HasOtherMethods)
                    {
                        foreach (var method in eventDef.OtherMethods)
                        {
                            CheckMethod(type, method, results, matchAct);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by scanning the code.
        /// </summary>
        private void CheckMethodBody(TypeDefinition type, MethodDefinition method, ref SearchDefinition results, 
            Action<TypeDefinition, string> matchAct)
        {
            if (method.HasBody)
            {
                foreach (var variable in method.Body.Variables)
                {
                    // Check any nested types in methods - the compiler will create one for every asynchronous method or iterator. 
                    if (variable.VariableType.IsNested)
                    {
                        CheckType(variable.VariableType.Resolve(), results, matchAct);
                    }
                    else
                    {
                        if (variable.VariableType.ContainsGenericParameter)
                        {
                            CheckGenericParameters(type, variable.VariableType.GenericParameters, matchAct);
                        }
                        matchAct(type, variable.VariableType.FullName);
                    }
                }

                // Check each instruction for references to our types
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand != null)
                    {
                        if (instruction.Operand is MemberReference mf)
                        {
                            CheckTypeReference(type, matchAct, mf.DeclaringType, results);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a set of generic parameters
        /// </summary>
        private static void CheckGenericParameters(TypeDefinition type, IEnumerable<GenericParameter> parameters, Action<TypeDefinition, string> matchAct)
        {
            foreach (var generic in parameters)
            {
                matchAct(type, generic.FullName);
            }
        }
    }
}
