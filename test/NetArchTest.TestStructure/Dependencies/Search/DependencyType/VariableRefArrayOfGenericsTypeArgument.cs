namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.    
    /// </summary>
    public class VariableRefArrayOfGenericsTypeArgument
    {
        public VariableRefArrayOfGenericsTypeArgument()
        {
            ref GenericClass<double> test1 = ref Factory.CreateGenericDouble();
            ref GenericClass<double>[] test2 = ref Factory.CreateArrayOfDoubles();
            ref GenericClass<ExampleDependency>[] test3 = ref Factory.CreateArrayOfGenerics();           
        }
    }
}
