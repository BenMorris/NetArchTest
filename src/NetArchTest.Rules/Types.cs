using NetArchTest.Rules.Matches;
using static NetArchTest.Rules.Utils.FunctionalExtensions;

namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using NetArchTest.Rules.Extensions;
    using Mono.Cecil;

    /// <summary>
    /// Creates a list of types that can have predicates and conditions applied to it.
    /// </summary>
    public sealed class Types
    {
        /// <summary> The list of types represented by this instance. </summary>
        private readonly List<TypeDefinition> _types;

        /// <summary> The list of namespaces to exclude from the current domain. </summary>
        private static List<string> _exclusionList = new List<string>
        { "System", "Microsoft", "Mono.Cecil", "netstandard", "NetArchTest.Rules", "<Module>", "xunit" };

        /// <summary>
        /// Prevents any external class initializing a new instance of the <see cref="Types"/> class.
        /// </summary>
        /// <param name="types">The list of types for the instance.</param>
        private Types(IEnumerable<TypeDefinition> types)
        {
            _types = types.ToList();
        }

        /// <summary>
        /// Creates a list of types based on all the assemblies in the current AppDomain
        /// </summary>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types InCurrentDomain()
        {
            var currentDomain = new List<Assembly>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!_exclusionList.Any(e => assembly.FullName.StartsWith(e)))
                {
                    currentDomain.Add(assembly);
                }
            }

            return Types.InAssemblies(currentDomain);
        }

        /// <summary>
        /// Creates a list of types based on a particular assembly.
        /// </summary>
        /// <param name="assembly">The assembly to base the list on.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types InAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return Types.InAssemblies(new List<Assembly> { assembly });
        }

        /// <summary>
        /// Creates a list of types based on a list of assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to base the list on.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types InAssemblies(IEnumerable<Assembly> assemblies)
        {
            var types = new List<TypeDefinition>();

            foreach (var assembly in assemblies)
            {
                if (!assembly.IsDynamic)
                {
                    // Load the assembly using Mono.Cecil.
                    UriBuilder uri = new UriBuilder(assembly.CodeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    var assemblyDef = AssemblyDefinition.ReadAssembly(path);

                    // Read all the types in the assembly 
                    types.AddRange(Types.GetAllTypes(assemblyDef.Modules.SelectMany(t => t.Types)));
                }
            }

            return new Types(types);
        }

        /// <summary>
        /// Creates a list of all the types in a particular namespace.
        /// </summary>
        /// <param name="name">The namespace to list types for. This is case insensitive.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types InNamespace(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            // We need to check all the assemblies in the domain
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<TypeDefinition>();

            foreach(var assembly in assemblies)
            {
                if (!assembly.IsDynamic)
                {
                    // Load the assembly using Mono.Cecil.
                    UriBuilder uri = new UriBuilder(assembly.CodeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    var assemblyDef = AssemblyDefinition.ReadAssembly(path);

                    // Read all the types in the assembly 
                    var matches = (assemblyDef.Modules
                        .SelectMany(t => t.Types)
                        .Where(t => t.Namespace != null && t.Namespace.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)))
                        .ToList();

                    if (matches.Count > 0)
                    {
                        types.AddRange(matches);
                    }
                }
            }

            var list = Types.GetAllTypes(types);
            return new Types(list);
        }

        /// <summary>
        /// Creates a list of all the types in a particular module file.
        /// </summary>
        /// <param name="filename">The filename of the module. This is case insensitive.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        /// <remarks>Assumes that the module is in the same directory as the executing assembly.</remarks>
        public static Types FromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            // Load the assembly from the current directory
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, filename);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find the assembly file {path}.");
            }
            var assemblyDef = AssemblyDefinition.ReadAssembly(path);

            // Read all the types in the assembly 
            var list = Types.GetAllTypes(assemblyDef.Modules.SelectMany(t => t.Types));
            return new Types(list);
        }

        /// <summary>
        /// Creates a list of all the types found on a particular path.
        /// </summary>
        /// <param name="path">The relative path to load types from.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types FromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var types = new List<TypeDefinition>();

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*.dll");

                foreach (var file in files)
                {
                    var assembly= AssemblyDefinition.ReadAssembly(file);

                    if (!_exclusionList.Any(e => assembly.FullName.StartsWith(e)))
                    {
                        types.AddRange(assembly.Modules.SelectMany(t => t.Types));
                    }
                }
            }
            else
            {
                throw new DirectoryNotFoundException($"Could not find the path {path}.");
            }

            var list = Types.GetAllTypes(types);
            return new Types(list);
        }

        /// <summary>
        /// Recursively fetch all the nested types in a collection of types.
        /// </summary>
        /// <returns>The expanded collection of types</returns>
        private static IEnumerable<TypeDefinition> GetAllTypes(IEnumerable<TypeDefinition> types)
        {
            var output = new List<TypeDefinition>();
            var check = new Queue<TypeDefinition>(types);
            
            while (check.Count > 0)
            {
                var type = check.Dequeue();

                if (!_exclusionList.Any(e => type.FullName.StartsWith(e, StringComparison.InvariantCultureIgnoreCase)))
                {
                    output.Add(type);

                    foreach (var nested in type.NestedTypes)
                    {
                        // Ignore compiler-generated async classes
                        if (!nested.Interfaces.Any(i => i.InterfaceType.FullName.Equals(typeof(IAsyncStateMachine).FullName)))
                        {
                            check.Enqueue(nested);
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Returns the list of <see cref="TypeDefinition"/> objects describing the types in this list.
        /// </summary>
        /// <returns>The list of <see cref="TypeDefinition"/> objects in this list.</returns>
        internal IEnumerable<TypeDefinition> GetTypeDefinitions()
        {
            return _types;
        }

        /// <summary>
        /// Returns the list of <see cref="Type"/> objects describing the types in this list.
        /// </summary>
        /// <returns>The list of <see cref="Type"/> objects in this list.</returns>
        public IEnumerable<Type> GetTypes()
        {
            return (_types.Select(t => t.ToType()));
        }
        
        public PredicateList That(Filter filter = null)
        {
            return new PredicateList(_types, filter);
        }
    }
}
