using NetArchTest.Rules.Matches;

namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.TestStructure.Generic;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.NameMatching.Namespace2;
    using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
    using Xunit;

    public class PredicateListTests
    {
        [Fact(DisplayName = "Predicates can be grouped together using 'or' logic.")]
        public void Or_AppliedToPredicates_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1.*")
                .Or()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.*")
                .Or()
                .ResideInNamespace("NetArchTest.TestStructure.Generic.*")
                .GetTypes();

            Assert.Equal(7, result.Count()); // seven types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(GenericType<>), result);
            Assert.Contains<Type>(typeof(NonGenericType), result);
        }

        [Fact(DisplayName = "Predicates can be chained together using 'and' logic.")]
        public void And_AppliedToPredicates_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .FullNameMatches(Globbing.New("NetArchTest.TestStructure.NameMatching.Namespace1.*"))
                .And()
                .HaveNameStartingWith("Class")
                .And()
                .HaveNameEndingWith("1")
                .GetTypes();

            Assert.Equal(2, result.Count()); // two types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
        }

        [Fact(DisplayName = "An Or() statement will signal the start of a separate group of predicates")]
        public void Or_MultipleInstances_TreatedAsSeparateGroups()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .FullNameMatches(Globbing.New("NetArchTest.TestStructure.NameMatching.Namespace1.*"))
                .And()
                .HaveNameStartingWith("ClassA")
                .Or()
                .FullNameMatches(Globbing.New("NetArchTest.TestStructure.NameMatching.Namespace2.*"))
                .And()
                .HaveNameStartingWith("ClassB")
                .GetTypes();

            // Results will be everything returned by both groups of statements
            Assert.Equal(3, result.Count()); // five types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
        }

    }
}
