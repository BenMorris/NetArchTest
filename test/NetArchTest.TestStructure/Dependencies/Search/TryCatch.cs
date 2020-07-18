namespace NetArchTest.TestStructure.Dependencies.Search
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
            catch (ExceptionDependency ex) 
            {

            }
        }
    }
}
