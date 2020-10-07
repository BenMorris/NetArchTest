namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NetArchTest.Rules.Slices.Model;

    internal sealed class TestResult : ITestResult
    {
        public bool IsSuccessful { get; private set; }
        public IEnumerable<IFailingType> FailingTypes { get; private set; }





     
        internal static TestResult Success()
        {
            return new TestResult
            {
                IsSuccessful = true
            };
        }
       
        internal static TestResult Failure(IEnumerable<TypeResult> failingTypes)
        {
            return new TestResult
            {
                IsSuccessful = false,
                FailingTypes = failingTypes
            };
        }
    }
}
