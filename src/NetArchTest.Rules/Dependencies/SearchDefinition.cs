namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using NetArchTest.Rules.Dependencies.DataStructures;

    /// <summary>
    /// Manages the parameters of dependency search.
    /// </summary>
    internal class SearchDefinition
    {
        public enum SearchType { FindTypesWithAnyDependencies, FindTypesWithAllDependencies }

        private readonly SearchType _searchType; 

        /// <summary> The list of dependencies being searched for. </summary>
        private readonly NamespaceTree _searchTree;
       

        public SearchDefinition(SearchType searchType, IEnumerable<string> dependencies)
        {
            _searchType = searchType;            
            _searchTree = new NamespaceTree(dependencies, true);
        }

        public bool IsTypeFound(HashSet<string> dependencies)
        {            
            switch (_searchType)
            {
                case SearchType.FindTypesWithAllDependencies:
                    return dependencies.Count == _searchTree.TerminatedNodesCount;
                case SearchType.FindTypesWithAnyDependencies:
                    return dependencies.Count > 0;
            }
            
            return false;
        }

        /// <summary>
        /// If we already know the final answer to the question if type was found,
        /// doing another search will not change the result
        /// </summary>     
        public bool CanWeSkipFurtherSearch(HashSet<string> dependencies)
        {
            return IsTypeFound(dependencies) == true;
        }          

        /// <summary>
        /// Searching search tree is costly (it requires a lot of operations on strings like SubString, IndexOf).
        /// For a given type we always get the same answer, so let us cache what search tree returns.
        /// </summary>        
        TypeReferenceTree<string[]> _cachedAnswersFromSearchTree = new TypeReferenceTree<string[]>();
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