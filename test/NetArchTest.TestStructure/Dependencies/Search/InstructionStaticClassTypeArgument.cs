namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an instruction invocation.
    /// </summary>
    public class InstructionStaticClassTypeArgument
    {
        public InstructionStaticClassTypeArgument()
        {
            StaticGenericClass<ExampleDependency>.Foo();
        }
    }
}
