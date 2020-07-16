namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a static local function body.    
    /// </summary>
    public class StaticLocalFunction
    {
        public void ExampleMethod()
        {

            static void LocalFunction() 
            {
                var y = new ExampleDependency(); 
            }
        }
    }
}
