namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public property definition.    
    /// </summary>
    public class PublicPropertyGet
    {
        public object ExampleProperty
        {
            get { return new ExampleDependency(); }
        }
    }
}
