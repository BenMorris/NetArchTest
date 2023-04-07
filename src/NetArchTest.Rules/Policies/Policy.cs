namespace NetArchTest.Rules.Policies
{
    using System;

    /// <summary>
    /// An aggregate of rules and results that can be used for reporting.
    /// </summary>
    public sealed class Policy
    {
        /// <summary> The simple name of the policy. </summary>
        private readonly string _name;

        /// <summary> A detailed description of the policy. </summary>
        private readonly string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> class.
        /// </summary>
        private Policy(string name, string description)
        {
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Defines a policy that aggregates a set of rules together for reporting.
        /// </summary>
        public static Policy Define(string name, string description)
        {
            return new Policy(name, description);
        }

        /// <summary>
        /// Sets the types that the policy will apply to.
        /// </summary>
        public PolicyDefinition For(Func<Types> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException();
            }

            return new PolicyDefinition(types, _name, _description);
        }

        /// <summary>
        /// Sets the types that the policy will apply to.
        /// </summary>
        public PolicyDefinition For(Types types)
        {
            if (types == null)
            {
                throw new ArgumentNullException();
            }

            var func = new Func<Types>(() => types);

            return new PolicyDefinition(func, _name, _description);
        }

    }
}