namespace NetArchTest.Rules.Dependencies.DataStructures
{  
    using System.Collections.Generic;
    using System.Linq;   
    using Mono.Cecil;

    internal class CachedNamespaceTree : ISearchTree
    {
        /// <summary> The list of dependencies being searched for. </summary>
        private readonly NamespaceTree _searchTree;

        public int TerminatedNodesCount { get => _searchTree.TerminatedNodesCount; }


        public CachedNamespaceTree(IEnumerable<string> dependencies)
        {
            _searchTree = new NamespaceTree(dependencies, true);
        }


        /// <summary>
        /// Searching search tree is costly (it requires a lot of operations on strings like SubString, IndexOf).
        /// For a given type we always get the same answer, so let us cache what search tree returns.
        /// </summary>        
        private readonly TypeReferenceTree<string[]> _cachedAnswersFromSearchTree = new TypeReferenceTree<string[]>();   
        public IEnumerable<string> GetAllMatchingNames(TypeReference type)
        {
            var node = _cachedAnswersFromSearchTree.GetNode(type);
            if (node.value == null)
            {
                node.value = _searchTree.GetAllMatchingNames(type).ToArray();
            }
            return node.value;
        }
    }
}