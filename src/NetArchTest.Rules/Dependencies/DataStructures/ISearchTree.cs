namespace NetArchTest.Rules.Dependencies.DataStructures
{
    using System.Collections.Generic;
    using Mono.Cecil;

    internal interface ISearchTree
    {
        IEnumerable<string> GetAllMatchingNames(TypeReference type);
        int TerminatedNodesCount { get; }
    }
}
