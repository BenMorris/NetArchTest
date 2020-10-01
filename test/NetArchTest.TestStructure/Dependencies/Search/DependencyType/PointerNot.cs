namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that does not include a dependency in pointer declaration.    
    /// </summary>
    public class PointerNot
    {
        public PointerNot()
        {
            unsafe
            {
                StructDependency test;
            }                      
        }
    }
}
