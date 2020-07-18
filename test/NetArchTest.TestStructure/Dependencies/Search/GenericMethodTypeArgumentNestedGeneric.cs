namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that has dependency as second type argument on the generic method invocation type argument list   
    /// </summary>
    public class GenericMethodTypeArgumentNestedGeneric
    {
        public void ExampleMethod()
        {
            GenericMethod<int, List<List<ExampleDependency>>>();
        }

        private void GenericMethod<T1, T2>()
        {
        }
    }
}