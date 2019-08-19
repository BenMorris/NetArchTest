namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public method definition.    
    /// </summary>
    public class PublicMethod
    {
        public void ExampleMethod()
        {
            var test = new ExampleDependency();
        }
    }
}
