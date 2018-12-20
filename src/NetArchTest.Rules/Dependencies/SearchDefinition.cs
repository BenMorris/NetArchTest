namespace NetArchTest.Rules.Dependencies
{
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// Manages the parameters and results for a dependency search.
    /// </summary>
    internal class SearchDefinition
    {
        /// <summary> The list of dependencies that has been found in the search. </summary>
        private readonly Dictionary<string, HashSet<string>> _found;

        /// <summary> The list of types that has been checked by the search. </summary>
        private readonly HashSet<string> _checked;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDefinition"/> class.
        /// </summary>
        internal SearchDefinition()
        {
            _found = new Dictionary<string, HashSet<string>>();
            _checked = new HashSet<string>();
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
        internal IReadOnlyList<string> TypesFound
        {
            get
            {
                return _found.Keys.ToArray();
            }
        }

        /// <summary>
        /// Gets an indication of whether a type has been searched.
        /// </summary>
        internal bool IsChecked(string typeFullName)
        {
            return string.IsNullOrEmpty(typeFullName) || _checked.Contains(typeFullName);
        }

        /// <summary>
        /// Adds an item to the list of types that have been searched.
        /// </summary>
        internal void AddToChecked(TypeDefinition type)
        {
            _checked.Add(type.FullName);
        }

        /// <summary>
        /// Adds an item to the list of dependencies that have been found.
        /// </summary>
        internal void AddToFound(TypeDefinition type, string dependency)
        {
            // For private nested types we should treat the parent as the dependency - e.g. async methods are always implemented as private nested classes
            var key = type.FullName;
            while (type.IsNestedPrivate || type == null)
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
    }
}
