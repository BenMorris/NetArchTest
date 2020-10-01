namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.    
    /// </summary>
    public class VariableRefGenericTypeArgument
    {
        public VariableRefGenericTypeArgument()
        {
            ref GenericClass<double> test1 = ref Factory.CreateGenericDouble();
            ref GenericClass<ExampleDependency> test2 = ref Factory.CreateGeneric();           
        }
    }
}
