namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an instruction invocation.
    /// </summary>
    public class GenericDependecyArray
    {
        public GenericDependecyArray()
        {
            GenericDependecy<double>[] dependecy1 = null;
            GenericDependecy<int>[] dependecy2 = null;
        }
    }
}
