namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public event definition.    
    /// </summary>
    public class EventPublic
    {
        public event Action<ExampleDependency> ExampleProperty;        
    }
}
