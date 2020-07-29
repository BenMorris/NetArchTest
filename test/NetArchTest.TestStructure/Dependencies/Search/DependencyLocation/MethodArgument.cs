namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as a method argument.
    /// </summary>
    public class MethodArgument
    {
        public MethodArgument()
        {
            ExampleMethod(new ExampleDependency());
        }


        private void ExampleMethod(object parameter)
        {
           
        }
    }
}
