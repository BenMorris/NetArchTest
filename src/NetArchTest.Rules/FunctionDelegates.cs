namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NetArchTest.Rules.Dependencies;
    using NetArchTest.Rules.Extensions;
    using Mono.Cecil;

    internal class StringAndComparisonStrategy
    {
		public string Value { get; }
        public StringComparison Comparer { get; }
    }

    /// <summary>
    /// Defines the various functions that can be applied to a collection of types.
    /// </summary>
    /// <remarks>
    /// These are used by both predicates and conditions so warrant a common definition.
    /// </remarks>
    internal static class FunctionDelegates
    {
        /// <summary> The base delegate type used by every function. </summary>
        internal delegate IEnumerable<TypeDefinition> FunctionDelegate<T>(IEnumerable<TypeDefinition> input, T arg, bool condition);

        /// <summary> Function for finding a specific type name. </summary>
        internal static FunctionDelegate<string> HaveName = delegate (IEnumerable<TypeDefinition> input, string name, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return input.Where(c => !c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
        };

        /// <summary> Function for matching a type name using a regular expression. </summary>
        internal static FunctionDelegate<string> HaveNameMatching = delegate (IEnumerable<TypeDefinition> input, string pattern, bool condition)
        {
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            if (condition)
            {
                return input.Where(c => r.Match(c.Name).Success);
            }
            else
            {
                return input.Where(c => !r.Match(c.Name).Success);
            }
        };

        /// <summary> Function for matching the start of a type name. </summary>
        internal static FunctionDelegate<string> HaveNameStartingWith = MakeFunctionDelegateUsingStringComparerForHaveNameStartingWith(StringComparison.InvariantCultureIgnoreCase);

        internal static FunctionDelegate<string> MakeFunctionDelegateUsingStringComparerForHaveNameStartingWith(StringComparison comparer) => delegate (IEnumerable<TypeDefinition> input, string start, bool condition)
        {
	        if (condition)
	        {
		        return input.Where(c => c.Name.StartsWith(start, comparer));
	        }
		    else
	        {
		        return input.Where(c => !c.Name.StartsWith(start, comparer));
	        }
        };

        /// <summary> Function for matching the end of a type name. </summary>
        internal static FunctionDelegate<string> HaveNameEndingWith = MakeFunctionDelegateUsingStringComparerForHaveNameEndingWith(StringComparison.InvariantCultureIgnoreCase);

        internal static FunctionDelegate<string> MakeFunctionDelegateUsingStringComparerForHaveNameEndingWith(StringComparison comparer) => delegate (IEnumerable<TypeDefinition> input, string end, bool condition)
        {
	        if (condition)
	        {
		        return input.Where(c => c.Name.EndsWith(end, comparer));
	        }
	        else
	        {
		        return input.Where(c => !c.Name.EndsWith(end, comparer));
	        }
        };

        /// <summary> Function for finding classes with a particular custom attribute. </summary>
        internal static FunctionDelegate<Type> HaveCustomAttribute = delegate (IEnumerable<TypeDefinition> input, Type attribute, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.CustomAttributes.Any(a => attribute.FullName.Equals(a.AttributeType.FullName, StringComparison.InvariantCultureIgnoreCase)));
            }
            else
            {
                return input.Where(c => !c.CustomAttributes.Any(a => attribute.FullName.Equals(a.AttributeType.FullName, StringComparison.InvariantCultureIgnoreCase)));
            }
        };

        /// <summary> Function for finding classes decorated with a particular custom attribute or derived one</summary>
        internal static FunctionDelegate<Type> HaveCustomAttributeOrInherit = delegate (IEnumerable<TypeDefinition> input, Type attribute, bool condition)
        {
            // Convert the incoming type to a definition
            var target = attribute.ToTypeDefinition();
            if (condition)
            {
                return input.Where(c => c.CustomAttributes.Any(a => a.AttributeType.Resolve().IsSubclassOf(target) || attribute.FullName.Equals(a.AttributeType.FullName, StringComparison.InvariantCultureIgnoreCase)));
            }
            else
            {
                return input.Where(c => !(c.CustomAttributes.Any(a => a.AttributeType.Resolve().IsSubclassOf(target) || attribute.FullName.Equals(a.AttributeType.FullName, StringComparison.InvariantCultureIgnoreCase))));
            }
        };


        /// <summary> Function for finding classes that inherit from a particular type. </summary>
        internal static FunctionDelegate<Type> Inherits = delegate (IEnumerable<TypeDefinition> input, Type type, bool condition)
        {
            // Convert the incoming type to a definition
            var target = type.ToTypeDefinition();
            if (condition)
            {
                return input.Where(c => c.IsSubclassOf(target));
            }
            else
            {
                return input.Where(c => !c.IsSubclassOf(target));
            }
        };

        /// <summary> Function for finding classes that implement a particular interface. </summary>
        internal static FunctionDelegate<Type> ImplementsInterface = delegate (IEnumerable<TypeDefinition> input, Type typeInterface, bool condition)
        {
            if (!typeInterface.IsInterface)
            {
                throw new ArgumentException($"The type {typeInterface.FullName} is not an interface.");
            }

            var target = typeInterface.FullName;
            var found = new List<TypeDefinition>();

            foreach (var type in input)
            {
                if (type.Interfaces.Any(t => t.InterfaceType.Resolve().FullName.Equals(target, StringComparison.InvariantCultureIgnoreCase)))
                {
                    found.Add(type);
                }
            }

            if (condition)
            {
                return found;
            }
            else
            {
                return input.Where(c => !found.Contains(c));
            }
        };

        /// <summary> Function for finding abstract classes. </summary>
        internal static FunctionDelegate<bool> BeAbstract = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsAbstract);
            }
            else
            {
                return input.Where(c => !c.IsAbstract);
            }
        };

        /// <summary> Function for finding classes. </summary>
        internal static FunctionDelegate<bool> BeClass = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsClass);
            }
            else
            {
                return input.Where(c => !c.IsClass);
            }
        };

        /// <summary> Function for finding interfaces. </summary>
        internal static FunctionDelegate<bool> BeInterface = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsInterface);
            }
            else
            {
                return input.Where(c => !c.IsInterface);
            }
        };

        /// <summary> Function for finding types with generic parameters. </summary>
        internal static FunctionDelegate<bool> BeGeneric = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.HasGenericParameters);
            }
            else
            {
                return input.Where(c => !c.HasGenericParameters);
            }
        };


        /// <summary> Function for finding nested classes. </summary>
        internal static FunctionDelegate<bool> BeNested = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsNested);
            }
            else
            {
                return input.Where(c => !c.IsNested);
            }
        };

        /// <summary> Function for finding nested public classes. </summary>
        internal static FunctionDelegate<bool> BeNestedPublic = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsNestedPublic);
            }
            else
            {
                return input.Where(c => !c.IsNestedPublic);
            }
        };

        /// <summary> Function for finding nested private classes. </summary>
        internal static FunctionDelegate<bool> BeNestedPrivate = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsNestedPrivate);
            }
            else
            {
                return input.Where(c => !c.IsNestedPrivate);
            }
        };


        /// <summary> Function for finding public classes. </summary>
        internal static FunctionDelegate<bool> BePublic = delegate (IEnumerable<TypeDefinition> input, bool dummy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsNested ? c.IsNestedPublic : c.IsPublic);
            }
            else
            {
                return input.Where(c => c.IsNested ? !c.IsNestedPublic : c.IsNotPublic);
            }
        };

        /// <summary> Function for finding sealed classes. </summary>
        internal static FunctionDelegate<bool> BeSealed = delegate (IEnumerable<TypeDefinition> input, bool dummmy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsSealed);
            }
            else
            {
                return input.Where(c => !c.IsSealed);
            }
        };

        /// <summary> Function for finding immutable classes. </summary>
        internal static FunctionDelegate<bool> BeImmutable = delegate (IEnumerable<TypeDefinition> input, bool dummmy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.IsImmutable());
            }
            else
            {
                return input.Where(c => !c.IsImmutable());
            }
        };

        /// <summary> Function for finding nullable classes. </summary>
        internal static FunctionDelegate<bool> HasNullableMembers = delegate (IEnumerable<TypeDefinition> input, bool dummmy, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.HasNullableMembers());
            }
            else
            {
                return input.Where(c => !c.HasNullableMembers());
            }
        };

        /// <summary> Function for finding types in a particular namespace. </summary>
        internal static FunctionDelegate<string> ResideInNamespace = delegate (IEnumerable<TypeDefinition> input, string name, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.FullName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return input.Where(c => !c.FullName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
            }
        };

        /// <summary> Function for matching a type name using a regular expression. </summary>
        internal static FunctionDelegate<string> ResideInNamespaceMatching = delegate (IEnumerable<TypeDefinition> input, string pattern, bool condition)
        {
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            if (condition)
            {
                return input.Where(c => r.Match(c.GetNamespace()).Success);
            }
            else
            {
                return input.Where(c => !r.Match(c.GetNamespace()).Success);
            }
        };

        /// <summary> Function for finding types that have a dependency on any of the supplied types. </summary>
        internal static FunctionDelegate<IEnumerable<string>> HaveDependencyOnAny = delegate (IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies, bool condition)
        {
            // Get the types that contain the dependencies
            var search = new DependencySearch();
            var results = search.FindTypesThatHaveDependencyOnAny(input, dependencies);

            if (condition)
            {
                return results;
            }
            else
            {
                return input.Where(t => !results.Contains(t));
            }
        };

        /// <summary> Function for finding types that have a dependency on all of the supplied types. </summary>
        internal static FunctionDelegate<IEnumerable<string>> HaveDependencyOnAll = delegate (IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies, bool condition)
        {
            // Get the types that contain the dependencies
            var search = new DependencySearch();
            var results = search.FindTypesThatHaveDependencyOnAll(input, dependencies);

            if (condition)
            {
                return results;
            }
            else
            {
                return input.Where(t => !results.Contains(t));
            }
        };

        /// <summary> Function for finding types that have a dependency on type other than one of the supplied types.</summary>
        internal static FunctionDelegate<IEnumerable<string>> OnlyHaveDependenciesOnAnyOrNone = delegate (IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies, bool condition)
        {            
            var search = new DependencySearch();
            var results = search.FindTypesThatOnlyHaveDependenciesOnAnyOrNone(input, dependencies);

            if (condition)
            {
                return results;
            }
            else
            {
                return input.Where(t => !results.Contains(t));
            }
        };

        /// <summary> Function for finding public classes. </summary>
        internal static FunctionDelegate<ICustomRule> MeetCustomRule = delegate (IEnumerable<TypeDefinition> input, ICustomRule rule, bool condition)
        {
            if (condition)
            {
                return input.Where(t => rule.MeetsRule(t));
            }
            else
            {
                return input.Where(t => !rule.MeetsRule(t));
            }
        };
    }
}