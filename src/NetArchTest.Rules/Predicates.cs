namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetArchTest.Rules.Extensions;
    using Mono.Cecil;

    /// <summary>
    /// A set of predicates that can be applied to a list of types.
    /// </summary>
    public sealed class Predicates
    {
        /// <summary> A list of types that conditions can be applied to. </summary>
        private readonly IEnumerable<TypeDefinition> _types;

        /// <summary> The sequence of conditions that is applied to the type of list. </summary>
        private readonly FunctionSequence _sequence;

        /// <summary>
        /// Initializes a new instance of the <see cref="Predicates"/> class.
        /// </summary>
        internal Predicates(IEnumerable<TypeDefinition> types)
        {
            _types = types.ToList();
            _sequence = new FunctionSequence();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Predicates"/> class.
        /// </summary>
        internal Predicates(IEnumerable<TypeDefinition> types, FunctionSequence calls)
        {
            _types = types.ToList();
            _sequence = calls;
        }

        /// <summary>
        /// Selects types that have a specific name.
        /// </summary>
        /// <param name="name">The name of the class to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveName(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveName, name, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a particular name.
        /// </summary>
        /// <param name="name">The name of the class to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveName(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveName, name, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types according to a regular expression matching their name.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveNameMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameMatching, pattern, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types according to a regular expression that does not match their name.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveNameMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameMatching, pattern, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose names start with the specified text.
        /// </summary>
        /// <param name="start">The text to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveNameStartingWith(string start)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameStartingWith, start, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose names do not start with the specified text.
        /// </summary>
        /// <param name="start">The text to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveNameStartingWith(string start)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameStartingWith, start, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose names end with the specified text.
        /// </summary>
        /// <param name="end">The text to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveNameEndingWith(string end)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameEndingWith, end, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose names do not end with the specified text.
        /// </summary>
        /// <param name="end">The text to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveNameEndingWith(string end)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameEndingWith, end, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that implement a specific custom attribute.
        /// </summary>
        /// <param name="attribute">The attribute to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveCustomAttribute(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttribute, attribute, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not implement a specific custom attribute.
        /// </summary>
        /// <param name="attribute">The attribute to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveCustomAttribute(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttribute, attribute, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that inherit a particular type.
        /// </summary>
        /// <param name="type">The type to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList Inherit(Type type)
        {
            _sequence.AddFunctionCall(FunctionDelegates.Inherits, type, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not inherit a particular type.
        /// </summary>
        /// <param name="type">The type to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotInherit(Type type)
        {
            _sequence.AddFunctionCall(FunctionDelegates.Inherits, type, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that implement a particular interface.
        /// </summary>
        /// <param name="interfaceType">The interface type to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList ImplementInterface(Type interfaceType)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ImplementsInterface, interfaceType, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not implement a particular interface.
        /// </summary>
        /// <param name="interfaceType">The interface type to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotImplementInterface(Type interfaceType)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ImplementsInterface, interfaceType, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are marked as abstract.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreAbstract()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeAbstract, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not marked as abstract.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotAbstract()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeAbstract, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are classes.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreClasses()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeClass, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not classes.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotClasses()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeClass, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have generic parameters.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreGeneric()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeGeneric, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have generic parameters.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotGeneric()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeGeneric, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are interfaces.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreInterfaces()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeInterface, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not interfaces.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotInterfaces()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeInterface, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are nested.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNested()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNested, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not nested.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotNested()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNested, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have public scope.
        /// </summary>
        /// <remarks>
        /// This method will only act on types that are visible to the function. Use InternalsVisibleTo if testing from a separate assembly.
        /// </remarks>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList ArePublic()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BePublic, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have public scope.
        /// </summary>
        /// <remarks>
        /// This method will only act on types that are visible to the function. Use InternalsVisibleTo if testing from a separate assembly.
        /// </remarks>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotPublic()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BePublic, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types according that are marked as sealed.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreSealed()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeSealed, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types according that are not marked as sealed.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotSealed()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeSealed, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that reside in a particular namespace.
        /// </summary>
        /// <param name="name">The namespace to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList ResideInNamespace(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespace, name, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not reside in a particular namespace.
        /// </summary>
        /// <param name="name">The namespace to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotResideInNamespace(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespace, name, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have a dependency on a particular type.
        /// </summary>
        /// <param name="dependency">The dependency type to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveDependencyOn(string dependency)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOn, new List<string> { dependency }, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a dependency on a particular type.
        /// </summary>
        /// <param name="dependency">The dependency type to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveDependencyOn(string dependency)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOn, new List<string> { dependency }, false);
            return new PredicateList(_types, _sequence);
        }
    }
}
