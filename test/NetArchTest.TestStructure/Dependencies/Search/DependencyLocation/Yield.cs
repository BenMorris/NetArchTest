namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a yield return statement.    
    /// </summary>
    public class Yield
    {
        public IEnumerable<object> ExampleMethod()
        {
            yield return 7;
            yield return new ExampleDependency();
            yield return 77;
        }
    }
}
