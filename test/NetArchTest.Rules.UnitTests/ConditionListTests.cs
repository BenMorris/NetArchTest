namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.TestStructure.Interfaces;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.NameMatching.Namespace2;
    using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
    using Xunit;

    public class ConditionListTests
    {
        [Fact(DisplayName = "Conditions can be grouped together using 'or' logic.")]
        public void Or_AppliedToConditions_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .HaveNameStartingWith("ClassA")
                .Or()
                .HaveNameEndingWith("1")
                .Or()
                .HaveNameEndingWith("2")
                .GetTypes();

            Assert.Equal(5, result.Count()); // five types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
        }

        [Fact(DisplayName = "Conditions can be chained together using 'and' logic.")]
        public void And_AppliedToConditions_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .HaveNameStartingWith("Class")
                .And()
                .HaveNameEndingWith("1")
                .And()
                .BeClasses()
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
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2")
                .Should()
                // First group (ClassA3)
                .HaveNameStartingWith("ClassA")
                .And()
                .HaveNameEndingWith("3")
                .Or()
                // Second group group (ClassB1)
                .HaveNameStartingWith("ClassB")
                .And()
                .HaveNameEndingWith("2")
                .GetTypes();

            // Results will be everything returned by both groups of statements
            Assert.Equal(2, result.Count()); // five types found
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
        }

        [Fact(DisplayName = "An ShouldNot() statement will inverse the subsequent conditions")]
        public void ShouldNot_FollowingConditions_Inversed()
        {
            // First example - single condition
            var predicates = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Interfaces")
                .And()
                .HaveNameStartingWith("Implements");

            var result1 = predicates.Should().ImplementInterface(typeof(IExample)).GetResult();
            var result1Not = predicates.ShouldNot().NotImplementInterface(typeof(IExample)).GetResult();

            // Third example - two conditions with an or() statement
            predicates = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(" NetArchTest.TestStructure.NameMatching.Namespace1");

            var result2 = predicates.Should().HaveNameStartingWith("ClassA").Or().HaveNameStartingWith("ClassB").GetResult();
            var result2Not = predicates.ShouldNot().NotHaveNameStartingWith("ClassA").Or().NotHaveNameStartingWith("ClassB").GetResult();

            Assert.Equal(result1.IsSuccessful, result1Not.IsSuccessful);
            Assert.Equal(result2.IsSuccessful, result2Not.IsSuccessful);
        }

        [Fact(DisplayName = "If a condition fails then a list of failing types should be returned.")]
        public void GetResult_Failed_ReturnFailedTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
                .Should()
                .HaveNameStartingWith("ClassA")
                .GetResult();

            Assert.False(result.IsSuccessful);
            Assert.Equal(2, result.FailingTypes.Count()); // two types found
            Assert.Contains<Type>(typeof(ClassB1), result.FailingTypes);
            Assert.Contains<Type>(typeof(ClassB2), result.FailingTypes);
        }

        [Fact(DisplayName = "If a condition fails using ShouldNot logic then a list of failing types should be returned.")]
        public void GetResult_FailedShouldNot_ReturnFailedTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .ShouldNot()
                .HaveNameStartingWith("ClassA")
                .GetResult();

            Assert.False(result.IsSuccessful);
            Assert.Equal(3, result.FailingTypes.Count()); // three types found
            Assert.Contains<Type>(typeof(ClassA1), result.FailingTypes);
            Assert.Contains<Type>(typeof(ClassA2), result.FailingTypes);
            Assert.Contains<Type>(typeof(ClassA3), result.FailingTypes);
        }

        [Fact(DisplayName = "If a condition succeeds then a list of failing types should be null.")]
        public void GetResult_Success_ReturnNullFailedTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
                .Should()
                .HaveNameStartingWith("ClassA")
                .Or()
                .HaveNameEndingWith("1")
                .Or()
                .HaveNameEndingWith("2")
                .GetResult();

            Assert.True(result.IsSuccessful);
            Assert.Null(result.FailingTypes);
        }
    }
}
