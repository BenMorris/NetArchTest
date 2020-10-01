namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Collections.Generic;   
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.     
    /// </summary>
    public class VariableGenericTypeArgument
    {
        public VariableGenericTypeArgument()
        {
            GenericClass<ExampleDependency> test = null;           
        }
    }
}
