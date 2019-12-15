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
    using Xunit;

    public class DependencySearchTests
    {
        [Fact(DisplayName = "Finds a dependency in an async method.")]
        public void DependencySearch_AsyncMethod_Found()
        {
            this.RunDependencyTest(typeof(AsyncMethod));
        }

        [Fact(DisplayName = "Finds a dependency in a generic parameter.")]
        public void DependencySearch_GenericParameter_Found()
        {
            this.RunDependencyTest(typeof(GenericPrameter));
        }

        [Fact(DisplayName = "Does not find a dependency in an indirect reference.")]
        public void DependencySearch_IndirectReference_NotFound()
        {
            // NB: We only look for dependencies in the types being searched 
            this.RunDependencyTest(typeof(IndirectReference), false);
        }

        [Fact(DisplayName = "Finds a dependency that a class inherits from.")]
        public void DependencySearch_Inherits_Found()
        {
            this.RunDependencyTest(typeof(Inherited));
        }

        [Fact(DisplayName = "Finds a dependency in a public method's return type.")]
        public void DependencySearch_MethodReturnType_Found()
        {
            this.RunDependencyTest(typeof(MethodReturnType));
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

        [Fact(DisplayName = "Finds a dependency in a public field.")]
        public void DependencySearch_PublicField_Found()
        {
            this.RunDependencyTest(typeof(PublicField));
        }

        [Fact(DisplayName = "Finds a dependency in a public method.")]
        public void DependencySearch_PublicMethod_Found()
        {
            this.RunDependencyTest(typeof(PublicMethod));
        }

        [Fact(DisplayName = "Finds a dependency in a public property.")]
        public void DependencySearch_PublicProperty_Found()
        {
            this.RunDependencyTest(typeof(PublicProperty));
        }

        [Theory(DisplayName = "Does not find a dependency that only partially matches actually referenced type.")]
        [InlineData(typeof(AsyncMethod))]
        [InlineData(typeof(GenericPrameter))]
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
        [InlineData(typeof(GenericPrameter))]
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
        [InlineData(typeof(GenericPrameter))]
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