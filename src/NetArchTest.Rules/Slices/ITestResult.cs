namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITestResult
    {
        bool IsSuccessful { get; }
        IEnumerable<IFailingType> FailingTypes { get; }
    }
}
