﻿namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetArchTest.Rules.Extensions;
    using Mono.Cecil;

    /// <summary>
    /// A set of conditions and types that have have conjunctions (i.e. "and", "or")
    /// and executors (i.e. Types(), GetResult()) applied to them.
    /// </summary>
    public sealed class ConditionList
    {
        /// <summary>
        /// A list of types that conditions can be applied to.
        /// </summary>
        private readonly IEnumerable<TypeDefinition> _types;

        /// <summary>
        /// The sequence of conditions that is applied to the type of list.
        /// </summary>
        private readonly FunctionSequence _sequence;

        /// <summary>
        /// Determines the polarity of the selection, i.e. "should" or "should not".
        /// </summary>
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
        /// <returns>An indication of whether the conditions are true, along with a list of types failing the check if they are not.</returns>
        public TestResult GetResult()
        {
            var typeDefinitions = _sequence
                .Execute(_types)
                .ToList();

            var success = _should
                ? typeDefinitions.Count == _types.Count()
                : !typeDefinitions.Any();

            if (success)
            {
                return TestResult.Success();
            }

            return TestResult.Failure(
                _should 
                    ? _types.Except(typeDefinitions).ToList()
                    : typeDefinitions.Distinct().ToList());
        }

        /// <summary>
        /// Returns the number of types that satisfy the conditions.
        /// </summary>
        /// <returns>A list of types.</returns>
        public int Count()
            => _sequence.Execute(_types).Count();

        /// <summary>
        /// Returns the list of types that satisfy the conditions.
        /// </summary>
        /// <returns>A list of types.</returns>
        public IEnumerable<Type> GetTypes()
            => _sequence.Execute(_types).Select(t => t.ToType());

        /// <summary>
        /// Specifies that any subsequent condition should be treated as an "and" condition.
        /// </summary>
        /// <returns>An set of conditions that can be applied to a list of classes.</returns>
        /// <remarks>And() has higher priority than Or() and it is computed first.</remarks>
        public Conditions And()
            => new Conditions(_types, _should, _sequence);

        /// <summary>
        /// Specifies that any subsequent conditions should be treated as part of an "or" condition.
        /// </summary>
        /// <returns>An set of conditions that can be applied to a list of classes.</returns>
        public Conditions Or()
        {
            // Create a new group of functions - this has the effect of creating an "or" condition
            _sequence.CreateGroup();
            return new Conditions(_types, _should, _sequence);
        }
    }
}
