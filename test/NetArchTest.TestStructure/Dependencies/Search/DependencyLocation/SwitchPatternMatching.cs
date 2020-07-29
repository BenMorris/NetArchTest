namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System;
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in switch pattern matching.    
    /// </summary>
    class SwitchPatternMatching
    {
        public SwitchPatternMatching()
        {
            var foo = new Object();
            switch (foo)
            {
                case ExampleDependency _:
                    break;
            }
        }
    }
}
