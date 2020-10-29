namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a finally.    
    /// </summary>
    public class TryFinallyBlock
    {
        public void ExampleMethod()
        {
            try
            {

            }
            finally 
            {
#pragma warning disable 219
                ExampleDependency foo = null;
#pragma warning restore 219
            }
        }
    }
}
