namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;
    using System.Collections.Generic;

    /// <summary>
    /// Example class that includes a dependency as a generic method constraint.    
    /// </summary>
    public class GenericConstraintMethod
    {
        void Method<T>(T foo) where T : ExampleDependency
        {

        }
    }
}
