namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NetArchTest.Rules.Slices.Model;

    [DebuggerDisplay("FailingTypes = {FailingTypes.Count}")]
    internal sealed class TestResult : ITestResult
    {
        public bool IsSuccessful { get; private set; }
        
        public IReadOnlyList<IFailingType> FailingTypes { get; private set; }

     
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
                
                FailingTypes = failingTypes.Select(x => new FailingType(x.Type)).ToArray()
            };
        }
    }
}
