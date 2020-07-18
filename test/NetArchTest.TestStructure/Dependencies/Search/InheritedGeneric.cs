namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;
    using System.Collections.Generic;

    /// <summary>
    /// Example class that includes a dependency as a generic parameter.    
    /// </summary>
    public class InheritedGeneric: GenericClass<ExampleDependency>
    {
    }
}
