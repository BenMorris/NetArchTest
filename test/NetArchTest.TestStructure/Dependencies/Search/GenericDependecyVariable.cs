namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an instruction invocation.
    /// </summary>
    public class GenericDependecyVariable
    {
        public GenericDependecyVariable()
        {
            GenericDependecy<int> dependecy = null;
        }
    }
}
