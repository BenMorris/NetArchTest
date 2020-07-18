namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;
        
    /// <summary>
    /// Example class that implements dependency.    
    /// </summary>   
    class ImplementedNestedGeneric : IGenericInterface<IGenericInterface<InterfaceDependecy>>
    {
    }
}
