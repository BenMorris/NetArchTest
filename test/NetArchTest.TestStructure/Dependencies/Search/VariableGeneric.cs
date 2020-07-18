namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;   
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.     
    /// </summary>
    public class VariableGeneric
    {
        public VariableGeneric()
        {
            List<ExampleDependency> test = null;           
        }
    }
}
