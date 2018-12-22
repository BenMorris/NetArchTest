using NetArchTest.Rules.Matches;

namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetArchTest.Rules.Extensions;
    using Mono.Cecil;

    /// <summary>
    /// A set of predicates and types that have have conjunctions (i.e. "and", "or") and executors (i.e. Types(), TypeDefinitions()) applied to them.
    /// </summary>
    public sealed class PredicateList
    {
        /// <summary> A list of types that conditions can be applied to. </summary>
        private readonly IEnumerable<TypeDefinition> _types;

        /// <summary> The sequence of conditions that is applied to the type of list. </summary>
        private readonly FunctionSequence _sequence;

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateList"/> class.
        /// </summary>
        internal PredicateList(IEnumerable<TypeDefinition> classes, FunctionSequence sequence) 
        {
            _types = classes;
            _sequence = sequence;
        }
        
        internal PredicateList(IEnumerable<TypeDefinition> classes, 
            Func<IEnumerable<TypeDefinition>, IEnumerable<TypeDefinition>> filter) 
        {
            _types = classes;
            _sequence = new FunctionSequence();
            if (filter!= null)
            {
                _sequence.Add(filter);    
            }            
        }
        
        internal PredicateList(IEnumerable<TypeDefinition> classes) 
        {
            _types = classes;
            _sequence = new FunctionSequence();
        }

        public ConditionList Should(Filter filter = null)
        {
            var inputs = _sequence.Execute(_types);
            var functionSequence = new FunctionSequence();
            if (filter!= null)
            {
                functionSequence.Add(filter);    
            }
            return new ConditionList(inputs, true, functionSequence);
        }
        
        public ConditionList ShouldNot(Filter filter = null)
        {
            var inputs = _sequence.Execute(_types);
            var functionSequence = new FunctionSequence();
            if (filter != null)
            {
                functionSequence.Add(!filter);    
            }
            return new ConditionList(inputs, true, functionSequence);
        }

        /// <summary>
        /// Returns the type definitions returned by these predicate.
        /// </summary>
        /// <returns>A list of type definitions.</returns>
        internal IEnumerable<TypeDefinition> GetTypeDefinitions()
        { 
            return _sequence.Execute(_types);
        }

        /// <summary>
        /// Returns the types returned by these predicates.
        /// </summary>
        /// <returns>A list of types.</returns>
        public IEnumerable<Type> GetTypes()
        {
            return _sequence.Execute(_types).Select(t => t.ToType());
        }
    }
}
