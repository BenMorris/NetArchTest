using NetArchTest.Rules.Matches;
using Xunit.Abstractions;
using static NetArchTest.Rules.Matches.Matchers;

namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.NameMatching.Namespace2;
    using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
    using Xunit;

    public class ConditionListTests
    {
        private readonly ITestOutputHelper _output;

        public ConditionListTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "Conditions can be grouped together using 'or' logic.")]
        public void Or_AppliedToConditions_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching"))
                .Should(HaveNameStartingWith("ClassA") | HaveNameEndingWith("1") | HaveNameEndingWith("2"))
                .GetTypes();
            Assert.Equal(5, result.Count()); // five types found
            Assert.Contains(typeof(ClassA1), result);
            Assert.Contains(typeof(ClassA2), result);
            Assert.Contains(typeof(ClassA3), result);
            Assert.Contains(typeof(ClassB1), result);
            Assert.Contains(typeof(ClassB2), result);
        }

        [Fact(DisplayName = "Conditions can be chained together using 'and' logic.")]
        public void And_AppliedToConditions_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching"))
                .Should(HaveNameStartingWith("Class") & HaveNameEndingWith("1") & BeClass())
                .GetTypes();

            Assert.Equal(2, result.Count()); // two types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
        }

        [Fact(DisplayName = "An Or() statement will signal the start of a separate group of Conditions")]
        public void Or_MultipleInstances_TreatedAsSeparateGroups()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2"))
                .Should(
                    HaveNameStartingWith("ClassA") & HaveNameEndingWith("3")
                    | HaveNameStartingWith("ClassB") & HaveNameEndingWith("2")
                )
                .GetTypes();

            // Results will be everything returned by both groups of statements
            Assert.Equal(2, result.Count()); // five types found
            Assert.Contains(typeof(ClassA3), result);
            Assert.Contains(typeof(ClassB2), result);
        }
    }
}
