using System;

namespace NetArchTest.Rules.Dependencies
{
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// Manages the parameters and results for a dependency search.
    /// </summary>
    internal class SearchResults
    {
        private readonly IEnumerable<TypeDefinition> _inputs;

        /// <summary> The list of dependencies that has been found in the search. </summary>
        private readonly Dictionary<string, HashSet<string>> _found;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResults"/> class.
        /// </summary>
        /// <param name="inputs"></param>
        internal SearchResults(IEnumerable<TypeDefinition> inputs)
        {
            _inputs = inputs;
            _found = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Gets the list of dependency names that have been found.
        /// </summary>
        internal IReadOnlyList<string> DependenciesFound
        {
            get
            {
                return _found.Values.SelectMany(t => t).ToArray();
            }
        }

        /// <summary>
        /// Gets the list of types that have dependencies.
        /// </summary>
        internal IReadOnlyList<string> TypesFound => _found.Keys.ToArray();

        /// <summary>
        /// Adds an item to the list of dependencies that have been found.
        /// </summary>
        internal void AddToFound(TypeDefinition type, string dependency)
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

            if (_found.ContainsKey(key))
            {
                _found[key].Add(dependency);
            }
            else
            {
                _found.Add(key, new HashSet<string> { dependency });
            }
        }

        public IEnumerable<TypeDefinition> GetResults(bool condition)
        {
            var output = new List<TypeDefinition>();
            foreach (var found in TypesFound)
            {
                // NB: Nested classes won't be picked up here
                var match = _inputs.FirstOrDefault(d => d.FullName.Equals(found, StringComparison.InvariantCultureIgnoreCase));
                if (match != null)
                {
                    output.Add(match);
                }
            }

            if (condition)
            {
                return output;
            }
            else
            {
                return _inputs.Where(t => !output.Contains(t));
            }
        }
    }
}
