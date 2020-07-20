namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an instruction invocation.
    /// </summary>
    public class GenericDependecy
    {
        public GenericDependecy()
        {
            GenericDependecy<int> dependecy = null;
        }
    }
}
