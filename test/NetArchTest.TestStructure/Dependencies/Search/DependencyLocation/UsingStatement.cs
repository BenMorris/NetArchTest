namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System.Collections.Generic;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in using statement.     
    /// </summary>
    class UsingStatement
    {
        public UsingStatement()
        {
            using (var foo = new DisposableDependency())
            {

            }
        }
    }
}
