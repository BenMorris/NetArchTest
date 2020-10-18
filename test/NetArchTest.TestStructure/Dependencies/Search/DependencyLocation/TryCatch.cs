namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a catch.    
    /// </summary>
    public class TryCatch
    {
        public void ExampleMethod()
        {
            try
            {

            }
#pragma warning disable 168
            catch (ExceptionDependency ex)
#pragma warning restore 168
            {

            }
        }
    }
}
