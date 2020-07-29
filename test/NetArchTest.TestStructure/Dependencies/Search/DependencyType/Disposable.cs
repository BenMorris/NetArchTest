namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System.Collections.Generic;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in using statement.     
    /// </summary>
    class Disposable
    {
        public Disposable()
        {
            using(var foo = new DisposableDependency())
            {

            }
        }
    }
}
