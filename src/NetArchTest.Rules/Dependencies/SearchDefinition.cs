namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

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

        public bool IsTypeFound(string typeFullName)
        {
            if (!_found.ContainsKey(typeFullName))
            {
                return false;
            }
            switch (_searchType)
            {
                case SearchType.FindTypesWithAllDependencies:
                    return _found[typeFullName].Count == _searchTree.TerminatedNodesCount;
                case SearchType.FindTypesWithAnyDependencies:
                    return true;              
            }
            return false;
        }

        public void AddDependency(TypeDefinition type, string dependency)
        {
            string typeFullName = GetTypeFullName(type);
            if (IsTypeFound(typeFullName)) return; // if we already know that type is found, doing another search does not change the result
            var matchedDependencies = GetAllMatchingNames(dependency);            
            foreach (var match in matchedDependencies)
            {
                AddToFound(typeFullName, match);
            }
        }

        public void AddDependencies(TypeDefinition type, IEnumerable<string> dependencies)
        {
            string typeFullName = GetTypeFullName(type);
            if (IsTypeFound(typeFullName)) return; // if we already know that type is found, doing another search does not change the result
            foreach(var dependency in dependencies)
            {
                var matchedDependencies = GetAllMatchingNames(dependency);                
                foreach (var match in matchedDependencies)
                {
                    AddToFound(typeFullName, match);
                }
            }
        }

        /// <summary>
        /// Searching tree is costly (it requires a lot of operations on strings like SubString, IndexOf).
        /// For a given dependency we always get the same answer, so let us cache what tree returns.
        /// </summary>
        private readonly Dictionary<string, IEnumerable<string>> cachedAnswersFromSearchTree = new Dictionary<string, IEnumerable<string>>();
        private IEnumerable<string> GetAllMatchingNames(string dependecy)
        {
            if (!cachedAnswersFromSearchTree.ContainsKey(dependecy))
            {
                cachedAnswersFromSearchTree[dependecy] = _searchTree.GetAllMatchingNames(dependecy).ToArray();
            }
            return cachedAnswersFromSearchTree[dependecy];
        }

        /// <summary>
        /// Adds an item to the list of dependencies that have been found.
        /// </summary>
        private void AddToFound(string typeFullName, string dependencyFullName)
        {  
            if (_found.ContainsKey(typeFullName))
            {
                _found[typeFullName].Add(dependencyFullName);
            }
            else
            {
                _found.Add(typeFullName, new HashSet<string> { dependencyFullName });
            }
        }
       

        private string GetTypeFullName(TypeDefinition type)
        {
            // For private nested types we should treat the parent as the dependency - e.g. async methods are always implemented as private nested classes
            var key = type.FullName;
            while (type != null && type.IsNestedPrivate)
            {
                type = type.DeclaringType;
                if (type != null)
                {
                    key = type.FullName;
                }
            }

            return key;
        }
    }
}