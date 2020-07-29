namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a captured variable by lambda closure.    
    /// </summary>
    public class LambdaCapturedVariable
    {
        public Func<object> ExampleMethod()
        {
            ExampleDependency test = null;
            return () => test; 
        }
    }
}
