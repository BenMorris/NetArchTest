namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public property definition.    
    /// </summary>
    public class PublicPropertyGeneric
    {
        public List<ExampleDependency> ExampleProperty
        {
            get { return null; }
        }
    }
}
