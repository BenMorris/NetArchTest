namespace NetArchTest.Rules.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An aggregate of rules and results that can be used for reporting.
    /// </summary>
    public sealed class PolicyDefinition
    {
        /// <summary> The function that defines the list of types to execute against each rule. </summary>
        private Func<Types> _typesLocator;

        /// <summary> The list of tests that have been added to the policy. </summary>
        private List<PolicyTest> _tests = new List<PolicyTest>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyDefinition"/> class.
        /// </summary>
        internal PolicyDefinition(Func<Types> types, string name, string description)
        {
            _typesLocator = types;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// The simple name of the policy.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A detailed description of the policy.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Adds a rule to the policy that can optionally be marked with a name and description.
        /// </summary>
        public PolicyDefinition Add(Func<Types, ConditionList> definition, string name, string description)
        { 
            if(definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            _tests.Add(new PolicyTest(definition, name, description));
            return this;
        }

        /// <summary>
        /// Adds a rule to the policy that can optionally be marked with a name and description.
        /// </summary>
        public PolicyDefinition Add(Func<Types, ConditionList> definition)
        {
            return Add(definition, null, null);
        }

        /// <summary>
        /// Evaluates all the rules that have been added to the policy against the types defined for the policy.
        /// </summary>
        /// <returns>A list of results.</returns>
        public PolicyResults Evaluate()
        {
            // Get all the types
            var types = _typesLocator();

            // Execute all the tests
            var results = _tests.Select(t => new PolicyResult(t.Definition(types).GetResult(), t.Name, t.Description));

            // Return the results (forcing evaluation)
            return new PolicyResults(results.ToList(), Name, Description);
        }
    }
}