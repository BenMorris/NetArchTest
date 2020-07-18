namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Collections.Generic;   
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.     
    /// </summary>
    public class VariableNestedGeneric
    {
        public VariableNestedGeneric()
        {
            List<List<ExampleDependency>> test = null;           
        }
    }
}
