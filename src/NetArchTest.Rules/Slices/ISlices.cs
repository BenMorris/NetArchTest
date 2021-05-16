namespace NetArchTest.Rules
{
    /// <summary>
    /// Allows dividing types into groups, also called slices.
    /// </summary>
    /// <returns></returns>
    public interface ISlices
    {
        /// <summary>
        /// Divides types into groups/slices according to the prefix pattern.
        /// It only selects types which namespaces start with a given prefix, rest of the types are ignored.
        /// Groups are defined by the first part of the namespace that is right after prefix:
        /// namespacePrefix.(groupName).restOfNamespace
        /// </summary>      
        ISliceList ByNamespacePrefix(string prefix);
    }
}