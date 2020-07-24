namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes an array dependency    
    /// </summary>
    public class ArrayGeneric
    { 
        public void ExampleMethod()
        {
            GenericDependecy<double>[] test1 = null;
            GenericDependecy<int>[] test2 = null;          
        }
    }
}
