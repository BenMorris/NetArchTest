﻿namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency as an attribute.    
    /// </summary>   
    public class AttributeOnProperty
    {
        [AttributeDependency()]
        private int Field { get; }
    }
}
