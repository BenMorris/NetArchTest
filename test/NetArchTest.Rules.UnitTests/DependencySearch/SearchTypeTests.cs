namespace NetArchTest.Rules.UnitTests.DependencySearch
{
    using System.Linq;
    using System.Reflection;
    using NetArchTest.Rules.Dependencies;
    using NetArchTest.TestStructure.Dependencies.Implementation;
    using Xunit;

    [CollectionDefinition("Dependency Search - search type tests ")]
    public class SearchTypeTests
    {
        [Theory(DisplayName = "A search for types with ANY dependencies returns types that have a dependency on at least one item in the list.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies.Examples.ExampleDependency", "NetArchTest.TestStructure.Dependencies.Examples.AnotherExampleDependency" }, "List contains two distinct dependencies.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies.Examples.ExampleDependency", "NetArchTest.TestStructure.Dependencies.Examples.AnotherExampleDependency", "NetArchTest.TestStructure.Dependencies" }, "List contains overlapping dependencies.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies" }, "List contains only ancestor namespace.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies", "NetArchTest.TestStructure.Dependencies" }, "List contains duplicated ancestor namespace.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies", "NetArchTest.TestStructure.Dependencies.Examples" }, "List contains overlapping namespaces.")]
        public void FindTypesWithAnyDependencies_Found(string[] dependecies, string comment)
        {
            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAnyDependencies(typeList, dependecies);

            // Assert
            Assert.Equal(3, result.Count); // Three types found   
            Assert.Equal(typeof(HasDependencies).FullName, result.First().FullName); // Correct types returned...
            Assert.Equal(typeof(HasAnotherDependency).FullName, result.Skip(1).First().FullName);
            Assert.Equal(typeof(HasDependency).FullName, result.Last().FullName);
        }

        [Theory(DisplayName = "A search for types with ALL dependencies returns types that have a dependency on all the items in the list.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies.Examples.ExampleDependency", "NetArchTest.TestStructure.Dependencies.Examples.AnotherExampleDependency" }, "List contains two distinct dependencies.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies.Examples.ExampleDependency", "NetArchTest.TestStructure.Dependencies.Examples.AnotherExampleDependency", "NetArchTest.TestStructure.Dependencies" }, "List contains overlapping dependencies.")]      
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies.Examples.ExampleDependency", "NetArchTest.TestStructure.Dependencies.Examples.AnotherExampleDependency", "NetArchTest.TestStructure.Dependencies.Examples.AnotherExampleDependency" }, "List contains duplicated dependencies.")]
        [InlineData(new string[] { "NetArchTest.TestStructure.Dependencies.Examples.ExampleDependency", "NetArchTest.TestStructure.Dependencies.Examples.AnotherExampleDependency", "NetArchTest.TestStructure.Dependencies", "NetArchTest.TestStructure.Dependencies.Examples" }, "List contains overlapping namespaces.")]
        public void FindTypesWithAllDependencies_Found(string[] dependecies, string comment)
        {
            // Arrange
            var search = new DependencySearch();
            var typeList = Types
                .InAssembly(Assembly.GetAssembly(typeof(HasDependency)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .GetTypeDefinitions();

            // Act
            var result = search.FindTypesWithAllDependencies(typeList, dependecies);

            // Assert
            Assert.Single(result); // One type found
            Assert.Equal(typeof(HasDependencies).FullName, result.First().FullName); // Correct type returned
        }  
    }
}