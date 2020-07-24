namespace NetArchTest.TestStructure.Dependencies.Search
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
            Tuple<int, ExampleDependency> test = null;           
        }
    }
}
