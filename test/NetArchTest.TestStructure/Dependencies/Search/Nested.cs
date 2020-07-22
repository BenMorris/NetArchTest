namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that has a dependency on a nested class.    
    /// </summary>
    public class Nested
    {
        public void ExampleMethod()
        {
            NestedDependency foo = null;
        }

        public class NestedDependency
        {
         
        }
    }
}
