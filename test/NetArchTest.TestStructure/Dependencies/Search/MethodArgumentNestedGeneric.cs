namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as a method argument.
    /// </summary>
    public class MethodArgumentNestedGeneric
    {
        public MethodArgumentNestedGeneric()
        {
            ExampleMethod(new List<List<ExampleDependency>>());
        }


        private void ExampleMethod(object parameter)
        {
           
        }
    }
}
