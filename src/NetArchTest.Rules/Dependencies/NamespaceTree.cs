namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
    internal class NamespaceTree
    {
        /// <summary>
        /// Represents a node in the namespace tree.
        /// </summary>
        private sealed class Node
        {
            /// <summary> Maps child namespace to its root node. </summary>
            private Dictionary<string, Node> Nodes { get; } = new Dictionary<string, Node>();

            /// <summary> Returs node's "terminated" flag. </summary>
            public bool IsTerminated
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

            /// <summary>
            /// Terminates the node.
            /// </summary>
            public void Terminate()
            {
                IsTerminated = true;
            }

            private static string NormalizeString(string str)
            {
                return str.Normalize(NormalizationForm.FormC);
            }
        }

        /// <summary> Holds the root for the namespace tree. </summary>
        private readonly Node _root = new Node();

        private static readonly char[] _namespaceSeparators = new char[] { '.', '<', '>', ':' };

        /// <summary>
        /// Initially fills the tree with given names.
        /// </summary>
        /// <param name="fullNames">Sequence of full names.</param>
        public NamespaceTree(IEnumerable<string> fullNames)
        {
            foreach (string fullName in fullNames)
            {
                Add(fullName);
            }
        }

        /// <summary>
        /// Splits full name into subnamespaces and adds corresponding nodes to the tree.
        /// </summary>
        /// <param name="fullName">Can be empty, but not null.</param>
        private void Add(string fullName)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException(nameof(fullName));
            }

            var deepestNode = _root;

            int subnameEndIndex = -1;
            while (subnameEndIndex != fullName.Length)
            {
                int subnameStartIndex = subnameEndIndex + 1;
                subnameEndIndex = GetSubnameEndIndex(fullName, subnameStartIndex);

                deepestNode = deepestNode.GetOrAddNode(fullName.Substring(subnameStartIndex, subnameEndIndex - subnameStartIndex));
            }

            if (!deepestNode.IsTerminated)
            {
                deepestNode.Terminate();
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
            var builder = new StringBuilder();

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
                    builder.Append(name);
                    yield return builder.ToString();

                    builder.Append('.');
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
