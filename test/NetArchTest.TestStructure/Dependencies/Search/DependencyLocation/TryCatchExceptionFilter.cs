namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in an exception filter.    
    /// </summary>
    public class TryCatchExceptionFilter
    {
        public void ExampleMethod()
        {
            try
            {

            }
            catch (Exception ex) when (ex is ExceptionDependency) 
            {

            }
        }
    }
}
