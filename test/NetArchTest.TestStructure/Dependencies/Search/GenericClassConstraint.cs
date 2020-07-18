namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;
    using System.Collections.Generic;

    /// <summary>
    /// Example class that includes a dependency as a generic class constraint.    
    /// </summary>
    public class GenericClassConstraint<T>  where T : ExampleDependency, new() 
    {
    }
}
