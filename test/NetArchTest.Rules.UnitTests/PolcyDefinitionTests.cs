namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.Rules.Policies;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
    using Xunit;

    public class PolcyDefinitionTests
    {
        private const string POLICY_NAME = "Name";
        private const string POLICY_DESCRIPTION = "Description";
        private const string RULE_NAME = "Rule Name";
        private const string RULE_DESCRIPTION = "Rule Description";

        [Fact(DisplayName = "A rule that is added to a policy will be executed when it is evaluated")]
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
            Assert.True(result.HasVoilations);
            Assert.False(result.Results.First().IsSuccessful);
            Assert.Single(result.Results.First().FailingTypes);
            Assert.Equal(typeof(ClassB2).FullName, result.Results.First().FailingTypes.First().FullName);
        }

        [Fact(DisplayName = "Rules can be aggregated together into a single policy.")]
        public void Evaluate_MultipleRulesAdded_Aggregated()
        {
            // Arrange
            var types = Types.InAssembly(Assembly.GetAssembly(typeof(ClassA1)));

            var policy = Policy.Define(POLICY_NAME, POLICY_DESCRIPTION).For(types);

            policy.Add(t => t.That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
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
            Assert.False(result.HasVoilations);
            Assert.True(result.Results.First().IsSuccessful);
            Assert.True(result.Results.Last().IsSuccessful);
        }

        [Fact(DisplayName = "A rule name and description will be associated with each result")]
        public void Evaluate_NAmeAndDescription_AssociatedWithResult()
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
            // This rule should fail and the result be available in the collection
            Assert.Equal(name, result.Results.First().Name);
            Assert.Equal(description, result.Results.First().Description);
        }
    }
}
