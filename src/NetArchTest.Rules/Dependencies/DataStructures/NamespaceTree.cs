namespace NetArchTest.Rules.Dependencies.DataStructures
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Mono.Cecil;
    using NetArchTest.Rules.Extensions;

    /// <summary>
    /// Holds tree structure of full names; child nodes of each parent are indexed for optimal time of search.
    /// </summary>
    /// <example>
    /// The sequence {"System.Linq", "System.Object", "System", "System.Collections.Generic", "NetArchTest.TestStructure.Dependencies.Examples"}
    /// produces the tree
    /// ""
    /// |- "NetArchTest"
    /// |  |- "TestStructure"
    /// |     |- "Dependencies"
    /// |        |- "Examples" (terminated)
    /// |
    /// |- "System" (terminated)
    ///    |- "Collections"
    ///    |  |- "Generic" (terminated)
    ///    |
    ///    |- "Linq" (terminated)
    ///    |- "Object" (terminated)
    /// </example>
    /// <remarks>
    /// In the example above the flag "terminated" means that marked name is presented in the sequence the tree is built for.
    /// Despite of the fact that the namespace "System" takes over its descendants "Linq", "Object" and "Collections.Generic"
    /// all of them are presented in the tree. For dependency search it provides the same results as original implementation
    /// based on the String.StartsWith(...) does.
    /// </remarks>
    internal class NamespaceTree : ISearchTree
    {
        [DebuggerDisplay("Node (nodes : {Nodes.Count})")]
        private sealed class Node
        {
            /// <summary> Maps child namespace to its root node. </summary>
            private Dictionary<string, Node> Nodes { get; } = new Dictionary<string, Node>();
                        
            public bool IsTerminated
            {
                get; private set;
            }
            /// <summary>Returns full path from root to terminated node. Only available on terminated node.</summary>
            public string FullName
            {
                get; private set;
            }

            /// <summary>
            /// Adds new child node with given name or returns existing one.
            /// </summary>
            /// <param name="name">Name of child node.</param>
            /// <returns>Child node with given name.</returns>
            public Node GetOrAddNode(string name)
            {
                name = NormalizeString(name);

                Node result;
                if (!Nodes.TryGetValue(name, out result))
                {
                    result = new Node();
                    Nodes.Add(name, result);
                }
                return result;
            }

            /// <summary>
            /// Checks whether child node with given name exists and returns it.
            /// </summary>
            /// <param name="name">Name of child node of interest.</param>
            /// <param name="node">Child node with given name, if it exists; otherwise, null.</param>
            /// <returns>True, if child node with given name exists; otherwise, false.</returns>
            public bool TryGetNode(string name, out Node node)
            {
                return Nodes.TryGetValue(NormalizeString(name), out node) && node != null;
            }
                       
            public void Terminate(string fullName)
            {
                IsTerminated = true;
                FullName = fullName;
            }

            private static string NormalizeString(string str)
            {
                return str.Normalize(NormalizationForm.FormC);
            }
        }

        /// <summary> Holds the root for the namespace tree. </summary>
        private readonly Node _root = new Node();

        private static readonly char[] _namespaceSeparators = new char[] { '.', ':', '/', '+' };

        /// <summary>
        /// Initially fills the tree with given names.
        /// </summary>
        /// <param name="fullNames">Sequence of full names.</param>
        /// <param name="parseNames">if names should be parsed by mono parser</param>
        public NamespaceTree(IEnumerable<string> fullNames, bool parseNames = false)
        {
            foreach (string fullName in fullNames)
            {
                Add(fullName, parseNames);
            }
        }

        /// <summary>
        /// Splits full name into subnamespaces and adds corresponding nodes to the tree.
        /// </summary>
        /// <param name="fullName">Can be empty, but not null.</param>
        /// <param name="parseNames">if names should be parsed by mono parser</param>
        private void Add(string fullName, bool parseNames)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException(nameof(fullName));
            }

            var deepestNode = _root;
            foreach (var token in TypeParser.Parse(fullName, parseNames))
            {              
                int subnameEndIndex = -1;
                while (subnameEndIndex != token.Length)
                {
                    int subnameStartIndex = subnameEndIndex + 1;
                    subnameEndIndex = GetSubnameEndIndex(token, subnameStartIndex);

                    deepestNode = deepestNode.GetOrAddNode(token.Substring(subnameStartIndex, subnameEndIndex - subnameStartIndex));
                }
            }
            if (!deepestNode.IsTerminated)
            {
                deepestNode.Terminate(fullName);
                TerminatedNodesCount++;
            }
        }

        /// <summary> Count of terminated nodes in the tree. </summary>
        public int TerminatedNodesCount { get; private set; } = 0;

        /// <summary>
        /// Retrieves the sequence of all matching names for given full name.
        /// A name matches some full name, if its node is terminated and whole path from tree root to that node
        /// builds the chain of namespaces in the full name.
        /// </summary>
        /// <param name="fullName">Full name to search matching names for.</param>
        /// <returns>Sequence of all matching names.</returns>
        /// <example>
        /// If the tree contains "System.Collections" and "System.Collections.IList", both are returned
        /// for the full name "System.Collections.IList".
        /// </example>
        public IEnumerable<string> GetAllMatchingNames(string fullName)
        {
            var deepestNode = _root;

            int subnameEndIndex = -1;
            while (subnameEndIndex != fullName.Length)
            {
                int subnameStartIndex = subnameEndIndex + 1;
                subnameEndIndex = GetSubnameEndIndex(fullName, subnameStartIndex);

                string name = fullName.Substring(subnameStartIndex, subnameEndIndex - subnameStartIndex);
                if (!deepestNode.TryGetNode(name, out deepestNode))
                {
                    yield break;
                }

                if (deepestNode.IsTerminated)
                {                  
                    yield return deepestNode.FullName;                    
                }
            }
        }

        public IEnumerable<string> GetAllMatchingNames(TypeReference reference)
        {
            var deepestNode = _root;
           
            foreach (var token in GetTokens(reference))
            {
                int subnameEndIndex = -1;
                while (subnameEndIndex != token.Length)
                {
                    int subnameStartIndex = subnameEndIndex + 1;
                    subnameEndIndex = GetSubnameEndIndex(token, subnameStartIndex);

                    string name = token.Substring(subnameStartIndex, subnameEndIndex - subnameStartIndex);
                    if (!deepestNode.TryGetNode(name, out deepestNode))
                    {
                        yield break;
                    }

                    if (deepestNode.IsTerminated)
                    {
                        yield return deepestNode.FullName;
                    }
                }
            }
        }

        /// <summary>
        /// Recursively extracts every part from type full name
        /// </summary>      
        private IEnumerable<string> GetTokens(TypeReference reference)
        {
            if ((reference.IsArray == false) && (reference.IsByReference == false) && (reference.IsPointer == false))
            {
                yield return reference.GetNamespace();
                yield return reference.Name;
            }
            else
            {
                var referenceAsTypeSpecification = reference as TypeSpecification;
                foreach (var token in GetTokens(referenceAsTypeSpecification.ElementType))
                {
                    yield return token;
                }
                if (reference.IsByReference)
                {
                    yield return "&";
                }
                if (reference.IsPointer)
                {
                    yield return "*";
                }
                if (reference.IsArray)
                {
                    var referenceAsArrayType = reference as ArrayType;
                    yield return referenceAsArrayType.Rank == 1 ? "[]" : "[,]";
                }
            }

            if (reference.IsGenericInstance)
            {                
                var referenceAsGenericInstance = reference as GenericInstanceType;
                if (referenceAsGenericInstance.HasGenericArguments)
                {
                    yield return "<";
                    for (int i = 0; i < referenceAsGenericInstance.GenericArguments.Count; i++)
                    {
                        foreach (var token in GetTokens(referenceAsGenericInstance.GenericArguments[i]))
                        {
                            yield return token;                            
                        }
                        yield return ",";
                    }
                }
            }
            
        }

        private static int GetSubnameEndIndex(string namespaceFullName, int subnameStartIndex)
        {
            int nextSeparatorIndex = namespaceFullName.IndexOfAny(_namespaceSeparators, subnameStartIndex);
            if (nextSeparatorIndex < 0)
            {
                nextSeparatorIndex = namespaceFullName.Length;
            }

            return nextSeparatorIndex;
        }
    }
}