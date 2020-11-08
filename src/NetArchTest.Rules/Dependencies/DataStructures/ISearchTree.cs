namespace NetArchTest.Rules.Dependencies.DataStructures
{
    using System.Collections.Generic;
    using Mono.Cecil;

    internal interface ISearchTree
    {
        IEnumerable<string> GetAllMatchingNames(TypeReference type);
        IEnumerable<string> GetAllMatchingNames(string fullName);
        int TerminatedNodesCount { get; }
    }
}
