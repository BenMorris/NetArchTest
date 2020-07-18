namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.    
    /// </summary>
    public class Variable
    {
        public Variable()
        {
            ExampleDependency test = null;           
        }
    }
}
