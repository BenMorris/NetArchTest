namespace NetArchTest.Rules.UnitTests.DependencySearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Mono.Cecil;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Dependencies.Search;
    using NetArchTest.TestStructure.Dependencies.Search.DependencyLocation;
    using Xunit;

    internal static class Utils
    {
        /// <summary>
        /// Run a generic test against a target type to ensure that a single dependency is picked up by the search.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expectToFind"></param>
        public static void RunDependencyTest(Type input, bool expectToFind = true)
        {
            RunDependencyTest(input, typeof(ExampleDependency), expectToFind, expectToFind);
        }       

        public static void RunDependencyTest(Type input, IEnumerable<string> dependenciesToSearch, bool expectToFind)
        {
            IEnumerable<Mono.Cecil.TypeDefinition> subject = GetSubject(input);
            FindTypesWithAnyDependencies(subject, dependenciesToSearch, expectToFind);
        }

        public static void RunDependencyTest(Type input, Type dependencyToSearch, bool expectToFindClass, bool expectToFindNamespace)
        {
            IEnumerable<Mono.Cecil.TypeDefinition> subject = GetSubject(input);

            // Search against the type name and its namespace - this demonstrates that namespace based searches also work
            FindTypesWithAnyDependencies(subject, new List<string> { dependencyToSearch.FullName }, expectToFindClass);
            FindTypesWithAnyDependencies(subject, new List<string> { dependencyToSearch.Namespace }, expectToFindNamespace);
        }

        private static IEnumerable<TypeDefinition> GetSubject(Type input)
        {
            IEnumerable<TypeDefinition> subject;
            if (input != null)
            {
                subject = Types.InAssembly(Assembly.GetAssembly(input)).That().HaveName(input.Name).GetTypeDefinitions();
            }
            else
            {
                subject = Types.InAssembly(Assembly.GetAssembly(typeof(InstructionCtor)))
                    .That()
                    .ResideInNamespaceStartingWith(typeof(InstructionCtor).Namespace)
                    .And()
                    .DoNotHaveName(typeof(IndirectReference).Name)
                    .GetTypeDefinitions();
            }

            return subject;
        }

        private static void FindTypesWithAnyDependencies(IEnumerable<Mono.Cecil.TypeDefinition> subjects, IEnumerable<string> dependencies, bool expectToFind)
        {
            // Arrange
            var search = new global::NetArchTest.Rules.Dependencies.DependencySearch();

            // Act
            // Search against the dependencies
            var resultClass = search.FindTypesWithAnyDependencies(subjects, dependencies);

            // Assert
            if (expectToFind)
            {
                Assert.Equal(subjects.Count(), resultClass.Count);
                Assert.Equal(subjects.First().FullName, resultClass.First().FullName); 
            }
            else
            {
                Assert.Equal(0, resultClass.Count); 
            }
        }
    }
}
