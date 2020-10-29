namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an event add accessor.    
    /// </summary>
    public class EventAdd
    {
        public event EventHandler ExampleProperty
        {
            add
            {
#pragma warning disable 219
                ExampleDependency test = null;
#pragma warning restore 219
            }
            remove
            {

            }
        }
    }
}
