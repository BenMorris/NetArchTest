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
        /// <summary> The base delegate type used by every function. </summary>
        internal delegate IEnumerable<TypeDefinition> FunctionDelegate<T>(IList<TypeDefinition> input, T arg, bool condition);

        /// <summary> Function for finding a specific type name. </summary>
        internal static FunctionDelegate<string> HaveName = delegate (IList<TypeDefinition> input, string name, bool condition)
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
        internal static FunctionDelegate<string> HaveNameMatching = delegate (IList<TypeDefinition> input, string pattern, bool condition)
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
        internal static FunctionDelegate<string> HaveNameStartingWith = delegate (IList<TypeDefinition> input, string start, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.Name.StartsWith(start, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return input.Where(c => !c.Name.StartsWith(start, StringComparison.InvariantCultureIgnoreCase));
            }
        };

        /// <summary> Function for matching the end of a type name. </summary>
        internal static FunctionDelegate<string> HaveNameEndingWith = delegate (IList<TypeDefinition> input, string end, bool condition)
        {
            if (condition)
            {
                return input.Where(c => c.Name.EndsWith(end, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return input.Where(c => !c.Name.EndsWith(end, StringComparison.InvariantCultureIgnoreCase));
            }
        };

        /// <summary> Function for finding classes with a particular custom attribute. </summary>
        internal static FunctionDelegate<Type> HaveCustomAttribute = delegate (IList<TypeDefinition> input, Type attribute, bool condition)
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

        /// <summary> Function for finding classes that inherit from a particular type. </summary>
        internal static FunctionDelegate<Type> Inherits = delegate (IList<TypeDefinition> input, Type type, bool condition)
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
        internal static FunctionDelegate<Type> ImplementsInterface = delegate (IList<TypeDefinition> input, Type typeInterface, bool condition)
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
        internal static FunctionDelegate<bool> BeAbstract = delegate (IList<TypeDefinition> input, bool dummy, bool condition)
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
        internal static FunctionDelegate<bool> BeClass = delegate (IList<TypeDefinition> input, bool dummy, bool condition)
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
        internal static FunctionDelegate<bool> BeInterface = delegate (IList<TypeDefinition> input, bool dummy, bool condition)
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
        internal static FunctionDelegate<bool> BeGeneric = delegate (IList<TypeDefinition> input, bool dummy, bool condition)
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
        internal static FunctionDelegate<bool> BeNested = delegate (IList<TypeDefinition> input, bool dummy, bool condition)
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

        /// <summary> Function for finding public classes. </summary>
        internal static FunctionDelegate<bool> BePublic = (input, dummy, condition) =>
        {
            return condition ? input.Where(c => c.IsPublic) : input.Where(c => c.IsNotPublic);
        };

        /// <summary> Function for finding sealed classes. </summary>
        internal static FunctionDelegate<bool> BeSealed = (input, dummmy, condition) =>
            condition ? input.Where(c => c.IsSealed) : input.Where(c => !c.IsSealed);

        /// <summary> Function for finding types in a particular namespace. </summary>
        internal static FunctionDelegate<string> ResideInNamespace =
            (input, name, condition) =>
                condition
                    ? input.Where(c => c.FullName.StartsWith(name))
                    : input.Where(c => !c.FullName.StartsWith(name));
        
        internal static FunctionDelegate<Func<string, bool>> MatchNamespace =
            (input, match, condition) => condition ? input.Where(c => match(c.Namespace)) : input.Where(c => !match(c.Namespace));

        /// <summary> Function for finding types that have a dependency on a specific type. </summary>
        internal static FunctionDelegate<IEnumerable<string>> HaveDependencyOn = (inputs, dependencies, condition) =>
            new DependencySearch(target => dependencies.Any(target.StartsWith))
                .FindTypesWithDependenciesMatch(inputs).GetResults(condition);

        internal static FunctionDelegate<Func<string, bool>> HaveMatchedDependencyOn 
            = (input, match, condition) =>
                new DependencySearch(match).FindTypesWithDependenciesMatch(input).GetResults(condition);
    }
}
