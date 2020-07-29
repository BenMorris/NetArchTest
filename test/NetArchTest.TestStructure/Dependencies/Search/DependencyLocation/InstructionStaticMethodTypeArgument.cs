namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an instruction invocation.
    /// </summary>
    public class InstructionStaticMethodTypeArgument
    {
        public InstructionStaticMethodTypeArgument()
        {
            StaticGenericClass<int>.Foo<ExampleDependency>();
        }
    }
}
