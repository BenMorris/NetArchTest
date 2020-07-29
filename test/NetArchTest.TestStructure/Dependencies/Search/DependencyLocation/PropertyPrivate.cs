namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a private property definition.    
    /// </summary>
    public class PropertyPrivate
    {
        private ExampleDependency ExampleProperty
        {
            get { return null; }
        }
    }
}
