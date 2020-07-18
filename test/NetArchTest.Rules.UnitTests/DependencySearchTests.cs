namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.Rules.Dependencies;
    using NetArchTest.TestStructure.Dependencies.Example;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Dependencies.Implementation;
    using NetArchTest.TestStructure.Dependencies.Search;
    using NetArchTest.TestStructure.FalsePositives.NamespaceMatch;
    using Xunit;

    public class DependencySearchTests
    {
        [Fact(DisplayName = "Finds a dependency in an async method.")]
        public void DependencySearch_AsyncMethod_Found()
        {
            this.RunDependencyTest(typeof(AsyncMethod));
        }

        [Theory(DisplayName = "Finds a dependency in an attribute.")]
        [InlineData(typeof(AttributeOnClass))]
        [InlineData(typeof(AttributeOnEvent))]
        [InlineData(typeof(AttributeOnField))]
        [InlineData(typeof(AttributeOnMethod))]
        [InlineData(typeof(AttributeOnParameter))]
        [InlineData(typeof(AttributeOnProperty))]
        [InlineData(typeof(AttributeOnReturnValue))]
        public void DependencySearch_Attribute_Found(Type input)
        {
            this.RunDependencyTest(input, typeof(AttributeDependency), true, true);
        }       

        [Fact(DisplayName = "Finds a dependency in a default interface method body.")]
        public void DependencySearch_DefaultInterfaceMethod_Found()
        {
            this.RunDependencyTest(typeof(DefaultInterfaceMethod));
        }

        [Fact(DisplayName = "Finds a dependency in a generic class constraint.")]
        public void DependencySearch_GenericClassConstraint_Found()
        {
            this.RunDependencyTest(typeof(GenericClassConstraint<>));
        }

        [Fact(DisplayName = "Finds a dependency in a generic method constraint.")]
        public void DependencySearch_GenericMethodConstraint_Found()
        {
            this.RunDependencyTest(typeof(GenericMethodConstraint));
        }       

        [Theory(DisplayName = "Finds a dependency in a generic type argument.")]
        [InlineData(typeof(GenericMethodTypeArgument))]
        [InlineData(typeof(GenericMethodTypeArgumentGeneric))]
        [InlineData(typeof(GenericMethodTypeArgumentNestedGeneric))]
        [InlineData(typeof(GenericMethodTypeArgumentTuple))]
        public void DependencySearch_GenericMethodTypeArgument_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Fact(DisplayName = "Does not find a dependency in an indirect reference.")]
        public void DependencySearch_IndirectReference_NotFound()
        {
            // NB: We only look for dependencies in the types being searched 
            this.RunDependencyTest(typeof(IndirectReference), false);
        }

        [Theory(DisplayName = "Finds a dependency that a class implements from.")]
        [InlineData(typeof(Implemented))]
        [InlineData(typeof(ImplementedGeneric))]
        [InlineData(typeof(ImplementedNestedGeneric))]
        [InlineData(typeof(ImplementedTuple))]
        public void DependencySearch_ImplementedInterface_Found(Type input)
        {
            this.RunDependencyTest(input, typeof(InterfaceDependecy), true, true);
        }

        [Theory(DisplayName = "Finds a dependency that a class inherits from.")]
        [InlineData(typeof(Inherited))]
        [InlineData(typeof(InheritedGeneric))]
        [InlineData(typeof(InheritedNestedGeneric))]
        [InlineData(typeof(InheritedTuple))]
        public void DependencySearch_InheritedClass_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Theory(DisplayName = "Finds a dependency in an instruction invocation.")]
        [InlineData(typeof(InstructionCtor))]
        [InlineData(typeof(InstructionStaticClassTypeArgument))]
        [InlineData(typeof(InstructionStaticMethodTypeArgument))]
        public void DependencySearch_Instruction_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Theory(DisplayName = "Finds a dependency in a static instruction invocation.")]
        [InlineData(typeof(InstructionStaticClass))]
        public void DependencySearch_InstructionStatic_Found(Type input)
        {
            this.RunDependencyTest(input, typeof(StaticDependency<>), true, true);
        }

        [Fact(DisplayName = "Does not find things that are not dependency at all")]       
        public void DependencySearch_Instruction_NotFound()
        {
            var search = new DependencySearch();
            var subject = Types
               .InAssembly(Assembly.GetAssembly(typeof(InstructionCtor)))
               .That().DoNotHaveNameStartingWith("NetArchTest.TestStructure.Dependencies.Search").GetTypeDefinitions();

            var resultClass = search.FindTypesWithAnyDependencies(subject, new List<string> { "System.Object::.ctor()" , "T", "T1", "T2", "ctor()", "!1)", "::.ctor(!0" });
            Assert.Equal(0, resultClass.Count);
        }

        [Theory(DisplayName = "Finds a dependency in a public method's argument.")]
        [InlineData(typeof(MethodArgument))]
        [InlineData(typeof(MethodArgumentGeneric))]
        [InlineData(typeof(MethodArgumentNestedGeneric))]
        [InlineData(typeof(MethodArgumentTuple))]
        public void DependencySearch_MethodArgument_Found(Type input)
        {
            this.RunDependencyTest(input);
        }        

        [Theory(DisplayName = "Finds a dependency in a public method's parameter.")]
        [InlineData(typeof(MethodParameter))]
        [InlineData(typeof(MethodParameterGeneric))]
        [InlineData(typeof(MethodParameterNestedGeneric))]
        [InlineData(typeof(MethodParameterTuple))]
        public void DependencySearch_MethodParameter_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Theory(DisplayName = "Finds a dependency in a public method's return type.")]
        [InlineData(typeof(MethodReturnType))]
        [InlineData(typeof(MethodReturnTypeGeneric))]
        [InlineData(typeof(MethodReturnTypeNestedGeneric))]
        [InlineData(typeof(MethodReturnTypeTuple))]
        public void DependencySearch_MethodReturnType_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Fact(DisplayName = "Finds a dependency in a nested private class.")]
        public void DependencySearch_NestedPrivateClass_Found()
        {
            this.RunDependencyTest(typeof(NestedPrivateClass));
        }

        [Fact(DisplayName = "Finds a dependency in a private constructor.")]
        public void DependencySearch_PrivateConstructor_Found()
        {
            this.RunDependencyTest(typeof(PrivateConstructor));
        }

        [Fact(DisplayName = "Finds a dependency in a private field.")]
        public void DependencySearch_PrivateField_Found()
        {
            this.RunDependencyTest(typeof(PrivateField));
        }

        [Fact(DisplayName = "Finds a dependency in a private method.")]
        public void DependencySearch_PrivateMethod_Found()
        {
            this.RunDependencyTest(typeof(PrivateMethod));
        }

        [Fact(DisplayName = "Finds a dependency in a private property.")]
        public void DependencySearch_PrivateProperty_Found()
        {
            this.RunDependencyTest(typeof(PrivateProperty));
        }

        [Fact(DisplayName = "Finds a dependency in a public constructor.")]
        public void DependencySearch_PublicConstructor_Found()
        {
            this.RunDependencyTest(typeof(PublicConstructor));
        }

        [Fact(DisplayName = "Finds a dependency in a public event.")]
        public void DependencySearch_PublicEvent_Found()
        {
            this.RunDependencyTest(typeof(PublicEvent));
        }

        [Theory(DisplayName = "Finds a dependency in a public field.")]
        [InlineData(typeof(PublicField))]
        [InlineData(typeof(PublicFieldGeneric))]
        [InlineData(typeof(PublicFieldNestedGeneric))]
        [InlineData(typeof(PublicFieldTuple))]
        public void DependencySearch_PublicField_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Fact(DisplayName = "Finds a dependency in a public indexer.")]
        public void DependencySearch_PublicIndexer_Found()
        {
            this.RunDependencyTest(typeof(PublicIndexer));
        }

        [Fact(DisplayName = "Finds a dependency in a public method.")]
        public void DependencySearch_PublicMethod_Found()
        {
            this.RunDependencyTest(typeof(PublicMethod));
        }

        [Theory(DisplayName = "Finds a dependency in a public property.")]
        [InlineData(typeof(PublicProperty))]
        [InlineData(typeof(PublicPropertyGeneric))]
        [InlineData(typeof(PublicPropertyNestedGeneric))]
        [InlineData(typeof(PublicPropertyTuple))]
        [InlineData(typeof(PublicPropertyGet))]
        [InlineData(typeof(PublicPropertySet))]
        public void DependencySearch_PublicProperty_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Fact(DisplayName = "Finds a dependency in a static local function body.")]
        public void DependencySearch_StaticLocalFunctions_Found()
        {
            this.RunDependencyTest(typeof(StaticLocalFunction));
        }

        [Fact(DisplayName = "Finds a dependency in a switch pattern matching.")]
        public void DependencySearch_SwitchPatternMatching_Found()
        {
            this.RunDependencyTest(typeof(SwitchPatternMatching));
        }        

        [Fact(DisplayName = "Finds a dependency in a catch statement.")]
        public void DependencySearch_TryCatch_Found()
        {
            this.RunDependencyTest(typeof(TryCatch), typeof(ExceptionDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a catch block.")]
        public void DependencySearch_TryCatchBlock_Found()
        {
            this.RunDependencyTest(typeof(TryCatchBlock), typeof(ExceptionDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in an exception filter.")]
        public void DependencySearch_TryCatchExceptionFilter_Found()
        {
            this.RunDependencyTest(typeof(TryCatchExceptionFilter), typeof(ExceptionDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a finally block.")]
        public void DependencySearch_TryFinallyBlock_Found()
        {
            this.RunDependencyTest(typeof(TryFinallyBlock));
        }

        [Fact(DisplayName = "Finds a dependency in a using statement.")]
        public void DependencySearch_UsingStatement_Found()
        {
            this.RunDependencyTest(typeof(UsingStatement), typeof(DisposableDependency), true, true);
        }

        [Theory(DisplayName = "Finds a dependency in a variable.")]
        [InlineData(typeof(Variable))]
        [InlineData(typeof(VariableGeneric))]
        [InlineData(typeof(VariableNestedGeneric))]
        [InlineData(typeof(VariableTuple))]
        public void DependencySearch_Variable_Found(Type input)
        {
            this.RunDependencyTest(input);
        }

        [Theory(DisplayName = "Does not find a dependency that only partially matches actually referenced type.")]
        [InlineData(typeof(AsyncMethod))]
        [InlineData(typeof(InheritedGeneric))]
        [InlineData(typeof(IndirectReference))]
        [InlineData(typeof(Inherited))]
        [InlineData(typeof(MethodReturnType))]
        [InlineData(typeof(NestedPrivateClass))]
        [InlineData(typeof(PrivateConstructor))]
        [InlineData(typeof(PrivateField))]
        [InlineData(typeof(PrivateMethod))]
        [InlineData(typeof(PrivateProperty))]
        [InlineData(typeof(PublicConstructor))]
        [InlineData(typeof(PublicField))]
        [InlineData(typeof(PublicMethod))]
        [InlineData(typeof(PublicProperty))]
        public void DependencySearch_PartiallyMatchingDependency_NotFound(Type input)
        {
            this.RunDependencyTest(input, dependencyToSearch: typeof(ExampleDep),
                                   expectToFindClass: false, expectToFindNamespace: input != typeof(IndirectReference));
        }

        [Theory(DisplayName = "Does not find a dependency from the namespace matching partially to the namespace of actually referenced type.")]
        [InlineData(typeof(AsyncMethod))]
        [InlineData(typeof(InheritedGeneric))]
        [InlineData(typeof(IndirectReference))]
        [InlineData(typeof(Inherited))]
        [InlineData(typeof(MethodReturnType))]
        [InlineData(typeof(NestedPrivateClass))]
        [InlineData(typeof(PrivateConstructor))]
        [InlineData(typeof(PrivateField))]
        [InlineData(typeof(PrivateMethod))]
        [InlineData(typeof(PrivateProperty))]
        [InlineData(typeof(PublicConstructor))]
        [InlineData(typeof(PublicField))]
        [InlineData(typeof(PublicMethod))]
        [InlineData(typeof(PublicProperty))]
        public void DependencySearch_PartiallyMatchingNamespace_NotFound(Type input)
        {
            this.RunDependencyTest(input, dependencyToSearch: typeof(ExampleDependencyInPartiallyMatchingNamespace),
                                   expectToFindClass: false, expectToFindNamespace: false);
        }

        [Theory(DisplayName = "Does not find a dependency that differs only in case from actually referenced type.")]
        [InlineData(typeof(AsyncMethod))]
        [InlineData(typeof(InheritedGeneric))]
        [InlineData(typeof(Inherited))]
        [InlineData(typeof(MethodReturnType))]
        [InlineData(typeof(NestedPrivateClass))]
        [InlineData(typeof(PrivateConstructor))]
        [InlineData(typeof(PrivateField))]
        [InlineData(typeof(PrivateMethod))]
        [InlineData(typeof(PrivateProperty))]
        [InlineData(typeof(PublicConstructor))]
        [InlineData(typeof(PublicField))]
        [InlineData(typeof(PublicMethod))]
        [InlineData(typeof(PublicProperty))]
        public void DependencySearch_DependencyWithDifferentCaseOfCharacters_NotFound(Type input)
        {
            this.RunDependencyTest(input, dependencyToSearch: typeof(ExampleDEPENDENCY),
                                   expectToFindClass: false, expectToFindNamespace: true);
        }

        [Fact(DisplayName = "A dependency search will not return false positives for pattern matched classes.")]
        public void FindTypesWithAllDependencies_PatternMatchedClasses_NotReturned()
        {
            // In this example, searching for a dependency on "PatternMatch" should not return "PatternMatchTwo"

            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .HaveName("ClassMatchingExample")
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAllDependencies(typeList, new List<string> { typeof(PatternMatch).FullName });

            // Assert: Before PR#36 this would have returned PatternMatchToo in the results
            Assert.Empty(result); // No results returned
        }

        [Fact(DisplayName = "A dependency search will not return false positives for pattern matched namespaces.")]
        public void FindTypesWithAllDependencies_PatternMatchedNamespaces_NotReturned()
        {
            // In this example, searching for a dependency on "NamespaceMatch" should not return classes in "NamespaceMatchToo"

            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .HaveName("NamespaceMatchingExample")
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAllDependencies(typeList, new List<string> { typeof(PatternMatch).Namespace });

            // Assert: Before PR#36 this would have returned classes in NamespaceMatchToo in the results
            Assert.Empty(result); // No results returned
        }

        [Fact(DisplayName = "A search for types with ANY dependencies returns types that have a dependency on at least one item in the list.")]
        public void FindTypesWithAnyDependencies_PublicProperty_Found()
        {
            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAnyDependencies(typeList, new List<string> { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName });

            // Assert
            Assert.Equal(3, result.Count); // Three types found
            Assert.Equal(typeof(HasDependencies).FullName, result.First().FullName); // Correct types returned...
            Assert.Equal(typeof(HasAnotherDependency).FullName, result.Skip(1).First().FullName);
            Assert.Equal(typeof(HasDependency).FullName, result.Last().FullName);
        }

        [Fact(DisplayName = "A search for types with ANY dependencies returns types that have a dependency on at least one item in the list. List contains overlapping dependencies.")]
        public void FindTypesWithAnyDependencies_WithOverlappingDependencies_Found()
        {
            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAnyDependencies(typeList, new List<string> { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName, "NetArchTest.TestStructure.Dependencies" });

            // Assert
            Assert.Equal(3, result.Count); // Three types found           
        }

        [Fact(DisplayName = "A search for types with ANY dependencies returns types that have a dependency on at least one item in the list. List contains only parent namespace")]
        public void FindTypesWithAnyDependencies_WithOnlyParentNamespace_Found()
        {
            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAnyDependencies(typeList, new List<string> {  "NetArchTest.TestStructure.Dependencies" });

            // Assert
            Assert.Equal(3, result.Count); // Three types found           
        }

        [Fact(DisplayName = "A search for types with ALL dependencies returns types that have a dependency on all the items in the list.")]
        public void FindTypesWithAllDependencies_PublicProperty_Found()
        {
            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAllDependencies(typeList, new List<string> { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName });

            // Assert
            Assert.Single(result); // One type found
            Assert.Equal(typeof(HasDependencies).FullName, result.First().FullName); // Correct type returned
        }

        [Fact(DisplayName = "A search for types with ALL dependencies returns types that have a dependency on all the items in the list. List contains overlapping dependencies.")]
        public void FindTypesWithAllDependencies_WithOverlappingDependencies_Found()
        {
            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAllDependencies(typeList, new List<string> { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName, "NetArchTest.TestStructure.Dependencies" });

            // Assert
            Assert.Single(result); // One type found
            Assert.Equal(typeof(HasDependencies).FullName, result.First().FullName); // Correct type returned
        }        

        /// <summary>
        /// Run a generic test against a target type to ensure that a single dependency is picked up by the search.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expectToFind"></param>
        private void RunDependencyTest(Type input, bool expectToFind = true)
        {
            RunDependencyTest(input, typeof(ExampleDependency), expectToFind, expectToFind);
        }

        private void RunDependencyTest(Type input, Type dependencyToSearch, bool expectToFindClass, bool expectToFindNamespace)
        {
            // Arrange
            var search = new DependencySearch();
            var subject = Types
                .InAssembly(Assembly.GetAssembly(input))
                .That().HaveName(input.Name).GetTypeDefinitions();

            // Act
            // Search against the type name and its namespace - this demonstrates that namespace based searches also work
            var resultClass = search.FindTypesWithAnyDependencies(subject, new List<string> { dependencyToSearch.FullName });
            var resultNamespace = search.FindTypesWithAnyDependencies(subject, new List<string> { dependencyToSearch.Namespace });

            // Assert
            if (expectToFindClass)
            {
                Assert.Single(resultClass); // Only one dependency found
                Assert.Equal(input.FullName, resultClass.First().FullName); // The correct dependency found
            }
            else
            {
                Assert.Equal(0, resultClass.Count); // No dependencies found
            }

            if (expectToFindNamespace)
            {
                Assert.Single(resultNamespace); // Only one dependency found
                Assert.Equal(input.FullName, resultNamespace.First().FullName); // The correct dependency found
            }
            else
            {
                Assert.Equal(0, resultNamespace.Count); // No dependencies found
            }
        }
    }
}