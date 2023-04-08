namespace NetArchTest.Rules.Policies
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A set of results for a policy that has been executed against a list of types.
    /// </summary>
    public sealed class PolicyResults
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyResults"/> class.
        /// </summary>
        internal PolicyResults(IReadOnlyList<PolicyResult> results, string name, string description)
        {
            Results = results;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets whether or not the policy has any rule violations
        /// </summary>
        public bool HasViolations
            => Results.Any(r => !r.IsSuccessful);

        /// <summary>
        /// Gets the results of each rule that was added the policy.
        /// </summary>
        public IReadOnlyList<PolicyResult> Results { get; }

        /// <summary>
        /// Gets the simple name associated with the policy.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the detailed description associated with the policy.
        /// </summary>
        public string Description { get; }

    }
}
