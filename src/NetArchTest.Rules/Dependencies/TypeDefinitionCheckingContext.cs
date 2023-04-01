using System.Linq;

namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections.Generic;
    using Mono.Cecil;
    using NetArchTest.Rules.Dependencies.DataStructures;
    using NetArchTest.Rules.Extensions;

    internal class TypeDefinitionCheckingContext
    {
        private readonly TypeDefinition _typeToCheck;
        private readonly TypeDefinitionCheckingResult _result;        
        private readonly bool _serachForDependencyInFieldConstant;

        public TypeDefinitionCheckingContext(TypeDefinition typeToCheck, TypeDefinitionCheckingResult.SearchType searchType, ISearchTree searchTree, bool serachForDependencyInFieldConstant = false)
        {
            _typeToCheck = typeToCheck;
            _result = new TypeDefinitionCheckingResult(searchType, searchTree);
            _serachForDependencyInFieldConstant = serachForDependencyInFieldConstant;
        }

        public bool IsTypeFound()
        {
            CheckType(_typeToCheck);
            return _result.IsTypeFound();
        }

        /// <summary>
        /// Finds matching dependencies for a given type by walking through the type.
        /// </summary>
        private void CheckType(TypeDefinition type)
        {
            CheckBaseType(type);
            CheckCustomAttributes(type);
            CheckImplementedInterfaces(type);
            CheckGenericTypeParametersConstraints(type);
            CheckFields(type);
            if (_result.CanWeSkipFurtherSearch()) return;
            CheckProperties(type);
            if (_result.CanWeSkipFurtherSearch()) return;
            CheckEvents(type);
            if (_result.CanWeSkipFurtherSearch()) return;
            CheckMethods(type);
            if (_result.CanWeSkipFurtherSearch()) return;
            CheckNestedCompilerGeneratedTypes(type);
        }

        private void CheckBaseType(TypeDefinition typeToCheck)
        {
            if (typeToCheck.BaseType != null)
            {
                CheckTypeReference(typeToCheck.BaseType);
            }
        }
        
        private void CheckCustomAttributes(ICustomAttributeProvider typeToCheck)
        {
            if (!typeToCheck.HasCustomAttributes)
            {
                return;
            }
            
            foreach (var ca in typeToCheck.CustomAttributes)
            {
                CheckTypeReference(ca.AttributeType);
            }
        }    
        
        private void CheckImplementedInterfaces(TypeDefinition typeToCheck)
        {
            if (!typeToCheck.HasInterfaces)
            {
                return;
            }
            
            foreach (var i in typeToCheck.Interfaces)
            {
                CheckTypeReference(i.InterfaceType);
            }
        }
        
        private void CheckGenericTypeParametersConstraints(IGenericParameterProvider typeToCheck)
        {
            if (!typeToCheck.HasGenericParameters)
            {
                return;
            }
            
            typeToCheck.GenericParameters
                .Where(p => p.HasConstraints)
                .SelectMany(p => p.Constraints)
                .ToList()
                .ForEach(c => CheckTypeReference(c.ConstraintType));
        }
        
        private void CheckFields(TypeDefinition typeToCheck)
        {
            if (!typeToCheck.HasFields)
            {
                return;
            }
            
            foreach (var field in typeToCheck.Fields)
            {
                CheckCustomAttributes(field);
                CheckTypeReference(field.FieldType);
                
                if (_serachForDependencyInFieldConstant && field.HasConstant && field.FieldType.FullName == typeof(string).FullName)
                {
                    _result.CheckDependency(field.Constant.ToString());
                }
            }
        }
        
        private void CheckProperties(TypeDefinition typeToCheck)
        {
            if (!typeToCheck.HasProperties)
            {
                return;
            }
            
            foreach (var property in typeToCheck.Properties)
            {
                CheckCustomAttributes(property);
                CheckTypeReference(property.PropertyType);
            }
        }
        
        private void CheckEvents(TypeDefinition typeToCheck)
        {
            if (!typeToCheck.HasEvents)
            {
                return;
            }
            
            foreach (var @event in typeToCheck.Events)
            {
                CheckCustomAttributes(@event);
                CheckTypeReference(@event.EventType);

                if (!@event.HasOtherMethods)
                {
                    continue;
                }
                
                // are we sure that event can have other methods? TODO : we need unit test for this case
                
                foreach (var method in @event.OtherMethods)
                {
                    CheckMethodHeader(method);
                    CheckMethodBodyVariables(method);
                    CheckMethodBodyInstructions(method);
                }
            }
        }
        
        private void CheckMethods(TypeDefinition typeToCheck)
        {
            if (!typeToCheck.HasMethods)
            {
                return;
            }
            
            // checking method body is the most costly checking from all, 
            // therefore we want to do it as late as possible and end as fast as we can
            foreach (var method in typeToCheck.Methods)
            {
                if (_result.CanWeSkipFurtherSearch()) return;
                CheckMethodHeader(method);
            }

            foreach (var method in typeToCheck.Methods)
            {
                if (_result.CanWeSkipFurtherSearch()) return;
                CheckMethodBodyVariables(method);
            }

            foreach (var method in typeToCheck.Methods)
            {
                if (_result.CanWeSkipFurtherSearch()) return;
                CheckMethodBodyInstructions(method);
            }
        }
        
        private void CheckNestedCompilerGeneratedTypes(TypeDefinition typeToCheck)
        {
            if (!typeToCheck.HasNestedTypes)
            {
                return;
            }

            typeToCheck.NestedTypes
                .Where(n => n.IsCompilerGenerated())
                .ToList()
                .ForEach(n =>
                {
                    if (_result.CanWeSkipFurtherSearch()) return;
                    CheckType(n);
                });
        }
        
        private void CheckMethodHeader(MethodDefinition methodToCheck)
        {
            CheckCustomAttributes(methodToCheck);
            CheckGenericTypeParametersConstraints(methodToCheck);
            CheckCustomAttributes(methodToCheck.MethodReturnType);
            CheckTypeReference(methodToCheck.ReturnType);

            if (!methodToCheck.HasParameters)
            {
                return;
            }
            
            foreach (var parameter in methodToCheck.Parameters)
            {
                CheckCustomAttributes(parameter);
                CheckTypeReference(parameter.ParameterType);
            }
        }
        
        private void CheckMethodBodyVariables(MethodDefinition methodToCheck)
        {
            if (!methodToCheck.HasBody || !methodToCheck.Body.HasVariables)
            {
                return;
            }
            
            foreach (var variable in methodToCheck.Body.Variables)
            {
                CheckTypeReference(variable.VariableType);
            }
        }
        
        private void CheckMethodBodyInstructions(MethodDefinition methodToCheck)
        {
            if (!methodToCheck.HasBody)
            {
                return;
            }
            
            foreach (var instruction in methodToCheck.Body.Instructions)
            {
                switch (instruction.Operand)
                {
                    case TypeReference reference:
                        CheckTypeReference(reference);
                        break;
                    case GenericInstanceMethod genericInstanceMethod:
                        CheckTypeReference(genericInstanceMethod.DeclaringType);
                        if (genericInstanceMethod.HasGenericArguments)
                        {
                            foreach (var argument in genericInstanceMethod.GenericArguments)
                            {
                                CheckTypeReference(argument);
                            }
                        }
                        break;
                    case FieldReference fieldReference:
                        if (fieldReference.DeclaringType != _typeToCheck)
                        {
                            CheckTypeReference(fieldReference.DeclaringType);
                        }
                        break;
                    case MethodReference methodReference:
                        if (methodReference.DeclaringType != _typeToCheck)
                        {
                            CheckTypeReference(methodReference.DeclaringType);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Recursively checks every array, generic or not type reference
        /// <example>
        /// for closed generic : List{Tuple{Task{int}, int}}
        /// it will check: List{Tuple{Task{int}, int}}, Tuple{Task{int}, int}, Task{int}, int, int
        /// for open generic : List{T}
        /// only List will be checked, T as a generic parameter will be skipped
        /// for arrays: int[][]
        /// it will check : int[][], int[], int
        /// </example>         
        /// </summary>      
        private void CheckTypeReference(TypeReference reference)
        {
            if (reference.IsGenericParameter)
            {
                return;
            }
            
            CheckDependency(reference);

            if (reference.IsGenericInstance)
            {
                var referenceAsGenericInstance = reference as GenericInstanceType;
                
                if (referenceAsGenericInstance?.HasGenericArguments ?? false)
                {
                    foreach (var genericArgument in referenceAsGenericInstance.GenericArguments)
                    {
                        CheckTypeReference(genericArgument);
                    }
                }
            }

            if (!reference.IsArray && !reference.IsPointer && !reference.IsByReference)
            {
                return;
            }
            
            var referenceAsTypeSpecification = reference as TypeSpecification;
            CheckTypeReference(referenceAsTypeSpecification?.ElementType);
        }
        
        private void CheckDependency(TypeReference dependency)
        {
            _result.CheckDependency(dependency);
        }
    }
}