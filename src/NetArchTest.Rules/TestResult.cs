namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a result from a test carried out on a <see cref="ConditionList"/>.
    /// </summary>
    public class TestResult
    {
        private TestResult()
        {
        }

        /// <summary>
        /// Flag indicating the success or failure of the test.
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Collection populated with a list of types that failed the test, if the test was a failure.
        /// </summary>
        public IEnumerable<Type> FailingTypes { get; private set; }

        /// <summary>
        /// The Rule associated with this TestResult
        /// </summary>
        internal Rule Rule { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="TestResult"/> indicating a successful test.
        /// </summary>
        /// <returns>Instance of <see cref="TestResult"/></returns>
        public static TestResult Success()
        {
            return new TestResult
            {
                IsSuccessful = true
            };
        }

        /// <summary>
        /// Creates a new instance of <see cref="TestResult"/> indicating a failed test.
        /// </summary>
        /// <returns>Instance of <see cref="TestResult"/></returns>
        public static TestResult Failure(IEnumerable<Type> failingTypes)
        {
            return new TestResult
            {
                IsSuccessful = false,
                FailingTypes = failingTypes
            };
        }

        /// <summary>
        /// Assigns a <see cref="Rule"/> to this TestResult
        /// </summary>
        /// <param name="ruleName">The simple name of the rule <see cref="Rule.Name"/></param>
        /// <param name="ruleDescription">The detailed name of the rule <see cref="Rule.Description"/></param>
        /// <param name="ruleId">The optional Rule Id <see cref="Rule.Id"/></param>
        /// <returns></returns>
        public TestResult MarkForRule(string ruleName, string ruleDescription = "", int? ruleId = null)
        {
            Rule = new Rule
            {
                Name = ruleName,
                Description = ruleDescription,
                Id = ruleId
            };
            return this;
        }
    }
}