namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using NetArchTest.Rules.Dependencies.DataStructures;

    /// <summary>
    /// Manages the results of dependency search.
    /// </summary>
    internal class TypeDefinitionCheckingResult
    {
        public enum SearchType 
        { 
            HaveDependencyOnAny,
            HaveDependencyOnAll, 
            OnlyHaveDependenciesOnAnyOrNone,
            OnlyHaveDependenciesOnAny,
            OnlyHaveDependenciesOnAll 
        }

        private readonly SearchType _searchType;
        private readonly ISearchTree _searchTree;
        /// <summary> The list of dependencies that have been found in the search.</summary>
        private HashSet<string> _foundDependencies = new HashSet<string>();
        private bool _hasDependencyFromOutsideOfSearchTree;
       

        public TypeDefinitionCheckingResult(SearchType searchType, ISearchTree searchTree)
        {
            _searchType = searchType;
            _searchTree = searchTree;
            _hasDependencyFromOutsideOfSearchTree = false;
        }


        public bool IsTypeFound()
        {            
            switch (_searchType)
            {
                case SearchType.HaveDependencyOnAll:
                    return _foundDependencies.Count == _searchTree.TerminatedNodesCount;
                case SearchType.HaveDependencyOnAny:
                    return _foundDependencies.Count > 0;
                case SearchType.OnlyHaveDependenciesOnAnyOrNone:
                    return !_hasDependencyFromOutsideOfSearchTree && _foundDependencies.Count >= 0;
                case SearchType.OnlyHaveDependenciesOnAny:
                    return !_hasDependencyFromOutsideOfSearchTree && _foundDependencies.Count > 0;
                case SearchType.OnlyHaveDependenciesOnAll:
                    return !_hasDependencyFromOutsideOfSearchTree && _foundDependencies.Count == _searchTree.TerminatedNodesCount;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// If we already know the final answer to the question if type was found,
        /// doing another search will not change the result
        /// </summary>     
        public bool CanWeSkipFurtherSearch()
        {
            switch (_searchType)
            {
                case SearchType.HaveDependencyOnAny:                  
                case SearchType.HaveDependencyOnAll:
                    return IsTypeFound() == true;
                case SearchType.OnlyHaveDependenciesOnAnyOrNone:                   
                case SearchType.OnlyHaveDependenciesOnAny:               
                case SearchType.OnlyHaveDependenciesOnAll:
                    return _hasDependencyFromOutsideOfSearchTree;                  
                default:
                    throw new NotImplementedException();
            }           
        }

        public void CheckDependency(string dependencyTypeFullName)
        {
            var matchedDependencies = _searchTree.GetAllMatchingNames(dependencyTypeFullName);
            if (matchedDependencies.Any())
            {
                foreach (var match in matchedDependencies)
                {
                    _foundDependencies.Add(match);
                }
            }
            else
            {
                _hasDependencyFromOutsideOfSearchTree = true;
            }
        }

        public void CheckDependency(TypeReference dependency)
        {
            var matchedDependencies = _searchTree.GetAllMatchingNames(dependency);
            if (matchedDependencies.Any())
            {
                foreach (var match in matchedDependencies)
                {
                    _foundDependencies.Add(match);
                }
            } 
            else
            {
                if (_hasDependencyFromOutsideOfSearchTree == false)
                {
                    bool isGlobalAnonymousCompilerGeneratedType = String.IsNullOrEmpty(dependency.Namespace) && dependency.Name.StartsWith("<>");
                    if (!isGlobalAnonymousCompilerGeneratedType)
                    {
                        _hasDependencyFromOutsideOfSearchTree = true;
                    }
                }
            }
        }
    }
}