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

            CheckCustomAttributes(type, type, ref results);
            CheckInterfaces(type, ref results);
                

            // Check the properties
            CheckProperties(type, ref results);

            // Check the generic parameters for the type
            if (type.HasGenericParameters)
            {
                CheckGenericTypeParameters(type, type.GenericParameters, ref results);
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

        private void CheckCustomAttributes(TypeDefinition type, ICustomAttributeProvider typeToCheck, ref SearchDefinition results)
        {
            if (typeToCheck.HasCustomAttributes)
            {
                foreach (var customAttribute in typeToCheck.CustomAttributes)
                {
                    CheckTypeReference(type, results, customAttribute.AttributeType);
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the IL instructions.
        /// </summary>
        private void CheckMethod(TypeDefinition type, MethodDefinition method, ref SearchDefinition results)
        {
            CheckCustomAttributes(type, method, ref results);
            CheckCustomAttributes(type, method.MethodReturnType, ref results);

            // Check the return type
            if (method.ReturnType.ContainsGenericParameter)
            {
                CheckParameters(type, method.ReturnType.GenericParameters, ref results);
            }

            if (method.ReturnType.IsGenericInstance)
            {
                var returnTypeAsGenericInstance = method.ReturnType as GenericInstanceType;
                if (returnTypeAsGenericInstance.HasGenericArguments)
                {
                    CheckParameters(type, returnTypeAsGenericInstance.GenericArguments, ref results);
                }
            }

            if (results.GetAllMatchingDependencies(method.ReturnType.FullName).Any())
            {
                results.AddToFound(type, method.ReturnType.FullName);
            }

            // Check for any generic parameters
            if (method.ContainsGenericParameter)
            {
                CheckGenericTypeParameters(type, method.GenericParameters, ref results);
            }

            if (method.HasParameters)
            {
                foreach (var parameter in method.Parameters)
                {
                    CheckCustomAttributes(type, parameter, ref results);
                }
                CheckParameters(type, method.Parameters.Select(x => x.ParameterType), ref results);
            }

            // Check the contents of the method body
            CheckMethodBody(type, method, ref results);
        }

        private void CheckInterfaces(TypeDefinition type, ref SearchDefinition results)
        {
            if (type.HasInterfaces)
            {
                foreach (var @interface in type.Interfaces)
                {
                    CheckTypeReference(type, results, @interface.InterfaceType);
                }
            }
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
                    CheckCustomAttributes(type, property, ref results);

                    // The property could be a generic property
                    if (property.ContainsGenericParameter)
                    {
                        CheckParameters(type, property.PropertyType.GenericParameters, ref results);
                    }

                    // Check the property type
                    if (results.GetAllMatchingDependencies(property.PropertyType.FullName).Any())
                    {
                        results.AddToFound(type, property.PropertyType.FullName);
                    }
                }
            }
        }

 
        private void CheckFields(TypeDefinition type, ref SearchDefinition results)
        {
            if (type.HasFields)
            {
                foreach (var field in type.Fields)
                {
                    CheckCustomAttributes(type, field, ref results);
                    CheckTypeReference(type, results, field.FieldType);                  
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
                    CheckCustomAttributes(type, eventDef, ref results);
                    
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
                        CheckTypeReference(type, results, variable.VariableType);                       
                    }
                }

                // Check each instruction for references to our types
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand != null)
                    {
                        var operands = ExtractTypeNames(instruction.Operand.ToString());
                        var matches = results.GetAllDependenciesMatchingAnyOf(operands);
                        foreach (var item in matches)
                        {
                            results.AddToFound(type, item);
                        }
                    }
                }
            }
        }


        private void CheckGenericTypeParameters(TypeDefinition type, IEnumerable<GenericParameter> parameters, ref SearchDefinition results)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.HasConstraints)
                {
                    foreach (var constraint in parameter.Constraints)
                    {
                        if (results.GetAllMatchingDependencies(constraint.ConstraintType.FullName).Any())
                        {
                            results.AddToFound(type, constraint.ConstraintType.FullName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a set of generic or not parameters
        /// </summary>
        private void CheckParameters(TypeDefinition type, IEnumerable<TypeReference> parameters, ref SearchDefinition results)
        {
            foreach (var parameter in parameters)
            {                
                if (parameter.IsGenericParameter)
                {
                    // generic parameters do not contain information about type, they are only placeholder for a type
                    continue;
                }
                if (IsTypeGeneric(parameter.FullName))
                {
                    var types = ExtractTypeNames(parameter.FullName);
                    var matches = results.GetAllDependenciesMatchingAnyOf(types);
                    foreach (var item in matches)
                    {
                        results.AddToFound(type, item);
                    }
                }
                else
                {
                    if (results.GetAllMatchingDependencies(parameter.FullName).Any())
                    {
                        results.AddToFound(type, parameter.FullName);
                    }
                }
            }
        }




        private void CheckTypeReference(TypeDefinition type, SearchDefinition results, TypeReference reference)
        {
            if (reference.IsGenericInstance == false)
            {
                // happy path, type refence is not generic
                if (results.GetAllMatchingDependencies(reference.FullName).Any())
                {
                    results.AddToFound(type, reference.FullName);
                }
            }
            else
            {
                var types = ExtractTypeNames(reference);
                var matches = results.GetAllDependenciesMatchingAnyOf(types);
                foreach (var item in matches)
                {
                    results.AddToFound(type, item);
                }
            }
        }

        /// <summary>
        /// Extract type names from generic or not reference
        /// </summary>
        private IEnumerable<string> ExtractTypeNames(TypeReference reference)
        {
            if (!reference.IsGenericParameter)
            {
                yield return reference.FullName;
            }
            if (reference.IsGenericInstance)
            {
                var referenceAsGenericInstance = reference as GenericInstanceType;
                if (referenceAsGenericInstance.HasGenericArguments)
                {
                    foreach (var genericArgument in referenceAsGenericInstance.GenericArguments)
                    {                       
                        foreach(var name in ExtractTypeNames(genericArgument))
                        {
                            yield return name;
                        }
                    }
                }
            }
        }


        static readonly char[] GenericSepartors = new char[] { ' ', '<', ',', '>' };
        private IEnumerable<string> ExtractTypeNames(string fullName)
        {
            return fullName.Split(GenericSepartors).Where(x => !String.IsNullOrWhiteSpace(x));
        }
        private bool IsTypeGeneric(string fullName)
        {
            foreach(char separtor in GenericSepartors)
            {                
                if (fullName.Contains(separtor))
                {
                    return true;
                }
            }
            return false;
        }
    }
}