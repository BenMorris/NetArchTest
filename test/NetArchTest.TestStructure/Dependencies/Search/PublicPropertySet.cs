namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public property definition.    
    /// </summary>
    public class PublicPropertySet
    {
        public ExampleDependency ExampleProperty
        {
            set { var foo = value as ExampleDependency; }
        }
    }
}
