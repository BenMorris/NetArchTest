﻿namespace NetArchTest.Rules.UnitTests
{
    using Mono.Cecil;
    using System;

    /// <summary>
    /// A simple custom rule example that always passes.
    /// </summary>
    public class CustomRuleExample : ICustomRule
    {
        /// <summary> Used by tests to indicate whether the test method has been called. </summary>
        public bool TestMethodCalled => _timesCalled > 0;
        
        /// <summary>The number of times the meetsrule method has been called.</summary>
        public int TimesTimesCalled => _timesCalled;
        
        private int _timesCalled = 0;
        
        /// <summary> The delegate that is used for the rule - this allows the test to define the custom rule. </summary>
        private Func<TypeDefinition, bool> _test;

        public CustomRuleExample(Func<TypeDefinition, bool> test)
        {
            _test = test;
        }

        public bool MeetsRule(TypeDefinition type)
        {
            _timesCalled++;
            return _test(type);
        }
    }
}
