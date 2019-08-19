namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in the return type of a method.
    /// </summary>
    public class MethodReturnType
    {
        private ExampleDependency ExampleMethod()
        {
            return null;
        }
    }
}
