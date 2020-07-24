namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes an array dependency    
    /// </summary>
    public class ArrayGenericNested
    { 
        public void ExampleMethod()
        {
            GenericClass<int>[] test1 = null;
            GenericClass<ExampleDependency>[] test2 = null;          
        }
    }
}
