using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace NetArchTest.Rules.Dependencies
{
    internal class DependencySearch
    {
        private readonly IDictionary<string, (bool RefChecked, bool DefChecked)> _checkedTypes 
            = new Dictionary<string, (bool, bool)>();
        private SearchResults _results;

        public DependencySearch(Func<string, bool> match)
        {
            _match = match;
        }

        private void AddRefChecked(string fullName)
        {
            if (_checkedTypes.TryGetValue(fullName, out var result))
            {
                result.RefChecked = true;
                _checkedTypes[fullName] = result;
            }
            else
            {
                result.RefChecked = true;
                _checkedTypes.Add(fullName, result);                
            }
        }

        private void AddDefChecked(string fullName)
        {
            if (_checkedTypes.TryGetValue(fullName, out var result))
            {
                result.DefChecked = true;
                _checkedTypes[fullName] = result;
            }
            else
            {
                result.DefChecked = true;
                _checkedTypes.Add(fullName, result);                
            }
        }

        private bool IsDefChecked(string fullName)
        {
            _checkedTypes.TryGetValue(fullName, out var result);
            return result.DefChecked;
        }
        
        private bool IsRefChecked(string fullName)
        {
            _checkedTypes.TryGetValue(fullName, out var result);
            return result.RefChecked;
        }

        private void MatchAction(TypeDefinition type, string target)
        {
            if (_match(target))
            {
                _results.AddToFound(type, target);
            }            
        }

        private readonly Func<string, bool> _match;
        public SearchResults FindTypesWithDependenciesMatch(IList<TypeDefinition> inputs)
        {
            _results = new SearchResults(inputs);
            // Check each type in turn
            foreach (var type in inputs)
            {
                CheckTypeDefinition(type);
            }            
            return _results;
        }

        /// <summary>
        /// Finds matching dependencies for a given type by walking through the type contents.
        /// </summary>
        private void CheckTypeDefinition(TypeDefinition type)
        {
            // Have we already checked this type?
            if (IsDefChecked(type.FullName))
            {
                return;
            }
            AddDefChecked(type.FullName);

            // Does this directly inherit from a dependency?
            if (type.BaseType != null)
            {
                CheckTypeReference(type, type.BaseType);
            }

            // Check the properties
            CheckProperties(type);

            // Check the generic parameters for the type
            if (type.HasGenericParameters)
            {
                CheckGenericParameters(type, type.GenericParameters);
            }

            // Check the fields
            CheckFields(type);

            // Check the events
            CheckEvents(type);

            // Check the nested types
            foreach (var nested in type.NestedTypes)
            {
                CheckTypeDefinition(nested);
            }

            // Check each method 
            foreach (var method in type.Methods)
            {
                CheckMethod(type, method);
            }
        }
        
        /// <summary>
        /// Finds matching dependencies for a given method by walking through the IL instructions.
        /// </summary>
        private void CheckMethod(TypeDefinition type, MethodDefinition method)
        {
            // Check the return type
            if (method.ReturnType.ContainsGenericParameter)
            {
                CheckGenericParameters(type, method.ReturnType.GenericParameters);
            }

            MatchAction(type, method.ReturnType.FullName);
            
            CheckMethodParameters(type, method.Parameters);

            // Check for any generic parameters
            if (method.ContainsGenericParameter)
            {
                CheckGenericParameters(type, method.GenericParameters);
            }

            // Check the contents of the method body
            CheckMethodBody(type, method);
        }

        private void CheckMethodParameters(TypeDefinition type,
            Collection<ParameterDefinition> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType != null)
                {
                    CheckTypeReference(type, parameter.ParameterType);    
                }
            }
        }

        private void CheckTypeReference(TypeDefinition type, TypeReference typeReference)
        {
            if (typeReference.IsGenericParameter)
            {
                return;
            }
            if (IsRefChecked(typeReference.FullName))
            {
                return;
            }
            AddRefChecked(typeReference.FullName);

            if (typeReference is GenericInstanceType genericInstanceType)
            {
                foreach (var genericArgument in genericInstanceType.GenericArguments)
                {
                    MatchAction(type, genericArgument.FullName);
                }
                CheckGenericParameters(type, typeReference.GenericParameters);
            }
            MatchAction(type, typeReference.FullName);
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the properties.
        /// </summary>
        private void CheckProperties(TypeDefinition type)
        {
            if (type.HasProperties)
            {
                foreach (var property in type.Properties)
                {
                    // The property could be a generic property
                    if (property.ContainsGenericParameter)
                    {
                        CheckGenericParameters(type, property.PropertyType.GenericParameters);
                    }
                    MatchAction(type, property.PropertyType.FullName);
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the fields.
        /// </summary>
        private void CheckFields(TypeDefinition type)
        {
            if (type.HasFields)
            {
                foreach (var field in type.Fields)
                {
                    // The field could be a generic property
                    if (field.ContainsGenericParameter)
                    {
                        CheckGenericParameters(type, field.FieldType.GenericParameters);
                    }
                    MatchAction(type, field.FieldType.FullName); 
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by walking through the events.
        /// </summary>
        private void CheckEvents(TypeDefinition type)
        {
            if (type.HasEvents)
            {
                foreach (var eventDef in type.Events)
                {
                    if (eventDef.HasOtherMethods)
                    {
                        foreach (var method in eventDef.OtherMethods)
                        {
                            CheckMethod(type, method);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds matching dependencies for a given method by scanning the code.
        /// </summary>
        private void CheckMethodBody(TypeDefinition type, MethodDefinition method)
        {
            if (method.HasBody)
            {
                foreach (var variable in method.Body.Variables)
                {
                    // Check any nested types in methods - the compiler will create one for every asynchronous method or iterator. 
                    if (variable.VariableType.IsNested)
                    {
                        CheckTypeReference(type, variable.VariableType);
                    }
                    else
                    {
                        if (variable.VariableType.ContainsGenericParameter)
                        {
                            CheckGenericParameters(type, variable.VariableType.GenericParameters);
                        }
                        MatchAction(type, variable.VariableType.FullName);
                    }
                }

                // Check each instruction for references to our types
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand != null)
                    {
                        if (instruction.Operand is MemberReference mf)
                        {
                            if (mf.DeclaringType != null)
                            {
                                CheckTypeReference(type, mf.DeclaringType);    
                            }
                        }
                    }
                }
            }
        }
        
        private void CheckGenericParameters(TypeDefinition type, IEnumerable<GenericParameter> parameters)
        {
            foreach (var generic in parameters)
            {
                MatchAction(type, generic.FullName);
            }
        }
    }
}
