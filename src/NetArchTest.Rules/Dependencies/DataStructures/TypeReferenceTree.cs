namespace NetArchTest.Rules.Dependencies.DataStructures
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Mono.Cecil;
    using NetArchTest.Rules.Extensions;

    /// <summary>
    /// Similar tree to <see cref="NamespaceTree"/>, but this is aware of the structure of type full name,
    /// which allows traversing tree without allocating new strings.
    /// </summary>   
    internal class TypeReferenceTree<T>
    {
        private readonly StartOfTypeNode _root = new StartOfTypeNode();

        
        public NameNode GetNode(TypeReference reference)
        {          
            return TraverseThroughReferenceName(reference, _root);
        }

        private NameNode TraverseThroughReferenceName(TypeReference reference, StartOfTypeNode startOfTypeNode)
        {
            NameNode deepestNameNode;
            if ((reference.IsArray == false) && (reference.IsByReference == false) && (reference.IsPointer == false))
            {
                deepestNameNode = startOfTypeNode.GetNamespace(reference.GetNamespace()).GetName(reference.Name);
                deepestNameNode = GoDeeperIntoGenericArgumentList(reference, deepestNameNode);
            } 
            else
            {
                var referenceAsTypeSpecification = reference as TypeSpecification;
                deepestNameNode = TraverseThroughReferenceName(referenceAsTypeSpecification.ElementType, startOfTypeNode);
                deepestNameNode = deepestNameNode.AddTypeSpecification(referenceAsTypeSpecification);
            }
            return deepestNameNode;
        }
        private NameNode GoDeeperIntoGenericArgumentList(TypeReference reference, NameNode nameNode)
        {
            var deepestNameNode = nameNode;
            if (reference.IsGenericInstance)
            {
                var startOfTypeNode = deepestNameNode.StartArgumentList();
                var referenceAsGenericInstance = reference as GenericInstanceType;
                if (referenceAsGenericInstance.HasGenericArguments)
                {
                    for (int i = 0; i < referenceAsGenericInstance.GenericArguments.Count; i++)
                    {
                        deepestNameNode = TraverseThroughReferenceName(referenceAsGenericInstance.GenericArguments[i], startOfTypeNode);
                        if (i < referenceAsGenericInstance.GenericArguments.Count - 1) startOfTypeNode = deepestNameNode.AddAnotherArgument();
                    }                    
                }
                deepestNameNode = deepestNameNode.EndArgumentList();
            }
            return deepestNameNode;
        }

        [DebuggerDisplay("StartOfTypeNode (namespaces : {namespaces.Count})")]
        public sealed class StartOfTypeNode
        {
            private Dictionary<string, NamespaceNode> namespaces { get; set; } = new Dictionary<string, NamespaceNode>();

            public NamespaceNode GetNamespace(string @namespace)
            {
                NamespaceNode result;
                if (!namespaces.TryGetValue(@namespace, out result))
                {
                    result = new NamespaceNode();
                    namespaces.Add(@namespace, result);
                }
                return result;
            }
        }

        [DebuggerDisplay("NamespaceNode (names : {names.Count})")]
        public sealed class NamespaceNode
        {
            private Dictionary<string, NameNode> names { get; set; } = new Dictionary<string, NameNode>();

            public NameNode GetName(string name)
            {                
                NameNode result;
                if (!names.TryGetValue(name, out result))
                {
                    result = new NameNode();
                    names.Add(name, result);
                }
                return result;
            }
        }

        [DebuggerDisplay("NameNode")]
        public sealed class NameNode
        {
            public T value;
            private StartOfTypeNode startNode;
            private StartOfTypeNode andNode;
            private Dictionary<int, NameNode> typeSpecifications { get; set; }


            public StartOfTypeNode StartArgumentList()
            {
                startNode = startNode ?? new StartOfTypeNode();
                return startNode;
            }
            public StartOfTypeNode AddAnotherArgument()
            {
                andNode = andNode ?? new StartOfTypeNode();
                return andNode;
            }
            public NameNode EndArgumentList()
            {
                // We only need to know where a new list starts and where a comma is placed for unambiguous identification of a generic type,
                // thus we do not need store information about list end, and we can simply return the last name from the list
                return this;
            }
            public NameNode AddTypeSpecification(TypeSpecification typeSpecification)
            {
                typeSpecifications = typeSpecifications ?? new Dictionary<int, NameNode>();

                int specificationNumber = (int)typeSpecification.MetadataType;
                if (typeSpecification.IsArray)
                {
                    var arrayType = typeSpecification as ArrayType;
                    if (arrayType.Rank > 1)
                    {
                        specificationNumber = 666;
                    }
                }

                NameNode result;
                if (!typeSpecifications.TryGetValue(specificationNumber, out result))
                {
                    result = new NameNode();
                    typeSpecifications.Add(specificationNumber, result);
                }
                return result;
            }
        }
    }
}