﻿namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetArchTest.Rules.Extensions;
    using Mono.Cecil;

    /// <summary>
    /// A set of conditions and types that have have conjunctions (i.e. "and", "or") and executors (i.e. Types(), GetResult()) applied to them.
    /// </summary>
    public sealed class ConditionList
    {
        /// <summary> A list of types that conditions can be applied to. </summary>
        private readonly IEnumerable<TypeDefinition> _types;

        /// <summary> The sequence of conditions that is applied to the type of list. </summary>
        private readonly FunctionSequence _sequence;

        /// <summary> Determines the polarity of the selection, i.e. "should" or "should not". </summary>
        private readonly bool _should;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionList"/> class.
        /// </summary>
        internal ConditionList(IEnumerable<TypeDefinition> classes, bool should, FunctionSequence sequence)
        {
            _types = classes.ToList();
            _should = should;
            _sequence = sequence;
        }

        /// <summary>
        /// Returns an indication of whether all the selected types satisfy the conditions.
        /// </summary>
        /// <returns>An indication of whether the conditions are true.</returns>
        public bool GetResult()
        {
            if (_should)
            {
                // All the classes should meet the condition
                return (_sequence.Execute(_types).Count() == _types.Count());
            }
            else
            {
                // No classes should meet the condition
                return (!_sequence.Execute(_types).Any());
            }
        }

        /// <summary>
        /// Returns the number of types that satisfy the conditions.
        /// </summary>
        /// <returns>A list of types.</returns>
        public int Count()
        {
            return _sequence.Execute(_types).Count();
        }

        /// <summary>
        /// Returns the list of types that satisfy the conditions.
        /// </summary>
        /// <returns>A list of types.</returns>
        public IEnumerable<Type> GetTypes()
        {
            return _sequence.Execute(_types).Select(t => t.ToType());
        }
    }
}
