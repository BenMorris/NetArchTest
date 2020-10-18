namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an event add accessor.    
    /// </summary>
    public class EventRemove
    {
        public event EventHandler ExampleProperty
        {
            add
            {
                
            }
            remove
            {
#pragma warning disable 219
                ExampleDependency test = null;
#pragma warning restore 219
            }
        }
    }
}
