namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a private method definition.
    /// </summary>
    public class PrivateMethod
    {
        private void ExampleMethod()
        {
            var test = new ExampleDependency();
        }
    }
}
