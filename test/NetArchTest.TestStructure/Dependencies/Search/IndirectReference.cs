namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes an indirect reference to a dependency in an different class.    
    /// </summary>
    public class IndirectReference
    {
        public void ExampleMethod()
        {
            var test = new ReferringType();
        }
    }

    public class ReferringType
    {
        public void Example()
        {
            var reference = new ExampleDependency();
        }
    }
}
