namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public property definition.    
    /// </summary>
    public class PropertySetter
    {
        public object ExampleProperty
        {
            set { var foo = value as ExampleDependency; }
        }
    }
}
