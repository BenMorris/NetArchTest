namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// A set of conditions that can be applied to a list of types.
    /// </summary>
    public sealed class Conditions
    {
        /// <summary> A list of types that conditions can be applied to. </summary>
        private readonly IEnumerable<TypeDefinition> _types;

        /// <summary> The sequence of conditions that is applied to the type of list. </summary>
        private readonly FunctionSequence _sequence;

        /// <summary> Determines the polarity of the selection, i.e. "should" or "should not". </summary>
        private readonly bool _should;

        /// <summary>
        /// Initializes a new instance of the <see cref="Conditions"/> class.
        /// </summary>
        internal Conditions(IEnumerable<TypeDefinition> types, bool should)
        {
            _types = types.ToList();
            _should = should;
            _sequence = new FunctionSequence();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Conditions"/> class.
        /// </summary>
        internal Conditions(IEnumerable<TypeDefinition> types, bool should, FunctionSequence calls)
        {
            _types = types.ToList();
            _should = should;
            _sequence = calls;
        }

        /// <summary>
        /// Selects types that have a specific name.
        /// </summary>
        /// <param name="name">The name of the class to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList HaveName(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveName, name, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a particular name.
        /// </summary>
        /// <param name="name">The name of the class to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotHaveName(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveName, name, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types according to a regular expression matching their name.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList HaveNameMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameMatching, pattern, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types according to a regular expression that does not match their name.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotHaveNameMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameMatching, pattern, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types whose names start with the specified text.
        /// </summary>
        /// <param name="start">The text to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList HaveNameStartingWith(string start)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameStartingWith, start, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types whose names do not start with the specified text.
        /// </summary>
        /// <param name="start">The text to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotHaveNameStartingWith(string start)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameStartingWith, start, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types whose names do not end with the specified text.
        /// </summary>
        /// <param name="end">The text to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList HaveNameEndingWith(string end)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameEndingWith, end, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types whose names do not end with the specified text.
        /// </summary>
        /// <param name="end">The text to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotHaveNameEndingWith(string end)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveNameEndingWith, end, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that implement a specific custom attribute.
        /// </summary>
        /// <param name="attribute">The attribute to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList HaveCustomAttribute(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttribute, attribute, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not implement a specific custom attribute.
        /// </summary>
        /// <param name="attribute">The attribute to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotHaveCustomAttribute(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttribute, attribute, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that inherit a particular type.
        /// </summary>
        /// <param name="type">The type to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList Inherit(Type type)
        {
            _sequence.AddFunctionCall(FunctionDelegates.Inherits, type, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not inherit a particular type.
        /// </summary>
        /// <param name="type">The type to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotInherit(Type type)
        {
            _sequence.AddFunctionCall(FunctionDelegates.Inherits, type, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that implement a particular interface.
        /// </summary>
        /// <param name="interfaceType">The interface type to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList ImplementInterface(Type interfaceType)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ImplementsInterface, interfaceType, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not implement a particular interface.
        /// </summary>
        /// <param name="interfaceType">The interface type to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotImplementInterface(Type interfaceType)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ImplementsInterface, interfaceType, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are marked as abstract.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeAbstract()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeAbstract, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are not marked as abstract.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotBeAbstract()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeAbstract, true, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are classes.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeClasses()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeClass, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are not classes.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotBeClasses()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeClass, true, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that have generic parameters.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeGeneric()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeGeneric, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not have generic parameters.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotBeGeneric()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeGeneric, true, false);
            return new ConditionList(_types, _should, _sequence);
        }


        /// <summary>
        /// Selects types that are interfaces.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeInterfaces()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeInterface, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are not interfaces.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotBeInterfaces()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeInterface, true, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are nested.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeNested()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNested, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are not nested.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotBeNested()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNested, true, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that are have public scope.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BePublic()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BePublic, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not have public scope.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotBePublic()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BePublic, true, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types according that are marked as sealed.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeSealed()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeSealed, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types according that are not marked as sealed.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotBeSealed()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeSealed, true, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types according that are immutable.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeImmutable()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeImmutable, true, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types according that are mutable.
        /// </summary>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList BeMutable()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeImmutable, true, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that reside in a particular namespace.
        /// </summary>
        /// <param name="name">The namespace to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList ResideInNamespace(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespace, name, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not reside in a particular namespace.
        /// </summary>
        /// <param name="name">The namespace to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotResideInNamespace(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespace, name, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that reside in a namespace matching a regular expression.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList ResideInNamespaceMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, pattern, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not reside in a namespace matching a regular expression.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotResideInNamespaceMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, pattern, false);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that have a dependency on a particular type.
        /// </summary>
        /// <param name="dependency">The dependency to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList HaveDependencyOn(string dependency)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOn, new List<string> { dependency }, true);
            return new ConditionList(_types, _should, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a dependency on a particular type.
        /// </summary>
        /// <param name="dependency">The dependency type to match against.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public ConditionList NotHaveDependencyOn(string dependency)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOn, new List<string> { dependency }, false);
            return new ConditionList(_types, _should, _sequence);
        }
    }
}
