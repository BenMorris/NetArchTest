namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a nested class from difrent class.    
    /// </summary>
    public class NestedDependencyClass
    {
        private void ExampleMethod()
        {
            NestedDependencyTree.NestedLevel1.NestedDependency foo = null;
        }
    }    
}