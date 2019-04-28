namespace NetArchTest.Rules.Policies
{
    using System;

    /// <summary>
    /// A single test that has been added to a policy.
    /// </summary>
    internal sealed class PolicyTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyTest"/> class.
        /// </summary>
        internal PolicyTest(Func<Types, ConditionList> definition, string name, string description)
        {
            Definition = definition;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// The definition of the test expressed as a function.
        /// </summary>
        internal Func<Types, ConditionList> Definition { get; private set; }

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
