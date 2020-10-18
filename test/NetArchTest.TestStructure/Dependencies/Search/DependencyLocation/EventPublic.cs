namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public event definition.    
    /// </summary>
    public class EventPublic
    {
#pragma warning disable 67
        public event Action<ExampleDependency> ExampleProperty;
#pragma warning restore 67
    }
    }
