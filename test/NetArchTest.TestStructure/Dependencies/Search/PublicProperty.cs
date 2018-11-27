namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies;

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
