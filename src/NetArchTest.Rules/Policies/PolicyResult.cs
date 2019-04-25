namespace NetArchTest.Rules.Policies
{
    /// <summary>
    /// A single test result for a policy.
    /// </summary>
    public sealed class PolicyResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyResult"/> class.
        /// </summary>
        internal PolicyResult(TestResult result, string name, string description)
        {
            TestResult = result;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets the result of the test.
        /// </summary>
        public TestResult TestResult { get; private set; }

        /// <summary>
        /// Gets the simple name associated with the test.
        /// </summary>
        internal string Name { get; private set; }

        /// <summary>
        /// Gets the detailed description associated with the test.
        /// </summary>
        internal string Description { get; private set; }
    }
}
