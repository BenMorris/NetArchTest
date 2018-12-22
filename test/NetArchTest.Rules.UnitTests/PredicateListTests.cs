using static NetArchTest.Rules.Matches.Matchers;
using System.Linq;
using System.Reflection;
using NetArchTest.TestStructure.Generic;
using NetArchTest.TestStructure.NameMatching.Namespace1;
using NetArchTest.TestStructure.NameMatching.Namespace2;
using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
using Xunit;

namespace NetArchTest.Rules.UnitTests
{

    public class PredicateListTests
    {
        [Fact(DisplayName = "Predicates can be grouped together using 'or' logic.")]
        public void Or_AppliedToPredicates_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(
                    ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1")
                    | ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2")
                    | ResideInNamespace("NetArchTest.TestStructure.Generic"))
                .GetTypes();

            Assert.Equal(7, result.Count()); // seven types found
            Assert.Contains(typeof(ClassA1), result);
            Assert.Contains(typeof(ClassA2), result);
            Assert.Contains(typeof(ClassA3), result);
            Assert.Contains(typeof(ClassB1), result);
            Assert.Contains(typeof(ClassB2), result);
            Assert.Contains(typeof(GenericType<>), result);
            Assert.Contains(typeof(NonGenericType), result);
        }

        [Fact(DisplayName = "Predicates can be chained together using 'and' logic.")]
        public void And_AppliedToPredicates_SelectCorrectTypes()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1")
                & HaveNameStartingWith("Class") & HaveNameEndingWith("1"))
                .GetTypes();

            Assert.Equal(2, result.Count()); // two types found
            Assert.Contains(typeof(ClassA1), result);
            Assert.Contains(typeof(ClassB1), result);
        }

        [Fact(DisplayName = "An Or() statement will signal the start of a separate group of predicates")]
        public void Or_MultipleInstances_TreatedAsSeparateGroups()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(
                    ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1") & HaveNameStartingWith("ClassA")
                    | ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2") & HaveNameStartingWith("ClassB")
                )
                .GetTypes();

            // Results will be everything returned by both groups of statements
            Assert.Equal(3, result.Count()); // five types found
            Assert.Contains(typeof(ClassA1), result);
            Assert.Contains(typeof(ClassA2), result);
            Assert.Contains(typeof(ClassB2), result);
        }
    }
}
