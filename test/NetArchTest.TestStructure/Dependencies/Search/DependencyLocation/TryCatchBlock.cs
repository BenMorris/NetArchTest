namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a catch.    
    /// </summary>
    public class TryCatchBlock
    {
        public void ExampleMethod()
        {
            try
            {

            }
            catch (Exception ex) 
            {
                object foo = ex as ExceptionDependency;
            }
        }
    }
}
