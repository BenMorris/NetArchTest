namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;
    using System.Collections.Generic;

    /// <summary>
    /// Example class that includes a dependency as a generic method constraint.    
    /// </summary>
    public class GenericMethodConstraint
    {
        void Method<T>(T foo) where T : ExampleDependency
        {

        }
    }
}
