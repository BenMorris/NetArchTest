namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
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

#pragma warning disable 8321
            static void LocalFunction()
            {
                var y = new ExampleDependency();
            }
#pragma warning restore 8321
        }
    }
}
