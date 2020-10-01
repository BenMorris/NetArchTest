namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Example class that includes a dependency as a generic class constraint.    
    /// </summary>
    public class GenericConstraintClass<T>  where T : ExampleDependency, new() 
    {
        private List<Task<T>> foo;
    }
}
