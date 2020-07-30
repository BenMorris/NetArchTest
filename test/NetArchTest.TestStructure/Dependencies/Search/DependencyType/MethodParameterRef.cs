namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in method's parameter passed by reference.
    /// </summary>
    public class MethodParameterRef
    {
        public void Foo(ref ExampleDependency example)
        {
            
        }
    }
}
