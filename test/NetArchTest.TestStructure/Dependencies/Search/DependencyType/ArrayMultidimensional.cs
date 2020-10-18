namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes an array dependency    
    /// </summary>
    public class ArrayMultidimensional
    { 
        public void ExampleMethod()
        {
#pragma warning disable 219
            ExampleDependency[,] test = null;
#pragma warning restore 219

        }
    }
}
