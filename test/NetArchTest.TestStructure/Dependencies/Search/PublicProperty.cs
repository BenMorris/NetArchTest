namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public property definition.    
    /// </summary>
    public class PublicProperty
    {
        public ExampleDependency ExampleProperty
        {
            get { return null; }
        }
    }
}
