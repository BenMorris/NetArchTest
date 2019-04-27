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
        /// Gets a flag indicating the success or failure of the test.
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Gets a collection populated with a list of any types that failed the test.
        /// </summary>
        public IEnumerable<Type> FailingTypes { get; private set; }

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
    }
}