namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a private field definition.    
    /// </summary>
    public class PrivateField
    {
        private ExampleDependency Example = new ExampleDependency();

        public PrivateField()
        {
        }
    }
}
