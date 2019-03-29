namespace NetArchTest.Rules
{
    /// <summary>
    /// Defines a rule for reporting a Test result
    /// </summary>
    public sealed class Rule
    {
        /// <summary>
        /// The simple name of the rule
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A more detailed description of the rule
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional rule id to correlate with a master system for metrics and KPI's
        /// </summary>
        public int? Id { get; set; }
    }
}