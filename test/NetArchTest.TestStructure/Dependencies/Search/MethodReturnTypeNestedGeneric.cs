namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in the return type of a method.
    /// </summary>
    public class MethodReturnTypeNestedGeneric
    {
        private Task<List<ExampleDependency>> ExampleMethod()
        {
            return null;
        }
    }
}
