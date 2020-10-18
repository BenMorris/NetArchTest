namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes an array dependency    
    /// </summary>
    public class ArrayOfGenericsTypeArgument
    { 
        public void ExampleMethod()
        {
#pragma warning disable 219
            GenericClass<int>[] test1 = null;
            GenericClass<ExampleDependency>[] test2 = null;
#pragma warning restore 219
        }
    }
}
