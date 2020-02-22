namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// A sequence of function calls that are combined to select types.
    /// </summary>
    public sealed class FunctionSequence
    {
        /// <summary> Holds the groups of function calls. </summary>
        private readonly List<List<FunctionCall>> _groups;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionSequence"/> class.
        /// </summary>
        internal FunctionSequence()
        {
            _groups = new List<List<FunctionCall>>();
            _groups.Add(new List<FunctionCall>());
        }

        /// <summary>
        /// Adds a function call to the current list.
        /// </summary>
        internal void AddFunctionCall<T>(FunctionDelegates.FunctionDelegate<T> method, T value, bool condition)
        {
            _groups.Last().Add(new FunctionCall(method, value, condition));
        }

        /// <summary>
        /// Creates a new logical grouping of function calls.
        /// </summary>
        internal void CreateGroup()
        {
            _groups.Add(new List<FunctionCall>());
        }

        /// <summary>
        /// Executes all the function calls that have been specified.
        /// </summary>
        /// <returns>A list of types that are selected by the predicates (or not selected if optional reversing flag is passed).</returns>
        internal IEnumerable<TypeDefinition> Execute(IEnumerable<TypeDefinition> input, bool selected = true)
        {
            var resultSets = new List<List<TypeDefinition>>();

            // Execute each group of calls - each group represents a separate "or"
            foreach (var group in _groups)
            {
                // Create a copy of the class collection
                var results = new List<TypeDefinition>();
                foreach (var type in input)
                {
                    results.Add(type);
                }

                // Invoke the functions iteratively - functions within a group are treated as "and" statements
                foreach (var func in group)
                {
                    var funcResults = func.FunctionDelegate.DynamicInvoke(results, func.Value, func.Condition) as IEnumerable<TypeDefinition>;
                    results = funcResults.ToList();
                }

                if (results.Count > 0)
                {
                    resultSets.Add(results);
                }
            }

            if (selected)
            {
                // Return all the types that appear in at least one of the result sets
                return resultSets.SelectMany(list => list.Select(def => def)).Distinct();
            }
            else
            {
                // Return all the types that *do not* appear in at least one of the result sets
                var selectedTypes = resultSets.SelectMany(list => list.Select(def => def)).Distinct().Select(t => t.FullName);
                var notSelected = input.Where(t => !selectedTypes.Contains(t.FullName));
                return notSelected;
            }
        }


        /// <summary>
        /// Represents a single function call.
        /// </summary>
        internal class FunctionCall
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FunctionCall"/> class.
            /// </summary>
            internal FunctionCall(Delegate func, object value, bool condition)
            {
                this.FunctionDelegate = func;
                this.Value = value;
                this.Condition = condition;
            }

            /// <summary>
            /// A delegate for a function call.
            /// </summary>
            public Delegate FunctionDelegate { get; private set; }

            /// <summary>
            /// The input value for the function call.
            /// </summary>
            public object Value { get; private set; }

            /// <summary>
            /// The Condition to apply to the call - i.e. "is" or "is not".
            /// </summary>
            public bool Condition { get; private set; }

        }
    }
}
