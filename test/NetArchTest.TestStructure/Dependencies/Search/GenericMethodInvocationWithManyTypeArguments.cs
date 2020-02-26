namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that has dependency as second type argument on the generic method invocation type argument list   
    /// </summary>
    public class GenericMethodInvocationWithManyTypeArguments
    {
        public void ExampleMethod()
        {
            GenericMethod<int, ExampleDependency>();
        }

        private void  GenericMethod<T1, T2>()
        {
        }
    }
}