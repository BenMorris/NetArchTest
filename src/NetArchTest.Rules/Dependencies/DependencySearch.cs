namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// Finds dependencies within a given set of types.
    /// </summary>
    internal class DependencySearch
    {
        /// <summary>
        /// Finds types that have a dependency on any item in a given list of dependencies.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of found types.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesWithAnyDependencies(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {            
            var results = new SearchDefinition(SearchDefinition.SearchType.FindTypesWithAnyDependencies, dependencies);
            return FindTypes(input, results);           
        }

        /// <summary>
        /// Finds types that have a dependency on every item in a given list of dependencies.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of found types.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesWithAllDependencies(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {       
            var results = new SearchDefinition(SearchDefinition.SearchType.FindTypesWithAllDependencies, dependencies);
            return FindTypes(input, results);         
        }

        private List<TypeDefinition> FindTypes(IEnumerable<TypeDefinition> input, SearchDefinition searchDefinition)
        {
            var output = new List<TypeDefinition>();

            foreach (var type in input)
            {
                var context = new TypeDefinitionCheckingContext(type, searchDefinition);
                if (context.IsTypeFound())
                {
                    output.Add(type);
                }
            }

            return output;
        }
    }
}