namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes an array dependency    
    /// </summary>
    public class Array
    { 
        public void ExampleMethod()
        {
            var test = new ExampleDependency[7];          
        }
    }
}
