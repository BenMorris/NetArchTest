namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a nested class from difrent class.    
    /// </summary>
    public class NestedDependencyClass
    {
        private void ExampleMethod()
        {
            NestedLevel1.NestedLevel2.NestedDependency foo = null;
        }
    }

    public class NestedLevel1
    {
        public class NestedLevel2
        {
            public class NestedDependency
            {

            }
        }
    }
}
