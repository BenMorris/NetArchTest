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
        public IReadOnlyList<TypeDefinition> FindTypesWithAnyDependencies(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {            
            var results = new SearchDefinition(SearchDefinition.SearchType.FindTypesWithAnyDependencies, dependencies);
            return FindTypes(input, results);           
        }

        /// <summary>
        /// Finds types that have a dependency on every items in a given list of dependencies.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of dependencies found in the input classes.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesWithAllDependencies(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {       
            var results = new SearchDefinition(SearchDefinition.SearchType.FindTypesWithAllDependencies, dependencies);
            return FindTypes(input, results);         
        }
        private List<TypeDefinition> FindTypes(IEnumerable<TypeDefinition> input, SearchDefinition results)
        {
            // Check each type in turn
            foreach (var type in input)
            {
                CheckType(type, results);
            }

            var output = input.Where(x =>results.IsTypeFound(x.FullName)).ToList();
            return output;
        }

        /// <summary>
        /// Finds matching dependencies for a given type by walking through the type contents.
        /// </summary>
        private void CheckType(TypeDefinition type, SearchDefinition results)
        {
            // Have we already checked this type or type is null?
            if (results.ShouldTypeBeChecked(type) == false)
            {
                return;
            }

            // Does this directly inherit from a dependency?
            if (type.BaseType != null)
            {
                CheckTypeReference(type, results, type.BaseType);
            }

            CheckCustomAttributes(type, results, type);
            CheckImplementedInterfaces(type, results);
            CheckGenericTypeParametersConstraints(type, results, type);
            CheckFields(type, results);
            CheckProperties(type, results);           
            CheckEvents(type, results);

            // if we already know that type is found, we can skip
            if (results.IsTypeFound(type.FullName)) return;

            CheckMethods(type, results);

            if (results.IsTypeFound(type.FullName)) return;

            CheckNestedTypes(type, results);
        }

        private void CheckCustomAttributes(TypeDefinition type, SearchDefinition results, ICustomAttributeProvider typeToCheck)
        {
            if (typeToCheck.HasCustomAttributes)
            {
                foreach (var customAttribute in typeToCheck.CustomAttributes)
                {
                    CheckTypeReference(type, results, customAttribute.AttributeType);
                }
            }
        }
        private void CheckImplementedInterfaces(TypeDefinition type, SearchDefinition results)
        {
            if (type.HasInterfaces)
            {
                foreach (var @interface in type.Interfaces)
                {
                    CheckTypeReference(type, results, @interface.InterfaceType);
                }
            }
        }
        private void CheckGenericTypeParametersConstraints(TypeDefinition type, SearchDefinition results, IGenericParameterProvider typeToCheck)
        {
            if (typeToCheck.HasGenericParameters)
            {
                foreach (var parameter in typeToCheck.GenericParameters)
                {
                    if (parameter.HasConstraints)
                    {
                        foreach (var constraint in parameter.Constraints)
                        {
                            CheckTypeReference(type, results, constraint.ConstraintType);
                        }
                    }
                }
            }
        }
        private void CheckFields(TypeDefinition type, SearchDefinition results)
        {
            if (type.HasFields)
            {
                foreach (var field in type.Fields)
                {
                    CheckCustomAttributes(type, results, field);
                    CheckTypeReference(type, results, field.FieldType);
                }
            }
        }
        private void CheckProperties(TypeDefinition type, SearchDefinition results)
        {
            if (type.HasProperties)
            {
                foreach (var property in type.Properties)
                {
                    CheckCustomAttributes(type, results, property);
                    CheckTypeReference(type, results, property.PropertyType);
                }
            }
        }
        private void CheckEvents(TypeDefinition type, SearchDefinition results)
        {
            if (type.HasEvents)
            {
                foreach (var @event in type.Events)
                {
                    CheckCustomAttributes(type, results, @event);
                    CheckTypeReference(type, results, @event.EventType);

                    if (@event.HasOtherMethods) // are we sure that event can have others method? TODO : we need unit test for this case
                    {
                        foreach (var method in @event.OtherMethods)
                        {
                            CheckMethod(type, results, method);
                        }
                    }
                }
            }
        }
        private void CheckMethods(TypeDefinition type, SearchDefinition results)
        {
            if (type.HasMethods)
            {
                foreach (var method in type.Methods)
                {
                    this.CheckMethod(type, results, method);
                }
            }
        }
        private void CheckNestedTypes(TypeDefinition type, SearchDefinition results)
        {
            if (type.HasNestedTypes)
            {
                foreach (var nested in type.NestedTypes)
                {
                    this.CheckType(nested, results);
                }
            }
        }
        private void CheckMethod(TypeDefinition type, SearchDefinition results, MethodDefinition method)
        {
            CheckCustomAttributes(type, results, method);
            CheckCustomAttributes(type, results, method.MethodReturnType);
            CheckGenericTypeParametersConstraints(type, results, method);
            CheckTypeReference(type, results, method.ReturnType);

            if (method.HasParameters)
            {
                foreach (var parameter in method.Parameters)
                {
                    CheckCustomAttributes(type, results, parameter);
                    CheckTypeReference(type, results, parameter.ParameterType);
                }               
            }
            
            CheckMethodBody(type, results, method);
        }
        /// <summary>
        /// Finds matching dependencies for a given method by walking through the IL instructions.
        /// </summary>
        private void CheckMethodBody(TypeDefinition type, SearchDefinition results, MethodDefinition method)
        {
            if (method.HasBody)
            {
                if (method.Body.HasVariables)
                {
                    foreach (var variable in method.Body.Variables)
                    {
                        // Check not nested types in methods - the compiler will create nested one for every asynchronous method or iterator or lambda closure and they are already checked in CheckNestedTypes().
                        if (!variable.VariableType.IsNested)
                        {
                            CheckTypeReference(type, results, variable.VariableType);
                        }
                    }
                }                

                foreach (var instruction in method.Body.Instructions)
                {                   
                    switch (instruction.Operand)
                    {
                        case TypeReference reference:
                            CheckTypeReference(type, results, reference);
                            break;                         
                        case GenericInstanceMethod genericInstanceMethod:
                            CheckTypeReference(type, results, genericInstanceMethod.DeclaringType);
                            if (genericInstanceMethod.HasGenericArguments)
                            {
                                foreach (var argument in genericInstanceMethod.GenericArguments)
                                {
                                    CheckTypeReference(type, results, argument);
                                }
                            }
                            break;
                        case MethodReference methodReference:                              
                            CheckTypeReference(type, results, methodReference.DeclaringType);                               
                            break; 
                    }                    
                }
            }
        }

        private void CheckTypeReference(TypeDefinition type, SearchDefinition results, TypeReference reference)
        {
            if (reference.IsGenericInstance == false)
            {
                if (reference.IsGenericParameter == false)
                {
                    // happy/fast path, type reference is not generic  
                    results.AddDependency(type, reference.FullName);                    
                }
            }
            else
            {
                // slower path, we need to extract all names from generic type reference
                var types = ExtractTypeNames(reference);
                results.AddDependencies(type, types);               
            }
        }

        /// <summary>
        /// Extract type names from generic or not type reference
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
    }
}