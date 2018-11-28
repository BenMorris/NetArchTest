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
