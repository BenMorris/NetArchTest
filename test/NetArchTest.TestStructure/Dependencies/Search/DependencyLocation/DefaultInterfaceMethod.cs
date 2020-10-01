namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{   
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example interface that includes a dependency in default method.    
    /// </summary>
    public interface DefaultInterfaceMethodBody
    {
        public void ExampleMethod()
        {
            var test = new ExampleDependency();            
        }
    }
}
