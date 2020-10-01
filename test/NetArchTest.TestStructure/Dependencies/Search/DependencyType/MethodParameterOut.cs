namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an instruction invocation.
    /// </summary>
    public class MethodParameterOut
    {
        public void Foo(out ExampleDependency example)
        {
            example = null;
        }
    }
}
