namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as an attribute.    
    /// </summary>    
    public class AttributeOnReturnValue
    {
        [return : AttributeDependency()]
        private int method()
        {            
            return 7;
        }
    }
}
