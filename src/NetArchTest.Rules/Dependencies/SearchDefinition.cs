namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using NetArchTest.Rules.Dependencies.DataStructures;

    /// <summary>
    /// Manages the parameters and results for a dependency search.
    /// </summary>
    internal class SearchDefinition
    {
        public enum SearchType { FindTypesWithAnyDependencies, FindTypesWithAllDependencies }

        private readonly SearchType _searchType;

        /// <summary> The list of dependencies that has been found in the search. </summary>
        private readonly Dictionary<string, HashSet<string>> _found;

        /// <summary> The list of types that has been checked by the search. </summary>
        private readonly HashSet<string> _checkedTypes;

        /// <summary> The list of dependencies being searched for. </summary>
        private readonly NamespaceTree _searchTree;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDefinition"/> class.
        /// </summary>
        public SearchDefinition(SearchType searchType, IEnumerable<string> dependencies)
        {
            _searchType = searchType;
            _found = new Dictionary<string, HashSet<string>>();
            _checkedTypes = new HashSet<string>();
            _searchTree = new NamespaceTree(dependencies);
        }
                             

        /// <summary>
        /// Gets an indication of whether a type has been checked.
        /// If type has not been checked, it is marked as checked.
        /// </summary>
        public bool ShouldTypeBeChecked(TypeDefinition type)
        {
            if ((type == null) || _checkedTypes.Contains(type.FullName))
            {
                return false;
            }
            // Add the current type to the checked list - this prevents any circular checks
            _checkedTypes.Add(type.FullName);
            return true;
        }

        public bool IsTypeFound(TypeDefinition type)
        {
            if (_found.TryGetValue(type.FullName, out HashSet<string> bucket))
            {
                switch (_searchType)
                {
                    case SearchType.FindTypesWithAllDependencies:
                        return bucket.Count == _searchTree.TerminatedNodesCount;
                    case SearchType.FindTypesWithAnyDependencies:
                        // if the bucket exists in _found, we know that at least one dependency exists
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// If we already know the final answer to the question if type was found,
        /// doing another search will not change the result
        /// </summary>     
        public bool CanWeSkipFurtherSearch(TypeDefinition type)
        {
            return IsTypeFound(type) == true;
        }

        public void AddDependency(TypeDefinition type, TypeReference dependency)
        {
            type = GetParentTypeIfTypeIsNested(type);
            if (CanWeSkipFurtherSearch(type)) return;
            var matchedDependencies = GetAllMatchingNames(dependency);
            foreach (var match in matchedDependencies)
            {
                AddToFound(type.FullName, match);
            }
        }      

        /// <summary>
        /// Searching search tree is costly (it requires a lot of operations on strings like SubString, IndexOf).
        /// For a given type we always get the same answer, so let us cache what search tree returns.
        /// </summary>        
        TypeReferenceTree<IEnumerable<string>> cachedAnswersFromSearchTree = new TypeReferenceTree<IEnumerable<string>>();
        private IEnumerable<string> GetAllMatchingNames(TypeReference type)
        {
            var node = cachedAnswersFromSearchTree.GetNode(type);
            if (node.value == null)
            {
                node.value = _searchTree.GetAllMatchingNames(type).ToArray();
            }
            return node.value;
        }

        /// <summary>
        /// Adds an item to the list of dependencies that have been found.
        /// </summary>
        private void AddToFound(string typeFullName, string dependencyFullName)
        {  
            if (_found.TryGetValue(typeFullName, out var bucket))
            {
                bucket.Add(dependencyFullName);
            }
            else
            {
                _found.Add(typeFullName, new HashSet<string> { dependencyFullName });
            }
        }       

        private TypeDefinition GetParentTypeIfTypeIsNested(TypeDefinition type)
        {
            // For private nested types we should treat the parent as the dependency - e.g. async methods are always implemented as private nested classes
            while (type.IsNestedPrivate && type.DeclaringType != null)
            {
                type = type.DeclaringType;               
            }

            return type;
        }
    }
}