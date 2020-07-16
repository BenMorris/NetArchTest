namespace NetArchTest.TestStructure.Dependencies.Search
{   
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example interface that includes a dependency in default method.    
    /// </summary>
    public interface DefaultInterfaceMethod
    {
        public void ExampleMethod()
        {
            var test = new ExampleDependency();            
        }
    }
}
