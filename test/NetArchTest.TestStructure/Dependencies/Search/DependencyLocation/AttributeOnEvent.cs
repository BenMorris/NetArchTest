namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
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
#pragma warning disable 67
        private event Action foo;
#pragma warning restore 67

    }
}
