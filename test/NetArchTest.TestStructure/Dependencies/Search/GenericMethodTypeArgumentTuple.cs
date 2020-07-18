namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that has dependency as type argument  
    /// </summary>
    public class GenericMethodTypeArgumentTuple
    {
        public void ExampleMethod()
        {
            GenericMethod<(int, ExampleDependency)>();
        }

        private void GenericMethod<T1>()
        {
        }
    }
}