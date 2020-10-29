namespace NetArchTest.TestStructure.Dependencies.Search.DependencyType
{
    using System;
    using System.Collections.Generic;   
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in variable declaration.     
    /// </summary>
    public class VariableTuple
    {
        public VariableTuple()
        {
#pragma warning disable 219
            Tuple<int, ExampleDependency> test = null;
#pragma warning restore 219
        }
    }
}
