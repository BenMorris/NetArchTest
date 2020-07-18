namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public event definition.    
    /// </summary>
    public class PublicEvent
    {
        public event Action<ExampleDependency> ExampleProperty;        
    }
}
