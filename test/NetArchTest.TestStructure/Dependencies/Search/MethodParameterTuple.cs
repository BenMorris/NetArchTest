namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as a method parameter.
    /// </summary>
    public class MethodParameterTuple
    {
        private void ExampleMethod((int foo, ExampleDependency dependency) exampleDependency)
        {

        }
    }
}
