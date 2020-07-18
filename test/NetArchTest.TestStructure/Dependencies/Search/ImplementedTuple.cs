namespace NetArchTest.TestStructure.Dependencies.Search
{
    using NetArchTest.TestStructure.Dependencies.Examples;
        
    /// <summary>
    /// Example class that implements dependency.    
    /// </summary>   
    class ImplementedTuple : ITuple<int , InterfaceDependecy>
    {
    }


    public interface ITuple<T1, T2>
    {
    }
}
