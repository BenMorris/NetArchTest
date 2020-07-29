namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a private constructor definition.    
    /// </summary>
    public class ConstructorPrivate
    {
        private ConstructorPrivate()
        {
            var test = new ExampleDependency();
        }

        public static ConstructorPrivate GetInstance()
        {
            return new ConstructorPrivate();
        }
    }
}
