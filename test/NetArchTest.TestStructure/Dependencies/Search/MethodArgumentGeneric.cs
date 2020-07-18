namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as a method argument.
    /// </summary>
    public class MethodArgumentGeneric
    {
        public MethodArgumentGeneric()
        {
            ExampleMethod(new List<ExampleDependency>());
        }


        private void ExampleMethod(object parameter)
        {
           
        }
    }
}
