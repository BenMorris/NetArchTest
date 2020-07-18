namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as a method argument.
    /// </summary>
    public class MethodArgumentTuple
    {
        public MethodArgumentTuple()
        {
            ExampleMethod((0, new ExampleDependency()));
        }


        private void ExampleMethod(object parameter)
        {
           
        }
    }
}
