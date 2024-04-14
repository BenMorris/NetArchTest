namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// A set of predicates that can be applied to a list of types.
    /// </summary>
    public sealed class Predicates
    {
        /// <summary>
        /// A list of types that conditions can be applied to.
        /// </summary>
        private readonly IEnumerable<TypeDefinition> _types;

        ///<summary>
        /// The sequence of conditions that is applied to the type of list.
        /// </summary>
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
        public PredicateList HaveName(params string[] name)
        {
            foreach (var item in name)
            {
                _sequence.AddFunctionCall(FunctionDelegates.HaveName, item, true);
            }
            
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a particular name.
        /// </summary>
        /// <param name="name">The name of the class to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveName(params string[] name)
        {
            foreach (var item in name)
            {
                _sequence.AddFunctionCall(FunctionDelegates.HaveName, item, false);
            }
            
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
        public PredicateList HaveNameStartingWith(params string[] start)
        {
            foreach (var item in start)
            {
                _sequence.AddFunctionCall(FunctionDelegates.HaveNameStartingWith, item, true);
            }
            
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose names start with the specified text.
        /// </summary>
        /// <param name="start">The text to match against.</param>
        /// <param name="comparer">The string comparer.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveNameStartingWith(string start, StringComparison comparer)
        {
	        _sequence.AddFunctionCall(FunctionDelegates.MakeFunctionDelegateUsingStringComparerForHaveNameStartingWith(comparer), start, true);
	        return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose names do not start with the specified text.
        /// </summary>
        /// <param name="start">The text to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveNameStartingWith(params string[] start)
        {
            foreach (var item in start)
            {
                _sequence.AddFunctionCall(FunctionDelegates.HaveNameStartingWith, item, false);
            }
            
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose names do not start with the specified text.
        /// </summary>
        /// <param name="start">The text to match against.</param>
        /// <param name="comparer">The string comparer.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveNameStartingWith(string start, StringComparison comparer)
        {
	        _sequence.AddFunctionCall(FunctionDelegates.MakeFunctionDelegateUsingStringComparerForHaveNameStartingWith(comparer), start, false);
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
        /// Selects types whose names end with the specified text.
        /// </summary>
        /// <param name="end">The text to match against.</param>
        /// <param name="comparer">The string comparer.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveNameEndingWith(string end, StringComparison comparer)
        {
	        _sequence.AddFunctionCall(FunctionDelegates.MakeFunctionDelegateUsingStringComparerForHaveNameEndingWith(comparer), end, true);
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
        /// Selects types whose names do not end with the specified text.
        /// </summary>
        /// <param name="end">The text to match against.</param>
        /// <param name="comparer">The string comparer.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveNameEndingWith(string end, StringComparison comparer)
        {
	        _sequence.AddFunctionCall(FunctionDelegates.MakeFunctionDelegateUsingStringComparerForHaveNameEndingWith(comparer), end, false);
	        return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are decorated with a specific custom attribute.
        /// </summary>
        /// <param name="attribute">The attribute to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveCustomAttribute(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttribute, attribute, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are decorated with a specific custom attribute or derived one.
        /// </summary>
        /// <param name="attribute">The base attribute to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveCustomAttributeOrInherit(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttributeOrInherit, attribute, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not decorated with a specific custom attribute.
        /// </summary>
        /// <param name="attribute">The attribute to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveCustomAttribute(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttribute, attribute, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not decorated with a specific custom attribute or derived one.
        /// </summary>
        /// <param name="attribute">The base attribute to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveCustomAttributeOrInherit(Type attribute)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveCustomAttributeOrInherit, attribute, false);
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
        /// Selects types that are static.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreStatic()
        {
	        _sequence.AddFunctionCall(FunctionDelegates.BeStatic, true, true);
	        return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not static.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotStatic()
        {
	        _sequence.AddFunctionCall(FunctionDelegates.BeStatic, true, false);
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
        /// Selects types that are nested and declared as public.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNestedPublic()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNestedPublic, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are nested and declared as private.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNestedPrivate()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNestedPrivate, true, true);
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
        /// Selects types that are not nested and declared as public.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        /// <remarks>NB: This method will return non-nested types and nested types that are declared as private.</remarks>
        public PredicateList AreNotNestedPublic()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNestedPublic, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not nested and declared as private.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        /// <remarks>NB: This method will return non-nested types and nested types that are declared as public.</remarks>
        public PredicateList AreNotNestedPrivate()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeNestedPrivate, true, false);
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
        /// Selects types that are immutable.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreImmutable()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeImmutable, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are mutable.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreMutable()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeImmutable, true, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have only nullable members.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList OnlyHaveNullableMembers()
        {
            _sequence.AddFunctionCall(FunctionDelegates.HasNullableMembers, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have some non-nullable members.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveSomeNonNullableMembers()
        {
            _sequence.AddFunctionCall(FunctionDelegates.HasNullableMembers, true, false);
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
        /// Selects types whose namespaces match a regular expression.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList ResideInNamespaceMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, pattern, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose namespaces start with a particular name part.
        /// </summary>
        /// <param name="name">The namespace part to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList ResideInNamespaceStartingWith(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, $"^{name}", true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose namespaces end with a particular name part.
        /// </summary>
        /// <param name="name">The namespace part to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList ResideInNamespaceEndingWith(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, $"{name}$", true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose namespaces contain a particular name part.
        /// </summary>
        /// <param name="name">The namespace part to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList ResideInNamespaceContaining(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, $"^.*{name}.*$", true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose namespaces do not match a regular expression.
        /// </summary>
        /// <param name="pattern">The regular expression pattern to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotResideInNamespaceMatching(string pattern)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, pattern, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose namespaces start with a particular name part.
        /// </summary>
        /// <param name="name">The namespace part to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotResideInNamespaceStartingWith(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, $"^{name}", false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose namespaces end with a particular name part.
        /// </summary>
        /// <param name="name">The namespace part to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotResideInNamespaceEndingWith(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, $"{name}$", false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types whose namespaces contain a particular name part.
        /// </summary>
        /// <param name="name">The namespace part to match against.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotResideInNamespaceContaining(string name)
        {
            _sequence.AddFunctionCall(FunctionDelegates.ResideInNamespaceMatching, $"^.*{name}.*$", false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have a dependency on a particular type.
        /// </summary>
        /// <param name="dependency">The dependency type to match against. This can be a namespace or a specific type.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList HaveDependencyOn(string dependency)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOnAny, new List<string> { dependency }, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have a dependency on any of the supplied types.
        /// </summary>
        /// <param name="dependencies">The dependencies to match against. These can be namespaces or specific types.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public PredicateList HaveDependencyOnAny(params string[] dependencies)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOnAny, dependencies, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have a dependency on all of the particular types.
        /// </summary>
        /// <param name="dependencies">The dependencies to match against. These can be namespaces or specific types.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public PredicateList HaveDependencyOnAll(params string[] dependencies)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOnAll, dependencies, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have a dependency on any of the supplied types and cannot have any other dependency. 
        /// </summary>
        /// <param name="dependencies">The dependencies to match against. These can be namespaces or specific types.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public PredicateList OnlyHaveDependenciesOn(params string[] dependencies)
        {
            _sequence.AddFunctionCall(FunctionDelegates.OnlyHaveDependenciesOnAnyOrNone, dependencies, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a dependency on a particular type.
        /// </summary>
        /// <param name="dependency">The dependency type to match against. This can be a namespace or a specific type.</param>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveDependencyOn(string dependency)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOnAny, new List<string> { dependency }, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a dependency on any of the particular types.
        /// </summary>
        /// <param name="dependencies">The dependencies to match against. These can be namespaces or specific types.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveDependencyOnAny(params string[] dependencies)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOnAny, dependencies, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that do not have a dependency on all of the particular types.
        /// </summary>
        /// <param name="dependencies">The dependencies to match against. These can be namespaces or specific types.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public PredicateList DoNotHaveDependencyOnAll(params string[] dependencies)
        {
            _sequence.AddFunctionCall(FunctionDelegates.HaveDependencyOnAll, dependencies, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that have a dependency other than any of the given dependencies.
        /// </summary>
        /// <param name="dependencies">The dependencies to match against. These can be namespaces or specific types.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public PredicateList HaveDependenciesOtherThan(params string[] dependencies)
        {
            _sequence.AddFunctionCall(FunctionDelegates.OnlyHaveDependenciesOnAnyOrNone, dependencies, false);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that meet a custom rule.
        /// </summary>
        /// <param name="rule">An instance of the custom rule.</param>
        /// <returns>An updated set of conditions that can be applied to a list of types.</returns>
        public PredicateList MeetCustomRule(ICustomRule rule)
        {
            _sequence.AddFunctionCall(FunctionDelegates.MeetCustomRule, rule, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are enumerations.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreEnums()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeEnum, true, true);
            return new PredicateList(_types, _sequence);
        }

        /// <summary>
        /// Selects types that are not enumerations.
        /// </summary>
        /// <returns>An updated set of predicates that can be applied to a list of types.</returns>
        public PredicateList AreNotEnums()
        {
            _sequence.AddFunctionCall(FunctionDelegates.BeEnum, true, false);
            return new PredicateList(_types, _sequence);
        }
    }
}
