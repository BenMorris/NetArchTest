namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in the return type of a method.
    /// </summary>
    public class MethodReturnTypeGeneric
    {
        private Task<ExampleDependency> ExampleMethod()
        {
            return null;
        }
    }
}
