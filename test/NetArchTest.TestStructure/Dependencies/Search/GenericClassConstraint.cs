namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Example class that includes a dependency as a generic class constraint.    
    /// </summary>
    public class GenericClassConstraint<T>  where T : ExampleDependency, new() 
    {
        private List<Task<T>> foo;
    }
}
