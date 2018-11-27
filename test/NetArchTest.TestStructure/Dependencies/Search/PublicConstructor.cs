namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies;

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
