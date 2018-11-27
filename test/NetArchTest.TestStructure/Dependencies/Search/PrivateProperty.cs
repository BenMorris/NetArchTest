namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies;

    /// <summary>
    /// Example class that includes a dependency in a private property definition.    
    /// </summary>
    public class PrivateProperty
    {
        private ExampleDependency ExampleProperty
        {
            get { return null; }
        }
    }
}
