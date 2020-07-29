namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes an array dependency    
    /// </summary>
    public class ArrayOfGenerics
    { 
        public void ExampleMethod()
        {
            GenericDependecy<double>[] test1 = null;
            GenericDependecy<int>[] test2 = null;          
        }
    }
}
