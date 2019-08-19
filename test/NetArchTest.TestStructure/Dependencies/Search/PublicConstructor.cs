namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public constructor definition.    
    /// </summary>
    public class PublicConstructor
    {
        public PublicConstructor()
        {
            var test = new ExampleDependency();
        }
    }
}
