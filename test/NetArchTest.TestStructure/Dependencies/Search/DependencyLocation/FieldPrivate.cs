namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a private field definition.    
    /// </summary>
    public class FieldPrivate
    {
        private ExampleDependency Example;

        public FieldPrivate()
        {
        }
    }
}
