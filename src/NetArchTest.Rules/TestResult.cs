namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using NetArchTest.Rules.Extensions;

    /// <summary>
    /// Defines a result from a test carried out on a <see cref="ConditionList"/>.
    /// </summary>
    public sealed class TestResult
    {
        /// <summary> The list of types that failed the test. </summary>
        private IReadOnlyList<TypeDefinition> _failingTypes;

        private TestResult()
        {
        }

        /// <summary>
        /// Gets a flag indicating the success or failure of the test.
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Gets a list of the types that failed the test.
        /// </summary>
        /// <remarks>
        /// This method loads all the types and may throw dependency loading errors if the test project does not have a direct dependency on the type being loaded.
        /// </remarks>
        public IReadOnlyList<Type> FailingTypes
        {
            get
            {
                if (_failingTypes != null)
                {
                    return _failingTypes.Select(t => t.ToType()).ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a list of the type nmames that failed the test.
        /// </summary>
        /// <remarks>
        /// This is a "safer" way of getting a list of failed types as it does not load the types when enumerating the list. This can lead to dependency loading errors.
        /// </remarks>
        public IReadOnlyList<string> FailingTypeNames
        {
            get
            {
                if (_failingTypes != null)
                {
                    return _failingTypes.Select(t => t.FullName).ToList();
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Creates a new instance of <see cref="TestResult"/> indicating a successful test.
        /// </summary>
        /// <returns>Instance of <see cref="TestResult"/></returns>
        internal static TestResult Success()
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
        internal static TestResult Failure(IReadOnlyList<TypeDefinition> failingTypes)
        {
            return new TestResult
            {
                IsSuccessful = false,
                _failingTypes = failingTypes
            };
        }
    }
}