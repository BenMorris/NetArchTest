namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public constructor definition.    
    /// </summary>
    public class PrivateConstructor
    {
        private PrivateConstructor()
        {
            var test = new ExampleDependency();
        }

        public static PrivateConstructor GetInstance()
        {
            return new PrivateConstructor();
        }
    }
}
