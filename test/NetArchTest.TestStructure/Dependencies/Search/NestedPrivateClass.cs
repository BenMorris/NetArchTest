namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies;

    /// <summary>
    /// Example class that includes a dependency in a nested private class.    
    /// </summary>
    public class NestedPrivateClass
    {
        public void ExampleMethod()
        {
            
        }

        private class NestedLevel1
        {
            private class NestedLevel2
            {
                private ExampleDependency dependency = new ExampleDependency();
            }
        }
    }
}
