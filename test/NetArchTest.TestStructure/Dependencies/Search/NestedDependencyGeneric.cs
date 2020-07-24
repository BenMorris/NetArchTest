namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a nested class from difrent class.    
    /// </summary>
    public class NestedDependencyGeneric
    {
        private void ExampleMethod()
        {
            NestedDependencyTree.NestedLevel1<int>.NestedDependency foo = null;
        }
    }

    
}
