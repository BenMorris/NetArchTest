namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public property definition.    
    /// </summary>
    public class PublicPropertyTuple
    {
        public Tuple<int, ExampleDependency> ExampleProperty
        {
            get { return null; }
        }
    }
}
