namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.Rules.Policies;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
    using Xunit;

    public class PolicyDefinitionTests
    {
        private const string POLICY_NAME = "Name";
        private const string POLICY_DESCRIPTION = "Description";
        private const string RULE_NAME = "Rule Name";
        private const string RULE_DESCRIPTION = "Rule Description";

        [Fact(DisplayName = "A rule that is added to a policy will be executed when the policy is evaluated")]
        public void Evaluate_RuleAdded_ExecutedWhenEvaluated()
        {
            // Arrange
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(POLICY_NAME, POLICY_DESCRIPTION)
                .For(types)
                .Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3")
                .Should()
                .HaveName("XXXXXX"),
                RULE_NAME, RULE_DESCRIPTION
                );

            // Act
            var result = policy.Evaluate();

            // Assert
            // This rule should fail and the result be available in the collection
            Assert.True(result.HasViolations);
            Assert.False(result.Results.First().IsSuccessful);
            Assert.Single(result.Results.First().FailingTypes);
            Assert.Equal(typeof(ClassB2).FullName, result.Results.First().FailingTypes.First().FullName);
        }

        [Fact(DisplayName = "Rules can be aggregated together into a single policy")]
        public void Evaluate_MultipleRulesAdded_Aggregated()
        {
            // Arrange
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(POLICY_NAME, POLICY_DESCRIPTION).For(types);

            policy.Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
                .Should()
                .HaveNameStartingWith("Class"), "Rule1", "Rule1 Description");

            policy.Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .BeClasses(), "Rule2", "Rule2 Description");

            // Act
            var result = policy.Evaluate();

            // Assert
            // Both rules have been evaluated
            Assert.False(result.HasViolations);
            Assert.True(result.Results.First().IsSuccessful);
            Assert.True(result.Results.Last().IsSuccessful);
        }

        [Fact(DisplayName = "A policy can be evaluated more than once")]
        public void Evaluate_MultipleCalls_MultipleResults()
        {
            // Arrange
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(POLICY_NAME, POLICY_DESCRIPTION).For(types);

            // Act
            policy.Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
                .Should()
                .HaveNameStartingWith("Class"), "Rule1", "Rule1 Description");

            var resultSucceed = policy.Evaluate();

            policy.Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .BeSealed(), "Rule2", "Rule2 Description");

            var resultFail = policy.Evaluate();

            // Assert
            // The policy has been evaluated to give different results
            Assert.False(resultSucceed.HasViolations);
            Assert.Single(resultSucceed.Results);
            Assert.True(resultFail.HasViolations);
            Assert.Equal(2, resultFail.Results.Count());
        }

        [Fact(DisplayName = "Rule names and descriptions are optional")]
        public void Evaluate_NAmeAndDescription_Optional()
        {
            // Arrange
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(POLICY_NAME, POLICY_DESCRIPTION)
                .For(types)
                .Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3")
                .Should()
                .HaveName("ClassB2")
                );

            // Act
            var result = policy.Evaluate();

            // Assert
            // This rule should fail and the result be available in the collection
            Assert.Null(result.Results.First().Name);
            Assert.Null(result.Results.First().Description);
        }

        [Fact(DisplayName = "An empty policy will return an empty result set when evaluated")]
        public void Evaluate_EmptyPolicy_EvaluateToEmptyResults()
        {
            // Arrange
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(POLICY_NAME, POLICY_DESCRIPTION)
                .For(types);

            // Act
            var result = policy.Evaluate();

            // Assert
            Assert.False(result.HasViolations);
            Assert.Empty(result.Results);
        }

        [Fact(DisplayName = "A rule name and description will be associated with each result")]
        public void Evaluate_RuleNameAndDescription_AssociatedWithResult()
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(POLICY_NAME, POLICY_DESCRIPTION)
                .For(types)
                .Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3")
                .Should()
                .HaveName("ClassB2"),
                name, description
                );

            // Act
            var result = policy.Evaluate();

            // Assert
            Assert.Equal(name, result.Results.First().Name);
            Assert.Equal(description, result.Results.First().Description);
        }

        [Fact(DisplayName = "A policy's name and description are associated with the result set")]
        public void Evaluate_PolicyNameAndDescription_AssociatedWIthResultSet()
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(name, description)
                .For(types)
                .Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3")
                .Should()
                .HaveName("XXXXXX"),
                RULE_NAME, RULE_DESCRIPTION
                );

            // Act
            var result = policy.Evaluate();

            // Assert
            Assert.Equal(name, result.Name);
            Assert.Equal(description, result.Description);
        }
    }
}
