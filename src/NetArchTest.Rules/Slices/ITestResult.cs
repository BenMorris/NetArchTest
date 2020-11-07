namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    /// <summary>
    /// Defines a result from a test carried out on a <see cref="ISliceConditionList"/>.
    /// </summary>
    public interface ITestResult
    {
        /// <summary>
        /// Gets a flag indicating the success or failure of the test.
        /// </summary>
        bool IsSuccessful { get; }


        /// <summary>
        /// Gets a list of the types that failed the test.
        /// </summary>
        IReadOnlyList<IFailingType> FailingTypes { get; }
    }
}
