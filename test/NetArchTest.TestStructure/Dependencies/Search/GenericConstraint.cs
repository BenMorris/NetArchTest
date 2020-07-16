namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;
    using System.Collections.Generic;

    /// <summary>
    /// Example class that includes a dependency as a generic constraint.    
    /// </summary>
    public class GenericConstraint<T>  where T : ExampleDependency
    {
    }
}
