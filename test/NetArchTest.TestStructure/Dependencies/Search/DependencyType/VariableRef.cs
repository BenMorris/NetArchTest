namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.    
    /// </summary>
    public class VariableRef
    {
        public VariableRef()
        {
            ref ExampleDependency test = ref Factory.Create();           
        }
    }
}
