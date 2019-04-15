using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NetArchTest.Rules.UnitTests
{
    public class PolicyTests
    {
        [Fact(DisplayName = "Policy can be executed more than once")]
        public void Executed_Policy_Can_Be_Rerun()
        {
            var policy = Policy.Define("Unit Test Policy", "")
                .Add(t => t.ShouldNot().HaveNameStartingWith("System").GetResult().MarkForRule("Exclude System Types"))
                .For(Types.InCurrentDomain);

            policy.Evaluate();

            Assert.False(policy.HasVoilations);

            policy.Evaluate();
        }

        [Fact(DisplayName = "Policy Must have a Func with Types defined")]
        public void Executed_Policy_Requries_Types_Defined()
        {
            var policy = Policy.Define("Unit Test Policy", "")
                .Add(t => t.ShouldNot().HaveNameStartingWith("System").GetResult().MarkForRule("Exclude System Types"))
                ;

            Assert.Throws<InvalidOperationException>(policy.Evaluate);
        }
    }
}