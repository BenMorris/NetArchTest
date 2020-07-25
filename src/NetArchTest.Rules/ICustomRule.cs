namespace NetArchTest.Rules
{
    using Mono.Cecil;

    /// <summary>
    /// An externally defined rule that can be applied as a condition or a predicate.
    /// </summary>
    public interface ICustomRule
    {
        /// <summary>
        /// Tests whether the supplied type meets the rule.
        /// </summary>
        /// <param name="type">The type to be tested.</param>
        /// <returns>The result of the test.</returns>
        bool MeetsRule(TypeDefinition type);
    }
}
