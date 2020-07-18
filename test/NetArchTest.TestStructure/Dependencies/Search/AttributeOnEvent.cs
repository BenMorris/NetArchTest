namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as an attribute.    
    /// </summary>   
    public class AttributeOnEvent
    {
        [AttributeDependency()]
        private event Action foo;
    }
}
