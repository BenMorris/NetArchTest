using Mono.Cecil.Rocks;

namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NetArchTest.Rules.Dependencies;
    using NetArchTest.Rules.Extensions;
    using Mono.Cecil;

    /// <summary>
    /// Defines the various functions that can be applied to a collection of types.
    /// </summary>
    /// <remarks>
    /// These are used by both predicates and conditions so warrant a common definition.
    /// </remarks>
    internal static class FunctionDelegates
    {
        /// <summary>
        /// The base delegate type used by every function.
        /// </summary>
        internal delegate IEnumerable<TypeDefinition> FunctionDelegate<T>(IEnumerable<TypeDefinition> input, T arg, bool condition);

        /// <summary>
        /// Function for finding a specific type name.
        /// </summary>
        internal static readonly FunctionDelegate<string> HaveName = 
            delegate (IEnumerable<TypeDefinition> input, string name, bool condition)
            {
                return input.Where(c => 
                    c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) == condition);
            };

        /// <summary>
        /// Function for matching a type name using a regular expression.
        /// </summary>
        internal static readonly FunctionDelegate<string> HaveNameMatching = 
            delegate (IEnumerable<TypeDefinition> input, string pattern, bool condition)
            {
                var r = new Regex(pattern, RegexOptions.IgnoreCase);
                
                return input.Where(c => r.Match(c.Name).Success == condition);
            };

        /// <summary>
        /// Function for matching the start of a type name.
        /// </summary>
        internal static readonly FunctionDelegate<string> HaveNameStartingWith = 
            MakeFunctionDelegateUsingStringComparerForHaveNameStartingWith(StringComparison.InvariantCultureIgnoreCase);

        internal static FunctionDelegate<string> MakeFunctionDelegateUsingStringComparerForHaveNameStartingWith(StringComparison comparer) 
            => delegate (IEnumerable<TypeDefinition> input, string start, bool condition) 
            {
                return input.Where(c => c.Name.StartsWith(start, comparer) == condition);
            };

        /// <summary>
        /// Function for matching the end of a type name.
        /// </summary>
        internal static readonly FunctionDelegate<string> HaveNameEndingWith = 
            MakeFunctionDelegateUsingStringComparerForHaveNameEndingWith(StringComparison.InvariantCultureIgnoreCase);

        internal static FunctionDelegate<string> MakeFunctionDelegateUsingStringComparerForHaveNameEndingWith(StringComparison comparer) 
            => delegate (IEnumerable<TypeDefinition> input, string end, bool condition)
            {
                return input.Where(c => c.Name.EndsWith(end, comparer) == condition);
            };

        /// <summary>
        /// Function for finding classes with a particular custom attribute.
        /// </summary>
        internal static readonly FunctionDelegate<Type> HaveCustomAttribute = 
            delegate (IEnumerable<TypeDefinition> input, Type attribute, bool condition)
            {
                return input.Where(c => 
                    c.CustomAttributes.Any(a => 
                        attribute.FullName?.Equals(a.AttributeType.FullName, StringComparison.InvariantCultureIgnoreCase) ?? false)
                    == condition);
            };

        /// <summary>
        /// Function for finding classes decorated with a particular custom attribute or derived one.
        /// </summary>
        internal static readonly FunctionDelegate<Type> HaveCustomAttributeOrInherit = 
            delegate (IEnumerable<TypeDefinition> input, Type attribute, bool condition)
            {
                var target = attribute.ToTypeDefinition();
                
                return input.Where(c => 
                    c.CustomAttributes.Any(a => 
                        a.AttributeType.Resolve().IsSubclassOf(target) 
                        || (attribute.FullName?.Equals(a.AttributeType.FullName, StringComparison.InvariantCultureIgnoreCase) ?? false)
                    ) == condition);
            };

        /// <summary>
        /// Function for finding classes that inherit from a particular type.
        /// </summary>
        internal static readonly FunctionDelegate<Type> Inherits = 
            delegate (IEnumerable<TypeDefinition> input, Type type, bool condition)
            {
                var target = type.ToTypeDefinition();

                return input.Where(c => c.IsSubclassOf(target) == condition);
            };

        /// <summary>
        /// Function for finding classes that implement a particular interface.
        /// </summary>
        internal static readonly FunctionDelegate<Type> ImplementsInterface = 
            delegate (IEnumerable<TypeDefinition> input, Type typeInterface, bool condition)
            {
                if (!typeInterface.IsInterface)
                {
                    throw new ArgumentException($"The type {typeInterface.FullName} is not an interface.");
                }
            
                var typeDefinitions = input.ToList();
                var target = typeInterface.FullName;

                var matchingTypes = typeDefinitions
                    .Where(t => t.Interfaces.Any(i =>
                        i.InterfaceType.Resolve().FullName.Equals(target, StringComparison.InvariantCultureIgnoreCase)));

                return condition
                    ? matchingTypes
                    : typeDefinitions.Except(matchingTypes);
            };

        /// <summary>
        /// Function for finding abstract classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeAbstract = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsAbstract == condition);
            };

        /// <summary>
        /// Function for finding classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeClass = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsClass == condition);
            };

        /// <summary>
        /// Function for finding interfaces.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeInterface = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsInterface == condition);
            };

        /// <summary>
        /// Function for finding static classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeStatic = 
            delegate(IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => ClassIsStatic(c) == condition);

                bool ClassIsStatic(TypeDefinition c) => 
                    c.IsAbstract 
                    && c.IsSealed 
                    && !c.IsInterface 
                    && !c.GetConstructors().Any(m => m.IsPublic);
            };

        /// <summary>
        /// Function for finding types with generic parameters.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeGeneric = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.HasGenericParameters == condition);
            };

        /// <summary>
        /// Function for finding nested classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeNested = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsNested == condition);
            };

        /// <summary>
        /// Function for finding nested public classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeNestedPublic = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsNestedPublic == condition);
            };

        /// <summary>
        /// Function for finding nested private classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeNestedPrivate = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsNestedPrivate == condition);
            };

        /// <summary>
        /// Function for finding public classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BePublic = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return condition 
                    ? input.Where(c => c.IsNested ? c.IsNestedPublic : c.IsPublic)
                    : input.Where(c => c.IsNested ? !c.IsNestedPublic : c.IsNotPublic);
            };

        /// <summary>
        /// Function for finding sealed classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeSealed = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsSealed == condition);
            };

        /// <summary>
        /// Function for finding immutable classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeImmutable = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsImmutable() == condition);
            };

        /// <summary>
        /// Function for finding nullable classes.
        /// </summary>
        internal static readonly FunctionDelegate<bool> HasNullableMembers = 
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.HasNullableMembers() == condition);
            };

        /// <summary>
        /// Function for finding types in a particular namespace.
        /// </summary>
        internal static readonly FunctionDelegate<string> ResideInNamespace = 
            delegate (IEnumerable<TypeDefinition> input, string name, bool condition)
            {
                return input.Where(c => 
                    c.FullName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase) == condition);
            };

        /// <summary>
        /// Function for matching a type name using a regular expression.
        /// </summary>
        internal static readonly FunctionDelegate<string> ResideInNamespaceMatching = 
            delegate (IEnumerable<TypeDefinition> input, string pattern, bool condition)
            {
                var r = new Regex(pattern, RegexOptions.IgnoreCase);
            
                return input.Where(c => r.Match(c.GetNamespace()).Success == condition);
            };

        /// <summary>
        /// Function for finding types that have a dependency on any of the supplied types.
        /// </summary>
        internal static readonly FunctionDelegate<IEnumerable<string>> HaveDependencyOnAny = 
            delegate (IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies, bool condition)
            {
                var typeDefinitions = input.ToList();
            
                var search = new DependencySearch();
                var matchingTypes = search.FindTypesThatHaveDependencyOnAny(typeDefinitions, dependencies);

                return condition
                    ? matchingTypes
                    : typeDefinitions.Except(matchingTypes);
            };

        /// <summary>
        /// Function for finding types that have a dependency on all of the supplied types.
        /// </summary>
        internal static readonly FunctionDelegate<IEnumerable<string>> HaveDependencyOnAll = 
            delegate (IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies, bool condition)
            {
                var typeDefinitions = input.ToList();
            
                var search = new DependencySearch();
                var matchingTypes = search.FindTypesThatHaveDependencyOnAll(typeDefinitions, dependencies);

                return condition
                    ? matchingTypes
                    : typeDefinitions.Except(matchingTypes);
            };

        /// <summary>
        /// Function for finding types that have a dependency on type other than one of the supplied types.
        /// </summary>
        internal static readonly FunctionDelegate<IEnumerable<string>> OnlyHaveDependenciesOnAnyOrNone = 
            delegate (IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies, bool condition)
            {
                var typeDefinitions = input.ToList();
            
                var search = new DependencySearch();
                var matchingTypes = search.FindTypesThatOnlyHaveDependenciesOnAnyOrNone(typeDefinitions, dependencies);

                return condition
                    ? matchingTypes
                    : typeDefinitions.Except(matchingTypes);
            };

        /// <summary>
        /// Function for finding public classes.
        /// </summary>
        internal static readonly FunctionDelegate<ICustomRule> MeetCustomRule = 
            delegate (IEnumerable<TypeDefinition> input, ICustomRule rule, bool condition)
            {
                return input.Where(t => rule.MeetsRule(t) == condition);
            };

        /// <summary>
        /// Function for finding enumerations.
        /// </summary>
        internal static readonly FunctionDelegate<bool> BeEnum =
            delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
            {
                return input.Where(c => c.IsEnum == condition);
            };
    }
}