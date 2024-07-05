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
    using NetArchTest.Rules.Dependencies.DataStructures;

    /// <summary>
    /// Creates a list of types that can have predicates and conditions applied to it.
    /// </summary>
    public sealed class Types
    {
        /// <summary>
        /// The list of types represented by this instance.
        /// </summary>
        private readonly List<TypeDefinition> _types;

        /// <summary>
        /// The list of namespaces to exclude from the current domain.
        /// </summary>
        private static readonly List<string> _exclusionList = new List<string>
        { "System", "Microsoft", "Mono.Cecil", "netstandard", "NetArchTest.Rules", "<Module>", "xunit", "<PrivateImplementationDetails>" };

        private static readonly NamespaceTree _exclusionTree = new NamespaceTree(_exclusionList);

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
            var currentDomain = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !_exclusionTree.GetAllMatchingNames(a.FullName).Any());

            return InAssemblies(currentDomain);
        }

        /// <summary>
        /// Creates a list of types based on a particular assembly.
        /// </summary>
        /// <param name="assembly">The assembly to base the list on.</param>
        /// <param name="searchDirectories">An optional list of search directories to allow resolution of referenced assemblies.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types InAssembly(Assembly assembly, IEnumerable<string> searchDirectories = null)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return InAssemblies(new List<Assembly> { assembly }, searchDirectories);
        }

        /// <summary>
        /// Creates a list of types based on a list of assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to base the list on.</param>
        /// <param name="searchDirectories">An optional list of search directories to allow resolution of referenced assemblies.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types InAssemblies(IEnumerable<Assembly> assemblies, IEnumerable<string> searchDirectories = null)
        {
            var types = new List<TypeDefinition>();
            var directories = searchDirectories?.ToList()
                ?? new List<string>();

            foreach (var assembly in assemblies)
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                AssemblyDefinition assemblyDef;

                if (directories.Any())
                {
                    var defaultAssemblyResolver = new DefaultAssemblyResolver();

                    foreach (var dir in directories)
                    {
                        defaultAssemblyResolver.AddSearchDirectory(dir);
                    }

                    assemblyDef = ReadAssemblyDefinition(assembly.Location,
                        new ReaderParameters { AssemblyResolver = defaultAssemblyResolver });
                }
                else
                {
                    assemblyDef = ReadAssemblyDefinition(assembly.Location);
                }

                // Read all the types in the assembly 
                if (assemblyDef != null)
                {
                    types.AddRange(GetAllTypes(assemblyDef.Modules.SelectMany(t => t.Types)));
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

            foreach (var assembly in assemblies)
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                var assemblyDef = ReadAssemblyDefinition(assembly.Location);

                if (assemblyDef == null)
                {
                    continue;
                }

                // Read all the types in the assembly 
                var matches =
                    (assemblyDef.Modules
                        .SelectMany(t => t.Types)
                        .Where(t => t.Namespace != null && t.Namespace.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();

                if (matches.Count > 0)
                {
                    types.AddRange(matches);
                }
            }

            var list = GetAllTypes(types);
            return new Types(list);
        }

        /// <summary>
        /// Creates a list of all the types in a particular module file.
        /// </summary>
        /// <param name="filename">The filename of the module. This is case insensitive.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        /// <remarks>Assumes that the module is in the same directory as the executing assembly, unless absolute path is provided.</remarks>
        public static Types FromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            // Load the assembly from the current directory
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                      ?? string.Empty;

            var path = Path.IsPathRooted(filename) ?
                filename :
                Path.Combine(dir, filename);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find the assembly file {path}.");
            }

            var assemblyDef = ReadAssemblyDefinition(path);

            return assemblyDef != null
                ? new Types(GetAllTypes(assemblyDef.Modules.SelectMany(t => t.Types)))
                : new Types(new List<TypeDefinition>());
        }

        /// <summary>
        /// Creates a list of all the types in a list of particular module files.
        /// </summary>
        /// <param name="filenames">The list of filenames of the modules. This is case insensitive.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        /// <remarks>Assumes that the modules are in the same directory as the executing assembly, unless absolute paths are provided.</remarks>
        public static Types FromFiles(IEnumerable<string> filenames)
        {
            var types = new List<TypeDefinition>();

            // Load the assemblies from the current directory
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                      ?? string.Empty;

            foreach (var file in filenames)
            {
                var path = Path.IsPathRooted(file) ?
                    file :
                    Path.Combine(dir, file);

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Could not find the assembly file {path}.");
                }

                var assembly = ReadAssemblyDefinition(path);

                if (assembly != null && !_exclusionTree.GetAllMatchingNames(assembly.FullName).Any())
                {
                    types.AddRange(assembly.Modules.SelectMany(t => t.Types));
                }
            }

            var list = GetAllTypes(types);
            return new Types(list);
        }

        /// <summary>
        /// Creates a list of all the types found on a particular path.
        /// </summary>
        /// <param name="path">The relative path to load types from.</param>
        /// <param name="searchDirectories">An optional list of search directories to allow resolution of referenced assemblies.</param>
        /// <returns>A list of types that can have predicates and conditions applied to it.</returns>
        public static Types FromPath(string path, IEnumerable<string> searchDirectories = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var types = new List<TypeDefinition>();
            var directories = searchDirectories?.ToList() ?? new List<string>();

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Could not find the path {path}.");
            }

            var files = Directory.GetFiles(path, "*.dll");
            var readerParams = new ReaderParameters();

            if (directories.Any())
            {
                var defaultAssemblyResolver = new DefaultAssemblyResolver();

                foreach (var dir in directories)
                {
                    defaultAssemblyResolver.AddSearchDirectory(dir);
                }

                readerParams.AssemblyResolver = defaultAssemblyResolver;
            }

            foreach (var file in files)
            {
                var assembly = ReadAssemblyDefinition(file, readerParams);

                if (assembly != null && !_exclusionTree.GetAllMatchingNames(assembly.FullName).Any())
                {
                    types.AddRange(assembly.Modules.SelectMany(t => t.Types));
                }
            }

            var list = GetAllTypes(types);
            return new Types(list);
        }


        /// <summary>
        /// Recursively fetch all the nested types in a collection of types.
        /// </summary>
        /// <returns>The expanded collection of types</returns>
        private static IEnumerable<TypeDefinition> GetAllTypes(IEnumerable<TypeDefinition> types)
        {
            var output = new List<TypeDefinition>(
                types.Where(x => !_exclusionTree.GetAllMatchingNames(x.FullName).Any()));

            for (var i = 0; i < output.Count; ++i)
            {
                var type = output[i];

                output.AddRange(type.NestedTypes
                    .Where(t =>
                        t.CustomAttributes.All(x =>
                            x?.AttributeType?.FullName != typeof(CompilerGeneratedAttribute).FullName)));
            }

            return output;
        }

        /// <summary>
        /// Returns the list of <see cref="TypeDefinition"/> objects describing the types in this list.
        /// </summary>
        /// <returns>The list of <see cref="TypeDefinition"/> objects in this list.</returns>
        internal IEnumerable<TypeDefinition> GetTypeDefinitions()
            => _types;

        /// <summary>
        /// Returns the list of <see cref="Type"/> objects describing the types in this list.
        /// </summary>
        /// <returns>The list of <see cref="Type"/> objects in this list.</returns>
        public IEnumerable<Type> GetTypes()
            => _types.Select(t => t.ToType());

        /// <summary>
        /// Allows a list of types to be applied to one or more filters.
        /// </summary>
        /// <returns>A list of types onto which you can apply a series of filters.</returns>
        public Predicates That()
            => new Predicates(_types);

        /// <summary>
        /// Applies a set of conditions to the list of types.
        /// </summary>
        /// <returns></returns>
        public Conditions Should()
            => new Conditions(_types, true);

        /// <summary>
        /// Applies a negative set of conditions to the list of types.
        /// </summary>
        /// <returns></returns>
        public Conditions ShouldNot()
            => new Conditions(_types, false);

        /// <summary>
        /// Reads the assembly, ignoring a BadImageFormatException
        /// </summary>
        /// <param name="path">The path to the exception</param>
        /// <param name="parameters">A set of optional parameters - normally used to specify custom assembly resolvers. </param>
        /// <returns>The assembly definition for the path (if it exists).</returns>
        private static AssemblyDefinition ReadAssemblyDefinition(string path, ReaderParameters parameters = null)
        {
            try
            {
                return parameters == null
                    ? AssemblyDefinition.ReadAssembly(path)
                    : AssemblyDefinition.ReadAssembly(path, parameters);
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }
    }
}
